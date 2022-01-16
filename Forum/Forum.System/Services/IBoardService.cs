using Forum.System.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.System.Services
{
    public interface IBoardService
    {
        Task CreateBoard(Board board, Guid modId);
        IList<Board> GetAllBoards();
        Board GetBoard(Guid id);
        Task UpdateBoardName(Board board, Guid modId);
        Task DeleteBoard(Board board, Guid modId);
    }
}