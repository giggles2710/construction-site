using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class SpecificationsTableModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public static string GetTypeNameStatic(int typeCode)
        {
            foreach (var specification in Constant.SpecificationType)
            {
                if (specification.Item1 == typeCode)
                {
                    return specification.Item2;
                }
            }

            return string.Empty;
        }

        public int GetTypeCode()
        {
            foreach (var specification in Constant.SpecificationType)
            {
                if (specification.Item2.Equals(Name))
                {
                    return specification.Item1;
                }
            }

            return 0;
        }

        public int GetTypeCode(string typeName)
        {
            foreach(var specification in Constant.SpecificationType)
            {
                if (specification.Item2.Equals(typeName))
                {
                    return specification.Item1;
                }
            }

            return 0;
        }

        public string GetTypeName(int typeCode)
        {
            foreach (var specification in Constant.SpecificationType)
            {
                if (specification.Item1 == typeCode)
                {
                    return specification.Item2;
                }
            }

            return string.Empty;
        }
    }
}