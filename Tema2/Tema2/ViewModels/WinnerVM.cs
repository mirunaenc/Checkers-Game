using Tema2.Models;
using Tema2.Services;

namespace Tema2.ViewModels
{
    public class WinnerVM : BaseNotification
    {

        private GameLogic gameLogic;

        private Winner winner;
        private void GameLogic_OnPiecesRemainingChanged(int whiteRemaining, int redRemaining)
        {
            WhiteRemaining = whiteRemaining;
            RedRemaining = redRemaining;
        }


        public WinnerVM(GameLogic gameLogic, Winner winner)
        {
            this.gameLogic = gameLogic;
            this.winner = winner;
            gameLogic.OnPiecesRemainingChanged += GameLogic_OnPiecesRemainingChanged;
            whiteRemaining = 12;
            redRemaining = 12;

        }

        public Winner WinnerPlayer
        {
            get { return winner; }
            set
            {
                winner = value;
                NotifyPropertyChanged("WinnerPlayer");
            }
        }

        private int whiteRemaining;
        public int WhiteRemaining
        {
            get { return whiteRemaining; }
            set
            {
                if (whiteRemaining != value)
                {
                    whiteRemaining = value;
                    NotifyPropertyChanged(nameof(WhiteRemaining));
                }
            }
        }

        private int redRemaining;
        public int RedRemaining
        {
            get { return redRemaining; }
            set
            {
                if (redRemaining != value)
                {
                    redRemaining = value;
                    NotifyPropertyChanged(nameof(RedRemaining));
                }
            }
        }
    }
}
