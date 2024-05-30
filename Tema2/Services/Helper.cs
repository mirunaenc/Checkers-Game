using Microsoft.Win32;
using Tema2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Tema2.Enums;

namespace Tema2.Services
{
    public class Helper
    {
        #region constValues
        //const image paths
        public const string redSquare = "/Tema2;component/Resources/squareRed.png";
        public const string whiteSquare = "/Tema2;component/Resources/squareWhite.png";
        public const string redPiece = "/Tema2;component/Resources/simple_red.jpg";
        public const string whitePiece = "/Tema2;component/Resources/simple_black.jpg";
        public const string hintSquare = "/Tema2;component/Resources/squareHint.png";
        public const string redKingPiece = "/Tema2;component/Resources/king_red.jpg";
        public const string whiteKingPiece = "/Tema2;component/Resources/king_black.jpg";
        public const string SQUARE_HIGHLIGHT = "NULL";
        //const values for serialization
        public const char NO_PIECE = 'N';
        public const char WHITE_PIECE = 'W';
        public const char RED_PIECE = 'R';
        public const char RED_KING = 'K';
        public const char WHITE_KING = 'L';
        public const char WHITE_TURN = '2';
        public const char RED_TURN = '1';
        public const char HAD_COMBO = 'H';
        public const char EXTRA_PATH = 'E';
        //board constants
        public const int boardSize = 8;
        #endregion
        #region staticValues
        public static GameSquare CurrentSquare { get; set; }
        private static Dictionary<GameSquare, GameSquare> currentNeighbours = new Dictionary<GameSquare, GameSquare>();
        private static Player turn = new Player(PieceColor.Rosu);
        private static bool extraMove = false;
        private static bool extraPath = false;
        private static int collectedRedPieces = 0;
        private static int collectedWhitePieces = 0;
        #endregion

        public static Dictionary<GameSquare, GameSquare> CurrentNeighbours
        {
            get
            {
                return currentNeighbours;
            }
            set
            {
                currentNeighbours = value;
            }
        }

        public static Player Turn
        {
            get
            {
                return turn;
            }
            set
            {
                turn = value;
            }
        }

        public static bool ExtraMove
        {
            get
            {
                return extraMove;
            }
            set
            {
                extraMove = value;
            }
        }

        public static bool ExtraPath
        {
            get
            {
                return extraPath;
            }
            set
            {
                extraPath = value;
            }
        }

        public static int CollectedWhitePieces
        {
            get { return collectedWhitePieces; }
            set { collectedWhitePieces = value; }
        }

        public static int CollectedRedPieces
        {
            get { return collectedRedPieces; }
            set { collectedRedPieces = value; }
        }
        #region UtilityMethods
        public static ObservableCollection<ObservableCollection<GameSquare>> initBoard()
        {
            ObservableCollection<ObservableCollection<GameSquare>> board = new ObservableCollection<ObservableCollection<GameSquare>>();
            const int boardSize = 8;

            for (int row = 0; row < boardSize; ++row)
            {
                board.Add(new ObservableCollection<GameSquare>());
                for (int column = 0; column < boardSize; ++column)
                {
                    if ((row + column) % 2 == 0)
                    {
                        board[row].Add(new GameSquare(row, column, Square.Light, null));
                    }
                    else if (row < 3)
                    {
                        board[row].Add(new GameSquare(row, column, Square.Dark, new Piece(PieceColor.Alb)));
                    }
                    else if (row > 4)
                    {
                        board[row].Add(new GameSquare(row, column, Square.Dark, new Piece(PieceColor.Rosu)));
                    }
                    else
                    {
                        board[row].Add(new GameSquare(row, column, Square.Dark, null));
                    }
                }
            }

            return board;
        }

        public static void ResetGameBoard(ObservableCollection<ObservableCollection<GameSquare>> squares)
        {
            for (int index1 = 0; index1 < boardSize; index1++)
            {
                for (int index2 = 0; index2 < boardSize; index2++)
                {
                    if ((index1 + index2) % 2 == 0)
                    {
                        squares[index1][index2].Piece = null;
                    }
                    else
                        if (index1 < 3)
                    {
                        squares[index1][index2].Piece = new Piece(PieceColor.Alb);
                        squares[index1][index2].Piece.Square = squares[index1][index2];
                        //pieces
                    }
                    else
                        if (index1 > 4)
                    {
                        squares[index1][index2].Piece = new Piece(PieceColor.Rosu);
                        squares[index1][index2].Piece.Square = squares[index1][index2];
                        //pieces
                    }
                    else
                    {
                        squares[index1][index2].Piece = null;
                    }
                }
            }
        }
        public static bool isInBounds(int row, int column)
        {
            return row >= 0 && column >= 0 && row < boardSize && column < boardSize;
        }

