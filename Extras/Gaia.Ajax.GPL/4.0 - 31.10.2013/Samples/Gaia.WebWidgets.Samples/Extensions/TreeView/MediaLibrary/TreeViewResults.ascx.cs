using System;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Samples.Extensions.TreeView.MediaLibrary
{
    public partial class ViewStuffFromTreeView : UserControl
    {
        private string ViewId
        {
            get { return StateUtil.Get(ViewState, "ViewId", string.Empty); }
            set { StateUtil.Set(ViewState, "ViewId", value, string.Empty); }
        }

        private string ViewType
        {
            get { return StateUtil.Get(ViewState, "ViewType", string.Empty); }
            set { StateUtil.Set(ViewState, "ViewType", value, string.Empty); }
        }

        public void Page_Load(object sender, EventArgs e)
        {
            View(ViewId, ViewType);
        }

        public void View(string id, string type)
        {
            ViewId = id;
            ViewType = type;
            pnl.Controls.Clear();

            switch (type)
            {
                case "beautiful":
                case "people":
                case "food":
                    label.Visible = false;
                    GetImage(id);
                    break;
                case "jeff_dunham":
                    label.Visible = false;
                    GetVideo(id);
                    break;
                case "jazz_music":
                case "classic":
                    label.Visible = false;
                    GetMusician(id);
                    break;
            }
        }

        private void GetMusician(string id)
        {
            switch (id)
            {
                case "miles":
                    {
                        CreateMusician(id, @"
Widely considered one of the most influential musicians of the 20th century, 
Davis was at the forefront of almost every major development in jazz from World War 
II to the 1990s. He played on various early bebop records and recorded one of the first 
cool jazz records. He was partially responsible for the development of modal jazz, and 
jazz fusion arose from his work with other musicians in the late 1960s and early 1970s.");
                    } break;
                case "holiday":
                    {
                        CreateMusician(id, @"
Nicknamed Lady Day by her sometime collaborator Lester Young, Holiday was a seminal 
influence on jazz, and pop singers' critic John Bush wrote that she changed the 
art of American pop vocals forever. Her vocal style strongly inspired by 
instrumentalists pioneered a new way of manipulating wording and tempo, 
and also popularized a more personal and intimate approach to singing.");
                    } break;
                case "armstrong":
                    {
                        CreateMusician(id, @"
Armstrong was a charismatic, innovative performer whose improvised soloing 
was the main influence for a fundamental change in jazz, shifting its focus 
from collective improvisation to the solo player and improvised soloing. 
One of the most famous jazz musicians of the 20th century, he was first 
known as a cornet player, then as a trumpet player, and toward the end of 
his career he was best known as a vocalist and became one of the most 
influential jazz singers.");
                    } break;
                case "beethoven":
                    {
                        CreateMusician(id, @"
Born in Bonn, then in the Electorate of Cologne (now in modern day Germany), 
he moved to Vienna, Austria, in his early twenties and settled there, 
studying with Joseph Haydn and quickly gaining a reputation as a virtuoso pianist. 
Beethoven's hearing gradually deteriorated beginning in his twenties, yet he 
continued to compose masterpieces, and to conduct and perform, even after 
he was completely deaf.");
                    } break;
                case "mozart":
                    {
                        CreateMusician(id, @"
Baptized Joannes Chrysostomus Wolfgangus Theophilus Mozart) (27 January 1756 – 5 
December 1791) was a prolific and influential composer of the Classical era. 
His output of over 600 compositions includes works widely acknowledged as 
pinnacles of symphonic, concertante, chamber, piano, operatic, and choral music. 
Mozart is among the most enduringly popular of classical composers, and many of 
his works are part of the standard concert repertoire.");
                    } break;
                case "bach":
                    {
                        CreateMusician(id, @"
While Bach's fame as an organist was great during his lifetime, he was not 
particularly well-known as a composer. His adherence to Baroque forms and 
contrapuntal style was considered old-fashioned by his contemporaries, 
especially late in his career when the musical fashion tended towards 
Rococo and later Classical styles. A revival of interest and performances 
of his music began early in the 19th century, and he is now widely 
considered to be one of the greatest composers in the Western tradition.");
                    } break;
            }
        }

        private void CreateMusician(string id, string text)
        {
            var p2 = new Panel();
            p2.Style["display"] = "none";
            p2.Effects.Add(new WebWidgets.Effects.EffectAppear());

            var img = new Image {AlternateText = "Musician", ImageUrl = string.Format("Images/{0}.jpg", id)};
            img.Style["float"] = "left";
            img.Style["padding"] = "5px";
            p2.Controls.Add(img);

            var lbl = new Label {Text = text};
            p2.Controls.Add(lbl);
            pnl.Controls.Add(p2);

            pnl.Style["width"] = "280px";
            pnl.Style["padding"] = "10px";
            ((ASP.WebControl) pnl.Parent.Parent).Style["width"] = "302px";
        }

        private void GetVideo(string id)
        {
            // Creating HTML for YouTube videos...
            var html =
                string.Format(@"
<div style='width:425px;'>
    <object width='425px' height='355px'>
        <param name='movie' value='http://www.youtube.com/v/{0}&hl=en'></param>
        <param name='wmode' value='transparent'></param>
        <embed src='http://www.youtube.com/v/{0}&hl=en' type='application/x-shockwave-flash' wmode='transparent' width='425' height='355'></embed>
    </object>
</div>", id);

            // Creating our control to hold our YouTube player
            var lit = new LiteralControl {Text = html};

            pnl.Style["padding"] = "0";
            pnl.Style["width"] = "425px";
            pnl.ForceAnUpdate();
            pnl.Controls.Add(lit);

            ((ASP.WebControl)pnl.Parent.Parent).Style["width"] = "427px";
        }

        private void GetImage(string id)
        {
            var img = new Image
                          {
                              Width = ASP.Unit.Pixel(300),
                              ImageUrl = string.Format("Images/{0}.jpg", id),
                              AlternateText = "Images taken from www.publicdomainimages.net"
                          };
            img.Style["display"] = "none";
            img.Effects.Add(new WebWidgets.Effects.EffectAppear());

            pnl.Style["padding"] = "0";
            pnl.Style["width"] = "300px";
            pnl.Controls.Add(img);

            ((ASP.WebControl)pnl.Parent.Parent).Style["width"] = "302px";
        }
    }
}