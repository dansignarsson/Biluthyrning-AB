using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiluthyrningAB.Models;
using BiluthyrningAB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BiluthyrningAB.Controllers
{
    public class CarsController : Controller
    {

        //SERVICICEEC
        CarsService service;

        public CarsController( CarsService service)
        {
            this.service = service;
        }


        [HttpGet]
        [Route("/CarsIndex/")]
        public IActionResult CarsIndex()
        {
            return View(service.GetAllCarsFromDB());
        }

        [HttpGet]
        [Route("/AddCar/")]
        public IActionResult AddCar()
        {
            return View();
        }

        [HttpPost]
        [Route("/AddCar/")]
        public IActionResult AddCar(CarVM carToAdd)
        {
            if (!ModelState.IsValid)
                return View();

            service.AddCarToDb(carToAdd);
            return RedirectToAction(nameof(AddCar));
        }

        [HttpGet]
        [Route("/CarDetails/{id}")]
        public IActionResult CarDetails(int id)
        {
            return View(service.GetCarDetails(id));
        }

        [HttpPost]
        [Route("/UpdateCarService/")]
        public IActionResult UpdateCarService(CarVM carToUpdate)
        {
            if (!ModelState.IsValid)
                return View();

            service.UpdateCarService(carToUpdate);
            return RedirectToAction("CarDetails", new { id = carToUpdate.Id });
        }

        [HttpPost]
        [Route("/UpdateCarCleaning/")]
        public IActionResult UpdateCarCleaning(CarVM carToUpdate)
        {
            if (!ModelState.IsValid)
                return View();

            service.UpdateCarCleaning(carToUpdate);
            return RedirectToAction("CarDetails", new { id = carToUpdate.Id });
        }

        [HttpPost]
        [Route("/RemoveCarFromDb/")]
        public IActionResult RemoveCarFromDb(int id)
        {
            if (!ModelState.IsValid)
                return View();

            service.RemoveCarfromDb(id);
            return RedirectToAction("CarsIndex");
        }

        //[HttpPost]
        //[Route("~/AvailableCars")]
        //public IActionResult AvailableCars([FromBody]RentPeriodData viewModel)
        //{
        //    CarsListOfAllVM[] x = service.CheckCarsAvailabilityDuringPeriod(viewModel);
        //    //return RedirectToAction(nameof(AvailableCars));
        //    //return RedirectToAction("AvailableCars", "Cars", x);
        //    //return RedirectToAction(nameof(AvailableCars), new { CarsListOfAllVM = x});
        //    return AvailableCars(x);


        //}
        //[HttpGet]
        //[Route("~/AvailableCars")]
        //public IActionResult AvailableCars(CarsListOfAllVM[] x)
        //{

        //    //CarsListOfAllVM[] x = service.CheckCarsAvailabilityDuringPeriod(viewModel);
        //    return PartialView("_AvailableCars", x);

        //}
    }
}