using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiluthyrningAB.Models;
using BiluthyrningAB.Models.Data;
using BiluthyrningAB.Models.Entities;
using BiluthyrningAB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BiluthyrningAB.Controllers
{
    public class CustomersController : Controller
    {
        readonly CustomersService service;

        public CustomersController(CustomersService service)
        {
            this.service = service;
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

        [HttpPost]
        [Route("/GetCustomer")]
        public IActionResult GetCustomer([FromBody]CustomerSsnData dataModel)
        {
            CustomerDetailsData x = service.GetCustomerData(dataModel.Ssn);
            return Json(x);
        }
    }
}