﻿using Forum.System.BusinessObjects;
using Forum.System.UnitOfWorks;
using EO = Forum.System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.Services
{
    public class TopicService : ITopicService
    {
        private readonly ISystemUnitOfWork _unitOfWork;
        private readonly IProfileService _profileService;

        public TopicService(ISystemUnitOfWork unitOfWork, IProfileService profileService)
        {
            _unitOfWork = unitOfWork;
            _profileService = profileService;
        }

        public async Task CreateTopic(Topic topic, Guid userId)
        {
            if (topic == null)
                throw new ArgumentNullException("No topic provided");

            var user = await _profileService.GetUserByIdAsync(userId);

            if (user == null)
                throw new FileNotFoundException("User not found with the user id");

            var claims = await _profileService.GetClaimAsync(user);

            if (claims == null)
                throw new NullReferenceException("Claim is required for creating a topic");

            var claim = claims.FirstOrDefault();

            if (claim.Type != "Moderator" && claim.Type != "User")
            {
                throw new InvalidOperationException("You are not permited to create a Board");
            }

            var board = _unitOfWork.Boards.GetById(topic.BoardId);

            if (board == null)
                throw new InvalidOperationException("No topic can be created without proper boardId");

            var topics = _unitOfWork.Topics.Get(x => x.TopicName == topic.TopicName && x.BoardId == topic.BoardId, "");

            if (topics.Count > 0)
                throw new InvalidOperationException("Topic Name Already exists.");

            await _unitOfWork.Topics.AddAsync(new EO.Topic { TopicName = topic.TopicName, BoardId = topic.BoardId, CreatorId = userId });
            _unitOfWork.Save();
        }

        public IList<Topic> GetAllTopics(Guid boardId)
        {
            if (boardId == Guid.Empty)
                throw new ArgumentNullException("board id can not be empty.");

            var board = _unitOfWork.Boards.GetById(boardId);

            if (board == null)
                return null;

            var topics = new List<Topic>();
            var topicsEntity = _unitOfWork.Topics.Get(x => x.BoardId == boardId, "");

            foreach (var topicEntity in topicsEntity)
            {
                topics.Add(new Topic { TopicName = topicEntity.TopicName, Id = topicEntity.Id, BoardId = topicEntity.BoardId, CreatorId = topicEntity.CreatorId });
            }

            return topics;
        }

        public Topic GetTopic(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id can not be empty.");

            var topicEntity = _unitOfWork.Topics.GetById(id);

            if (topicEntity == null)
                return null;

            return new Topic { Id = topicEntity.Id, TopicName = topicEntity.TopicName, CreatorId = topicEntity.CreatorId, BoardId = topicEntity.BoardId };
        }

        public async Task UpdateTopicName(Topic topic)
        {
            if (topic == null)
                throw new ArgumentNullException("No topic provided");

            var user = await _profileService.GetUserByIdAsync(topic.CreatorId);

            if (user == null)
                throw new FileNotFoundException("User not found with the creator id");

            var claims = await _profileService.GetClaimAsync(user);

            if (claims == null)
                throw new NullReferenceException("Claim is required for updating a topic");

            var claim = claims.FirstOrDefault();

            if (claim.Type != "Moderator" && claim.Type != "User")
            {
                throw new InvalidOperationException("You are not permited to update a topic");
            }

            var topicEntity = _unitOfWork.Topics.GetById(topic.Id);

            if (topicEntity == null)
                throw new FileNotFoundException("The topic is not valid");

            if (topicEntity.CreatorId != topic.CreatorId)
                throw new InvalidOperationException("You are not cretor of the topic.");

            var board = _unitOfWork.Boards.GetById(topic.BoardId);

            if (board == null)
                throw new InvalidOperationException("No topic can be edited without proper boardId");

            if (topicEntity.BoardId != topic.BoardId)
                throw new InvalidOperationException("Board not matched");

            var topics = _unitOfWork.Topics.Get(x => x.TopicName == topic.TopicName && x.BoardId == topic.BoardId, "");

            if (topics.Count > 0)
                throw new InvalidOperationException("Topic Name Already exists.");

            topicEntity.TopicName = topic.TopicName;
            _unitOfWork.Save();
        }

        public async Task DeleteTopic(Topic topic)
        {
            if (topic == null)
                throw new ArgumentNullException("No topic provided");

            var user = await _profileService.GetUserByIdAsync(topic.CreatorId);

            if (user == null)
                throw new FileNotFoundException("User not found with the creator id");

            var claims = await _profileService.GetClaimAsync(user);

            if (claims == null)
                throw new NullReferenceException("Claim is required for deleting a topic");

            var claim = claims.FirstOrDefault();

            if (claim.Type != "Moderator" && claim.Type != "User")
            {
                throw new InvalidOperationException("You are not permited to delete a topic");
            }

            var topicEntity = _unitOfWork.Topics.GetById(topic.Id);

            if (topicEntity == null)
                throw new FileNotFoundException("The topic is not valid");

            if (topicEntity.CreatorId != topic.CreatorId)
                throw new InvalidOperationException("You are not cretor of the topic.");

            var board = _unitOfWork.Boards.GetById(topic.BoardId);

            if (board == null)
                throw new InvalidOperationException("No topic can be deleted without proper boardId");

            if (topicEntity.BoardId != topic.BoardId)
                throw new InvalidOperationException("Board not matched");

            _unitOfWork.Topics.Remove(topicEntity);
            _unitOfWork.Save();
        }
    }
}