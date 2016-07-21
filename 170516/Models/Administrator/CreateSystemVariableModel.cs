using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class CreateSystemVariableModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int VariableId { get; set; }
    }
}