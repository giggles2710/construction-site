using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _170516.Models.Administrator
{
    public class UpdateOrderModel
    {
        public UpdateOrderModel()
        {
            Shippers = new List<ViewShipperItem>();

            OrderDetails = new List<ViewOrderDetailsItem>();

            Statuses = new List<SelectListItem>();
            Statuses.Add(new SelectListItem { Text = "Đơn hàng đã được tạo", Value = ((int)OrderStatuses.OrderIsCreated).ToString() });
            Statuses.Add(new SelectListItem {Text = "Đang được xử lý", Value = ((int)OrderStatuses.OrderIsBeingProcessing).ToString() });
            Statuses.Add(new SelectListItem { Text = "Đã được chuyển đến", Value = ((int)OrderStatuses.OrderIsDelivered).ToString() });
            Statuses.Add(new SelectListItem { Text = "Đơn hàng đã hủy", Value = ((int)OrderStatuses.OrderIsCanceled).ToString() });
            Statuses.Add(new SelectListItem { Text = "Đơn hàng đã xong", Value = ((int)OrderStatuses.OrderIsFulfilled).ToString() });
            
            Fulfills = new List<SelectListItem>();            
            Fulfills.Add(new SelectListItem { Text = "Đã xong", Value = true.ToString() });
            Fulfills.Add(new SelectListItem { Text = "Chưa giao", Value = false.ToString()});
        }

        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string OrderNumber { get; set; }
        public System.DateTime OrderDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public Nullable<System.DateTime> ShipDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public Nullable<System.DateTime> RequiredDate { get; set; }
        public int ShipperID { get; set; }
        public string ShipperCompanyName { get; set; }
        [Required(ErrorMessage ="Vui lòng điền số tiền cước")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = false)]
        public double Freight { get; set; }
        [Required(ErrorMessage = "Vui lòng điền số tiền thuế")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = false)]
        public double SalesTax { get; set; }
        public string OrderStatus { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public double Paid { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public Nullable<System.DateTime> PaymentDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUserID { get; set; }
        public string ModifiedUserName { get; set; }

        public List<ViewOrderDetailsItem> OrderDetails { get; set; }

        public  List<ViewShipperItem> Shippers { get; set; }

        public List<SelectListItem> Statuses { get; set; }

        public List<SelectListItem> Fulfills { get; set; }
    }
}