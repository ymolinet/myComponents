namespace Gaia.WebWidgets.Samples.Core.DynamicControls.DynamicInserts
{
    using System;
    using System.Collections.Generic;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Default :SamplePage
    {
        
        void PopulateAllFlags()
        {
            CurrentFlags.Clear();
            zContainer.Controls.Clear();

            for (int i = 0; i < Flags.Count; i++)
                CurrentFlags.Add(Flags[i]);
            
            AddFlagControls();
        }

        void AddFlagControls()
        {
            for (int i = 0; i < CurrentFlags.Count; i++)
                zContainer.Controls.Add(CreateImage(CurrentFlags[i]));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AddFlagControls();
        }

        private static List<MediaImage> _flags;
        private static List<MediaImage> Flags
        {
            get { return _flags ?? (_flags = new List<MediaImage>(MediaUtility.GetImageFiles("flags"))); }
        }

        bool UseForceUpdate
        {
            get { return int.Parse(zRenderOption.SelectedValue) == 0; }
        }

        Image CreateImage(MediaImage mediaInfo)
        {
            Image image = new Image();
            image.ID = mediaInfo.Text;
            image.ImageUrl = mediaInfo.ImageUrl;
            image.AlternateText = mediaInfo.Text;
            image.Click += delegate
                               {
                                   zContainer.Controls.Remove(image);
                                   CurrentFlags.Remove(mediaInfo);

                                   if (UseForceUpdate)
                                       zContainer.ForceAnUpdate();

                               }; // Add support for removing a control
            return image;
        }

        private List<MediaImage> CurrentFlags
        {
            get
            {
                if (Session["flags"] == null) Session["flags"] = new List<MediaImage>();
                return Session["flags"] as List<MediaImage>;
            }
        }

        MediaImage GetNewMediaInfo()
        {
            // Check if we have retrieved all flags
            if (CurrentFlags.Count == Flags.Count)
                return null; 

            // Retrieve the flags left unrendered
            List<MediaImage> flagsLeft = Flags.FindAll(delegate(MediaImage media) {
                                                                                    return !CurrentFlags.Contains(media);
            });

            return flagsLeft[new Random().Next(0, flagsLeft.Count)];
        }

        protected void zTimer_Tick(object sender, EventArgs e)
        {
            // We stop the timer if we have rendered all the flags
            if (zContainer.Controls.Count == Flags.Count)
                TurnOnTimer(false);

            CreateRandomFlag();
            if (UseForceUpdate)
                zContainer.ForceAnUpdate();
        }

        private void CreateRandomFlag()
        {
            MediaImage newFlag = GetNewMediaInfo();
            if(newFlag == null)
                return;

            int randomPosition = new Random().Next(0, CurrentFlags.Count);
            CurrentFlags.Insert(randomPosition, newFlag);
            Image image = CreateImage(newFlag);
            //image.Effects.Add(new Gaia.WebWidgets.Effects.EffectScale(120));
            zContainer.Controls.AddAt(randomPosition, image);
        }

        protected void zButtonStartStop_Click(object sender, EventArgs e)
        {
            TurnOnTimer(!zTimer.Enabled);
        }

        void TurnOnTimer(bool on)
        {
            zTimer.Enabled = on;
            zButtonStart.Visible = !on;
            zButtonStop.Visible = on;
        }

        protected void zButtonClear_Click(object sender, EventArgs e)
        {
            zContainer.Controls.Clear();
            CurrentFlags.Clear();
            if (UseForceUpdate)
                zContainer.ForceAnUpdate();

        }

        protected void zButtonManualAdd_Click(object sender, EventArgs e)
        {
            CreateRandomFlag();
            if (UseForceUpdate)
                zContainer.ForceAnUpdate();

        }

        protected void zButtonRenderAll_Click(object sender, EventArgs e)
        {
            PopulateAllFlags();
            if (UseForceUpdate)
                zContainer.ForceAnUpdate();

        }
    }
}