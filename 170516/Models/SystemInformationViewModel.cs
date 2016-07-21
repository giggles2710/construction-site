using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models
{
    public class SystemInformationViewModel
    {
        // cart information
        public int CartCount { get; set; }
        public List<SystemVariableModel> SystemVariables { get; set; }
    }
}