        public static void initializeNeighboursToBeChecked(GameSquare square, HashSet<Tuple<int, int>> neighboursToCheck)
        {
            if (square.Piece.Type == PieceStatus.Rege)
            {
                neighboursToCheck.Add(new Tuple<int, int>(-1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(-1, 1));
                neighboursToCheck.Add(new Tuple<int, int>(1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(1, 1));
            }
            else if (square.Piece.Color == PieceColor.Rosu)
            {
                neighboursToCheck.Add(new Tuple<int, int>(-1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(-1, 1));
            }
            else
            {
                neighboursToCheck.Add(new Tuple<int, int>(1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(1, 1));
            }
        }
        #endregion
        #region Serialization
        public static void LoadGame(ObservableCollection<ObservableCollection<GameSquare>> squares)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            bool? answer = openDialog.ShowDialog();

            if (answer == true)
            {
                string path = openDialog.FileName;
                using (var reader = new StreamReader(path))
                {
                    string text;
                    //current
                    if (CurrentSquare != null)
                    {
                        CurrentSquare.Texture = redSquare;
                    }
                    text = reader.ReadLine();
                    if (text != NO_PIECE.ToString())
                    {
                        CurrentSquare = squares[(int)char.GetNumericValue(text[0])][(int)char.GetNumericValue(text[1])];
                        CurrentSquare.Texture = SQUARE_HIGHLIGHT;
                    }
                    else
                    {
                        CurrentSquare = null;
                    }

                    text = reader.ReadLine();
                    if (text != NO_PIECE.ToString())
                    {
                        ExtraMove = true;
                    }
                    else
                    {
                        ExtraMove = false;
                    }
                    text = reader.ReadLine();
                    if (text != NO_PIECE.ToString())
                    {
                        ExtraPath = true;
                    }
                    else
                    {
                        ExtraPath = false;
                    }
                    //to_do_multi_JUMP
                    text = reader.ReadLine();
                    if (text == RED_TURN.ToString())
                    {
                        Turn.PlayerColor = PieceColor.Rosu;
                        Turn.TurnImage = redPiece;
                    }
                    else
                    {
                        Turn.PlayerColor = PieceColor.Alb;
                        Turn.TurnImage = redPiece;
                    }
                    //board
                    for (int index1 = 0; index1 < boardSize; index1++)
                    {
                        text = reader.ReadLine();
                        for (int index2 = 0; index2 < boardSize; index2++)
                        {
                            squares[index1][index2].LegalSquareSymbol = null;
                            switch (text[index2])
                            {
                                case { } when text[index2] == NO_PIECE:
                                    squares[index1][index2].Piece = null;
                                    break;
                                case { } when text[index2] == RED_PIECE:
                                    squares[index1][index2].Piece = new Piece(PieceColor.Rosu, PieceStatus.Normal);
                                    squares[index1][index2].Piece.Square = squares[index1][index2];
                                    //to_DO 
                                    break;
                                case { } when text[index2] == RED_KING:
                                    squares[index1][index2].Piece = new Piece(PieceColor.Rosu, PieceStatus.Rege);
                                    squares[index1][index2].Piece.Square = squares[index1][index2];

                                    //todo
                                    break;
                                case { } when text[index2] == WHITE_PIECE:
                                    squares[index1][index2].Piece = new Piece(PieceColor.Alb, PieceStatus.Normal);
                                    squares[index1][index2].Piece.Square = squares[index1][index2];
                                    break;
                                case { } when text[index2] == WHITE_KING:
                                    squares[index1][index2].Piece = new Piece(PieceColor.Alb, PieceStatus.Rege);
                                    squares[index1][index2].Piece.Square = squares[index1][index2];
                                    break;
                            }
                        }
                    }

                    foreach (var square in CurrentNeighbours.Keys)
                    {
                        square.LegalSquareSymbol = null;
                    }

                    CurrentNeighbours.Clear();

                    do
                    {
                        text = reader.ReadLine();
                        if (text == "-")
                        {
                            if (text.Length == 1)
                            {
                                break;
                            }
                            CurrentNeighbours.Add(squares[(int)char.GetNumericValue(text[0])][(int)char.GetNumericValue(text[1])], null);
                        }
                        else
                        {
                            CurrentNeighbours.Add(squares[(int)char.GetNumericValue(text[0])][(int)char.GetNumericValue(text[1])],
                                squares[(int)char.GetNumericValue(text[2])][(int)char.GetNumericValue(text[3])]);
                            //TO-DO
                        }
                    } while (text != "-");

                }
            }
        }

        public static void SaveGame(ObservableCollection<ObservableCollection<GameSquare>> squares)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            bool? answer = saveDialog.ShowDialog();
            if (answer == true)
            {
                var path = saveDialog.FileName;
                using (var writer = new StreamWriter(path))
                {
                    //current
                    if (CurrentSquare != null)
                    {
                        writer.Write(CurrentSquare.Row.ToString() + CurrentSquare.Column.ToString());
                    }
                    else
                    {
                        writer.Write(NO_PIECE);
                    }
                    writer.WriteLine();
                    if (ExtraMove)
                    {
                        writer.Write(HAD_COMBO);
                    }
                    else
                    {
                        writer.Write(NO_PIECE);
                    }
                    writer.WriteLine();
                    if (ExtraPath)
                    {
                        writer.Write(ExtraPath);
                    }
                    else
                    {
                        writer.Write(NO_PIECE);
                    }
                    writer.WriteLine();
                    //TO_DO_MULTI_JUMP
                    if (Turn.PlayerColor.Equals(PieceColor.Rosu))
                    {
                        writer.Write(RED_TURN);
                    }
                    else
                    {
                        writer.Write(WHITE_TURN);
                    }
                    writer.WriteLine();
                    //board
                    foreach (var line in squares)
                    {
                        foreach (var square in line)
                        {
                            switch (square)
                            {
                                case { } when square.Piece == null:
                                    writer.Write(NO_PIECE);
                                    break;
                                case { } when square.Piece.Color.Equals(PieceColor.Rosu) && square.Piece.Type == PieceStatus.Normal:
                                    writer.Write(RED_PIECE);
                                    break;
                                case { } when square.Piece.Color.Equals(PieceColor.Alb) && square.Piece.Type == PieceStatus.Normal:
                                    writer.Write(WHITE_PIECE);
                                    break;
                                case { } when square.Piece.Color.Equals(PieceColor.Rosu) && square.Piece.Type == PieceStatus.Rege:
                                    writer.Write(WHITE_KING);
                                    break;
                                case { } when square.Piece.Color.Equals(PieceColor.Rosu) && square.Piece.Type == PieceStatus.Rege:
                                    writer.Write(RED_KING);
                                    break;
                                default:
                                    break;
                            }
                        }
                        writer.WriteLine();

                    }

                    foreach (var square in CurrentNeighbours.Keys)
                    {
                        if (CurrentNeighbours[square] == null)
                        {
                            writer.Write(square.Row.ToString() + square.Column.ToString() + NO_PIECE);
                        }
                        else
                        {
                            writer.Write(square.Row.ToString() + square.Column.ToString() + CurrentNeighbours[square].Row.ToString() + CurrentNeighbours[square].Column.ToString());
                        }
                        writer.WriteLine();
                    }
                    writer.Write("-\n");
                }
            }
        }

        public static void ResetGame(ObservableCollection<ObservableCollection<GameSquare>> squares)
        {
            foreach (var square in CurrentNeighbours.Keys)
            {
                square.LegalSquareSymbol = null;
            }

            if (CurrentSquare != null)
            {
                CurrentSquare.Texture = redSquare;
            }

            currentNeighbours.Clear();
            CurrentSquare = null;
            ExtraMove = false;
            ExtraPath = false;
            CollectedWhitePieces = 0;
            CollectedRedPieces = 0;
            Turn.PlayerColor = PieceColor.Rosu;
            //texture add
            ResetGameBoard(squares);
        }
        #endregion

        #region Credits
        public static void About()
        {
            //string PATH = Path.GetDirectoryName(System.Environment.CurrentDirectory);
            //PATH = Directory.GetParent(PATH).FullName + @"\Resources\about.txt";
            string PATH = "D:\\FACULTATE\\Anul II\\Semestrul II\\MVP\\Teme\\Tema2\\Tema2\\Tema2\\Resources\\winnerText.txt";

            using (var reader = new StreamReader(PATH))
            {
                MessageBox.Show(reader.ReadToEnd(), "About", MessageBoxButton.OKCancel);
            }
        }
        #endregion

        #region textFileHandling
        public static void writeScore(int r, int w)
        {
            string PATH = "D:\\FACULTATE\\Anul II\\Semestrul II\\MVP\\Teme\\Tema2\\Tema2\\Tema2\\Resources\\winnerText.txt";
            using (var writer = new StreamWriter(PATH))
            {
                writer.WriteLine(r + "," + w);
            }
        }

        public static Winner getScore()
        {
            Winner aux = new Winner(0, 0);
            string PATH = "D:\\FACULTATE\\Anul II\\Semestrul II\\MVP\\Teme\\Tema2\\Tema2\\Tema2\\Resources\\winnerText.txt";
            using (var reader = new StreamReader(PATH))
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    var splitted = line.Split(',');
                    if (splitted.Length >= 2) // Verifică dacă există cel puțin două elemente în splitted
                    {
                        aux.RedWins = int.Parse(splitted[0]);
                        aux.WhiteWins = int.Parse(splitted[1]);
                    }
                    else
                    {
                        // Poți gestiona această situație aici, de exemplu:
                        Console.WriteLine("Linia nu conține suficiente elemente pentru a obține scorul.");
                    }
                }
                else
                {
                    // Poți gestiona această situație aici, de exemplu:
                    Console.WriteLine("Fișierul este gol sau nu conține nicio linie.");
                }
            }
            return aux;
        }
        #endregion
    }
}
