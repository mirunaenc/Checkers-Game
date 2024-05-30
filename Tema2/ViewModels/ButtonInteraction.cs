
using System.Windows.Input;
using Tema2.Commands;
using Tema2.Services;

namespace Tema2.ViewModels
{
    public class ButtonInteractionVM : BaseNotification
    {
        private GameLogic gameLogic;
        private ICommand resetCommand;
        private ICommand saveCommand;
        private ICommand aboutCommand;
        private ICommand loadCommand;

        public ButtonInteractionVM(GameLogic gameLogic)
        {
            this.gameLogic = gameLogic;
        }

        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new SimpleCommand(gameLogic.ResetGame);
                }
                return resetCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new SimpleCommand(gameLogic.SaveGame);
                }
                return saveCommand;
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                if (loadCommand == null)
                {
                    loadCommand = new SimpleCommand(gameLogic.LoadGame);
                }
                return loadCommand;
            }
        }

        public ICommand AboutCommand
        {
            get
            {
                if (aboutCommand == null)
                {
                    aboutCommand = new SimpleCommand(gameLogic.About);
                }
                return aboutCommand;
            }
        }
    }
}
