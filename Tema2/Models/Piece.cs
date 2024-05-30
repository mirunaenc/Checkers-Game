using Tema2.Services;
using System.ComponentModel;


namespace Tema2.Models
{
    public class Piece : INotifyPropertyChanged
    {
        private PieceColor color;
        private PieceStatus type;
        private string texture;
        private GameSquare square;

        public event PropertyChangedEventHandler PropertyChanged;

        public Piece(PieceColor color)
        {
            this.color = color;
            type = PieceStatus.Normal;
            if (color == PieceColor.Rosu)
            {
                texture = Helper.redPiece;
            }
            else
            {
                texture = Helper.whitePiece;
            }
        }

        public Piece(PieceColor color, PieceStatus type)
        {
            this.color = color;
            this.type = type;
            if (color == PieceColor.Rosu)
            {
                texture = Helper.redPiece;
            }
            else
            {
                texture = Helper.whitePiece;
            }
            if (type == PieceStatus.Rege && color == PieceColor.Rosu)
            {
                texture = Helper.redKingPiece;
            }
            if (type == PieceStatus.Rege && color == PieceColor.Alb)
            {
                texture = Helper.whiteKingPiece;
            }
        }

        public PieceColor Color
        {
            get
            {
                return color;
            }
        }

        public PieceStatus Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                NotifyPropertyChanged("Type");
            }
        }

        public string Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
                NotifyPropertyChanged("Texture");
            }
        }

        public GameSquare Square
        {
            get
            {
                return square;
            }
            set
            {
                square = value;
                NotifyPropertyChanged("Square");
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}