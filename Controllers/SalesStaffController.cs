using deAndrade_Project_II.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deAndrade_Project_II.Controllers
{
    public class SalesStaffController : Controller
    {
        // List Get: SalesStaff
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string id, int sortBy = 0, bool isDesc = false)
        {
            CompanyEntities context = new CompanyEntities();
            List<SalesStaff> salesStaff;

            switch (sortBy)
            {
                case 1:
                    {
                        salesStaff = isDesc ? context.SalesStaffs.OrderByDescending(s => s.Employee.First_Name + " " + s.Employee.Last_Name).ToList() :
                            context.SalesStaffs.OrderBy(s => s.Employee.First_Name + " " + s.Employee.Last_Name).ToList();
                        break;
                    }
                case 2:
                    {
                        salesStaff = isDesc ? context.SalesStaffs.OrderByDescending(s => s.Full_Time).ToList() :
                            context.SalesStaffs.OrderBy(s => s.Full_Time).ToList();
                        break;
                    }
                case 3:
                    {
                        salesStaff = isDesc ? context.SalesStaffs.OrderByDescending(s => s.Hire_Date).ToList() :
                            context.SalesStaffs.OrderBy(s => s.Hire_Date).ToList();
                        break;
                    }
                case 4:
                    {
                        salesStaff = isDesc ? context.SalesStaffs.OrderByDescending(s => s.Salary).ToList() :
                            context.SalesStaffs.OrderBy(s => s.Salary).ToList();
                        break;
                    }
                case 0:
                default:
                    salesStaff = isDesc ? context.SalesStaffs.OrderByDescending(d => d.Id).ToList() :
                        context.SalesStaffs.OrderBy(d => d.Id).ToList();
                    break;
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                salesStaff = salesStaff.Where(s =>
                    s.Id.ToString().ToLower().Contains(id) ||
                    (s.Employee.First_Name + " " + s.Employee.Last_Name).ToLower().Contains(id) ||
                    s.Full_Time.ToString().Contains(id) ||
                    s.Hire_Date.ToString().Contains(id) ||
                    s.Salary.ToString().Contains(id)
                ).ToList();
            }

            return View(salesStaff);
        }

        /// Upsert Get: SalesStaff
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upsert(int id)
        {
            CompanyEntities context = new CompanyEntities();
            SalesStaff salesStaff = context.SalesStaffs.Where(s => s.Id == id).FirstOrDefault();
            List<Employee> employees = context.Employees.Where(e => e.Department.Name.ToUpper() != "SALES").ToList();

            UpsertSalesStaffModel viewModel = new UpsertSalesStaffModel()
            {
                SalesStaff = salesStaff,
                Employees = employees
            };

            return View(viewModel);
        }

        /// Upsert Post: SalesStaff
        /// <summary>
        /// 
        /// </summary>
        [HttpPost]
        public ActionResult Upsert(SalesStaff salesStaff)
        {
            SalesStaff newSalesStaff = salesStaff;
            CompanyEntities context = new CompanyEntities();

            try
            {
                // update
                if (context.SalesStaffs.Where(s => s.Id == salesStaff.Id).Count() > 0)
                {
                    var salesStaffToSave = context.SalesStaffs.Where(s => s.Id == newSalesStaff.Id).ToList()[0];

                    salesStaffToSave.Full_Time = newSalesStaff.Full_Time;
                    salesStaffToSave.Hire_Date = newSalesStaff.Hire_Date;
                    salesStaffToSave.Salary = newSalesStaff.Salary;
                }
                else
                {
                    context.SalesStaffs.Add(newSalesStaff);

                    // get sales department id
                    int salesDepartmentId = (context.Departments.Where(d => d.Name.ToUpper() == "SALES").ToList()[0]).Id;

                    // get employee to update
                    var employeeToSave = context.Employees.Where(e => e.Id == salesStaff.Employee_Id).ToList()[0];

                    // get old department id
                    int oldDepartmentId = employeeToSave.Department_Id;

                    // update employee table -> department id
                    employeeToSave.Department_Id = salesDepartmentId;

                    // get department to update
                    var oldDepartmentToSave = context.Departments.Where(d => d.Id == oldDepartmentId).ToList()[0];
                    oldDepartmentToSave.Num_Employees--;

                    var newDepartmentToSave = context.Departments.Where(d => d.Id == salesDepartmentId).ToList()[0];
                    newDepartmentToSave.Num_Employees++;
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("List");
        }
    }
}