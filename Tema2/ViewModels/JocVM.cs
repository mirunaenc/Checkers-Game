
using System.Collections.ObjectModel;
using Tema2.Services;
using Tema2.Models;

namespace Tema2.ViewModels
{
    public class JocVM
    {
        public ObservableCollection<ObservableCollection<GameSquareVM>> Board { get; set; }
        public GameLogic Logic { get; set; }
        public ButtonInteractionVM Interactions { get; set; }

        public WinnerVM WinnerVM { get; set; }

        public PlayerVM PlayerTurnVM { get; set; }

        public string RED_PIECE { get; set; }
        public string WHITE_PIECE { get; set; }

        public JocVM()
        {
            ObservableCollection<ObservableCollection<GameSquare>> board = Helper.initBoard();
            Player playerTurn = new Player(PieceColor.Rosu);
            Winner winner = new Winner(0, 0);
            Logic = new GameLogic(board, playerTurn, winner);
            PlayerTurnVM = new PlayerVM(Logic, playerTurn);
            WinnerVM = new WinnerVM(Logic, winner);
            Board = CellBoardToCellVMBoard(board);
            Interactions = new ButtonInteractionVM(Logic);
            RED_PIECE = Helper.redPiece;
            WHITE_PIECE = Helper.whitePiece;
        }


        private ObservableCollection<ObservableCollection<GameSquareVM>> CellBoardToCellVMBoard(ObservableCollection<ObservableCollection<GameSquare>> board)
        {
            ObservableCollection<ObservableCollection<GameSquareVM>> result = new ObservableCollection<ObservableCollection<GameSquareVM>>();
            for (int i = 0; i < board.Count; i++)
            {
                ObservableCollection<GameSquareVM> line = new ObservableCollection<GameSquareVM>();
                for (int j = 0; j < board[i].Count; j++)
                {
                    GameSquare c = board[i][j];
                    GameSquareVM cellVM = new GameSquareVM(c, Logic);
                    line.Add(cellVM);
                }
                result.Add(line);
            }
            return result;
        }
    }
}