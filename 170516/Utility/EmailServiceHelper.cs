using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using _170516.Models;

namespace _170516.Utility
{
    public class EmailServiceHelper
    {
        private const string _defaultEmailFrom = "EMAIL_TRANSMISSION_FROM_EMAIL";
        private const string _defaultEmailName = "EMAIL_TRANSMISSION_FROM_NAME";
        private const string _defaultEmailPassword = "EMAIL_TRANSMISSION_FROM_PASSWORD";

        public static bool Send(EmailDeliveryModel ob)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 30000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("doanhhnqt74@gmail.com", "naythipass1235$");

            MailMessage mm = new MailMessage("doanhhnqt74@gmail.com", ob.SendTo);

            mm.IsBodyHtml = ob.IsBodyHtml;
            mm.Body = ob.Body;
            mm.Subject = ob.Subject;
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            try
            {
                client.Send(mm);

                ob.Status = (int) EmailStatus.Sent;
                ob.SentDate = DateTime.Now;
                return true;
            }
            catch (Exception ex)
            {
                ob.Retry += 1;
                ob.Status = (int) EmailStatus.Fail;
                ob.SentDate = DateTime.Now;
                ob.DetailFailure = ex.Message;
                return false;
            }
        }
    }

    public enum EmailStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Sent")]
        Sent = 2,
        [Description("Fail")]
        Fail = 3
    }
}