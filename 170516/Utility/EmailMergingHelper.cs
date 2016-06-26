using _170516.Entities;
using _170516.Models;
using _170516.Models.Administrator;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace _170516.Utility
{
    public class EmailMergingHelper
    {
        protected static ConstructionSiteEntities dbContext = new ConstructionSiteEntities();

        public static EmailDeliveryModel MergeFeedbackEmail(AnswerRequestModel model)
        {
            var request = dbContext.Requests.FirstOrDefault(r => r.RequestID == model.RequestID);

            var replyContent = model.ReplyContent.Replace("[Tên]", request.FullName);
            replyContent = model.ReplyContent.Replace("[T&ecirc;n]", request.FullName);

            var companyName = dbContext.SystemVariables.Where(s => s.Name == "CompanyName").FirstOrDefault();

            if (companyName != null)
            {
                replyContent = replyContent.Replace("[Tên công ty]", companyName.Value);
                replyContent = replyContent.Replace("[T&ecirc;n c&ocirc;ng ty]", companyName.Value);
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