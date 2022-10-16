using Forum.DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.Entities
{
    public class Topic : IEntity<Guid>
    {
        public Guid Id { get; set; }
        [Required, MaxLength(100)]
        public string TopicName { get; set; }
        [ForeignKey("Board")]
        public Guid BoardId { get; set; }
        public Board Board { get; set; }
        public Guid CreatorId { get; set; }
    }
}