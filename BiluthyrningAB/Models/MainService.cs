using BiluthyrningAB.Models.Entities;
using BiluthyrningAB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models
{
    public class MainService
    {

        public MainService(MercuryContext context)
        {
            this.context = context;
        }

        readonly MercuryContext context;

        internal void BookCar(OrderVM newOrder)
        {

            var carToBook = context.Cars.First(c => c.Id == newOrder.CarId);

            Orders x = new Orders
            {
                CustomerId = context.Customers
                .Where(c => c.Ssn == newOrder.Ssn)
                .Select(c => c.CustomerId)
                .FirstOrDefault(),
                CarId = newOrder.CarId,
                PickUpDate = newOrder.PickUpDate,
                ReturnDate = newOrder.ReturnDate,
                CurrentMileage = carToBook.Mileage,
                IsReturned = false,
                IsActive = true,
            };

            carToBook.IsAvailable = false;
            carToBook.TimesRented += 1;

            context.Orders.Add(x);

            HistoryLog y = new HistoryLog
            {
                CustomerId = x.CustomerId,
                CarId = newOrder.CarId,
                Activity = $"Kund {x.CustomerId} hyrde bil {newOrder.CarId}"
            };

            context.HistoryLog.Add(y);
            context.SaveChanges();
        }

        public OrderVM[] ViewBookings()
        {

            var orders = context.Orders
                .Select(d => new OrderVM
                {
                    BookingNr = d.BookingNr,
                    CustomerId = d.CustomerId,
                    CarId = (int)d.CarId,
                    IsReturned = d.IsReturned,
                    IsActive = d.IsActive,
                    RegNr = context.Cars
                    .Where(c => c.Id == d.CarId)
                    .Select(c => c.RegNr).FirstOrDefault(),
                    CarType = context.Cars
                    .Where(c => c.Id == d.CarId)
                    .Select(c => c.CarType).FirstOrDefault(),
                })
                .Where(d => d.IsActive == true)
                .OrderBy(d => d.PickUpDate)
                .ToArray();

            return orders;
        }

        internal OrderVM GetBookingById(int id)
        {
            var orderDetails = context.Orders
               .Where(b => b.BookingNr == id)
               .Select(b => new OrderVM
               {
                   BookingNr = b.BookingNr,
                   CarId = (int)b.CarId,
                   PickUpDate = b.PickUpDate,
                   ReturnDate = b.ReturnDate,
                   CurrentMileage = b.CurrentMileage,
               })
               .Single();

            return orderDetails;
        }

        internal void CarToReturn(OrderVM carToReturn)
        {
            var orderReturned = context.Orders.First(x => x.BookingNr == carToReturn.BookingNr);
            var car = context.Cars.First(c => c.Id == orderReturned.CarId);

            orderReturned.MileageOnReturn = carToReturn.MileageOnReturn;
            orderReturned.DrivenMiles = carToReturn.MileageOnReturn - orderReturned.CurrentMileage;
            orderReturned.IsReturned = true;
            orderReturned.IsActive = false;

            car.ToBeCleaned = true;
            car.IsAvailable = true;
            car.Mileage += orderReturned.DrivenMiles;
            if (car.TimesRented == 3)
            {
                car.NeedService = true;
                car.TimesRented = 0;
            }
            if (car.Mileage >= 2000)
                car.ToBeRemoved = true;

            HistoryLog y = new HistoryLog
            {
                OrderId = carToReturn.BookingNr,
                CustomerId = orderReturned.CustomerId,
                CarId = car.Id,
                Activity = $"Kund {orderReturned.CustomerId} Lämnade tillbaka bil {car.Id}"
            };

            context.HistoryLog.Add(y);
            context.SaveChanges();
        }


        public RecieptVM CreateReciept(int id)
        {
            var order = context.Orders.First(o => o.BookingNr == id);
            var car = context.Cars.First(c => c.Id == order.CarId);
            var customer = context.Customers.First(k => k.CustomerId == order.CustomerId);

            int baseDayRental = 400;
            int kmPrice = 5;
            double totalPrice = 0;
            int drivenMiles = order.DrivenMiles;
            double totalDays = (order.ReturnDate.Date - order.PickUpDate.Date).TotalDays;
            string carType = car.CarType; 

            customer.TimesRented += 1;
            customer.MilesDriven += order.DrivenMiles;
            context.SaveChanges();


            if(customer.Vipstatus != 3)
            UpdateVipStatus(customer);

            int vipLevel = CheckVipStatus(customer);


            if (vipLevel > 0)
                baseDayRental /= 2;

            if (vipLevel > 1 && totalDays == 3)
                totalDays = 2;

            if (vipLevel > 1 && totalDays >= 4)
                totalDays -= 2;


            if (vipLevel > 2 && drivenMiles > 20)
                drivenMiles -= 20;


            switch (carType)
            {
                case "Small Car":
                    totalPrice = totalDays * baseDayRental;
                    break;

                case "Minibus":
                    totalPrice = totalDays * (baseDayRental * 1.2) + (order.DrivenMiles * kmPrice);
                    break;

                case "Van":
                    totalPrice = totalDays * (baseDayRental * 1.7) + (order.DrivenMiles * kmPrice * 1.5);
                    break;

                default:
                    break;
            }


            RecieptVM reciept = new RecieptVM
            {
                BookingNr = order.BookingNr,
                CarType = car.CarType,
                RegNr = car.RegNr,
                PickUpDate = (DateTime)order.PickUpDate,
                ReturnDate = order.ReturnDate,
                MileageOnPickup = order.CurrentMileage,
                MileageOnReturn = order.MileageOnReturn,
                DrivenMiles = order.DrivenMiles,
                DaysRented = totalDays,
                TotalPrice = totalPrice,
                CustomerId = customer.CustomerId,
                Ssn = customer.Ssn,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };

            return reciept;

        }

        public int CheckVipStatus(Customers customer)
        {
            return customer.Vipstatus;
        }

        private void UpdateVipStatus(Customers customer)
        {

            if (customer.TimesRented == 3)
            {
                customer.Vipstatus += 1;

                HistoryLog y = new HistoryLog
                {
                    CustomerId = customer.CustomerId,
                    Activity = $"Kund {customer.CustomerId} uppgraderas till VIP nivå Brons"
                };
                context.HistoryLog.Add(y);
                context.SaveChanges();
            }

            if (customer.TimesRented == 5)
            {
                customer.Vipstatus += 1;
                HistoryLog y = new HistoryLog
                {
                    CustomerId = customer.CustomerId,
                    Activity = $"Kund {customer.CustomerId} uppgraderas till VIP nivå Silver"
                };
                context.HistoryLog.Add(y);
                context.SaveChanges();
            }

            if (customer.TimesRented > 4 && customer.MilesDriven > 1000)
            {
                customer.Vipstatus += 1;
                HistoryLog y = new HistoryLog
                {
                    CustomerId = customer.CustomerId,
                    Activity = $"Kund {customer.CustomerId} uppgraderas till VIP nivå Guld"
                };
                context.HistoryLog.Add(y);
                context.SaveChanges();
            }

        }

        internal HistoryLogVM[] GetAllHistory()
        {
            return context.HistoryLog
                .Select(h => new HistoryLogVM
                {
                    Activity = h.Activity,
                })
                .ToArray();
        }

    }
}

