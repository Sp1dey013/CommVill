using System;

namespace CommVill.Models
{
    public class Comment
    {
        public Guid CommnetId { get; set; }
        public string? CommentData { get; set; }
        public DateTime CommentTime { get; set; }
        public Guid? PostId { get; set; }
        public Guid? PrimaryCommentId { get; set; }
    }
}
