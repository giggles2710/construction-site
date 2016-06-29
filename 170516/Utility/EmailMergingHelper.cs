using _170516.Entities;
using _170516.Models;
using _170516.Models.Administrator;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Web;

namespace _170516.Utility
{
    public class EmailMergingHelper
    {
        protected static ConstructionSiteEntities dbContext = new ConstructionSiteEntities();

        public static EmailDeliveryModel MergeOrderConfirmationEmail(int orderId)
        {
            var emailTemp = dbContext.EmailTemplates.FirstOrDefault(e => e.EmailType == (int)FieldTypes.OrderConfirmation);
            var order = dbContext.Orders.FirstOrDefault(o => o.OrderID == orderId);

            if (emailTemp == null || order == null)
            {
                return null;
            }

            string content = emailTemp.HtmlBody;
            
            content = content.Replace(Constant.CustomerNameField, order.Customer.Fullname);
            content = content.Replace(Constant.CustomerEmailField, order.Customer.EmailAddress);
            content = content.Replace(Constant.CustomerPhoneField, order.Customer.Phone);
            content = content.Replace(Constant.CustomerAddressField, order.Customer.Address);
            content = content.Replace(Constant.CustomerShipToAddressField, order.Customer.ShipAddress);

            var companyName = dbContext.SystemVariables.Where(s => s.Name == "CompanyName").FirstOrDefault();
            var companyWebsite = dbContext.SystemVariables.Where(s => s.Name == "CompanyWebsite").FirstOrDefault();

            if (companyName != null)
            {
                content = content.Replace(Constant.CompanyNameField, companyName.Value);
            }

            if (companyWebsite != null)
            {
                content = content.Replace(Constant.CompanyNameField, companyWebsite.Value);
            }

            content = content.Replace(Constant.OrderNumberField, order.OrderNumber);
            content = content.Replace(Constant.OrderDateField, order.OrderDate.ToString());

            //order detail
            StringBuilder orderDetails = new StringBuilder();
            orderDetails.Append("<table border=\"1px\" style = \"width: 469px;\" cellpadding = \"0\">");
            orderDetails.Append("<tbody>");

            decimal total = 0;
            foreach (var item in order.OrderDetails)
            {
                orderDetails.Append("<tr>");
                orderDetails.Append("<td style = \"width: 103px;\"><img class=\" src=\"http://htmlcoder.me/preview/the_project/v.1.2/template/images/logo_light_blue.png\" height =\"115\"/></td>");
                orderDetails.Append("<td style = \"width: 277px;\">");
                orderDetails.Append(string.Format("<p><a href=\"\">{0}</a></p>", item.Product.Name));
                orderDetails.Append("<p>&nbsp;</p>");
                orderDetails.Append("</td>");
                orderDetails.Append(string.Format("<td style = \"width: 77px;\" ><strong>{0} đ</strong></td>", item.Total.ToString("0.##")));
                orderDetails.Append("</tr>");
                total += item.Price * item.Quantity;
            }

            orderDetails.Append("</tbody>");
            orderDetails.Append("</table>");

            content = content.Replace(Constant.OrderDetailsField, orderDetails.ToString());

            //total order
            decimal totalHaveToPaid = total + (decimal)order.Freight + order.SalesTax;

            StringBuilder summaryOrder = new StringBuilder();
            summaryOrder.Append("<table style=\"height: 92px; width: 472px; \">");
            summaryOrder.Append("<tbody>");
            summaryOrder.Append("<tr>");
            summaryOrder.Append("<td style=\"width: 383px; text - align: right; \" > Tổng tiền cho c&aacute;c sản phẩm:</td>");
            summaryOrder.Append(string.Format("<td style=\"width: 73px; \" >{0} đ</td>", total.ToString("0.##")));
            summaryOrder.Append("</tr>");
            summaryOrder.Append("<tr>");
            summaryOrder.Append("<td style=\"width: 383px; text - align: right; \" > Cước vận chuyển:</td>");
            summaryOrder.Append(string.Format("<td style=\"width: 73px; \" >{0} đ</td>", order.Freight));
            summaryOrder.Append("</tr>");
            summaryOrder.Append("<tr>");
            summaryOrder.Append("<td style=\"width: 383px; text - align: right; \" > Thuế:</td>");
            summaryOrder.Append(string.Format("<td style=\"width: 73px; \" >{0} đ</td>", order.SalesTax.ToString("0.##")));
            summaryOrder.Append("</tr>");
            summaryOrder.Append("<tr>");
            summaryOrder.Append("<td style=\"width: 383px; text - align: right; \" ><strong>Tổng phải trả:</strong></td>");
            summaryOrder.Append(string.Format("<td style=\"width: 73px; \" ><strong>{0} đ</strong></td>", totalHaveToPaid.ToString("0.##")));
            summaryOrder.Append("</tr>");
            summaryOrder.Append("<tr>");
            summaryOrder.Append("<td style=\"width: 383px; \" > &nbsp;</td>");
            summaryOrder.Append("</tr>");
            summaryOrder.Append("</tbody>");
            summaryOrder.Append("</table>");

            content = content.Replace(Constant.OrderSummaryField, summaryOrder.ToString());

            var model = new EmailDeliveryModel {
                IsBodyHtml = true,
                Body = content,
                Subject = emailTemp.EmailSubject,
                SendTo = order.Customer.EmailAddress,
                Status = (int)EmailStatus.Pending
            };

            return model;
        }

        public static EmailDeliveryModel MergeFeedbackEmail(AnswerRequestModel model)
        {
            var request = dbContext.Requests.FirstOrDefault(r => r.RequestID == model.RequestID);

            var replyContent = model.ReplyContent.Replace("[Tên]", request.FullName);
            replyContent = model.ReplyContent.Replace("[T&ecirc;n]", request.FullName);

            var companyName = dbContext.SystemVariables.Where(s => s.Name == "CompanyName").FirstOrDefault();

            if (companyName != null)
            {                
                replyContent = replyContent.Replace(Constant.CompanyNameField, companyName.Value);
            }
            
            return new EmailDeliveryModel
            {
                IsBodyHtml = true,
                Body = replyContent,
                Subject = model.ReplySubject,
                SendTo = request.EmailAddress,
                Status = (int) EmailStatus.Pending
            };
        }


    }
}