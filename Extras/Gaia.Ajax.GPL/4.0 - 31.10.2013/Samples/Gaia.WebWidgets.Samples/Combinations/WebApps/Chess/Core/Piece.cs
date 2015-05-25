namespace Gaia.WebWidgets.Samples.Combinations.WebApps.Chess.Core
{
    public struct Piece
    {
        public static readonly Piece Empty;

        public Piece(PieceColor color, PieceName name)
        {
            _color = color;
            _name = name;
            _hasMoved = false;
        }

        #region [-- Private Fields --]

        private PieceColor _color;
        private PieceName _name;
        private bool _hasMoved;

        #endregion

        #region [-- Public Properties --]

        public PieceColor Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public PieceName Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool HasMoved
        {
            get { return _hasMoved; }
            set { _hasMoved = value; }
        }

        public bool IsEmpty
        {
            get { return (_color == 0 && _name == 0); }
        }

        #endregion

        public string GenerateId()
        {
            return string.Format("{0}-{1}", _color.ToString().ToLower(), _name.ToString().ToLower());
        }

       
    }
}