namespace Gaia.WebWidgets.Samples.Combinations.WebApps.Chess.Core
{
    using System;
    using System.Collections.Generic;

    public class Move
    {
        public Move(Square[] board, Piece piece, Square from, Square to)
        {
            _board = board;
            _piece = piece;
            _from = from;
            _to = to;
        }

        #region [-- Private Fields --]

        private readonly Square[] _board;
        private Piece _piece;
        private Square _from;
        private Square _to;

        #endregion

        #region [-- Public Properties --]

        public Square From
        {
            get { return _from; }
            set { _from = value; }
        }

        public Square To
        {
            get { return _to; }
            set { _to = value; }
        }

        public Piece Piece
        {
            get { return _piece; }
            set { _piece = value; }
        }

        public bool IsValid
        {
            get
            {
                if (_from.Equals(_to) || !_to.IsValid || (_to.IsOccupied && _to.Piece.Color != _piece.Color && _to.Piece.Name == PieceName.King))
                    return false;

                Square kingSquare = Array.Find(_board, delegate(Square s)
                {
                    return s.IsOccupied && s.Piece.Color == _piece.Color && s.Piece.Name == PieceName.King;
                });

                if (kingSquare != null && isKingThreatened(kingSquare))
                    return false;

                switch (_piece.Name)
                {
                    case PieceName.Bishop:
                        return isValidBishopMove;
                    case PieceName.King:
                        return isValidKingMove;
                    case PieceName.Knight:
                        return isValidKnightMove;
                    case PieceName.Pawn:
                        return isValidPawnMove;
                    case PieceName.Queen:
                        return isValidRookMove || isValidBishopMove;
                    case PieceName.Rook:
                        return isValidRookMove;
                    default:
                        return false;
                }
            }
        }

        #endregion

        #region [-- Private Properties --]

        private bool isValidPawnMove
        {
            get
            {
                List<Square> possibleMoves = new List<Square>();

                Square nextTopSquare = _from.GetNextSquare(_board, _piece.Color, Direction.Top);
                possibleMoves.Add(nextTopSquare);

                if (!_piece.HasMoved && !nextTopSquare.IsOccupied)
                    possibleMoves.Add(nextTopSquare.GetNextSquare(_board, _piece.Color, Direction.Top));

                Square nextTopRightSquare = _from.GetNextSquare(_board, _piece.Color, Direction.TopRight);

                if (nextTopRightSquare.IsOccupied && (nextTopRightSquare.Piece.Color != _piece.Color))
                    possibleMoves.Add(nextTopRightSquare);

                possibleMoves.RemoveAll(delegate(Square square)
                {
                    return square.IsEmpty || (square.IsOccupied && (square.Piece.Color == _piece.Color));
                });

                return (possibleMoves.Find(delegate(Square square) { return square.Equals(_to); }) != null);
            }
        }

        private bool isValidKnightMove
        {
            get
            {
                List<Square> possibleMoves = _from.GetPossibleKnightMoves(_board, _piece.Color);

                possibleMoves.RemoveAll(delegate(Square square)
                                            {
                                                return square.IsEmpty || (square.IsOccupied && (square.Piece.Color == _piece.Color));
                                            });

                return (possibleMoves.Find(delegate(Square square) { return square.Equals(_to); }) != null);
            }
        }

        private bool isValidRookMove
        {
            get
            {
                List<Square> possibleMoves = _from.GetPossibleRookMoves(_board, _piece.Color);
                return (possibleMoves.Find(delegate(Square square) { return square.Equals(_to); }) != null);
            }
        }

        private bool isValidBishopMove
        {
            get
            {
                List<Square> possibleMoves = _from.GetPossibleBishopMoves(_board, _piece.Color);
                return (possibleMoves.Find(delegate(Square square) { return square.Equals(_to); }) != null);
            }
        }

