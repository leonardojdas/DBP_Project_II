using System.Web;
using System.Web.Mvc;

namespace deAndrade_Project_II
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
