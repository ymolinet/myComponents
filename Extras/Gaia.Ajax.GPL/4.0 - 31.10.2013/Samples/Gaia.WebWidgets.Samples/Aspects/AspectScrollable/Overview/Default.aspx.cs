namespace Gaia.WebWidgets.Samples.Aspects.AspectScrollable.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //listen to the Vertical scroll of a Gaia Panel when it has reached the edge of it
            panel.Aspects.Add(new Gaia.WebWidgets.AspectScrollable(panel_Scrolled,
                                                              Gaia.WebWidgets.AspectScrollable.ScrollModes.Vertical, true));

            bioWindow.Aspects.Add(new Gaia.WebWidgets.AspectModal());

            if (IsPostBack)
            {
                RetrieveNextBatch(0, Current);
            }
            else
            {
                RetrieveNextBatch(0, 2);
                Current = 2;
            }
        }

        private void RetrieveNextBatch(int start, int end)
        {
            for (int idx = start; idx < end; idx++)
            {
                Panel div = new Panel();
                div.Style["margin"] = "15px";
                div.Style["padding"] = "15px";
                div.Style["border"] = "solid 1px #999";

                System.Web.UI.HtmlControls.HtmlTable tbl = new System.Web.UI.HtmlControls.HtmlTable();
                System.Web.UI.HtmlControls.HtmlTableRow row = new System.Web.UI.HtmlControls.HtmlTableRow();

                // Image
                System.Web.UI.HtmlControls.HtmlTableCell cellImage = new System.Web.UI.HtmlControls.HtmlTableCell();
                cellImage.Style["vertical-align"] = "top";
                ImageButton img = new ImageButton();
                img.Click += BioImg_Click;
                img.AlternateText = (idx % 6).ToString();
                img.ImageUrl = _images[idx % 6];
                cellImage.Controls.Add(img);
                row.Cells.Add(cellImage);

                // Bio
                System.Web.UI.HtmlControls.HtmlTableCell cellBio = new System.Web.UI.HtmlControls.HtmlTableCell();
                cellBio.Style["padding-left"] = "15px";
                cellBio.Style["vertical-align"] = "top";
                Label lbl = new Label();
                lbl.Text = _bios[idx % 6];
                cellBio.Controls.Add(lbl);
                row.Cells.Add(cellBio);

                tbl.Rows.Add(row);
                div.Controls.Add(tbl);

                panel.Controls.Add(div);
            }
        }

        void BioImg_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ImageButton img = sender as ImageButton;
            bioLbl.Text = _bios[int.Parse(img.AlternateText)];
            bioWindow.Visible = true;
        }

        protected int Current
        {
            get { return (int) (ViewState["Current"] ?? 0); }
            set { ViewState["Current"] = value; }
        }

        protected void panel_Scrolled(object sender, Gaia.WebWidgets.AspectScrollable.ScrollEventArgs e)
        {
            if (Current > 50)
                return;
            RetrieveNextBatch(Current, Current + 2);
            Current += 2;
        }

        private readonly string[] _images = new string[] { 
                                                            "img/spartacus.jpg", 
                                                            "img/martin-luther-king.jpg", 
                                                            "img/stanley-kubrick.jpg", 
                                                            "img/john-lennon.jpg", 
                                                            "img/nelson-mandela.jpg", 
                                                            "img/mahatma-gandhi.jpg" };

        private readonly string[] _bios = new string[] { 
                                                          @"<b>About Spartacus</b><br />
<b>Spartacus</b> (ca. 120 BC[1] – ca. 70 BC), according to Roman historians, 
was a gladiator-slave who became the leader (or possibly one of several) 
in the unsuccessful slave uprising against the Roman Republic known as the 
Third Servile War. Little is known about Spartacus beyond the events of 
the war, and the surviving historical accounts are sketchy and often 
contradictory. Spartacus' struggle, often perceived as the struggle of 
an oppressed people fighting for their freedom against a slave-owning aristocracy, 
has found new meaning for modern writers since the 19th century. 
The figure of Spartacus, and his rebellion, has become an inspiration to 
many modern literary and political writers, who have made the character of 
Spartacus an ancient/modern folk hero.
<br />
Source <a href='http://en.wikipedia.org/wiki/Spartacus'>Wikipedia - about Spartacus</a>",
                                                          @"<b>About Martin Luther King, Jr.</b><br />
<b>Martin Luther King, Jr.</b> (January 15, 1929 – April 4, 1968) 
was one of the main leaders of the American civil rights movement. He was a political 
activist and Baptist minister and is regarded as one of America's greatest orators. 
King's most influential and well-known public address is the 'I Have A Dream' speech, 
delivered on the steps of the Lincoln Memorial in Washington, D.C. in 1963. In 1964, 
King became the youngest man to be awarded the Nobel Peace Prize (for his work as a 
peacemaker, promoting nonviolence and equal treatment for different races). 
On April 4, 1968, King was assassinated in Memphis, Tennessee.
<br />
Source <a href='http://en.wikipedia.org/wiki/Martin_Luther_King,_Jr.'>Wikipedia - about Martin Luther King, Jr.</a>",
                                                          @"<b>About Stanley Kubrick</b><br />
<b>Stanley Kubrick</b> (July 26, 1928 – March 7, 1999) was an influential and 
acclaimed American film director and producer considered among the greatest of 
the 20th Century. He directed a number of highly acclaimed and sometimes controversial 
films.
<br />
Source <a href='http://en.wikipedia.org/wiki/Stanley_Kubrick'>Wikipedia - about Stanley Kubrick</a>",
                                                          @"<b>About John Lennon</b><br />
<b>John Lennon</b> MBE (9 October 1940 – 8 December 1980, 
born John Winston Lennon), was an English songwriter, 
singer, musician, graphic artist, author and peace 
activist who gained worldwide fame as one of the 
founders of The Beatles. Lennon and Paul McCartney formed 
a critically acclaimed and commercially successful partnership 
writing songs for The Beatles and other artists.[1] Lennon, 
with his cynical edge and knack for introspection, and 
McCartney, with his storytelling optimism and gift for melody, 
complemented each other.[2] In his solo career, Lennon wrote 
and recorded songs such as 'Imagine' and 'Give Peace a Chance'.
<br />
Source <a href='http://en.wikipedia.org/wiki/John_Lennon'>Wikipedia - about John Lennon</a>",
                                                          @"<b>About Nelson Mandela</b><br />
<b>Nelson Mandela</b> (born 18 July 1918) is the former President of South Africa, and 
the first to be elected in fully representative democratic elections. 
Before his presidency, Mandela was an anti-apartheid activist and leader of 
the African National Congress. He spent nearly three decades in prison for 
his struggle against apartheid. Through his 27 years in prison, 
much of it spent in a cell on Robben Island, Mandela became the most 
widely known figure in the struggle against apartheid. 
Among opponents of apartheid in South Africa and internationally, 
he became a cultural icon as a proponent of freedom and equality 
while the apartheid government and nations sympathetic to it condemned 
him and the ANC as communists and terrorists.
<br />
Source <a href='http://en.wikipedia.org/wiki/Nelson_Mandela'>Wikipedia - about Nelson Mandela</a>",
                                                          @"<b>About Mahatma Gandhi</b><br />
<b>Mahatma Gandhi</b> Mohandas Karamchand Gandhi (
Gujarati: મોહનદાસ કરમચંદ ગાંધી, IAST: 
<br />
mohandās karamcand 
gāndhī, IPA: [mohən̪d̪as kərəmtʃən̪d̪ gan̪d̪ʱi]) (October 2, 1869 – January 30, 1948) 
was a major political and spiritual leader of India and the Indian independence 
movement. He was the pioneer of Satyagraha—the resistance of tyranny through 
mass civil disobedience, firmly founded upon ahimsa or total non-violence—which 
was one of the strongest driving philosophies of the Indian independence 
movement and inspired movements for civil rights and freedom across the world. 
Gandhi is commonly known in India and across the world as Mahatma Gandhi 
(Sanskrit: महात्मा mahātmā – 
<br />
'Great Soul') and as Bapu 
<br />
(Gujarati: બાપુ bāpu – 'Father'). 
<br />
In India, he is officially accorded the honour of Father of the Nation and 
October 2nd, his birthday, is commemorated each year as Gandhi Jayanti, 
a national holiday. On 15 June 2007, the United Nations General Assembly 
unanimously adopted a resolution declaring October 2 to be the 'International Day of Non-Violence.'
<br />
Source <a href='http://en.wikipedia.org/wiki/Mahatma_Gandhi'>Wikipedia - about Mahatma Gandhi</a>"
                                                      };
    }
}