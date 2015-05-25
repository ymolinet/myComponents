<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ActivityListRepeater.ascx.cs" 
    Inherits="Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Views.ActivityListRepeater" %>

<gaia:Panel ID="w" runat="server" style="border: 1px solid #999; margin: 5px 5px 5px 0px;">
    <asp:Repeater ID="zView" runat="server">
        <ItemTemplate>           
            <gaia:Panel ID="c" runat="server">
                <gaia:Label ID="dt" style="border-bottom: 1px solid #abc; color:Green; " runat="server" Text='<%# Eval("ActivityDate", "{0:ddd dd. MMM}")%>' />
                <gaia:Label ID="p" Font-Bold="true" runat="server" Text=' <%# Eval("ContactPerson")%>' />
                <gaia:Label ID="t" ForeColor="red" runat="server" Text='<%# Eval("ActivityName")%>' />
            </gaia:Panel>
        </ItemTemplate>
    </asp:Repeater>
</gaia:Panel>