using Forum.DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.Entities
{
    public class Board : IEntity<Guid>
    {
        public Guid Id { get; set; }
        [Required, MaxLength(100)]
        public string? BoardName { get; set; }
        public IList<Topic>? Topics { get; set; }
    }
}