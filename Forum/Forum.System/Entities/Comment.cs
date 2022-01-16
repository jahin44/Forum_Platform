using Forum.DataLayer;
using Forum.Membership.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.Entities
{
    public class Comment : IEntity<Guid>
    {
        public Guid Id { get; set; }
        [ForeignKey("Post")]
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        [Required, MaxLength(1000)]
        public string CommentText { get; set; }
        [Required, MaxLength(100)]
        public string CreatorEmail { get; set; }
        [ForeignKey("ApplicationUser")]
        public Guid CreatorId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}