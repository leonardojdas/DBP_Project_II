using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deAndrade_Project_II.Models
{
    public class UpsertDepartmentModel
    {
        public Department Department { get; set; }

        public bool isNull()
        {
            bool r = false;
            if (Department is null) r = true;
            return r;
        }
    }
}