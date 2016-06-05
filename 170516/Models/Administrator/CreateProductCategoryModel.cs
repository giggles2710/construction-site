using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class CreateProductCategoryModel
    {
        public CreateProductCategoryModel()
        {
            CategoryList = new List<CreateProductCategoryListItem>();
        }

        public int CategoryID { get; set; }        

        [Required(ErrorMessage ="Vui lòng điền tên Danh mục")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng điền mô tả cho Danh mục")]
        public string Description { get; set; }
        public Nullable<int> ParentID { get; set; }

        public string ParentCategoryName { get; set; }

        public string CategoryImage { get; set; }        

        public List<CreateProductCategoryListItem> CategoryList { get; set; }
    }
}