        private bool isValidKingMove
        {
            get
            {
                if (isKingThreatened(_to))
                    return false;

                List<Square> possibleMoves = _from.GetPossibleKingMoves(_board, _piece.Color);

                possibleMoves.RemoveAll(delegate(Square square)
                {
                    return square.IsEmpty || (square.IsOccupied && (square.Piece.Color == _piece.Color));
                });

                return (possibleMoves.Find(delegate(Square square) { return square.Equals(_to); }) != null)
                       || isValidShortCastling || isValidLongCastling;
            }
        }

        private bool isValidShortCastling
        {
            get
            {
                if (_piece.Name != PieceName.King || _piece.HasMoved)
                    return false;

                Square shortCastlingSquare = (_piece.Color == PieceColor.White) ?
                                                                                    _from.GetNextSquare(_board, _piece.Color, Direction.Right).GetNextSquare(_board, _piece.Color, Direction.Right) :
                                                                                                                                                                                                        _from.GetNextSquare(_board, _piece.Color, Direction.Left).GetNextSquare(_board, _piece.Color, Direction.Left);

                if (!_to.Equals(shortCastlingSquare))
                    return false;

                Square rookSquare = (_piece.Color == PieceColor.White) ?
                                                                           Array.Find(_board, delegate(Square s) { return s.GenerateID() == "h1"; }) :
                                                                                                                                                         Array.Find(_board, delegate(Square s) { return s.GenerateID() == "h8"; });

                if (!rookSquare.IsOccupied || rookSquare.Piece.HasMoved)
                    return false;

                Square[] inBetween = (_piece.Color == PieceColor.White) ?
                                                                            inBetween = Array.FindAll(_board, delegate(Square s)
                {
                    return s.ToString().ToLower() == "f1" || s.GenerateID() == "g1";
                }) :
                                                                                   inBetween = Array.FindAll(_board, delegate(Square s)
                {
                    return s.ToString().ToLower() == "f8" || s.GenerateID() == "g8";
                });

                if (Array.TrueForAll(inBetween, delegate(Square s) { return !s.IsOccupied; }))
                    return true;

                return false;
            }
        }

        private bool isValidLongCastling
        {
            get
            {
                if (_piece.Name != PieceName.King || _piece.HasMoved)
                    return false;

                Square longCastlingSquare = (_piece.Color == PieceColor.White) ?
                                                                                   _from.GetNextSquare(_board, _piece.Color, Direction.Left).GetNextSquare(_board, _piece.Color, Direction.Left) :
                                                                                                                                                                                                     _from.GetNextSquare(_board, _piece.Color, Direction.Right).GetNextSquare(_board, _piece.Color, Direction.Right);

                if (!_to.Equals(longCastlingSquare))
                    return false;

                Square rookSquare = (_piece.Color == PieceColor.White) ?
                                                                           Array.Find(_board, delegate(Square s) { return s.GenerateID() == "a1"; }) :
                                                                                                                                                        Array.Find(_board, delegate(Square s) { return s.GenerateID() == "a8"; });

                if (!rookSquare.IsOccupied || rookSquare.Piece.HasMoved)
                    return false;

                Square[] inBetween = (_piece.Color == PieceColor.White) ?
                                                                            inBetween = Array.FindAll(_board, delegate(Square s)
                {
                    return s.GenerateID() == "b1" || s.GenerateID() == "c1" || s.GenerateID() == "d1";
                }) :
                                                                                   inBetween = Array.FindAll(_board, delegate(Square s)
                {
                    return s.GenerateID() == "b8" || s.GenerateID() == "c8" || s.GenerateID() == "c8";
                });

                if (Array.TrueForAll(inBetween, delegate(Square s) { return !s.IsOccupied; }))
                    return true;

                return false;
            }
        }

