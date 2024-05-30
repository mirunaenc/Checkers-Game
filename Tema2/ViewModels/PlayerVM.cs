
using Tema2.Services;
using Tema2.Models;

namespace Tema2.ViewModels
{
    public class PlayerVM : BaseNotification
    {
        private GameLogic gameLogic;
        private Player playerTurn;

        public PlayerVM(GameLogic gameLogic, Player playerTurn)
        {
            this.gameLogic = gameLogic;
            this.playerTurn = playerTurn;
        }

        public Player PlayerIcon
        {
            get
            {
                return playerTurn;
            }
            set
            {
                playerTurn = value;
                NotifyPropertyChanged("PlayerIcon");
            }
        }
    }
}