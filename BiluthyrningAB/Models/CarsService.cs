using BiluthyrningAB.Models.Entities;
using BiluthyrningAB.Models.ViewModels;
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

    }
}
