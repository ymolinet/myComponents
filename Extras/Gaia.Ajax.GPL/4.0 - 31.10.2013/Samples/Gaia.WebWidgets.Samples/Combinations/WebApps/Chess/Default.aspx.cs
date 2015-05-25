using Gaia.WebWidgets.Effects;

namespace Gaia.WebWidgets.Samples.Combinations.WebApps.Chess
{
    using System;
    using Core;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        private BoardControl _board;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack) 
                return;
            
            Game = new Game(new Player(string.Empty, true, PieceColor.White),
                            new Player(string.Empty, false, PieceColor.Black));

            SetCurrentTurnImage();
        }

        private void SetCurrentTurnImage()
        {
            zWhosNext.ImageUrl = Game.CurrentTurn == PieceColor.White ? 
                "img/white_pawn.png" : 
                "img/black_pawn.png";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeChessBoard();
        }

        private void InitializeChessBoard()
        {
            _board = new BoardControl(Game.Board, MoveMade);
            _board.ID = "b";
            _board.CssClass = "chess-board";

            c.Controls.Add(_board);
        }

        void MoveMade(object sender, BoardControl.MoveMadeEventArgs e)
        {
            // Check for correct sourcePanel. We could risk having something else dropped here
            if (e.FromSquare == null)
                return;

            const decimal delay = 0.1M;
            const decimal duration = 0.4M;
            EffectMove effectMove = new Gaia.WebWidgets.Effects.EffectMove(0, 0, duration, delay,
                EffectMove.ModeEnum.Absolute, Easing.EaseInOutElastic);
           
            PieceControl pieceControl = e.FromSquare.GetPieceControl();
            if (e.MovedPiece.Color != Game.CurrentTurn)
            {
                pieceControl.Effects.Add(effectMove);
                return;
            }
            
            Move move = new Move(Game.Board.Squares, e.MovedPiece, e.FromSquare.Square, e.ToSquare.Square);

            if (!move.IsValid)
            {
                pieceControl.Effects.Add(effectMove);
                return;
            }

            move.Commit();
            Game.Moves.Add(move);
            _board.DoMove(e.FromSquare, e.ToSquare);

            Game.ToggleSide();
            SetCurrentTurnImage();

            pieceControl.Style["left"] = "0";
            pieceControl.Style["top"] = "0";

        }

        protected Game Game
        {
            get { return Session["currentChessGame"] as Game; }
            set { Session["currentChessGame"] = value; }
        }

      
    }
}
