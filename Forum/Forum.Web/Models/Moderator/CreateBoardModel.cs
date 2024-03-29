﻿using Autofac;
using Forum.System.BusinessObjects;
using Forum.System.Services;
using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Models.Moderator
{
    public class CreateBoardModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Board Name cannot exceed 100 characters.")]
        public string BoardName { get; set; }
        private ILifetimeScope _scope;
        private IBoardService _boardService;
        private IProfileService _profileService;

        public CreateBoardModel() { }

        public CreateBoardModel(IBoardService boardService, IProfileService profileService)
        {
            _boardService = boardService;
            _profileService = profileService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _boardService = _scope.Resolve<IBoardService>();
            _profileService = _scope.Resolve<IProfileService>();
        }

        public async Task CreateBoard(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new InvalidOperationException("User Email Must Be Provided For creating a Board");

            var user = await _profileService.GetUserAsync(userEmail);

            if (user == null)
                throw new FileNotFoundException("User not found with the email id");

            var claims = await _profileService.GetClaimAsync(user);
            if (claims == null)
                throw new NullReferenceException("Claim is required for creating a board");

            var claim = claims.FirstOrDefault();

            if (claim.Type != "Moderator")
            {
                throw new InvalidOperationException("You are not permited to create a Board");
            }

            await _boardService.CreateBoard(new Board { BoardName = BoardName }, user.Id);
        }
    }
}