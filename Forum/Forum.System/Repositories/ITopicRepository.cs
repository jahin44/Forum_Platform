using Forum.DataLayer;
using Forum.System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.Repositories
{
    public interface ITopicRepository : IRepository<Topic, Guid>
    {
    }
}