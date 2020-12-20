using EmployeePayrollMVC.Models;
using EmployeePayrollMVC.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeePayrollMVC.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Retrive data from the data base
        /// </summary>
        /// <returns></returns>
        public ActionResult EmployeeList()
        {
            List<EmployeeViewModel> list = GetAllEmployee();
            return View(list);
        }
        public List<EmployeeViewModel> GetAllEmployee()
        {
            try
            {
                List<EmployeeViewModel> list = (from e in db.Employees
                                                join d in db.Departments on e.DepartmentId equals d.DeptId
                                                join s in db.Salaries on e.SalaryId equals s.SalaryId
                                                select new EmployeeViewModel
                                                {
                                                    EmpId = e.EmpId,
                                                    Name = e.Name,
                                                    Gender = e.Gender,
                                                    DepartmentId = d.DeptId,
                                                    Department = d.DeptName,
                                                    SalaryId = s.SalaryId,
                                                    Amount = s.Amount,
                                                    StartDate = e.StartDate,
                                                    Description = e.Description
                                                }).ToList<EmployeeViewModel>();

                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}