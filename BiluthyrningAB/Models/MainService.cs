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
            Orders x = new Orders
            {
                CarType = newOrder.CarType,
                RegNr = newOrder.RegNr,
                CustomerId = newOrder.CustomerId,
                PickUpDate = newOrder.PickUpDate,
                ReturnDate = (DateTime)newOrder.PickUpDate,
                CurrentMileage = newOrder.CurrentMileage,
                IsReturned = newOrder.IsReturned
            };

            context.Orders.Add(x);
            context.SaveChanges();

        }
        internal OrderVM ListCustomersForBooking()
        {
            var customersList = context.Customers
                .Select(c => new CustomerVM
                {
                    CustomerId = c.CustomerId,
                    Ssn = c.Ssn,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                })
                .ToArray();

            OrderVM cList = new OrderVM();
            cList.Customers = customersList;

            return cList;
        }

        public OrderVM[] ViewBookings()
        {
            return context.Orders
                .OrderBy(d => d.PickUpDate)
                .Select(d => new OrderVM
                {
                    BookingNr = d.BookingNr,
                    RegNr = d.RegNr,
                    CarType = d.CarType,
                    CurrentMileage = d.CurrentMileage,
                    IsReturned = (bool)d.IsReturned
                })
                .Where(d => d.IsReturned != true)
                .ToArray();
        }

        internal OrderVM GetBookingById(int id)
        {
             var orderDetails = context.Orders
                .Where(b => b.BookingNr == id)
                .Select(b => new OrderVM
                {
                    BookingNr = b.BookingNr,
                    RegNr = b.RegNr,
                    CarType = b.CarType,
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
            orderReturned.ReturnDate = DateTime.Now;
            orderReturned.MileageOnReturn = carToReturn.MileageOnReturn;
            orderReturned.DrivenMiles = carToReturn.MileageOnReturn - orderReturned.CurrentMileage;
            orderReturned.IsReturned = true;

            context.SaveChanges();
        }
        
        public RecieptVM CreateReciept(int id)
        {
            int baseDayRental = 400;
            int kmPrice = 5;
            double totalPrice = 0;

            var booking = context.Orders.First(x => x.BookingNr == id);

            TimeSpan diffResult = booking.ReturnDate.Subtract((DateTime)booking.PickUpDate);


            if (booking.CarType == "Small car")
                totalPrice = diffResult.TotalDays * baseDayRental;

            else if (booking.CarType == "Van")
                totalPrice = diffResult.TotalDays * (baseDayRental * 1.2) + (booking.DrivenMiles * kmPrice);

            else
                totalPrice = diffResult.TotalDays * (baseDayRental * 1.7) + (booking.DrivenMiles * kmPrice * 1.5);


            RecieptVM reciept = new RecieptVM();

            reciept.BookingNr = booking.BookingNr;
            reciept.CarType = booking.CarType;
            reciept.RegNr = booking.RegNr;
            reciept.PickUpDate = (DateTime)booking.PickUpDate;
            reciept.ReturnDate = booking.ReturnDate;
            reciept.MileageOnPickup = booking.CurrentMileage;
            reciept.MileageOnReturn = booking.MileageOnReturn;
            reciept.DrivenMiles = booking.DrivenMiles;
            reciept.DaysRented = diffResult.Days;
            reciept.TotalPrice = totalPrice;

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
            return context.Orders
                .OrderBy(d => d.PickUpDate)
                .Select(d => new OrderVM
                {
                    BookingNr = d.BookingNr,
                    RegNr = d.RegNr,
                    CarType = d.CarType,
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
            var cDetails = context.Customers.First(c => c.CustomerId == id);
            var cOrders = context.Orders
                .Where(o => o.CustomerId == id && o.IsReturned == false)
                .Select(o => new OrderVM
                {
                    BookingNr = o.BookingNr,
                    CustomerId = o.CustomerId,
                    PickUpDate = o.PickUpDate,
                    RegNr = o.RegNr,
                    CarType = o.CarType,
                    CurrentMileage = o.CurrentMileage,
                })
                .ToArray();

            CustomerDetailsVM details = new CustomerDetailsVM();

            details.CustomerId = cDetails.CustomerId;
            details.Ssn = cDetails.Ssn;
            details.FirstName = cDetails.FirstName;
            details.LastName = cDetails.LastName;
            details.Orders = cOrders;

            return details;


        }

    }
}
