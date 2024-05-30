using Tema2.Services;
using System.ComponentModel;
using Tema2.Enums;

namespace Tema2.Models
{
    public class GameSquare : INotifyPropertyChanged
    {
        private int row;
        private int column;
        private Enums.Square shade;
        private string texture;
        private Piece piece;
        private string legalSquareSymbol;

        public event PropertyChangedEventHandler PropertyChanged;

        public GameSquare(int row, int column, Enums.Square shade, Piece piece)
        {
            this.row = row;
            this.column = column;
            this.shade = shade;
            if (shade == Enums.Square.Dark)
            {
                texture = Helper.redSquare;
            }
            else
            {
                texture = Helper.whiteSquare;
            }
            this.piece = piece;
        }

        public int Row
        {
            get
            {
                return row;
            }
        }

        public int Column
        {
            get
            {
                return column;
            }
        }

        public Enums.Square Shade
        {
            get
            {
                return shade;
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

        public Piece Piece
        {
            get
            {
                return piece;
            }
            set
            {
                piece = value;
                NotifyPropertyChanged("Piece");
            }
        }

        public string LegalSquareSymbol
        {
            get
            {
                return legalSquareSymbol;
            }
            set
            {
                legalSquareSymbol = value;
                NotifyPropertyChanged("LegalSquareSymbol");
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
