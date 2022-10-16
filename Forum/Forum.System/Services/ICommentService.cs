using Forum.System.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.Services
{
    public interface ICommentService
    {
        Task CreateComment(Comment comment);
        IList<Comment> GetComments(Guid postId);
        Comment GetComment(Guid commentId);
        Task Update(Comment comment);
        Task Delete(Comment comment);
    }
}