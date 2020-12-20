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
        /// Register or Add multiple employee details to database
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegisterEmployee(RegisterEmpRequestModel employee)
        {
            bool result = false;
            if (ModelState.IsValid)
            {
                result = this.RegisterEmployeeService(employee);
            }
            ModelState.Clear();

            if (result == true)
            {
                return RedirectToAction("EmployeeList");
            }
            return View("Register", employee);
        }
        public bool RegisterEmployeeService(RegisterEmpRequestModel employee)
        {
            try
            {
                Employee validEmployee = db.Employees.Where(x => x.Name == employee.Name && x.Gender == employee.Gender).FirstOrDefault();
                if (validEmployee == null)
                {
                    int departmentId = db.Departments.Where(x => x.DeptName == employee.Department).Select(x => x.DeptId).FirstOrDefault();
                    Employee newEmployee = new Employee()
                    {
                        Name = employee.Name,
                        Gender = employee.Gender,
                        DepartmentId = departmentId,
                        SalaryId = Convert.ToInt32(employee.SalaryId),
                        StartDate = employee.StartDate,
                        Description = employee.Description
                    };
                    Employee returnData = db.Employees.Add(newEmployee);
                }
                int result = db.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
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
        /// <summary>
        /// retrive the data from database and display in the edit webpage
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ActionResult Edit(EmployeeViewModel item)
        {
            RegisterEmpRequestModel emp = new RegisterEmpRequestModel
            {
                EmpId = item.EmpId,
                Name = item.Name,
                Gender = item.Gender,
                Department = item.Department,
                SalaryId = item.SalaryId.ToString(),
                StartDate = item.StartDate,
                Description = item.Description
            };

            return View(emp);
        }
        /// <summary>
        /// Edit the existing employee details and update the data in database
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public ActionResult EditEmployee(RegisterEmpRequestModel employee)
        {
            bool result = EditEmployeeService(employee);
            if (result == true)
            {
                List<EmployeeViewModel> list = GetAllEmployee();
                return View("EmployeeList", list);
            }
            else
            {
                return View("Error");
            }
        }
        public bool EditEmployeeService(RegisterEmpRequestModel employee)
        {
            try
            {
                int departmentId = db.Departments.Where(x => x.DeptName == employee.Department).Select(x => x.DeptId).FirstOrDefault();

                Employee emp = db.Employees.Find(employee.EmpId);
                emp.Name = employee.Name;
                emp.SalaryId = Convert.ToInt32(employee.SalaryId);
                emp.StartDate = employee.StartDate;
                emp.Description = employee.Description;
                emp.Gender = employee.Gender;
                emp.DepartmentId = departmentId;

                //db.Entry(emp).State = EntityState.Modified;

                int result = db.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Delete Employee details from the data base
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            bool result = DeleteEmployee(id);
            if (result == true)
            {
                List<EmployeeViewModel> list = GetAllEmployee();
                return View("EmployeeList", list);
            }
            else
            {
                return View("Error");
            }
        }
        public bool DeleteEmployee(int id)
        {
            try
            {
                Employee employee = db.Employees.Find(id);
                if (employee == null)
                {
                    return false;
                }
                db.Employees.Remove(employee);
                int result = db.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}