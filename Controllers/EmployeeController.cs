using deAndrade_Project_II.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deAndrade_Project_II.Controllers
{
    public class EmployeeController : Controller
    {
        // List Get: Employee
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string id, int sortBy = 0, bool isDesc = false)
        {
            CompanyEntities context = new CompanyEntities();
            List<Employee> employee;

            switch (sortBy)
            {
                case 1:
                    {
                        employee = isDesc ? context.Employees.OrderByDescending(e => e.First_Name).ToList() :
                            context.Employees.OrderBy(e => e.First_Name).ToList();
                        break;
                    }
                case 2:
                    {
                        employee = isDesc ? context.Employees.OrderByDescending(e => e.Last_Name).ToList() :
                            context.Employees.OrderBy(e => e.Last_Name).ToList();
                        break;
                    }
                case 3:
                    {
                        employee = isDesc ? context.Employees.OrderByDescending(e => e.Email).ToList() :
                            context.Employees.OrderBy(e => e.Email).ToList();
                        break;
                    }
                case 4:
                    {
                        employee = isDesc ? context.Employees.OrderByDescending(e => e.Department.Name).ToList() :
                            context.Employees.OrderBy(e => e.Department.Name).ToList();
                        break;
                    }
                case 0:
                default:
                    employee = isDesc ? context.Employees.OrderByDescending(d => d.Id).ToList() :
                        context.Employees.OrderBy(d => d.Id).ToList();
                    break;
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                employee = employee.Where(e =>
                    e.Id.ToString().ToLower().Contains(id) ||
                    e.First_Name.ToLower().Contains(id) ||
                    e.Last_Name.ToLower().Contains(id) ||
                    e.Email.ToLower().Contains(id) ||
                    e.Department.Name.ToLower().Contains(id)
                ).ToList();
            }

            return View(employee);
        }

        // Upsert Get: Employee
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upsert(int id)
        {
            CompanyEntities context = new CompanyEntities();
            Employee employee = context.Employees.Where(e => e.Id == id).FirstOrDefault();
            List<Department> departments = context.Departments.ToList();
            SalesStaff salesStaff = context.SalesStaffs.Where(s => s.Employee_Id == id).FirstOrDefault();

            UpsertEmployeeModel viewModel = new UpsertEmployeeModel()
            {
                Employee = employee,
                Departments = departments,
                SalesStaff = salesStaff
            };

            return View(viewModel);
        }

        // Upsert Post: Employee
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upsert(UpsertEmployeeModel model)
        {
            Employee newEmployee = model.Employee;
            CompanyEntities context = new CompanyEntities();
            List<Department> departments = context.Departments.ToList();

            try
            {
                var newDepartment = context.Departments.Where(d => d.Id == newEmployee.Department_Id).ToList()[0];

                // get the name of the new employee department, since the newEmployee model has only its ID
                string newEmployeeDepartment = (context.Departments.Where(d => d.Id == newEmployee.Department_Id).ToList()[0]).Name;

                // update
                if (context.Employees.Where(e => e.Id == newEmployee.Id).Count() > 0)
                {
                    // define employee fields
                    var employeeToSave = context.Employees.Where(e => e.Id == newEmployee.Id).ToList()[0];
                    employeeToSave.First_Name = newEmployee.First_Name;
                    employeeToSave.Last_Name = newEmployee.Last_Name;
                    employeeToSave.Email = newEmployee.Email;

                    // update the object with the old department
                    var oldDepartment = context.Departments.Where(d => d.Id == employeeToSave.Department_Id).ToList()[0];

                    // verifies the employee's department, if SALES update the relational table SalesStaff
                    if (employeeToSave.Department.Name.ToUpper() == "SALES")
                    {
                        SalesStaff newSalesStaff = model.SalesStaff;
                        var salesStaffToSave = context.SalesStaffs.Where(s => s.Employee_Id == newEmployee.Id).ToList()[0];

                        // update an existent employee to salestaff
                        if (newEmployee.Department_Id == 2)
                        {
                            salesStaffToSave.Full_Time = newSalesStaff.Full_Time;
                            salesStaffToSave.Hire_Date = newSalesStaff.Hire_Date;
                            salesStaffToSave.Salary = newSalesStaff.Salary;
                        }
                        // delete an existent employee from SalesStaff
                        else
                        {
                            context.SalesStaffs.Remove(salesStaffToSave);

                            // delete sales from this SalesStaff
                            var salesToDelete = context.Sales.Where(s => s.SalesStaff.Employee_Id == newEmployee.Id).ToList();

                            foreach (var item in salesToDelete)
                            {
                                context.Sales.Remove(item);
                            }
                        }

                    }
                    // insert an existent employee to SalesStaff
                    else if (newEmployeeDepartment.ToUpper() == "SALES")
                    {
                        SalesStaff newSalesStaff = model.SalesStaff;
                        context.SalesStaffs.Add(newSalesStaff);
                    }

                    employeeToSave.Department_Id = newEmployee.Department_Id;

                    // update Departments table -> number of employees
                    oldDepartment.Num_Employees--;
                }
                else
                {
                    // insert a new employee to SalesStaff
                    if (newEmployeeDepartment.ToUpper() == "SALES")
                    {
                        SalesStaff newSalesStaff = model.SalesStaff;
                        context.SalesStaffs.Add(newSalesStaff);
                    }
                    context.Employees.Add(newEmployee);
                }

                // update Departments table -> number of employees
                newDepartment.Num_Employees++;

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