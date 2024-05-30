
using System.Windows.Input;
using Tema2.Commands;
using Tema2.Models;
using Tema2.Services;


namespace Tema2.ViewModels
{
    public class GameSquareVM : BaseNotification
    {
        private GameLogic gameLogic;
        private GameSquare genericSquare;
        private ICommand clickPieceCommand;
        private ICommand movePieceCommand;

        public GameSquareVM(GameSquare square, GameLogic gameLogic)
        {
            genericSquare = square;
            this.gameLogic = gameLogic;
        }

        public GameSquare GenericSquare
        {
            get
            {
                return genericSquare;
            }
            set
            {
                genericSquare = value;
                NotifyPropertyChanged("GenericSquare");
            }
        }

        public ICommand ClickPieceCommand
        {
            get
            {
                if (clickPieceCommand == null)
                {
                    clickPieceCommand = new RelayCommand<GameSquare>(gameLogic.ClickPiece);
                }
                return clickPieceCommand;
            }
        }

        public ICommand MovePieceCommand
        {
            get
            {
                if (movePieceCommand == null)
                {
                    movePieceCommand = new RelayCommand<GameSquare>(gameLogic.MovePiece);
                }
                return movePieceCommand;
            }
        }
    }
}
