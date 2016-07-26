using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class CreateSystemVariableModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã thông tin")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên thông tin")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giá trị của thông tin")]
        public string Value { get; set; }
        public int VariableId { get; set; }
    }
}