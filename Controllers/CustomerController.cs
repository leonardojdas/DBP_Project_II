using deAndrade_Project_II.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deAndrade_Project_II.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string id, int sortBy = 0, bool isDesc = false)
        {
            CompanyEntities context = new CompanyEntities();
            List<Customer> customers;

            switch (sortBy)
            {
                case 1:
                    {
                        customers = isDesc ? context.Customers.OrderByDescending(c => c.First_Name).ToList() :
                            context.Customers.OrderBy(c => c.First_Name).ToList();
                        break;
                    }
                case 2:
                    {
                        customers = isDesc ? context.Customers.OrderByDescending(c => c.Last_Name).ToList() :
                            context.Customers.OrderBy(c => c.Last_Name).ToList();
                        break;
                    }
                case 3:
                    {
                        customers = isDesc ? context.Customers.OrderByDescending(c => c.Email).ToList() :
                            context.Customers.OrderBy(c => c.Email).ToList();
                        break;
                    }
                case 4:
                    {
                        customers = isDesc ? context.Customers.OrderByDescending(c => c.Address).ToList() :
                            context.Customers.OrderBy(c => c.Address).ToList();
                        break;
                    }
                case 5:
                    {
                        customers = isDesc ? context.Customers.OrderByDescending(c => c.Creation_Date).ToList() :
                            context.Customers.OrderBy(c => c.Creation_Date).ToList();
                        break;
                    }
                case 0:
                default:
                    customers = isDesc ? context.Customers.OrderByDescending(c => c.Id).ToList() :
                        context.Customers.OrderBy(c => c.Id).ToList();
                    break;
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                customers = customers.Where(c =>
                    c.Id.ToString().ToLower().Contains(id) ||
                    c.First_Name.ToLower().Contains(id) ||
                    c.Last_Name.ToLower().Contains(id) ||
                    c.Email.ToLower().Contains(id) ||
                    c.Address.ToLower().Contains(id) ||
                    c.Creation_Date.ToString().Contains(id)
                ).ToList();
            }

            return View(customers);
        }

        [HttpGet]
        public ActionResult Upsert(int id)
        {
            CompanyEntities context = new CompanyEntities();
            Customer customer = context.Customers.Where(c => c.Id == id).FirstOrDefault();

            return View(customer);
        }

        [HttpPost]
        public ActionResult Upsert(Customer customer)
        {
            Customer newCustomer = customer;
            CompanyEntities context = new CompanyEntities();

            try
            {
                if (context.Customers.Where(c => c.Id == customer.Id).Count() > 0)
                {
                    var customerToSave = context.Customers.Where(c => c.Id == newCustomer.Id).ToList()[0];
                    customerToSave.First_Name = newCustomer.First_Name;
                    customerToSave.Last_Name = newCustomer.Last_Name;
                    customerToSave.Email = newCustomer.Email;
                    customerToSave.Address = newCustomer.Address;
                    customerToSave.Creation_Date = newCustomer.Creation_Date;
                }
                else
                {
                    context.Customers.Add(newCustomer);
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