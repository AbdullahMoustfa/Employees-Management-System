using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize]

	public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly IDepartmentRepoistory _unitOfWork.DepartmentRepoistory;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IUnitOfWork unitOfWork,IMapper mapper, IWebHostEnvironment env /*IDepartmentRepoistory departmentRepoistory*/)
            // Ask CLR for Creating Object from IDepartmentRepository which this Interface Ask The Class DepartmentRepoisotry To Create an Object
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
            //_unitOfWork.DepartmentRepoistory = departmentRepoistory;
        }

        // /Department/Index 
        //[HttpGet]
        public async Task<IActionResult> Index()
        {
            var departments =await _unitOfWork.DepartmentRepoistory.GetAllAsync();
            var mappedDpts = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);

            return View(mappedDpts);
        }

        //[HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var mappedDpt = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

              await _unitOfWork.DepartmentRepoistory.AddAsync(mappedDpt);
                int count = await _unitOfWork.Compelete();
               
                if (count > 0)
                {
                    TempData["Message"] = "Department Is Created !!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View (departmentVM);   
        }

        // /Department/Details/10 
        // / Department/Details 
        //[HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName="Details")
        {
            if (id is null)
                return BadRequest(); // 400

            var department =await _unitOfWork.DepartmentRepoistory.GetAsync(id.Value);

            var mappedDpt = _mapper.Map<Department, DepartmentViewModel>(department);

            if (department is null)
                return NotFound();    // 404

            return View(viewName, mappedDpt);    
        }

        // /Department/Edit/10 
        // / Department/Edit 
       // [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
          /// if (!id.HasValue)
          ///     return BadRequest();  // 400
          ///
          /// var department = _unitOfWork.DepartmentRepoistory.Get(id.Value);
          ///
          /// if (department is null)
          ///     return NotFound();    // 404
          /// return View(department);   
          
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id,  DepartmentViewModel departmentVM)
        {

            if (!ModelState.IsValid)
                return View (departmentVM);

            try
            {
                var mappedDpt = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                _unitOfWork.DepartmentRepoistory.Update(mappedDpt);
                await _unitOfWork.Compelete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception => يعني لما اليوزر يدخل داتا مخالفة اللي انا محددها هيبقى فيه اكسبشن ف فايدة اللوج هو انها بتسجل ايه الداتا اللي دخلت علشان تعرف خدمة الدعم ازاي تصلحها
                // 2. Friendly Messege  => ابعت رسالة في الفورم لليوزر بتقوله اللي بتعمله ده غلط وغير مسموح بيه 
                if (_env.IsDevelopment())   
                    ModelState.AddModelError(string .Empty, ex.Message);    
                else
                    ModelState.AddModelError(string.Empty, "An Error has occurred During Updating Department");

                return View(departmentVM);    
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            /// if (!id.HasValue)
            ///     return BadRequest();  // 400
            ///
            /// var department = _unitOfWork.DepartmentRepoistory.Get(id.Value);
            ///
            /// if (department is null)
            ///     return NotFound();    // 404
            /// return View(department);   

            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DepartmentViewModel departmentVM)
        {
            if(!ModelState.IsValid) 
                return View(departmentVM);

            try
            {
                var mappedDpt = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                _unitOfWork.DepartmentRepoistory.Delete(mappedDpt);
               await _unitOfWork.Compelete();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                if(_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error has occurred ");

                return View(departmentVM);
            }
        }
    }
}
