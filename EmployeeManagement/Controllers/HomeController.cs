using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class HomeController:Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;
        public HomeController(IEmployeeRepository employeeRepository,IHostingEnvironment hostingEnvironment,ILogger logger)
        {
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        [AllowAnonymous]
        public ViewResult Index()
        {
            return View(this._employeeRepository.GetEmployees());
        }
        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            Employee employee = this._employeeRepository.GetEmployee(id.Value);
            if(employee==null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"
            };
           
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
                string fileUniqueName = ProcessUploadedFile(employeeCreateViewModel);
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
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel();
            employeeEditViewModel.Name = employee.Name;
            employeeEditViewModel.Email = employee.Email;
            employeeEditViewModel.Department = employee.Department;
            employeeEditViewModel.ExistingPhotoPath = employee.PhotoPath;
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel employeeEditViewModel)
        {
            if (ModelState.IsValid)
            {
                string fileUniqueName = ProcessUploadedFile(employeeEditViewModel);
                Employee employee = new Employee();
                employee.Id = employeeEditViewModel.Id;
                employee.Name = employeeEditViewModel.Name;
                employee.Email = employeeEditViewModel.Email;
                employee.Department = employeeEditViewModel.Department;
                if(employeeEditViewModel.Photo!=null)
                {
                    if (employeeEditViewModel.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "image", employeeEditViewModel.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = fileUniqueName;
                }
                else
                {
                    employee.PhotoPath = employeeEditViewModel.ExistingPhotoPath;
                }
                

                _employeeRepository.Update(employee);

                return RedirectToAction("Details", new { id = employee.Id });
            }
            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel employeeCreateViewModel)
        {
            string fileUniqueName = null;
            if (employeeCreateViewModel.Photo != null)
            {
                string uploadsFolder = Path.Combine(this._hostingEnvironment.WebRootPath, "image");
                fileUniqueName = Guid.NewGuid() + "_" + employeeCreateViewModel.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, fileUniqueName);
                using (var fileStream= new FileStream(filePath, FileMode.Create))
                {
                    employeeCreateViewModel.Photo.CopyTo(fileStream);
                }
                
            }

            return fileUniqueName;
        }
    }
}
