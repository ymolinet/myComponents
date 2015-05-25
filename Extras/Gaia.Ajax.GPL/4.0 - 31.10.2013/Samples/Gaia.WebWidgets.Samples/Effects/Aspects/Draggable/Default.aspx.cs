namespace Gaia.WebWidgets.Samples.Effects.Aspects.Draggable
{
    using System;
    using Gaia.WebWidgets.Samples.UI;


    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitPanel1();
            InitPanel2();
            InitPanel3();
        }

        void InitPanel1()
        {
            AddDraggable(zPanel1);
        }

        #region EffectEventReverting
        void InitPanel2()
        {
            AddDraggable(zPanel2);
            var effectMove = new Gaia.WebWidgets.Effects.EffectMove
                                 {
                                     Duration = 1.2M
                                 };
            zPanel2.Effects.Add(AspectDraggable.EffectEventReverting, effectMove);
        } 
        #endregion

        void InitPanel3()
        {
            AddDraggable(zPanel3);

            var scaleEffect = new Gaia.WebWidgets.Effects.EffectScale(120);
            scaleEffect.ScaleMode = "box";
            zPanel3.Effects.Add(
                AspectDraggable.EffectEventStartDragging,
                new Gaia.WebWidgets.Effects.EffectParallel(
                    scaleEffect,
                    new Gaia.WebWidgets.Effects.EffectMorph("backgroundColor: #fcc", 0.3M)));

            const string morphTo = "backgroundColor: #ffc; height: 60px; width: 150px;";
            zPanel3.Effects.Add(
                AspectDraggable.EffectEventEndDragging,
                new Gaia.WebWidgets.Effects.EffectMorph(morphTo, 0.3M));

            zPanel3.Effects.Add(AspectDraggable.EffectEventReverting,
                                new Gaia.WebWidgets.Effects.EffectMove());

        }

        static void AddDraggable(params IAjaxContainerControl[] controls)
        {
            foreach (IAjaxContainerControl a in controls)
            {
                a.Aspects.Add(new Gaia.WebWidgets.AspectDraggable {Revert = true});
            }
        }
    }
}