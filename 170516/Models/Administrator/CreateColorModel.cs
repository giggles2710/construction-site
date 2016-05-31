using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class CreateColorModel
    {
        public int ColorID { get; set; } // color id
        public string ColorName { get; set; }
        public string ColorDescription { get; set; }
        public string ColorBase64String { get; set; }
        public string Extension { get; set; }
    }
}