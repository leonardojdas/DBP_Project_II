using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deAndrade_Project_II.Models
{
    public class UpsertEmployeeModel
    {
        public Employee Employee { get; set; }
        public List<Department> Departments { get; set; }
        public SalesStaff SalesStaff { get; set; }

        public bool isNull() {
            bool r = false;
            if (Employee is null) r = true;
            return r;
        }
    }
}