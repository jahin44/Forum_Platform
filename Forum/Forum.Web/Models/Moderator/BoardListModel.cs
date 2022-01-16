using Autofac;
using Forum.System.BusinessObjects;
using Forum.System.Services;

namespace Forum.Web.Models.Moderator
{
    public class BoardListModel
    {
        public IList<Board> Boards { get; set; }
        private ILifetimeScope _scope;
        private IBoardService _boardService;
        public BoardListModel() { }

        public BoardListModel(IBoardService boardService)
        {
            _boardService = boardService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _boardService = _scope.Resolve<IBoardService>();
        }

        public void LoadAllBoards()
        {
            Boards = _boardService.GetAllBoards();
        }
    }
}
