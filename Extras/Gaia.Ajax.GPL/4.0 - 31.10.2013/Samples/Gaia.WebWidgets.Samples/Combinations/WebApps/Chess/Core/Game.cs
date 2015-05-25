namespace Gaia.WebWidgets.Samples.Combinations.WebApps.Chess.Core
{
    using System.Collections.Generic;

    public class Game
    {
        public Game(Player player1, Player player2)
        {
            _players = new Player[2];
            _players[0] = player1;
            _players[1] = player2;
            _moves = new List<Move>();

            CurrentTurn = PieceColor.White;

            _board = new Board();
        }

        private PieceColor _side;

        public PieceColor CurrentTurn
        {
            get { return _side; }
            set { _side = value; }
        }

        public void ToggleSide()
        {
            CurrentTurn = CurrentTurn == PieceColor.Black ? PieceColor.White : PieceColor.Black;
        }

        #region [-- Private Fields --]

        private Player[] _players;
        private Board _board;
        private List<Move> _moves;

        #endregion

        #region [-- Public Properties --]

        public Player[] Players
        {
            get { return _players; }
            set { _players = value; }
        }

        public Board Board
        {
            get { return _board; }
            set { _board = value; }
        }

        public List<Move> Moves
        {
            get { return _moves; }
            set { _moves = value; }
        }

        #endregion
    }
}