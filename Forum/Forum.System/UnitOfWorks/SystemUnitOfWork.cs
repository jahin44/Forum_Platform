using Forum.DataLayer;
using Forum.System.Contexts;
using Forum.System.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.UnitOfWorks
{
    public class SystemUnitOfWork : UnitOfWork, ISystemUnitOfWork
    {
        public IBoardRepository Boards { get; private set; }
        public ITopicRepository Topics { get; private set; }
        public IPostRepository Posts { get; private set; }
        public ICommentRepository Comments { get; private set; }

        public SystemUnitOfWork(ISystemDbContext context,
            IBoardRepository board,
            ITopicRepository topics,
            IPostRepository posts,
            ICommentRepository comments
            ) : base((DbContext)context)
        {
            Boards = board;
            Topics = topics;
            Posts = posts;
            Comments = comments;
        }
    }
}