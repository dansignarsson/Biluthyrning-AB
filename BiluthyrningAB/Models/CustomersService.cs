﻿using BiluthyrningAB.Models.Data;
using BiluthyrningAB.Models.Entities;
using BiluthyrningAB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models
{
    public class CustomersService
    {
        public CustomersService(MercuryContext context)
        {
            this.context = context;
        }

        readonly MercuryContext context;


        internal void CreateCustomer(CustomerVM newCustomer)
        {
            Customers x = new Customers();

            x.Ssn = newCustomer.Ssn;
            x.FirstName = newCustomer.FirstName;
            x.LastName = newCustomer.LastName;
            x.Vipstatus = 0;
            x.TimesRented = 0;
            x.MilesDriven = 0;

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
                    Vipstatus = c.Vipstatus,
                    
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
                    CurrentMileage = d.CurrentMileage,
                    IsReturned = d.IsReturned
                })
                .Where(d => d.CustomerId == id)
                .ToArray();
        }

        internal CustomerDetailsData GetCustomerData(string ssn)
        {
            CustomerDetailsData x = context.Customers
                .Where(c => c.Ssn == ssn)
                .Select(c => new CustomerDetailsData
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Ssn = c.Ssn,
                    Success = true
                }).SingleOrDefault();

            if (x == null)
            {
                CustomerDetailsData y = new CustomerDetailsData
                {
                    Success = false
                };
                return y;
            }
            return x;
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
                    Ssn = c.Ssn,
                    Vipstatus = c.Vipstatus,
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
        internal bool IsCustomerRegistered(string ssn)
        {
            var dbSsn = context.Customers.Any(c => c.Ssn == ssn);

            if (dbSsn == true)
                return true;
            else
                return false;

        }
    }
}
