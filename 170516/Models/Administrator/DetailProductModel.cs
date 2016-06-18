using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class DetailProductModel
    {
        public string Summary { get; set; }
        public string ImageSrc { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int QuantityUnit { get; set; }
        public double Weight { get; set; }
        public double Discount { get; set; }
        public string Size { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public double Price { get; set; }
        public string ProductUnit { get; set; }
        public string Description { get; set; }
        public DateTime DateModified { get; set; }
        public string UserModified { get; set; }

        public List<SpecificationsTableModel> SpecificationList { get; set; }
    }
}