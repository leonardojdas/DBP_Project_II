using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deAndrade_Project_II.Models
{
    public class UpsertSalesStaffModel
    {
        public SalesStaff SalesStaff { get; set; }
        public List<Employee> Employees { get; set; }

        public bool isNull()
        {
            bool r = false;
            if (SalesStaff is null) r = true;
            return r;
        }
    }
}