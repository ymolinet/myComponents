namespace Gaia.WebWidgets.Samples.Combinations.WebApps.ChatControl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetDefaultUsernameAndLastMessage();
                PostRandomMessageIfNothingSaidForAWhile();
            }

            DisplayMessagesInChatWindow();

            chatTxt.Aspects.Add(new Gaia.WebWidgets.AspectClickable(clicked));
        }

        protected void clicked(object sender, EventArgs e)
        {
            //erase if default message is present
            if (chatTxt.Text == "Type your message here!")
                chatTxt.Text = "";
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            //send typed text to the board
            PostMessageToMessageBoard(userName.Text, chatTxt.Text);

            //set the last message here
            RememberLastMessageToPreventNeedlessScrollingOfPage();

            //make sure we don't store to many in our session object
            //not a problem with handling traffic over wire since Gaia 3.6
            ClearOutOldMessages();

            //clear the Ajax input field and give it focus
            chatTxt.Text = "";
            chatTxt.Focus();
        }

        protected void timer_Tick(object sender, EventArgs e)
        {
            PostRandomMessageIfNothingSaidForAWhile();
            DisplayMessagesInChatWindow();
        }

        private void DisplayMessagesInChatWindow()
        {
            //clear the controls
            chatContent.Controls.Clear();

            //remember last message so we know when to scroll to the bottom page
            if (AnyMessagesInList)


                //Thread-lock messages collection for a moment.
                //We want to prevent modifications to the collection by 
                //other people whilst we iterate through it.
                lock (AllMessages)
                {
                    for (int i = 0; i < AllMessages.Count; i++)
                    {
                        Chat idx = AllMessages[i];

                        Panel chatContainer = new Panel();
                        chatContainer.ID = "chat" + idx.Id;

                        Panel chatItemHeader = new Panel();
                        chatItemHeader.ID = "headPnl" + idx.Id;
                        chatItemHeader.CssClass = "gaia-chat-headeritem-container";

                        Label humanTimeSpan = new Label();
                        humanTimeSpan.ID = "humanSpn" + idx.Id;
                        humanTimeSpan.CssClass = "human-span";
                        humanTimeSpan.Text = HumanReadableTimeSpan((DateTime.Now - idx.When));

                        chatItemHeader.Controls.Add(humanTimeSpan);

                        Label usernameSpan = new Label();
                        usernameSpan.ID = "userSpn" + idx.Id;
                        usernameSpan.CssClass = "user-span-color-" + GenerateCssClassColorForUsername(idx.User);
                        usernameSpan.Text = idx.User;

                        chatItemHeader.Controls.Add(usernameSpan);

                        Label chatItem = new Label();
                        chatItem.ID = "chatSpn" + idx.Id;
                        chatItem.CssClass = "gaia-chat-item-container";
                        chatItem.Text = idx.Content;

                        //add controls to parent
                        chatContainer.Controls.Add(chatItemHeader);
                        chatContainer.Controls.Add(chatItem);

                        chatContent.Controls.Add(chatContainer);

                        //if we are at last message, and there is new messages, we scroll to the bottom
                        if ((i == AllMessages.Count - 1) && HasNewMessages)
                           chatContent.Effects.Add(new EffectScrollToBottomOfControl());
                    }
                }

            //forget latest chat
            LastChat = null;
        }


        // *********************** BUSINESS LOGIC AND HELPER METHODS BELOW *****************************
        /// <summary>
        /// This region contains Business Logic and helper methods that aren't 
        /// crucial to demonstrating Gaia Ajax. 
        /// In a production-ready application, you'd pack most of this into business objects, 
        /// data access functions and view helper classes.
        /// </summary>
        #region BusinessLogicAndHelperMethods

        private void RememberLastMessageToPreventNeedlessScrollingOfPage()
        {
            LastChat = AllMessages[AllMessages.Count - 1];
        }

        private bool HasNewMessages
        {
            get
            {
                lock (AllMessages)
                {
                    return (AllMessages.Count > 0 && LastChat != null && LastChat == AllMessages[AllMessages.Count - 1]);
                }
            }
        }

        private void ClearOutOldMessages()
        {
            lock (AllMessages)
            {
                while (AllMessages.Count > 100)
                {
                    AllMessages.RemoveAt(0);
                }
            }
        }

        private bool AnyMessagesInList
        {
            get
            {
                lock (AllMessages)
                {
                    return AllMessages.Count > 0;
                }
            }
        }

        /// <summary>
        /// Gets the chats stored in application. Prepares them if not already set up for the application.
        /// </summary>
        public List<Chat> AllMessages
        {
            get
            {
                //init chats if not already done so
                if (Application["Chats"] == null)
                    InitChats();
                try
                {
                    return (List<Chat>)Application["Chats"];
                }
                catch (InvalidCastException)
                {
                    //Sometimes the cast fails. 
                    //This is when we have recompiled application but application has NOT restarted!!
                    InitChats();
                    return (List<Chat>)Application["Chats"];
                }
            }
        }

        /// <summary>
        /// Return the last chat in the list
        /// </summary>
        Chat LastChat
        {
            get { return (Chat)Session["last_chat"]; }
            set { Session["last_chat"] = value; }
        }

        void InitChats()
        {
            Application["Chats"] = new List<Chat>();
            PostMessageToMessageBoard("The Gaia Team", "Welcome to our Chat Client sample. Type some text in the message window. Then click view source to see how easy it was to build!");
        }

        private void SetDefaultUsernameAndLastMessage()
        {
            LastChat = null;

            //set the username to the hostname if not already set
            if (userName.Text == "")
            {
                userName.Text = "Someone";
                userName.SelectAll();
            }
        }

        void PostRandomMessageIfNothingSaidForAWhile()
        {
            if (AllMessages.Count == 0 || AllMessages[AllMessages.Count - 1].When < DateTime.Now.AddSeconds(-30))
            {
                Random r = new Random(DateTime.Now.Second);
                switch (r.Next(0, 5))
                {
                    case 0:
                        PostMessageToMessageBoard("GaiaBot", "Hey there :) <br/>See the code for this sample at the bottom of this page!");
                        break;
                    case 1:
                        PostMessageToMessageBoard("GaiaBot", "Did you know, Gaia needs no configuration before use?");
                        break;
                    case 2:
                        PostMessageToMessageBoard("Server", String.Format("This is the web server talking. I'd like to inform you I'm currently processing {0} requests/sec!",
                                                                          new PerformanceCounter("ASP.NET Applications", "Requests/Sec", "__Total__").NextValue()));
                        break;
                    case 3:
                        PostMessageToMessageBoard("GaiaBot", "Gaia gives you full Ajax without the need for JavaScript coding. Cool huh!");
                        break;
                    case 4:
                        PostMessageToMessageBoard("Server", String.Format("What time do you make it? On the server it's currently {0:HH:mm}", DateTime.Now));
                        break;
                    case 5:
                        PostMessageToMessageBoard("GaiaBot", "<a href='http://gaiaware.net/download'>Download Gaia</a> now and have all these samples installed on your computer.");
                        break;
                    default:
                        PostMessageToMessageBoard("GaiaBox", "Boo! ;-)");
                        break;
                }
            }
        }

        void PostMessageToMessageBoard(string username, string content)
        {
            lock (AllMessages)
            {
                int newIndex = AllMessages.Count;
                AllMessages.Add(new Chat(newIndex, username, content));
            }

            RememberLastMessageToPreventNeedlessScrollingOfPage();
        }

        private static string GenerateCssClassColorForUsername(string text)
        {
            switch (text.Length)
            {
                case 4:
                    return text.ToLower().IndexOf('o') > -1 ? "darkgreen" : "green";
                case 5:
                    return text.ToLower().IndexOf('o') > -1 ? "darkred" : "red";
                case 6:
                    return text.ToLower().IndexOf('o') > -1 ? "orange" : "brown";
                case 7:
                    return text.ToLower().IndexOf('o') > -1 ? "blue" : "navy";
                case 8:
                    return text.ToLower().IndexOf('o') > -1 ? "gray" : "black";
                case 9:
                    return text.ToLower().IndexOf('o') > -1 ? "red" : "pink";
                default:
                    return "black";

            }
        }

        private static string HumanReadableTimeSpan(TimeSpan ts)
        {

            if (ts.TotalSeconds < 20)
            {
                return "a few seconds ago";
            }

            List<string> parts = new List<string>();
            if (ts.Hours >= 1)
            {
                parts.Add(string.Format("{0} {1}", ts.Hours, ts.Hours <= 1 ? "hour" : "hours"));
            }
            if (ts.Minutes >= 1)
            {
                parts.Add(string.Format("{0} {1}", ts.Minutes, ts.Minutes <= 1 ? "minute" : "minutes"));
            }

            if (ts.Seconds >= 1)
            {
                parts.Add(string.Format("{0} {1}", ts.Seconds, ts.Seconds <= 1 ? "second" : "seconds"));
            }


            return String.Join(", ", parts.ToArray()) + " ago";
        }

        #endregion
    }
}