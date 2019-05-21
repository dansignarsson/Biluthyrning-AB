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

        MainService service;
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
            return View(service.ListAvailableCarsForBooking());
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
            if (!ModelState.IsValid)
                return View(nameof(Return));


            service.CarToReturn(carToReturn);
            return RedirectToAction("ReturnConfirmationReciept", new { id = carToReturn.BookingNr });
        }

        [Route("/ReturnConfirmationReciept/")]
        public IActionResult ReturnConfirmationReciept(int id)
        {
            return View(service.CreateReciept(id));
        }

        [HttpGet]
        [Route("/CustomersPage/")]
        public IActionResult CustomersPage()
        {
            return View(service.GetAllCustomers());
        }

        [HttpGet]
        [Route("/CreateNewCustomer/")]
        public IActionResult CreateNewCustomer()
        {
                return View();
        }

        [HttpPost]
        [Route("/CreateNewCustomer/")]
        public IActionResult CreateNewCustomer(CustomerVM newCustomer)
        {
            if (!ModelState.IsValid)
                return View();

            service.CreateCustomer(newCustomer);
            return RedirectToAction(nameof(CustomersPage));
        }

        [HttpGet]
        [Route("/CustomerDetails/{id}")]
        public IActionResult CustomerDetails(int id)
        {
            return View(service.GetCustomerDetailsAndOrders(id));
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
        [Route("/UpdateCarServices/")]
        public IActionResult UpdateCarServices(CarVM carToUpdate)
        {
            if (!ModelState.IsValid)
                return View();

            service.UpdateCarServices(carToUpdate);
            return RedirectToAction(nameof(CarsIndex));
        }


    }
}