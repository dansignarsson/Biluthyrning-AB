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
                ReturnDate = (DateTime)newOrder.PickUpDate,
                CurrentMileage = carToBook.Mileage,
                IsReturned = newOrder.IsReturned
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

        internal OrderVM ListAvailableCarsForBooking()
        {
            var carsList = context.Cars
                .Where(c => c.IsAvailable == true)
                .Select(c => new CarVM
                {
                    Id = c.Id,
                    CarType = c.CarType,
                    RegNr = c.RegNr,
                    IsAvailable = c.IsAvailable,
                })
                .ToArray();

            OrderVM order = new OrderVM();
            order.Cars = carsList;

            return order;
        }

        public OrderVM[] ViewBookings() //ÄNDRAA   ALJALKAJLKAJLKAJLKAAJ TYP FIxAT
        {

            var orders = context.Orders
                .OrderBy(d => d.PickUpDate)
                .Select(d => new OrderVM
                {
                    BookingNr = d.BookingNr,
                    CustomerId = d.CustomerId,
                    CarId = (int)d.CarId,
                    IsReturned = (bool)d.IsReturned,
                    RegNr = context.Cars
                    .Where(c => c.Id == d.CarId)
                    .Select(c => c.RegNr).FirstOrDefault(),
                    CarType = context.Cars
                    .Where(c => c.Id == d.CarId)
                    .Select(c => c.CarType).FirstOrDefault(),

                })
                .Where(d => d.IsReturned != true)
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
                   PickUpDate = (DateTime)b.PickUpDate,
                   ReturnDate = b.ReturnDate,
                   CurrentMileage = b.CurrentMileage
               })
               .Single();

            return orderDetails;
        }

        internal void CarToReturn(OrderVM carToReturn)
        {
            var orderReturned = context.Orders.First(x => x.BookingNr == carToReturn.BookingNr);
            var car = context.Cars.First(c => c.Id == orderReturned.CarId);

            orderReturned.ReturnDate = DateTime.Now;
            orderReturned.MileageOnReturn = carToReturn.MileageOnReturn;
            orderReturned.DrivenMiles = carToReturn.MileageOnReturn - orderReturned.CurrentMileage;
            orderReturned.IsReturned = true;

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
            int baseDayRental = 400;
            int kmPrice = 5;
            double totalPrice = 0;

            var booking = context.Orders.First(x => x.BookingNr == id);
            var carToBook = context.Cars.First(c => c.Id == booking.CarId);
            var customer = context.Customers.First(k => k.CustomerId == booking.CustomerId);

            TimeSpan diffResult = booking.ReturnDate.Subtract((DateTime)booking.PickUpDate);


            if (carToBook.CarType == "Small car")
                totalPrice = diffResult.TotalDays * baseDayRental;

            else if (carToBook.CarType == "Van")
                totalPrice = diffResult.TotalDays * (baseDayRental * 1.2) + (booking.DrivenMiles * kmPrice);

            else
                totalPrice = diffResult.TotalDays * (baseDayRental * 1.7) + (booking.DrivenMiles * kmPrice * 1.5);


            RecieptVM reciept = new RecieptVM();

            reciept.BookingNr = booking.BookingNr;
            reciept.CarType = carToBook.CarType;
            reciept.RegNr = carToBook.RegNr;
            reciept.PickUpDate = (DateTime)booking.PickUpDate;
            reciept.ReturnDate = booking.ReturnDate;
            reciept.MileageOnPickup = booking.CurrentMileage;
            reciept.MileageOnReturn = booking.MileageOnReturn;
            reciept.DrivenMiles = booking.DrivenMiles;
            reciept.DaysRented = diffResult.Days;
            reciept.TotalPrice = totalPrice;
            reciept.CustomerId = customer.CustomerId;
            reciept.Ssn = customer.Ssn;
            reciept.FirstName = customer.FirstName;
            reciept.LastName = customer.LastName;

            return reciept;
        }


        internal void CreateCustomer(CustomerVM newCustomer)
        {
            Customers x = new Customers();

            x.Ssn = newCustomer.Ssn;
            x.FirstName = newCustomer.FirstName;
            x.LastName = newCustomer.LastName;

            context.Customers.Add(x);
            context.SaveChanges();
        }

        internal CustomerVM[] GetAllCustomers()
        {
            return context.Customers
                .OrderBy(c => c.CustomerId)
                .Select(c => new CustomerVM
                {
                    CustomerId = c.CustomerId,
                    Ssn = c.Ssn,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                })
                .ToArray();
        }

        internal OrderVM[] GetCustomerOrders(int id)
        {
            //var carToBook = context.Cars.First(c => c.Id == booking.CarId); LÄGG TILL

            return context.Orders
                .OrderBy(d => d.PickUpDate)
                .Select(d => new OrderVM
                {
                    BookingNr = d.BookingNr,
                    CurrentMileage = d.CurrentMileage,
                    IsReturned = (bool)d.IsReturned
                })
                .Where(d => d.CustomerId == id)
                .ToArray();
        }
        public CustomerVM GetCustomerDetails(int id)
        {
            return context.Customers
                .Where(c => c.CustomerId == id)
                .Select(c => new CustomerVM
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Ssn = c.Ssn
                })
                .Single();
        }

        public CustomerDetailsVM GetCustomerDetailsAndOrders(int id)
        {
            var cDetails = context.Customers.First(c => c.CustomerId == id);                //LÄGGTILL!!
            var cOrders = context.Orders
                .Where(o => o.CustomerId == id && o.IsReturned == false)
                .Select(o => new OrderVM
                {
                    BookingNr = o.BookingNr,
                    CustomerId = o.CustomerId,
                    PickUpDate = o.PickUpDate,
                    CurrentMileage = o.CurrentMileage,
                })
                .ToArray();

            var cHistory = context.HistoryLog
                .Where(h => h.CustomerId == id)
                .Select(h => new HistoryLogVM
                {
                    CustomerId = h.CustomerId,
                    Activity = h.Activity
                }).ToArray();

            CustomerDetailsVM details = new CustomerDetailsVM();

            details.CustomerId = cDetails.CustomerId;
            details.Ssn = cDetails.Ssn;
            details.FirstName = cDetails.FirstName;
            details.LastName = cDetails.LastName;
            details.Orders = cOrders;
            details.History = cHistory;

            return details;
        }

        internal void AddCarToDb(CarVM carToAdd)
        {
            Cars c = new Cars();

            c.RegNr = carToAdd.RegNr;
            c.CarType = carToAdd.CarType;
            c.Mileage = carToAdd.Mileage;
            c.IsAvailable = true;
            c.TimesRented = 0;
            c.ToBeCleaned = false;
            c.ToBeRemoved = false;
            c.NeedService = false;

            context.Cars.Add(c);
            context.SaveChanges();
        }

        internal CarVM[] GetAllCarsFromDB()
        {
            return context.Cars
                .OrderBy(c => c.RegNr)
                .Select(c => new CarVM
                {
                    Id = c.Id,
                    RegNr = c.RegNr,
                    CarType = c.CarType,
                    Mileage = c.Mileage,
                    IsAvailable = c.IsAvailable,
                    ToBeCleaned = c.ToBeCleaned,
                    NeedService = c.NeedService,
                    ToBeRemoved = c.ToBeRemoved,
                })
                .ToArray();
        }

        internal CarVM GetCarDetails(int id)
        {
            return context.Cars
                .Where(c => c.Id == id)
                .Select(c => new CarVM
                {
                    Id = c.Id,
                    RegNr = c.RegNr,
                    CarType = c.CarType,
                    Mileage = c.Mileage,
                    IsAvailable = c.IsAvailable,
                    ToBeCleaned = c.ToBeCleaned,
                    NeedService = c.NeedService,
                    ToBeRemoved = c.ToBeRemoved,
                    History = context.HistoryLog
                    .Where(h => h.CarId == id)
                    .Select(h => new HistoryLogVM
                    {
                        CarId = h.CarId,
                        Activity = h.Activity,
                    }).ToArray()
                })
                .First();
        }

        internal void UpdateCarService(CarVM update)
        {
            var carToUpdate = context.Cars.First(c => c.Id == update.Id);
            carToUpdate.NeedService = update.NeedService;

            HistoryLog y = new HistoryLog
            {
                CarId = carToUpdate.Id,
                Activity = $"Bil {carToUpdate.Id} Lämnades in på service."
            };

            context.HistoryLog.Add(y);
            context.SaveChanges();
        }

        internal void UpdateCarCleaning(CarVM update)
        {
            var carToUpdate = context.Cars.First(c => c.Id == update.Id);
            carToUpdate.ToBeCleaned = update.ToBeCleaned;

            HistoryLog y = new HistoryLog
            {
                CarId = carToUpdate.Id,
                Activity = $"Bil {carToUpdate.Id} städades."
            };

            context.HistoryLog.Add(y);
            context.SaveChanges();
        }

        internal void RemoveCarfromDb(int deleteId)
        {
            var updateHistory = context.HistoryLog
                .Where(h => h.CarId == deleteId)
                .ToArray();

            var updateOrders = context.Orders
                .Where(o => o.CarId == deleteId)
                .ToArray();

            foreach (var order in updateOrders)
            {
                order.CarId = null;
                context.SaveChanges();
            }

            foreach (var log in updateHistory)
            {
                log.CarId = null;
            }

            var carToDelete = context.Cars
                .First(c => c.Id == deleteId);

            context.Cars.Remove(carToDelete);
            context.SaveChanges();
                
        }
        internal HistoryLogVM[] GetAllHistory()
        {
            return context.HistoryLog
                .Select(h => new HistoryLogVM
                {
                    Activity = h.Activity,
                }).ToArray();
        }

    }
}

