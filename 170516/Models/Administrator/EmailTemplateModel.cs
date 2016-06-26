using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _170516.Models.Administrator
{
    public class EmailTemplateModel
    {
        public int EmailTemplateId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên Email mẫu")]
        public string EmailTemplateName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string EmailSubject { get; set; }
        [AllowHtml]
        public string HtmlTextContent { get; set; }
        
        public string PlainTextContent { get; set; }

        public bool IsHTML { get; set; }
        public bool IsEnable { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }

        public List<string> MergeFields { get; set; } 
    }

    public enum FieldTypes
    {
        Common = 0,
        AcceptOrder = 1,
        ReplyRequest = 2
    }
}