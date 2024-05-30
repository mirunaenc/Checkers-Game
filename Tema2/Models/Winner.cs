using Tema2.ViewModels;

namespace Tema2.Models
{
    public class Winner : BaseNotification
    {
        private int redWins;
        private int whiteWins;

        public Winner(int redWins, int whiteWins)
        {
            this.redWins = redWins;
            this.whiteWins = whiteWins;
        }

        public int RedWins
        {
            get
            {
                return redWins;
            }
            set
            {
                redWins = value;
                NotifyPropertyChanged("RedWins");
            }
        }

        public int WhiteWins
        {
            get
            {
                return whiteWins;
            }
            set
            {
                whiteWins = value;
                NotifyPropertyChanged("WhiteWins");
            }
        }
    }
}
