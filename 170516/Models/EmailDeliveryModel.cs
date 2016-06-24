using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models
{
    public class EmailDeliveryModel
    {
        public int EmailDeliveryId { get; set; }
        public string SendTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int? EmailTemplateId { get; set; }
        public int RequestId { get; set; }
        public int Status { get; set; }
        public bool IsBodyHtml { get; set; }
        public int? Retry { get; set; }
        public string DetailFailure { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public byte[] AttachmentFile { get; set; }        
    }
}