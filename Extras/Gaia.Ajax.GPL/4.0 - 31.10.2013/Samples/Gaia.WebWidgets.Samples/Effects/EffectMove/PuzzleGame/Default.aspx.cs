namespace Gaia.WebWidgets.Samples.Effects.EffectMove.PuzzleGame
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Gaia.WebWidgets.Effects;
    using Gaia.WebWidgets.Extensions;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : FxSamplePage
    {
        private int _wordLength;
        private const int Width = 150;
        private const int Height = 170;
        private const int StartTop = 300;
        private const int StartLeft = 250;
        private readonly Panel _windowContainer = new Panel();
        private readonly  Random _rnd = new Random();
 
        protected void zChallenge_SelectedIndexChanged(object sender, EventArgs e)
        {
            string wordChallenge = zChallenge.SelectedItem.Text;
            int height = wordChallenge.Split(' ').Length == 1 ? 220 : 440;
            zContentPanel.Height = height;
            PreviousChallengeIndex = zChallenge.SelectedIndex;
            InitializeWindows();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeWindows();
            zContentPanel.Controls.Add(_windowContainer);

            zWindowSuccess.Effects.Add(Window.EffectEventAppear, new EffectGrow());
        }

        private void InitializeWindows()
        {
            _windowContainer.Controls.Clear();
            string challenge = zChallenge.Items[PreviousChallengeIndex].Text;
            char[] chars = challenge.Replace(" ", "").ToCharArray();
            _wordLength = challenge.Split(' ')[0].Length;
            int count = chars.Length;
            var points = GetRandomPoints(count);

            for (int i = 0; i < count; i++)
            {
                var window = new Window
                                 {
                                     CssClass = StyleSheetTheme,
                                     Width = Width,
                                     Height = Height,
                                     CenterInForm = false,
                                     Maximizable = false,
                                     Minimizable = false,
                                     Closable = false
                                 };

                var wrap = new System.Web.UI.WebControls.Panel {CssClass = "move-content"};

                var label = new Label {CssClass = "move-label", Text = chars[i].ToString()};

                wrap.Controls.Add(label);
                window.Controls.Add(wrap);

                _windowContainer.Controls.Add(window);

                window.Style["left"] = points[i].X + "px";
                window.Style["top"] = points[i].Y + "px";
            }
        }

        private List<Point> GetRandomPoints(int count)
        {
            int x = StartLeft; //starting point
            int y = StartTop;
            int i = 0;
            var points = new List<Point>(count);

            for (int j = 0; j < count; j++)
            {
                points.Add(new Point(x, y));
                x += (Width + 10); // add border

                if (++i % _wordLength != 0)
                    continue;

                y += (Height + 10); // add border;
                x = StartLeft; //starting point again
            }

            for (int j = 0; j < points.Count; j++)
            {
                int randomPosition = _rnd.Next(points.Count);
                var temp = points[j];
                points[j] = points[randomPosition];
                points[randomPosition] = temp;
            }

            return points;
        }

        private int HitCount
        {
            get { return (int)(ViewState["HitCount"] ?? 0); }
            set { ViewState["HitCount"] = value; }
        }

        protected void zButtonMove_Click(object sender, EventArgs e)
        {
            PlayGame();
        }

        private void PlayGame()
        {
            zCount.Text = string.Format("Number of tries: {0}", ++HitCount);

            var availableWindows = _windowContainer.Controls.OfType<Gaia.WebWidgets.Extensions.Window>().ToList();

            var randomPoints = GetRandomPoints(availableWindows.Count);
            var order = new List<KeyValuePair<char, Point>>();

            string challenge = zChallenge.SelectedItem.Text;
            char[] chars = challenge.Replace(" ", "").ToCharArray();

            for (int i = 0; i < availableWindows.Count; i++)
            {
                var point = randomPoints[i];
                var win = availableWindows[i];

                order.Add(new KeyValuePair<char, Point>(chars[i], point));

                var effectMove = new EffectMove(
                    point.X,
                    point.Y,
                    /* duration */ 1,
                    /* delay */ 0.0M,
                    EffectMove.ModeEnum.Absolute,
                    GetSelectedTransition());

                win.Effects.Add(effectMove);
            }

            order.Sort(SortPoints);
            string output = string.Empty;
            foreach (var item in order)
                output += item.Key.ToString();

            if (output != challenge)
                return;

            zWindowSuccess.Visible = true;
            zCount.Text = 
                string.Format("You won after {0} number of tries ", HitCount);
            
            HitCount = 0;
        }

        private Easing GetSelectedTransition()
        {
            string selectedEffect = zDesiredEffect.SelectedItem.Text;
            return (Easing)
                   Enum.Parse(typeof(Easing), selectedEffect);
        }

        private static int SortPoints(KeyValuePair<char, Point> left, KeyValuePair<char, Point> right)
        {
            var l = left.Value;
            var r = right.Value;
            if (l.X == r.X && l.Y == r.Y) // both equal
                return 0;

            if (l.Y > r.Y)
                return 1;

            if (l.Y < r.Y)
                return -1;

            return l.X > r.X ? 1 : -1;
        }

        protected void zWindowSuccess_Closing(object sender, Window.WindowClosingEventArgs e)
        {
            zCount.Text = string.Empty;
        }

        private int PreviousChallengeIndex
        {
            get { return StateUtil.Get(ViewState, "psi", 0); }
            set { StateUtil.Set(ViewState, "psi", value);    }
        }
    }
}