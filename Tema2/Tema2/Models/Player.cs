using Tema2;
using Tema2.Services;
using Tema2.ViewModels;

namespace Tema2.Models
{
    public class Player : BaseNotification
    {
        private PieceColor color;
        private string image;

        public Player(PieceColor color)
        {
            this.color = color;
            loadImages();
        }

        public void loadImages()
        {
            if (color == PieceColor.Rosu)
            {
                image = Helper.redPiece;
                return;
            }
            image = Helper.whitePiece;
        }

        public PieceColor PlayerColor
        {
            get { return color; }
            set
            {
                color = value;
                NotifyPropertyChanged("PlayerColor");
            }
        }

        public string TurnImage
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                NotifyPropertyChanged("TurnImage");
            }
        }
    }
}
