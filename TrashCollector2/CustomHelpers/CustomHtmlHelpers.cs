using System.Web.Mvc;

namespace TrashCollector2.CustomHelpers
{
    public static class CustomHtmlHelpers
    {
        public static MvcHtmlString PickupStatus(this HtmlHelper helper, bool condition)
        {
            var result = condition ? "Pickup Collected" : "Not collected";
            return new MvcHtmlString(result);
        }
        public static MvcHtmlString VacationStatus(this HtmlHelper helper, bool condition)
        {
            var result = condition ? "On Holiday" : "Not On Holiday";
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString DateFormat(this HtmlHelper helper, string Date)
        {
            var result = string.Format("{0:d}", Date);
            return new MvcHtmlString(result);
        }
    }
}