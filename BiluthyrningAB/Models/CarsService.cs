using BiluthyrningAB.Models.Data;
using BiluthyrningAB.Models.Entities;
using BiluthyrningAB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models
{
    public class CarsService
    {
        public CarsService(MercuryContext context)
        {
            this.context = context;
        }

        readonly MercuryContext context;


        internal void AddCarToDb(CarVM carToAdd)
        {
            Cars c = new Cars
            {
                RegNr = carToAdd.RegNr,
                CarType = carToAdd.CarType,
                Mileage = carToAdd.Mileage,
                IsAvailable = true,
                TimesRented = 0,
                ToBeCleaned = false,
                ToBeRemoved = false,
                NeedService = false
            };

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
                order.CarId = 0;
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
        internal CarVM[] GetAvailableCars(AvailableCarsData dataModel)
        {
            return context.Cars
            .Where(x => (x.Orders.All(o => !((dataModel.PickUpDate >= o.PickUpDate && dataModel.PickUpDate <= o.ReturnDate && o.IsActive) || (dataModel.ReturnDate >= o.PickUpDate && dataModel.ReturnDate <= o.ReturnDate && o.IsActive)))))
            .Select(x => new CarVM
            {
                Id = x.Id,
                RegNr = x.RegNr,
                CarType = x.CarType,
            })
            .ToArray();
        }

    }
}
