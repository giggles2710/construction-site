using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _170516.Models
{
    public class PostCommentModel
    {
        public int? ParentCommentId { get; set; }
        public int CommentId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
        public DateTime CommentDate { get; set; }
    }

    public class SubmitPostComment
    {
        public int CommentId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung bình luận")]
        public string Content { get; set; }
        public int Rating { get; set; }
        public string CommentType { get; set; }
        public int ObjectId { get; set; }
    }

    public class CommentListModel {
        public List<PostCommentModel> CommentList { get; set; }
    }

}