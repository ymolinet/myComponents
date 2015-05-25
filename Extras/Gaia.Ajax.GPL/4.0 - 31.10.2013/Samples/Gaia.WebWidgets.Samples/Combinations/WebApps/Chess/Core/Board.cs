namespace Gaia.WebWidgets.Samples.Combinations.WebApps.Chess.Core
{
    using System;
    using System.Collections.Generic;

    public class Board
    {
        public Board()
        {
            List<Square> squares = new List<Square>();
            
            for (int i = 1; i <= 8; ++i)
                for (char c = 'a'; c <= 'h'; ++c)
                    squares.Add(new Square(i, c));

            AddPieces(squares);

            _squares = squares.ToArray();
        }


        #region [-- Private Fields --]

        private Square[] _squares;

        #endregion

        #region [-- Public Properties --]

        public Square[] Squares
        {
            get { return _squares; }
            set { _squares = value; }
        }

        #endregion

        #region [-- Private Methods --]

        private static void AddPieces(List<Square> squares)
        {
            //Adding Pawns
            squares.FindAll(delegate(Square square)
                                {
                                    return square.Rank == 2 || square.Rank == 7;
                                })
                    .ForEach(delegate(Square square) 
                        {
                            square.Piece = new Piece((square.Rank == 2) ? PieceColor.White : PieceColor.Black, PieceName.Pawn);
                    }   );

            //Adding Rooks
            squares.FindAll(delegate(Square square) { return square.Rank == 1 || square.Rank == 8; })
                .FindAll(delegate(Square square) { return square.File == 'a' || square.File == 'h'; })
                .ForEach(delegate(Square square) {
                                                     square.Piece = new Piece((square.Rank == 1) ? PieceColor.White : PieceColor.Black, PieceName.Rook);
                });

            //Adding Knights
            squares.FindAll(delegate(Square square) { return square.Rank == 1 || square.Rank == 8; })
                .FindAll(delegate(Square square) { return square.File == 'b' || square.File == 'g'; })
                .ForEach(delegate(Square square) {
                                                     square.Piece = new Piece((square.Rank == 1) ? PieceColor.White : PieceColor.Black, PieceName.Knight);
                });

            //Adding Bishops
            squares.FindAll(delegate(Square square) { return square.Rank == 1 || square.Rank == 8; })
                .FindAll(delegate(Square square) { return square.File == 'c' || square.File == 'f'; })
                .ForEach(delegate(Square square) {
                                                     square.Piece = new Piece((square.Rank == 1) ? PieceColor.White : PieceColor.Black, PieceName.Bishop);
                });

            //Adding Queens
            squares.FindAll(delegate(Square square) { return square.Rank == 1 || square.Rank == 8; })
                .FindAll(delegate(Square square) { return square.File == 'd'; })
                .ForEach(delegate(Square square) {
                                                     square.Piece = new Piece((square.Rank == 1) ? PieceColor.White : PieceColor.Black, PieceName.Queen);
                });

            //Adding Kings
            squares.FindAll(delegate(Square square) { return square.Rank == 1 || square.Rank == 8; })
                .FindAll(delegate(Square square) { return square.File == 'e'; })
                .ForEach(delegate(Square square) {
                                                     square.Piece = new Piece((square.Rank == 1) ? PieceColor.White : PieceColor.Black, PieceName.King);
                });
        }

        #endregion
        
        public Square GetSquare(string id)
        {
            return Array.Find(_squares, delegate(Square square) { return square.GenerateID() == id; });
        }

        public Piece GetPiece(string id)
        {
            Square square = Array.Find(_squares, delegate(Square s) { 
                                                                        return s.IsOccupied && (s.Piece.GenerateId() + "-" + s.GenerateID() == id); 
            });
            if (square != null)
                return square.Piece;
            return Piece.Empty;
        }

    }
}