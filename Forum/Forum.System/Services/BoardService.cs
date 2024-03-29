﻿using Forum.DataLayer;
using Forum.System.UnitOfWorks;
using Forum.System.BusinessObjects;
using EO = Forum.System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.Services
{
    public class BoardService : IBoardService
    {
        private readonly ISystemUnitOfWork _unitOfWork;
        private readonly IProfileService _profileService;

        public BoardService(ISystemUnitOfWork unitOfWork, IProfileService profileService)
        {
            _unitOfWork = unitOfWork;
            _profileService = profileService;
        }

        public async Task CreateBoard(Board board, Guid modId)
        {
            if (board == null)
                throw new ArgumentNullException("Board can not be null");

            if (modId == Guid.Empty)
                throw new ArgumentNullException("Moderator id can not be empty.");

            var user = await _profileService.GetUserByIdAsync(modId);

            if (user == null)
                throw new FileNotFoundException("User not found with the modId");

            var claims = await _profileService.GetClaimAsync(user);

            if (claims == null)
                throw new NullReferenceException("Claim is required for creating a board");

            var claim = claims.FirstOrDefault();

            if (claim.Type != "Moderator")
            {
                throw new InvalidOperationException("You are not permited to create a Board");
            }

            var boards = _unitOfWork.Boards.Get(x => x.BoardName == board.BoardName, "");

            if (boards.Count > 0)
                throw new InvalidOperationException("Board Name Already exists.");

            await _unitOfWork.Boards.AddAsync(new EO.Board { BoardName = board.BoardName });
            _unitOfWork.Save();
        }

        public IList<Board> GetAllBoards()
        {
            var boardEntities = _unitOfWork.Boards.GetAll();
            var boards = new List<Board>();

            foreach (var boardEntity in boardEntities)
            {
                boards.Add(new Board { BoardName = boardEntity.BoardName, Id = boardEntity.Id });
            }

            return boards;
        }

        public Board GetBoard(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id can not be empty.");

            var boardEntity = _unitOfWork.Boards.GetById(id);

            if (boardEntity == null)
                return null;

            return new Board { Id = boardEntity.Id, BoardName = boardEntity.BoardName };
        }

        public async Task UpdateBoardName(Board board, Guid modId)
        {
            if (board == null)
                throw new ArgumentNullException("Board can not be null");

            if (modId == Guid.Empty)
                throw new ArgumentNullException("Moderator id can not be empty.");

            var user = await _profileService.GetUserByIdAsync(modId);

            if (user == null)
                throw new FileNotFoundException("User not found with the modId");

            var claims = await _profileService.GetClaimAsync(user);

            if (claims == null)
                throw new NullReferenceException("Claim is required for creating a board");

            var claim = claims.FirstOrDefault();

            if (claim.Type != "Moderator")
            {
                throw new InvalidOperationException("You are not permited to create a Board");
            }

            var boardEntity = _unitOfWork.Boards.GetById(board.Id);

            if (boardEntity == null)
                throw new FileNotFoundException("The board is not valid");

            var boards = _unitOfWork.Boards.Get(x => x.BoardName == board.BoardName, "");

            if (boards.Count > 0)
                throw new InvalidOperationException("Board Name Already exists.");

            boardEntity.BoardName = board.BoardName;
            _unitOfWork.Save();
        }

        public async Task DeleteBoard(Board board, Guid modId)
        {
            if (board == null)
                throw new ArgumentNullException("Board can not be null");

            if (modId == Guid.Empty)
                throw new ArgumentNullException("Moderator id can not be empty.");

            var user = await _profileService.GetUserByIdAsync(modId);

            if (user == null)
                throw new FileNotFoundException("User not found with the modId");

            var claims = await _profileService.GetClaimAsync(user);

            if (claims == null)
                throw new NullReferenceException("Claim is required for creating a board");

            var claim = claims.FirstOrDefault();

            if (claim.Type != "Moderator")
            {
                throw new InvalidOperationException("You are not permited to create a Board");
            }

            var boardEntity = _unitOfWork.Boards.GetById(board.Id);

            if (boardEntity == null)
                throw new FileNotFoundException("The board is not valid");

            _unitOfWork.Boards.Remove(boardEntity);
            _unitOfWork.Save();
        }
    }
}
