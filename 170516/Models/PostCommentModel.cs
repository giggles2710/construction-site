using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _170516.Models
{
    public class PostCommentModel
    {
        public int ParentCommentId { get; set; }
        public int CommentId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Title { get; set; }
        public int Rating { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung bình luận")]
        public string Content { get; set; }
    }
}