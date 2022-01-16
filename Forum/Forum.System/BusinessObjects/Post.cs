using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.BusinessObjects
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid TopicId { get; set; }
        public Guid CreatorId { get; set; }
        public string CreatorEmail { get; set; }
    }
}
