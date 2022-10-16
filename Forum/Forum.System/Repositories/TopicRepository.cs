using Forum.DataLayer;
using Forum.System.Contexts;
using Forum.System.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.Repositories
{
    public class TopicRepository : Repository<Topic, Guid>, ITopicRepository
    {
        public TopicRepository(ISystemDbContext context)
            : base((DbContext)context)
        {
        }
    }
}