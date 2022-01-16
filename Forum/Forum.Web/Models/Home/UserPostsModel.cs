using Autofac;
using BO = Forum.System.BusinessObjects;
using Forum.System.Services;

namespace Forum.Web.Models.Home
{
    public class UserPostsModel
    {
        public string CreatorEmail { get; set; }
        public IList<BO.Post> Posts { get; set; }
        private ILifetimeScope _scope;
        private IPostService _postService;
        private IProfileService _profileService;
        public UserPostsModel() { }

        public UserPostsModel(IPostService postService, IProfileService profileService)
        {
            _postService = postService;
            _profileService = profileService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _postService = _scope.Resolve<IPostService>();
            _profileService = _scope.Resolve<IProfileService>();
        }

        public async Task Load(Guid userId)
        {
            Posts = new List<BO.Post>();
            var user = await _profileService.GetUserAsync(userId);

            if (user == null)
                throw new FileNotFoundException("User not Found.");

            CreatorEmail = user.Email;
            Posts = await _postService.GetPostByUserId(userId);
        }
    }
}