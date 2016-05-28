using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class CreateProductModel
    {
        public CreateProductModel()
        {
            this.SupplierList = new List<CreateProductSupplierListItem>();
            this.AvailableColorList = new List<CreateProductColorListItem>();
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
        public double ProductUnit { get; set; }

        // another information
        public List<CreateProductCategoryListItem> CategoryList { get; set; }
        public List<CreateProductSupplierListItem> SupplierList { get; set; }
        public List<CreateProductColorListItem> AvailableColorList { get; set; }
    }

    public class CreateProductCategoryListItem
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }

    public class CreateProductSupplierListItem
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
    }

    public class CreateProductColorListItem
    {
        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public string RGB { get; set; }
    }
}