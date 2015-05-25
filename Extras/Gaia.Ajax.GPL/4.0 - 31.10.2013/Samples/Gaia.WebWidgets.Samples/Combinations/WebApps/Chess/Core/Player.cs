namespace Gaia.WebWidgets.Samples.Combinations.WebApps.Chess.Core
{
    using System.Collections.Generic;

    public class Player
    {
        public Player(string nick, bool owner, PieceColor side)
        {
            _nick = nick;
            _owner = owner;
            _side = side;

            _capturedPieces = new List<Piece>();
        }

        #region [-- Private Fields --]

        private string _nick;

        private bool _owner;

        private PieceColor _side;

        private List<Piece> _capturedPieces;

        #endregion

        #region [-- Public Properties --]

        public string Nick
        {
            get { return _nick; }
            set { _nick = value; }
        }

        public bool IsOwner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public PieceColor Side
        {
            get { return _side; }
            set { _side = value; }
        }

        public List<Piece> CapturedPieces
        {
            get { return _capturedPieces; }
            set { _capturedPieces = value; }
        }

        #endregion
    }
}