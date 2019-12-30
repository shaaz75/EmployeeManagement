using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController:Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        public HomeController(IEmployeeRepository employeeRepository,IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        public ViewResult Index()
        {
            return View(this._employeeRepository.GetEmployees());
        }
        public ViewResult Details(int? id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel();
            homeDetailsViewModel.Employee = this._employeeRepository.GetEmployee(id??1);
            homeDetailsViewModel.PageTitle = "Details Page";
            return View(homeDetailsViewModel);
        }
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel employeeCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                string fileUniqueName = null;
                if(employeeCreateViewModel.Photo!=null)
                {
                    string uploadsFolder = Path.Combine(this._hostingEnvironment.WebRootPath, "image");
                    fileUniqueName = Guid.NewGuid() + "_" + employeeCreateViewModel.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, fileUniqueName);
                    employeeCreateViewModel.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Employee employee = new Employee();
                employee.Name = employeeCreateViewModel.Name;
                employee.Email = employeeCreateViewModel.Email;
                employee.Department = employeeCreateViewModel.Department;
                employee.PhotoPath = fileUniqueName;

                _employeeRepository.Add(employee);

                return RedirectToAction("Details", new { id = employee.Id });
            }
            return View();
        }
    }
}
