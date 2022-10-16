using Forum.DataLayer;
using Forum.System.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.UnitOfWorks
{
    public interface ISystemUnitOfWork : IUnitOfWork
    {
        IBoardRepository Boards { get; }
        ITopicRepository Topics { get; }
        IPostRepository Posts { get; }
        ICommentRepository Comments { get; }
    }
}