using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class CreateColorModel
    {
        [Required]
        [StringLength(50)]
        public string ColorName { get; set; }

        [Required]
        [StringLength(150)]
        public string ColorDescription { get; set; }

        [Required]
        public string ColorBase64String { get; set; }

        [Required]
        public string Extension { get; set; }
    }
}