using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deAndrade_Project_II.Models
{
    public class UpsertSaleModel
    {
        public Sale Sale { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Product> Products { get; set; }
        public List<SalesStaff> SalesStaffs { get; set; }

        public bool isNull()
        {
            bool r = false;
            if (Sale is null) r = true;
            return r;
        }
    }
}