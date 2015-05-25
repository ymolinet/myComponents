namespace Gaia.WebWidgets.Samples.Combinations.WebApps.Chess
{
    using System;
    using Core;
    using Utilities;

    public class SquareControl : Panel
    {
        private readonly Square _square;

        public Square Square { get { return _square; } }

        public SquareControl(Square square, EventHandler<MoveMadeEventArgs> moveMadeHandler)
        {
            _square = square;
            MoveMade += moveMadeHandler;
            Dropped += OnDropped;
        }

        protected override void OnInit(EventArgs e)
        {
            ID = _square.GenerateID();

            if (_square.Rank % 2 == 1)
                CssClass = (_square.File % 2 == 1) ? "chess-black-square" : "chess-white-square";
            else
                CssClass = (_square.File % 2 == 1) ? "chess-white-square" : "chess-black-square";

            base.OnInit(e);
        }

        public class MoveMadeEventArgs : EventArgs
        {
            private readonly Panel _targetPanel;
            private readonly string _sourceSquareId;
            private readonly string _targetSquareId;
            private readonly string _id;

            public bool? Revert { get; set; }

            internal MoveMadeEventArgs(Panel targetPanel, string sourceSquareId, string targetSquareId, string id)
            {
                _targetPanel = targetPanel;
                _sourceSquareId = sourceSquareId;
                _targetSquareId = targetSquareId;
                _id = id;
            }

            public Panel TargetPanel
            {
                get { return _targetPanel; }
            }

            public string SourceSquareId
            {
                get { return _sourceSquareId; }
            }

            public string TargetSquareId
            {
                get { return _targetSquareId; }
            }

        
            public string PieceId
            {
                get
                {
                    bool isWhite = _id.LastIndexOf("white") != -1;
                    string color = isWhite ? "white" : "black";
                    return _id.Substring(_id.LastIndexOf(color));
                }
            }

           
        }

        event EventHandler<AspectDroppable.DroppedEventArgs> Dropped
        {
            add { Aspects.Bind<AspectDroppable>().Dropped += value; }
            remove { Aspects.Bind<AspectDroppable>().Dropped -= value; }
        }

        public event EventHandler<MoveMadeEventArgs> MoveMade;

        public PieceControl GetPieceControl()
        {
            return WebUtility.First<PieceControl>(Controls);
        }
     
        private void OnDropped(object sender, AspectDroppable.DroppedEventArgs e)
        {
            var toPanel = sender as Panel;
            if (toPanel ==null)
                return;

            var args = new MoveMadeEventArgs(
                toPanel, /* target panel */
                e.DraggedID.Substring(e.DraggedID.Length - 2, 2), /* last 2 chars should contain file and rank */
                toPanel.ID, /* target square id*/
                e.DraggedID /* id of the dragged control */);

            if (MoveMade != null) MoveMade(this,args);
        }
    }
}