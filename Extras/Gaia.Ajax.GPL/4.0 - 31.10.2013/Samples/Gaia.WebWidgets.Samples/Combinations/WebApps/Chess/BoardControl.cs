namespace Gaia.WebWidgets.Samples.Combinations.WebApps.Chess
{
    using Core;
    using Utilities;
    using System;
    using System.Web.UI;

    public class BoardControl : Panel
    {
        private readonly Board _board;

        public BoardControl(Board board, EventHandler<MoveMadeEventArgs> moveMadeEventHandler)
        {
            _board = board;
            MoveMade += moveMadeEventHandler;
        }

        protected override void OnInit(EventArgs e)
        {
            EnsureChildControls();

            base.OnInit(e);           
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            for (int i = 8; i >= 1; --i)
            {
                for (char c = 'a'; c <= 'h'; ++c)
                {
                    var square = _board.GetSquare(string.Format("{0}{1}", c, i));
                    var squareControl = new SquareControl(square, SquareControlMoveMade); //todo: movemade .. 

                    if (!square.Piece.IsEmpty)
                        squareControl.Controls.Add(new PieceControl(square));    

                    Controls.Add(squareControl);
                }

                Controls.Add(new LiteralControl("<br style=\"clear:both;\" />"));
            }


            base.CreateChildControls();
        }

        Control Find(string pieceId)
        {
            return WebUtility.FindControl(this, pieceId);
        }

        public class MoveMadeEventArgs : EventArgs
        {
            public SquareControl FromSquare;
            public SquareControl ToSquare;
            public Piece MovedPiece;
        }

        public event EventHandler<MoveMadeEventArgs> MoveMade;

        public void DoMove(SquareControl from, SquareControl to)
        {
            // get the piece moved and update it's square
            PieceControl fromPiece = from.GetPieceControl();
            fromPiece.ResetSquare(to.Square);

            //bug: # changes state when capturing piece, but keeps the old one even though it's cleared!

            // move the control to it's new container
            from.Controls.Clear();
            to.Controls.Clear();
            to.Controls.Add(fromPiece);

        }

        void SquareControlMoveMade(object sender, SquareControl.MoveMadeEventArgs e)
        {
            var args = new MoveMadeEventArgs
                           {
                               ToSquare = e.TargetPanel as SquareControl,
                               FromSquare = Find(e.SourceSquareId) as SquareControl,
                               MovedPiece = _board.GetPiece(e.PieceId)
                           };

            if (MoveMade != null)
                MoveMade(this, args);
        }

    }
}