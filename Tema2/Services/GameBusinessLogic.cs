using Tema2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Tema2.Services
{
    public class GameLogic
    {
        private ObservableCollection<ObservableCollection<GameSquare>> board;
        private Player playerTurn;
        private Winner winner;
        public GameLogic(ObservableCollection<ObservableCollection<GameSquare>> board, Player playerTurn, Winner winner)
        {
            this.board = board;
            this.playerTurn = playerTurn;
            this.winner = winner;
            this.winner.RedWins = 0;
            this.winner.WhiteWins = 0;
        }
        #region Logics
        public event Action<int, int> OnPiecesRemainingChanged;

        // Acestă metodă ar putea fi apelată după fiecare mutare sau capturare
        // pentru a recalcula și a notifica numărul de piese rămase.
        public void UpdateRemainingPieces()
        {
            int whiteRemaining = CalculateRemainingPieces(PieceColor.Alb);
            int redRemaining = CalculateRemainingPieces(PieceColor.Rosu);

            // Declanșează evenimentul cu valorile calculate
            OnPiecesRemainingChanged?.Invoke(whiteRemaining, redRemaining);
        }

        private int CalculateRemainingPieces(PieceColor culoarePiesa)
        {
            // Implementația specifică depinde de structura tabloului de joc
            int count = 0;
            foreach (var row in board)
            {
                foreach (var square in row)
                {
                    if (square.Piece != null && square.Piece.Color == culoarePiesa)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    

    private void SwitchTurns(GameSquare square)
        {
            if (square.Piece.Color == PieceColor.Rosu)
            {
                Helper.Turn.PlayerColor = PieceColor.Alb;
                Helper.Turn.TurnImage = Helper.whitePiece;
                playerTurn.PlayerColor = PieceColor.Alb;
                playerTurn.TurnImage = Helper.whitePiece;
            }
            else
            {
                Helper.Turn.PlayerColor = PieceColor.Rosu;
                Helper.Turn.TurnImage = Helper.redPiece;
                playerTurn.PlayerColor = PieceColor.Rosu;
                playerTurn.TurnImage = Helper.redPiece;
            }
        }

        private void FindNeighbours(GameSquare square)
        {
            var neighboursToCheck = new HashSet<Tuple<int, int>>();

            Helper.initializeNeighboursToBeChecked(square, neighboursToCheck);

            foreach (Tuple<int, int> neighbour in neighboursToCheck)
            {
                if (Helper.isInBounds(square.Row + neighbour.Item1, square.Column + neighbour.Item2))
                {
                    if (board[square.Row + neighbour.Item1][square.Column + neighbour.Item2].Piece == null)
                    {
                        if (!Helper.ExtraMove)
                        {
                            Helper.CurrentNeighbours.Add(board[square.Row + neighbour.Item1][square.Column + neighbour.Item2], null);
                        }
                    }
                    else if (Helper.isInBounds(square.Row + neighbour.Item1 * 2, square.Column + neighbour.Item2 * 2) &&
                        board[square.Row + neighbour.Item1][square.Column + neighbour.Item2].Piece.Color != square.Piece.Color &&
                        board[square.Row + neighbour.Item1 * 2][square.Column + neighbour.Item2 * 2].Piece == null)
                    {
                        Helper.CurrentNeighbours.Add(board[square.Row + neighbour.Item1 * 2][square.Column + neighbour.Item2 * 2], board[square.Row + neighbour.Item1][square.Column + neighbour.Item2]);
                        Helper.ExtraPath = true;
                    }
                }
            }
        }

        private void DisplayRegularMoves(GameSquare square)
        {
            if (Helper.CurrentSquare != square)
            {
                if (Helper.CurrentSquare != null)
                {
                    board[Helper.CurrentSquare.Row][Helper.CurrentSquare.Column].Texture = Helper.redSquare;

                    foreach (GameSquare selectedSquare in Helper.CurrentNeighbours.Keys)
                    {
                        selectedSquare.LegalSquareSymbol = null;
                    }
                    Helper.CurrentNeighbours.Clear();
                }

                FindNeighbours(square);

                if (Helper.ExtraMove && !   Helper.ExtraPath)
                {
                    Helper.ExtraMove = false;
                    SwitchTurns(square);
                }
                else
                {

                    foreach (GameSquare neighbour in Helper.CurrentNeighbours.Keys)
                    {
                        board[neighbour.Row][neighbour.Column].LegalSquareSymbol = Helper.hintSquare;
                    }

                    Helper.CurrentSquare = square;
                    Helper.ExtraPath = false;
                }
            }
            else
            {
                board[square.Row][square.Column].Texture = Helper.redSquare;

                foreach (GameSquare selectedSquare in Helper.CurrentNeighbours.Keys)
                {
                    selectedSquare.LegalSquareSymbol = null;
                }
                Helper.CurrentNeighbours.Clear();
                Helper.CurrentSquare = null;
            }
        }
        #endregion
        #region ClickCommands

        public void ResetGame()
        {
            Helper.ResetGame(board);
        }

        public void SaveGame()
        {
            Helper.SaveGame(board);
        }

        public void LoadGame()
        {
            Helper.LoadGame(board);
            playerTurn.TurnImage = Helper.Turn.TurnImage;
        }

        public void About()
        {
            Helper.About();
        }
        public void ClickPiece(GameSquare square)
        {
            if ((Helper.Turn.PlayerColor == PieceColor.Rosu && square.Piece.Color == PieceColor.Rosu ||
                Helper.Turn.PlayerColor == PieceColor.Alb && square.Piece.Color == PieceColor.Alb) &&
                !Helper.ExtraMove)
            {
                DisplayRegularMoves(square);
            }
        }

        public void MovePiece(GameSquare square)
        {
            square.Piece = Helper.CurrentSquare.Piece; //primesc eroare aici daca apas de 2 ori pe aceeasi piesa
            square.Piece.Square = square;

            if (Helper.CurrentNeighbours[square] != null)
            {
                Helper.CurrentNeighbours[square].Piece = null;
                Helper.ExtraMove = true;
            }
            else
            {
                Helper.ExtraMove = false;
                SwitchTurns(Helper.CurrentSquare);
            }

            board[Helper.CurrentSquare.Row][ Helper.CurrentSquare.Column].Texture = Helper.redSquare;

            foreach (GameSquare selectedSquare in Helper.CurrentNeighbours.Keys)
            {
                selectedSquare.LegalSquareSymbol = null;
            }
            Helper.CurrentNeighbours.Clear();
            Helper.CurrentSquare.Piece = null;
            Helper.CurrentSquare = null;

            if (square.Piece.Type == PieceStatus.Normal)
            {
                if (square.Row == 0 && square.Piece.Color == PieceColor.Rosu)
                {
                    square.Piece.Type = PieceStatus.Rege;
                    square.Piece.Texture = Helper.redKingPiece;
                }
                else if (square.Row == board.Count - 1 && square.Piece.Color == PieceColor.Alb)
                {
                    square.Piece.Type = PieceStatus.Rege;
                    square.Piece.Texture = Helper.whiteKingPiece;
                }
            }

            if (Helper.ExtraMove)
            {
                if (playerTurn.TurnImage == Helper.redPiece)
                {
                    Helper.CollectedWhitePieces++;
                }
                if (playerTurn.TurnImage == Helper.whitePiece)
                {
                    Helper.CollectedRedPieces++;
                }
                DisplayRegularMoves(square);
            }

            if (Helper.CollectedRedPieces == 12 ||  Helper.CollectedWhitePieces == 12)
            {
                GameOver();
            }
            UpdateRemainingPieces();

        }

        public void GameOver()
        {
            Winner aux = Helper.getScore();
            if (Helper.CollectedRedPieces == 12)
            {
                Helper.writeScore(aux.RedWins, ++aux.WhiteWins);
            }
            if (Helper.CollectedWhitePieces == 12)
            {
                Helper.writeScore(++aux.RedWins, aux.WhiteWins);
            }
            winner.RedWins = aux.RedWins;
            winner.WhiteWins = aux.WhiteWins;
            Helper.CollectedRedPieces = 0;
            Helper.CollectedWhitePieces = 0;
            MessageBox.Show("You won! You are the best <3");
            Helper.ResetGame(board);
        }
        #endregion 
    }
}
