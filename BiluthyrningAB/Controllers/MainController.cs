using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiluthyrningAB.Models;
using BiluthyrningAB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BiluthyrningAB.Controllers
{
    public class MainController : Controller
    {
        readonly MainService service;

        public MainController(MainService service)
        {
            this.service = service;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/Booking/")]
        public IActionResult Booking()
        {
            return View();
        }

        [HttpPost]
        [Route("/Booking/")]
        public IActionResult Booking(OrderVM order)
        {
            if (!ModelState.IsValid)
                return View();

            service.BookCar(order);
            return RedirectToAction(nameof(Index));
        }

        [Route("/Return/")]
        public IActionResult Return()
        {
            return View(service.ViewBookings());
        }

        [Route("BookingDetails/{id}")]
        public IActionResult BookingDetails(int id)
        {
            return View(service.GetBookingById(id));
        }

        [Route("/ConfirmReturn/")]
        public IActionResult ConfirmReturn(OrderVM carToReturn)
        {
            service.CarToReturn(carToReturn);
            return RedirectToAction("ReturnConfirmationReciept", new { id = carToReturn.BookingNr });
        }

        [Route("/ReturnConfirmationReciept/")]
        public IActionResult ReturnConfirmationReciept(int id)
        {
            return View(service.CreateReciept(id));
        }


        [HttpGet]
        [Route("/History/")]
        public IActionResult History()
        {
            return View(service.GetAllHistory());
        }


    }
}