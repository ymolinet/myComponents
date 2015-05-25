namespace Gaia.WebWidgets.Samples.Combinations.WebApps.Chess.Core
{
    using System;
    using System.Collections.Generic;

    public class Square
    {
        public Square(int rank, char file) : this(rank, file, Piece.Empty)
        { }

        public Square(int rank, char file, Piece piece)
        {
            _rank = rank;
            _file = file;
            _piece = piece;
        }

        #region [-- Private Fields --]

        private int _rank;
        private char _file;
        private Piece _piece;

        #endregion

        #region [-- Public Properties --]

        public int Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }

        public char File
        {
            get { return _file; }
            set { _file = value; }
        }

        public Piece Piece
        {
            get { return _piece; }
            set { _piece = value; }
        }

        public static Square Empty
        {
            get { return new Square(0, '\0'); }
        }

        public bool IsEmpty
        {
            get { return _rank == 0 && _file == '\0'; }
        }

        public bool IsOccupied
        {
            get { return !_piece.IsEmpty; }
        }

        public bool IsValid
        {
            get { return (_file <= 'h' && _file >= 'a' && _rank <= 8 && _rank >= 1); }
        }

        #endregion

        #region [-- Public Methods --]

        public Square GetNextSquare(Square[] squares, PieceColor playerSide, Direction direction)
        {
            int rank = _rank;
            char file = _file;
            if (playerSide == PieceColor.White)
            {

                switch (direction)
                {
                    case Direction.Right:
                        return new Square(rank, ++file).GetActualSquare(squares);
                    case Direction.Left:
                        return new Square(rank, --file).GetActualSquare(squares);
                    case Direction.Bottom:
                        return new Square(--rank, file).GetActualSquare(squares);
                    case Direction.TopRight:
                        return new Square(++rank, ++file).GetActualSquare(squares);
                    case Direction.TopLeft:
                        return new Square(++rank, --file).GetActualSquare(squares);
                    case Direction.BottomRight:
                        return new Square(--rank, ++file).GetActualSquare(squares);
                    case Direction.BottomLeft:
                        return new Square(--rank, --file).GetActualSquare(squares);
                    case Direction.Top:
                    default:
                        return new Square(++rank, file).GetActualSquare(squares);
                }
            }
            else
            {
                switch (direction)
                {
                    case Direction.Right:
                        return new Square(rank, --file).GetActualSquare(squares);
                    case Direction.Left:
                        return new Square(rank, ++file).GetActualSquare(squares);
                    case Direction.Bottom:
                        return new Square(++rank, file).GetActualSquare(squares);
                    case Direction.TopRight:
                        return new Square(--rank, --file).GetActualSquare(squares);
                    case Direction.TopLeft:
                        return new Square(--rank, ++file).GetActualSquare(squares);
                    case Direction.BottomRight:
                        return new Square(++rank, --file).GetActualSquare(squares);
                    case Direction.BottomLeft:
                        return new Square(++rank, ++file).GetActualSquare(squares);
                    case Direction.Top:
                    default:
                        return new Square(--rank, file).GetActualSquare(squares);
                }
            }
        }

        private Square GetActualSquare(Square[] squares)
        {
            if (IsValid)
                return Array.Find(squares, delegate(Square s) { return this.Equals(s); });
            return Square.Empty;
        }

        public List<Square> GetPossibleKnightMoves(Square[] board, PieceColor side)
        {
            return new List<Square>(new Square[] {
                                                     GetNextSquare(board, side, Direction.Top)
                                                         .GetNextSquare(board, side, Direction.Top).GetNextSquare(board, side, Direction.Right),
                
                                                     GetNextSquare(board, side, Direction.Top)
                                                         .GetNextSquare(board, side, Direction.Top).GetNextSquare(board, side, Direction.Left),

                                                     GetNextSquare(board, side, Direction.Top)
                                                         .GetNextSquare(board, side, Direction.Right).GetNextSquare(board, side, Direction.Right),

                                                     GetNextSquare(board, side, Direction.Top)
                                                         .GetNextSquare(board, side, Direction.Left).GetNextSquare(board, side, Direction.Left),

                                                     GetNextSquare(board, side, Direction.Bottom)
                                                         .GetNextSquare(board, side, Direction.Bottom).GetNextSquare(board, side, Direction.Right),

                                                     GetNextSquare(board, side, Direction.Bottom)
                                                         .GetNextSquare(board, side, Direction.Bottom).GetNextSquare(board, side, Direction.Left),

                                                     GetNextSquare(board, side, Direction.Bottom)
                                                         .GetNextSquare(board, side, Direction.Right).GetNextSquare(board, side, Direction.Right),

                                                     GetNextSquare(board, side, Direction.Bottom)
                                                         .GetNextSquare(board, side, Direction.Left).GetNextSquare(board, side, Direction.Left)
                                                 });
        }

        public List<Square> GetPossibleRookMoves(Square[] board, PieceColor side)
        {
            List<Square> possibleMoves = new List<Square>();

            Square topSquare = GetNextSquare(board, side, Direction.Top);
            Square leftSquare = GetNextSquare(board, side, Direction.Left);
            Square rightSquare = GetNextSquare(board, side, Direction.Right);
            Square bottomSquare = GetNextSquare(board, side, Direction.Bottom);

            while (!topSquare.IsEmpty && (!topSquare.IsOccupied || topSquare.Piece.Color != side))
            {
                possibleMoves.Add(topSquare);
                if (!topSquare.IsOccupied)
                    topSquare = topSquare.GetNextSquare(board, side, Direction.Top);
                else
                    break;
            }

            while (!leftSquare.IsEmpty && (!leftSquare.IsOccupied || leftSquare.Piece.Color != side))
            {
                possibleMoves.Add(leftSquare);
                if (!leftSquare.IsOccupied)
                    leftSquare = leftSquare.GetNextSquare(board, side, Direction.Left);
                else
                    break;
            }

            while (!rightSquare.IsEmpty && (!rightSquare.IsOccupied || rightSquare.Piece.Color != side))
            {
                possibleMoves.Add(rightSquare);
                if (!rightSquare.IsOccupied)
                    rightSquare = rightSquare.GetNextSquare(board, side, Direction.Right);
                else
                    break;
            }

            while (!bottomSquare.IsEmpty && (!bottomSquare.IsOccupied || bottomSquare.Piece.Color != side))
            {
                possibleMoves.Add(bottomSquare);
                if (!bottomSquare.IsOccupied)
                    bottomSquare = bottomSquare.GetNextSquare(board, side, Direction.Bottom);
                else
                    break;
            }

            return possibleMoves;
        }

        public List<Square> GetPossibleBishopMoves(Square[] board, PieceColor side)
        {
            List<Square> possibleMoves = new List<Square>();

            Square topRightSquare = GetNextSquare(board, side, Direction.TopRight);
            Square topLeftSquare = GetNextSquare(board, side, Direction.TopLeft);
            Square bottomRightSquare = GetNextSquare(board, side, Direction.BottomRight);
            Square bottomLeftSquare = GetNextSquare(board, side, Direction.BottomLeft);

            while (!topRightSquare.IsEmpty && (!topRightSquare.IsOccupied || topRightSquare.Piece.Color != side))
            {
                possibleMoves.Add(topRightSquare);
                if (!topRightSquare.IsOccupied)
                    topRightSquare = topRightSquare.GetNextSquare(board, side, Direction.TopRight);
                else
                    break;
            }

            while (!topLeftSquare.IsEmpty && (!topLeftSquare.IsOccupied || topLeftSquare.Piece.Color != side))
            {
                possibleMoves.Add(topLeftSquare);
                if (!topLeftSquare.IsOccupied)
                    topLeftSquare = topLeftSquare.GetNextSquare(board, side, Direction.TopLeft);
                else
                    break;
            }

            while (!bottomRightSquare.IsEmpty && (!bottomRightSquare.IsOccupied || bottomRightSquare.Piece.Color != side))
            {
                possibleMoves.Add(bottomRightSquare);
                if (!bottomRightSquare.IsOccupied)
                    bottomRightSquare = bottomRightSquare.GetNextSquare(board, side, Direction.BottomRight);
                else
                    break;
            }

            while (!bottomLeftSquare.IsEmpty && (!bottomLeftSquare.IsOccupied || bottomLeftSquare.Piece.Color != side))
            {
                possibleMoves.Add(bottomLeftSquare);
                if (!bottomLeftSquare.IsOccupied)
                    bottomLeftSquare = bottomLeftSquare.GetNextSquare(board, side, Direction.BottomLeft);
                else
                    break;
            }

            return possibleMoves;
        }

        public List<Square> GetPossibleKingMoves(Square[] board, PieceColor side)
        {
            return new List<Square>(new Square[] { 
                                                     GetNextSquare(board, side, Direction.Top), GetNextSquare(board, side, Direction.Bottom),
                                                     GetNextSquare(board, side, Direction.Right), GetNextSquare(board, side, Direction.Left),
                                                     GetNextSquare(board, side, Direction.TopRight), GetNextSquare(board, side, Direction.TopLeft),
                                                     GetNextSquare(board, side, Direction.BottomRight), GetNextSquare(board, side, Direction.BottomLeft)
                                                 });
        }

        #endregion

        #region [-- Overridden Methods --]

        public string GenerateID()
        {
            return String.Format("{0}{1}", _file, _rank);
        }

        public override bool Equals(object obj)
        {
            return (obj is Square) && ((obj as Square).File == _file) && ((obj as Square).Rank == _rank);
        }

        public override int GetHashCode()
        {
            return _rank.GetHashCode() ^ _file.GetHashCode() ^ _piece.Name.GetHashCode();
        }

        #endregion
    }
}