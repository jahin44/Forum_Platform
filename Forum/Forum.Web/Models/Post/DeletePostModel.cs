﻿using Autofac;
using Forum.System.Services;
using BO = Forum.System.BusinessObjects;

namespace Forum.Web.Models.Post
{
    public class DeletePostModel
    {
        public Guid Id { get; set; }
        public string? BoardName { get; set; }
        public string TopicName { get; set; }
        public Guid TopicId { get; set; }
        public Guid CreatorId { get; set; }
        public string Description { get; set; }
        public string? CreatorEmail { get; set; }
        private ILifetimeScope _scope;
        private IPostService _postService;
        private ITopicService _topicService;
        private IProfileService _profileService;
        private IBoardService _boardService;

        public DeletePostModel() { }

        public DeletePostModel(IPostService postService, ITopicService topicService,
            IProfileService profileService, IBoardService boardService)
        {
            _postService = postService;
            _topicService = topicService;
            _profileService = profileService;
            _boardService = boardService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _boardService = _scope.Resolve<IBoardService>();
            _topicService = _scope.Resolve<ITopicService>();
            _profileService = _scope.Resolve<IProfileService>();
            _postService = _scope.Resolve<IPostService>();
        }

        public async Task Load(Guid id, Guid userId)
        {
            var post = _postService.GetPost(id);

            if (post == null)
                throw new Exception("No post found.");

            var topic = _topicService.GetTopic(post.TopicId);

            if (topic == null)
            {
                BoardName = "404";
                TopicName = "Not Found";
                return;
            }

            var board = _boardService.GetBoard(topic.BoardId);

            if (board == null)
            {
                BoardName = "404";
                TopicName = "Not Found";
                return;
            }

            var user = await _profileService.GetUserByIdAsync(userId);

            if (user == null)
                throw new Exception("No user found");

            BoardName = board.BoardName;
            TopicName = topic.TopicName;

            if (user.Id != post.CreatorId)
                throw new Exception("You can not delete this post");

            TopicId = topic.Id;
            Id = post.Id;
            Description = post.Description;
            CreatorId = post.CreatorId;
            CreatorEmail = post.CreatorEmail;
        }

        public async Task Delete(Guid userId)
        {
            ArgumentNullException.ThrowIfNull(userId, "User id not provided.");
            var user = await _profileService.GetUserAsync(userId);

            if (user == null)
                throw new FileNotFoundException("User not found with the user id.");

            var claims = await _profileService.GetClaimAsync(user);

            if (claims == null)
                throw new NullReferenceException("Claim is required for deleting a post.");

            var claim = claims.FirstOrDefault();

            if (claim.Type != "Moderator" && claim.Type != "User")
            {
                throw new InvalidOperationException("You are not permited to delete a post.");
            }

            var post = _postService.GetPost(Id);

            if (post == null)
                throw new FileNotFoundException("No topic found with topic id");

            if (user.Id != post.CreatorId)
                throw new InvalidOperationException("You are not permitted to delete this post.");

            await _postService.DeletePost(new BO.Post
            {
                Id = Id,
                Description = Description,
                CreatorId = CreatorId,
                CreatorEmail = CreatorEmail,
                TopicId = TopicId
            });
        }
    }
}
