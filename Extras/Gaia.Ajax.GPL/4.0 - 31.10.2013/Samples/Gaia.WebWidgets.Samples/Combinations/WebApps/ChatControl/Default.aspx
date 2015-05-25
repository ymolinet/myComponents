<%@ Page 
    Language="C#" 
    MasterPageFile="~/Core.master" 
    AutoEventWireup="true" 
    Inherits="Gaia.WebWidgets.Samples.Combinations.WebApps.ChatControl.Default" 
    Title="Gaia Ajax: Chat Control" Codebehind="Default.aspx.cs" %>

<asp:Content 
    ID="Content1" 
    ContentPlaceHolderID="ContentPlaceHolder1" 
    Runat="Server">

    <h1>Gaia Ajax: Chat Control</h1>

    <p>
        Here is an example of how you would create an <em>Ajax Chat Control</em> using Gaia Ajax. If you click the "show code" you
        will see that there is around 50 lines of markup and 300 lines of C# and no custom JavaScript.
        By using Gaia you can easily combine controls together to create more complex samples and controls. In fact you can
        even create complex Ajax Server controls too utilizing Gaia without ever having to resort to JavaScript yourself.
    </p>
    <p>
        You can create chat controls for DotNetNuke, EPiServer, Umbraco or SharePoint and other popular 
        CMS systems using Gaia Ajax. Gaia is about two things;
        <em>flexibility and ease of use</em>.
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <gaia:ExtendedPanel 
        id="chatMainWindow"             
        Visible="true" 
        Caption="Ajax Chat client" 
        Draggable="false"
        runat="server">

        <gaia:Panel runat="server" DefaultButton="submit">

            <div style="border:1px solid #6EC6F2;">
                <gaia:Panel 
                    runat="server" 
                    ID="chatContent"
                    CssClass="gaia-chat-aroundall" />
               
                <div style="padding: 4px; vertical-align: middle; border-left: 2px solid #d6eefa; border-right: 2px solid #d6eefa;">
                    <gaia:Button 
                        runat="server" 
                        ID="submit" 
                        OnClick="submit_Click"
                        Text="Say it!"
                        style="float:right;" />

                    <img runat="server" src="~/media/contentImages/chat_quote.jpg" alt="chat" style="float:left;margin: 5px 15px 0 15px;" />
                    <div style="display:inline;">
                        <gaia:TextBox 
                            runat="server" 
                            Text="Type your message here!"
                            AutoCompleteType="None"
                            style="width:80%; border:1px solid #6EC6F2; color:green; font-weight:bold; padding-bottom:4px;padding-top:3px"                     
                            ID="chatTxt" />
                        <br />
                        <small style="color: gray;">press <em>ENTER</em> to Say It!</small>
                    </div>
                </div>

                <div 
                    id="settings" 
                    style="padding:4px; background-color:#d6eefa;">             
                    <span style="float:left; width:70px;margin-top: 4px;">Username:</span>
                    <gaia:TextBox 
                        runat="server"
                        ID="userName"
                        style="font-size:small; padding-bottom: 5px;" />
                </div>
            </div>
           
            <gaia:Timer 
                runat="server" 
                ID="timer" 
                Milliseconds="2000"
                Enabled="true" 
                OnTick="timer_Tick" />
        </gaia:Panel>

    </gaia:ExtendedPanel>
        
</asp:Content>

