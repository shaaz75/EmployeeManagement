using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController:Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [Route("Home/Index")]
        public ViewResult Index()
        {
            return View(this._employeeRepository.GetEmployees());
        }
        [Route("Home/Details/{id?}")]
        public ViewResult Details(int? id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel();
            homeDetailsViewModel.Employee = this._employeeRepository.GetEmployee(id??1);
            homeDetailsViewModel.PageTitle = "Details Page";
            return View(homeDetailsViewModel);
        }
    }
}
