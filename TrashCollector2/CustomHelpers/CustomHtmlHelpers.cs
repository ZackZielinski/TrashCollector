using System.Web.Mvc;

namespace TrashCollector2.CustomHelpers
{
    public static class CustomHtmlHelpers
    {
        public static MvcHtmlString PickupStatus(this HtmlHelper helper, bool condition)
        {
            var result = condition ? "Picked Up" : "En Route";
            return new MvcHtmlString(result);
        }
        public static MvcHtmlString VacationStatus(this HtmlHelper helper, bool condition)
        {
            var result = condition ? "On Holiday" : "Not On Holiday";
            return new MvcHtmlString(result);
        }
    }
}