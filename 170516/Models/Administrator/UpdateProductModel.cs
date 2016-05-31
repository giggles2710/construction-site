using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class UpdateProductModel
    {
        public UpdateProductModel()
        {
            this.SupplierList = new List<CreateProductSupplierListItem>();
            this.CategoryList = new List<CreateProductCategoryListItem>();
        }

        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductCategoryID { get; set; }
        public int ProductQuantity { get; set; }
        public double ProductWeight { get; set; }
        public double ProductDiscount { get; set; }
        public string ProductSize { get; set; }
        public int ProductSupplierID { get; set; }
        public double ProductPrice { get; set; }
        public string ProductUnit { get; set; }
        public string ProductImage { get; set; }

        // another information
        public List<CreateProductCategoryListItem> CategoryList { get; set; }
        public List<CreateProductSupplierListItem> SupplierList { get; set; }
    }
}