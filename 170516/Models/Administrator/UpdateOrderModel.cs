﻿using System;
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

            Statuses = new List<SelectListItem>();
            Statuses.Add(new SelectListItem {Text = "Đang được xử lý", Value = Constant.OrderIsProcessingtatus });
            Statuses.Add(new SelectListItem { Text = "Đã được chuyển đến", Value = Constant.OrderDeliveredStatus });
            Statuses.Add(new SelectListItem { Text = "Đơn hàng đã hủy", Value = Constant.OrderCanceledStatus });
            Statuses.Add(new SelectListItem { Text = "Đơn hàng đã xong", Value = Constant.OrderFulfilledStatus });
        }

        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string OrderNumber { get; set; }
        public System.DateTime OrderDate { get; set; }
        public System.DateTime ShipDate { get; set; }
        public System.DateTime RequiredDate { get; set; }
        public int ShipperID { get; set; }
        public string ShipperCompanyName { get; set; }
        [Required(ErrorMessage ="Vui lòng điền số tiền cước")]
        public double Freight { get; set; }
        [Required(ErrorMessage = "Vui lòng điền số tiền thuế")]
        public decimal SalesTax { get; set; }
        public string OrderStatus { get; set; }
        public bool IsFulfilled { get; set; }
        public bool IsCanceled { get; set; }
        public Nullable<decimal> Paid { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUserID { get; set; }
        public string ModifiedUserName { get; set; }

        public  List<ViewShipperItem> Shippers { get; set; }

        public List<SelectListItem> Statuses { get; set; }
    }
}