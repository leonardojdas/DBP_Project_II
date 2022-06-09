using deAndrade_Project_II.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deAndrade_Project_II.Controllers
{
    public class SaleController : Controller
    {
        // List Get: Sale
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string id, int sortBy = 0, bool isDesc = false)
        {
            CompanyEntities context = new CompanyEntities();
            List<Sale> sales;

            switch (sortBy)
            {
                case 1:
                    {
                        sales = isDesc ? context.Sales.OrderByDescending(s => s.Customer.First_Name + " " + s.Customer.Last_Name).ToList() :
                            context.Sales.OrderBy(s => s.Customer.First_Name + " " + s.Customer.Last_Name).ToList();
                        break;
                    }
                case 2:
                    {
                        sales = isDesc ? context.Sales.OrderByDescending(s => s.Amount).ToList() :
                            context.Sales.OrderBy(s => s.Amount).ToList();
                        break;
                    }
                case 3:
                    {
                        sales = isDesc ? context.Sales.OrderByDescending(s => s.Product.Price).ToList() :
                            context.Sales.OrderBy(s => s.Product.Price).ToList();
                        break;
                    }
                case 4:
                    {
                        sales = isDesc ? context.Sales.OrderByDescending(s => (s.Amount * s.Product.Price)).ToList() :
                            context.Sales.OrderBy(s => (s.Amount * s.Product.Price)).ToList();
                        break;
                    }
                case 5:
                    {
                        sales = isDesc ? context.Sales.OrderByDescending(s => s.SalesStaff.Employee.First_Name + " " + s.SalesStaff.Employee.Last_Name).ToList() :
                            context.Sales.OrderBy(s => s.SalesStaff.Employee.First_Name + " " + s.SalesStaff.Employee.Last_Name).ToList();
                        break;
                    }
                case 0:
                default:
                    sales = isDesc ? context.Sales.OrderByDescending(p => p.Id).ToList() :
                        context.Sales.OrderBy(p => p.Id).ToList();
                    break;
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                sales = sales.Where(s =>
                    s.Id.ToString().ToLower().Contains(id) ||
                    (s.Customer.First_Name + " " + s.Customer.Last_Name).ToLower().Contains(id) ||
                    s.Product.Name.ToLower().Contains(id) ||
                    s.Amount.ToString().Contains(id) ||
                    s.Product.Price.ToString().Contains(id) ||
                    (s.Amount * s.Product.Price).ToString().Contains(id) ||
                    (s.SalesStaff.Employee.First_Name + " " + s.SalesStaff.Employee.Last_Name).ToLower().Contains(id)
                ).ToList();
            }

            return View(sales);
        }

        /// Upsert Get: Sale
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upsert(int id)
        {
            CompanyEntities context = new CompanyEntities();
            Sale sale = context.Sales.Where(s => s.Id == id).FirstOrDefault();
            List<Customer> customers = context.Customers.ToList();
            List<Product> products = context.Products.ToList();
            List<SalesStaff> salesStaffs = context.SalesStaffs.ToList();

            UpsertSaleModel viewModel = new UpsertSaleModel()
            {
                Sale = sale,
                Customers = customers,
                Products = products,
                SalesStaffs = salesStaffs
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Upsert(Sale sale)
        {
            Sale newSale = sale;
            CompanyEntities context = new CompanyEntities();

            try
            {
                // update
                if (context.Sales.Where(s => s.Id == sale.Id).Count() > 0)
                {
                    var saleToSave = context.Sales.Where(s => s.Id == newSale.Id).ToList()[0];

                    saleToSave.Date = newSale.Date;
                    saleToSave.Amount = newSale.Amount;
                    saleToSave.Customer_Id = newSale.Customer_Id;
                    saleToSave.Product_Id = newSale.Product_Id;
                    saleToSave.SaleStaff_Id = newSale.SaleStaff_Id;
                }
                else
                {
                    context.Sales.Add(newSale);
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