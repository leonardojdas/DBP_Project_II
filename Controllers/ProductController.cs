using deAndrade_Project_II.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deAndrade_Project_II.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string id, int sortBy = 0, bool isDesc = false)
        {
            CompanyEntities context = new CompanyEntities();
            List<Product> products;

            switch (sortBy)
            {
                case 1:
                    {
                        products = isDesc ? context.Products.OrderByDescending(p => p.Name).ToList() :
                            context.Products.OrderBy(p => p.Name).ToList();
                        break;
                    }
                case 2:
                    {
                        products = isDesc ? context.Products.OrderByDescending(p => p.Description).ToList() :
                            context.Products.OrderBy(p => p.Description).ToList();
                        break;
                    }
                case 3:
                    {
                        products = isDesc ? context.Products.OrderByDescending(p => p.Price).ToList() :
                            context.Products.OrderBy(p => p.Price).ToList();
                        break;
                    }
                case 0:
                default:
                    products = isDesc ? context.Products.OrderByDescending(p => p.Id).ToList() :
                        context.Products.OrderBy(p => p.Id).ToList();
                    break;
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                products = products.Where(c =>
                    c.Id.ToString().ToLower().Contains(id) ||
                    c.Name.ToLower().Contains(id) ||
                    c.Description.ToLower().Contains(id) ||
                    c.Price.ToString().Contains(id)
                ).ToList();
            }

            return View(products);
        }

        /// Upsert Get: Product
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upsert(int id)
        {
            CompanyEntities context = new CompanyEntities();
            Product product = context.Products.Where(p => p.Id == id).FirstOrDefault();

            return View(product);
        }

        [HttpPost]
        public ActionResult Upsert(Product product)
        {
            Product newProduct = product;
            CompanyEntities context = new CompanyEntities();

            try
            {
                if (context.Products.Where(p => p.Id == newProduct.Id).Count() > 0)
                {
                    var productToSave = context.Products.Where(p => p.Id == newProduct.Id).ToList()[0];
                    productToSave.Name = newProduct.Name;
                    productToSave.Description = newProduct.Description;
                    productToSave.Price = newProduct.Price;
                }
                else
                {
                    context.Products.Add(newProduct);
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