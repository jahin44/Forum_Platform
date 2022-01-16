using Forum.System.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.Services
{
    public interface IPostService
    {
        Task CreatePost(Post post);
        IList<Post> GetAllPosts(Guid topicId);
        Post GetPost(Guid id);
        Task UpdatePostDescription(Post post);
        Task DeletePost(Post post);
        Task<IList<Post>> GetPostByUserId(Guid userId);
    }
}