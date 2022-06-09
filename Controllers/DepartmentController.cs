using deAndrade_Project_II.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deAndrade_Project_II.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        public ActionResult List(string id, int sortBy = 0, bool isDesc = false)
        {
            CompanyEntities context = new CompanyEntities();
            List<Department> department;

            switch (sortBy)
            {
                case 1:
                    {
                        department = isDesc ? context.Departments.OrderByDescending(d => d.Name).ToList() :
                            context.Departments.OrderBy(d => d.Name).ToList();
                        break;
                    }
                case 2:
                    {
                        department = isDesc ? context.Departments.OrderByDescending(d => d.Code).ToList() :
                            context.Departments.OrderBy(d => d.Code).ToList();
                        break;
                    }
                case 3:
                    {
                        department = isDesc ? context.Departments.OrderByDescending(d => d.Num_Employees).ToList() :
                            context.Departments.OrderBy(d => d.Num_Employees).ToList();
                        break;
                    }
                case 0:
                default:
                    department = isDesc ? context.Departments.OrderByDescending(d => d.Id).ToList() :
                        context.Departments.OrderBy(d => d.Id).ToList();
                    break;
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                department = department.Where(d =>
                    d.Id.ToString().ToLower().Contains(id) ||
                    d.Name.ToLower().Contains(id) ||
                    d.Code.ToLower().Contains(id) ||
                    d.Num_Employees.ToString().Contains(id)
                ).ToList();
            }

            return View(department);
        }

        [HttpGet]
        public ActionResult Upsert(int id)
        {
            CompanyEntities context = new CompanyEntities();
            Department department = context.Departments.Where(p => p.Id == id).FirstOrDefault();

            // I created UpsertDepartmentModel just to get access to isNull() method
            UpsertDepartmentModel viewModel = new UpsertDepartmentModel()
            {
                Department = department
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Upsert(Department department)
        {
            Department newDepartment = department;
            CompanyEntities context = new CompanyEntities();

            try
            {
                // update
                if (context.Departments.Where(d => d.Id == department.Id).Count() > 0)
                {
                    var departmentToSave = context.Departments.Where(d => d.Id == newDepartment.Id).ToList()[0];

                    departmentToSave.Name = newDepartment.Name;
                    departmentToSave.Code = newDepartment.Code;
                    departmentToSave.Num_Employees = newDepartment.Num_Employees;
                }
                else
                {
                    context.Departments.Add(newDepartment);
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