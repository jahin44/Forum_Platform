using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.BusinessObjects
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string CreatorEmail { get; set; }
        public string CommentText { get; set; }
        public Guid CreatorId { get; set; }
    }
}