        private bool isKingThreatened(Square target)
        {
            List<Square> possiblePawnThreats = new List<Square>(new Square[]{
                                                                                target.GetNextSquare(_board, _piece.Color, Direction.TopRight),
                                                                                target.GetNextSquare(_board, _piece.Color, Direction.TopLeft)
                                                                            });

            List<Square> possibleKingThreats = target.GetPossibleKingMoves(_board, _piece.Color);
            List<Square> possibleRookThreats = target.GetPossibleRookMoves(_board, _piece.Color);
            List<Square> possibleBishopThreats = target.GetPossibleBishopMoves(_board, _piece.Color);
            List<Square> possibleKnightThreats = target.GetPossibleKnightMoves(_board, _piece.Color);

            if (possiblePawnThreats.Find(delegate(Square s)
                                             {
                                                 return (!s.IsEmpty && s.IsOccupied && s.Piece.Color != _piece.Color && s.Piece.Name == PieceName.Pawn);
                                             }) != null)
                return true;

            if (possibleKingThreats.Find(delegate(Square s)
            {
                return (!s.IsEmpty && s.IsOccupied && s.Piece.Color != _piece.Color && s.Piece.Name == PieceName.King);
            }) != null)
                return true;

            if (possibleRookThreats.Find(delegate(Square s)
            {
                return (!s.IsEmpty && s.IsOccupied && s.Piece.Color != _piece.Color && (s.Piece.Name == PieceName.Rook || s.Piece.Name == PieceName.Queen));
            }) != null)
                return true;

            if (possibleBishopThreats.Find(delegate(Square s)
            {
                return (!s.IsEmpty && s.IsOccupied && s.Piece.Color != _piece.Color && (s.Piece.Name == PieceName.Bishop || s.Piece.Name == PieceName.Queen));
            }) != null)
                return true;

            if (possibleKnightThreats.Find(delegate(Square s)
            {
                return (!s.IsEmpty && s.IsOccupied && s.Piece.Color != _piece.Color && s.Piece.Name == PieceName.Knight);
            }) != null)
                return true;

            return false;
        }

        #endregion

        #region [-- Public Methods --]

        public void Commit()
        {
            if (isValidShortCastling)
            {
                Square rookFromSquare = (_piece.Color == PieceColor.White) ?
                                                                               Array.Find(_board, delegate(Square s) { return s.GenerateID() == "h1"; }) :
                                                                                                                                                             Array.Find(_board, delegate(Square s) { return s.GenerateID() == "h8"; });

                Square rookToSquare = (_piece.Color == PieceColor.White) ?
                                                                             rookFromSquare.GetNextSquare(_board, _piece.Color, Direction.Left).GetNextSquare(_board, _piece.Color, Direction.Left) :
                                                                                                                                                                                                       rookFromSquare.GetNextSquare(_board, _piece.Color, Direction.Right).GetNextSquare(_board, _piece.Color, Direction.Right);

                Piece rook = rookFromSquare.Piece;
                rook.HasMoved = true;
                rookToSquare.Piece = rook;
                rookFromSquare.Piece = Piece.Empty;
            }
            if (isValidLongCastling)
            {
                Square rookFromSquare = (_piece.Color == PieceColor.White) ?
                                                                               Array.Find(_board, delegate(Square s) { return s.GenerateID() == "a1"; }) :
                                                                                                                                                             Array.Find(_board, delegate(Square s) { return s.GenerateID() == "a8"; });

                Square rookToSquare = (_piece.Color == PieceColor.White) ?
                                                                             rookFromSquare.GetNextSquare(_board, _piece.Color, Direction.Right)
                                                                                 .GetNextSquare(_board, _piece.Color, Direction.Right).GetNextSquare(_board, _piece.Color, Direction.Right) :
                                                                                                                                                                                                rookFromSquare.GetNextSquare(_board, _piece.Color, Direction.Left)
                                                                                                                                                                                                    .GetNextSquare(_board, _piece.Color, Direction.Left).GetNextSquare(_board, _piece.Color, Direction.Left);

                Piece rook = rookFromSquare.Piece;
                rook.HasMoved = true;
                rookToSquare.Piece = rook;
                rookFromSquare.Piece = Piece.Empty;
            }

            _piece.HasMoved = true;
            _from.Piece = Piece.Empty;
            _to.Piece = _piece;
        }

        #endregion
    }
}