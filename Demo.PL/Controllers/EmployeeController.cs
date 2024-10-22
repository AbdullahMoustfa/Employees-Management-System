using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IUnitOfWork _unitOfWork;

      //  private readonly IEmployeeRepository _unitOfWork.EmployeeRepository;
      //  private readonly IDepartmentRepoistory _departmentRepoistory;

        // Ask CLR For Creating an Object From IEmployeeRepository
        public EmployeeController(IUnitOfWork unitOfWork,
            IMapper mapper, 
            IWebHostEnvironment env /*IEmployeeRepository employeeRepository, 
             IDepartmentRepoistory departmentRepoistory*/ ) 
        {
            _mapper = mapper;
            _env = env;
            _unitOfWork = unitOfWork;
          ///  _unitOfWork.EmployeeRepository = employeeRepository;
          ///  _departmentRepoistory = departmentRepoistory;
        } 



        // [HttpGet]
        public async Task<IActionResult> Index(string searchInp)
        {
           /// 1.ViewData => KeyValue Pair [Dictionary Object]
           /// 2.Transfer Data From Controller [Action] To its Value 
           /// ViewData["Message"] = "Hello Ya Hamda With ViewData !!";
           ///
           /// 2.ViewBag => KeyValue Pair [Dictionary Object]
           /// Dynamic Property 
           /// Display on .Net 4.0
           ///
           /// ViewBag.Message = "Hello Ya Hamda With  ViewBag !!";
           
            var employees = Enumerable.Empty<Employee>();   

            if (String.IsNullOrEmpty(searchInp))
                employees = (IEnumerable<Employee>) await _unitOfWork.EmployeeRepository.GetAllAsync();
            else
                employees = _unitOfWork.EmployeeRepository.SearchByName(searchInp.ToLower());
            var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmps);  
        }

        // [HttpGet]
        public IActionResult Create()
        {
            //ViewBag.Departments = _departmentRepoistory.GetAll();   
            return View();
        }
         [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {

                /// Manual Mapping 
                ///  var mapEmp = new Employee
                ///  {
                ///   Name    = employeeVM.Name,
                ///   Age     = employeeVM.Age, 
                ///   Address = employeeVM.Address,  
                ///   Salary  = employeeVM.Salary,
                ///   Email   = employeeVM.Email,    
                ///   PhoneNumber = employeeVM.PhoneNumber,
                ///   IsActive = employeeVM.IsActive,    
                ///   HirirngDate = employeeVM.HirirngDate
                ///  }; 


                employeeVM.ImageName = await DocumentSettings.UploadFile(employeeVM.Image, "Images");
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM); 
                _unitOfWork.EmployeeRepository.AddAsync(mappedEmp);
                var count = await _unitOfWork.Compelete();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }
        //[HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)

                return BadRequest();    // 400

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee); 

            if (employee is null)

                return NotFound();     // 404

            return View(ViewName, mappedEmp);
        }

        //[HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            /// if (id is null)
            ///     return BadRequest();    // 400
            ///
            /// var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            ///
            /// if (employee is null)
            ///     return NotFound();     // 404
            ///
            /// return View(employee);

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (!ModelState.IsValid)
                return View(employeeVM);
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.EmployeeRepository.Update(mappedEmp);
                await _unitOfWork.Compelete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error has Occurred During ");

                return View(employeeVM);
            }
        }

        // /Employee/Delete/10 
        //[HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
        {
            if (!ModelState.IsValid)
                return View(employeeVM);
            try
            {
               var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Delete(mappedEmp);
                await _unitOfWork.Compelete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error has occurred ");

                return View(employeeVM);
            }
        }
    }
}
