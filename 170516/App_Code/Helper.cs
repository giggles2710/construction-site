using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace _170516.App_Code
{
    public static class Helper
    {
        public static string FormatCurrency(this decimal amount, string currencyCode)
        {
            var culture = (from c in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                           let r = new RegionInfo(c.LCID)
                           where r != null
                           && r.ISOCurrencySymbol.ToUpper() == currencyCode.ToUpper()
                           select c).FirstOrDefault();

            if (culture == null)
                return amount.ToString("0.00");

            return string.Format(culture, "{0:C}", amount);
        }
    }
}