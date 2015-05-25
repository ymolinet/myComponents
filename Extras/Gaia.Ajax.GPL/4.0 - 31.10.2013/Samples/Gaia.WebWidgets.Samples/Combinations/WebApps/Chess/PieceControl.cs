namespace Gaia.WebWidgets.Samples.Combinations.WebApps.Chess
{
    using Core;
    using WebWidgets.Effects;

    public class PieceControl : Image
    {
        private Square _square;

        public void ResetSquare(Square newSquare)
        {
            _square = newSquare;
            SetId();
        }

        public PieceControl(Square square)
        {
            _square = square;
        }

        protected override void OnInit(System.EventArgs e)
        {
            SetId();
            ImageUrl = string.Format("img/{0}.png", _square.Piece.GenerateId().Replace("-", "_"));

            var draggable = new AspectDraggable
                                {
                                    Silent = true,
                                    HitEffect = true,
                                    UseDocumentBody = true,
                                    DeepCopy = true,
                                    MakeGhost = false,
                                    Revert = true
                                };
            Aspects.Add(draggable);

            const decimal delay = 0.1M;
            const decimal duration = 0.4M;
            var effectMove = new EffectMove(0, 0, duration, delay, EffectMove.ModeEnum.Relative,
                                            Easing.EaseInBounce);
            Effects.Add(AspectDraggable.EffectEventReverting, effectMove);

            base.OnInit(e);
        }

        private void SetId()
        {
            ID = string.Format("{0}-{1}", 
                               _square.Piece.GenerateId() /* piece id first */
                               ,_square.GenerateID());
        }
    }
}