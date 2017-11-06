using System.Web.Mvc;

namespace TrashCollector2.CustomHelpers
{
    public static class CustomHtmlHelpers
    {
        public static MvcHtmlString PickupStatus(this HtmlHelper helper, bool Condition)
        {
            string result = Condition ? "Pickup Collected" : "Not collected";
            return new MvcHtmlString(result);
        }
        public static MvcHtmlString VacationStatus(this HtmlHelper helper, bool Condition)
        {
            string result = Condition ? "On Holiday" : "Not On Holiday";
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString DisplayDate(this HtmlHelper helper, string Date)
        {
            string result = string.Format("{0:d}", Date);
            return new MvcHtmlString(result);
        }

        public static MvcHtmlString DisplayPayment(this HtmlHelper helper, float Payment)
        {
            string result = Payment.ToString("N2");
            return new MvcHtmlString(result);
        }
    }
}