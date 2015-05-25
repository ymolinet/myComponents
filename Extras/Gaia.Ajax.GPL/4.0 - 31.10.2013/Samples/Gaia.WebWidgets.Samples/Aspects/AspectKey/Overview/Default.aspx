<%@ Page Language="C#" MasterPageFile="~/Core.master" AutoEventWireup="True"
    Inherits="Gaia.WebWidgets.Samples.Aspects.AspectKey.Overview.Default"
    Title="Gaia Ajax: Aspect Key" Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <samples:GDoc ID="GDoc1" runat="server" Member="T:Gaia.WebWidgets.AspectKey" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <p>AspectKey is added to a textbox which is hidden from the UI via css.
    As long as this textbox keeps focus it will trap key events for you. 
    <gaia:LinkButton id="zFocusBack" runat="server"
    Text="Click here if you need to set focus back to this control again"
    OnClick="zFocusBack_Click"></gaia:LinkButton>
    </p>
    <gaia:TextBox
        autocomplete="off" 
        runat="server" 
        ID="txt"></gaia:TextBox><br />
    <br />
    <div style="overflow: auto;">
        <h2>
            Special Keys</h2>
        <gaia:Panel runat="server" ID="ESC" CssClass="keyBindings">
            esc</gaia:Panel>
        <gaia:Panel runat="server" ID="TAB" CssClass="keyBindings">
            tab</gaia:Panel>
        <gaia:Panel runat="server" ID="SPACE" CssClass="keyBindings">
            space</gaia:Panel>
        <gaia:Panel runat="server" ID="RETURN" CssClass="keyBindings">
            return</gaia:Panel>
        <gaia:Panel runat="server" ID="BACKSPACE" CssClass="keyBindings">
            backspace</gaia:Panel>
        <gaia:Panel runat="server" ID="SCROLL" CssClass="keyBindings">
            scroll</gaia:Panel>
        <gaia:Panel runat="server" ID="CAPSLOCK" CssClass="keyBindings">
            capslock</gaia:Panel>
        <gaia:Panel runat="server" ID="NUMLOCK" CssClass="keyBindings">
            numlock</gaia:Panel>
        <gaia:Panel runat="server" ID="PAUSE" CssClass="keyBindings">
            pause</gaia:Panel>
        <gaia:Panel runat="server" ID="INSERT" CssClass="keyBindings">
            insert</gaia:Panel>
        <gaia:Panel runat="server" ID="HOME" CssClass="keyBindings">
            home</gaia:Panel>
        <gaia:Panel runat="server" ID="DELETE" CssClass="keyBindings">
            delete</gaia:Panel>
        <gaia:Panel runat="server" ID="END" CssClass="keyBindings">
            end</gaia:Panel>
        <gaia:Panel runat="server" ID="PAGEUP" CssClass="keyBindings">
            pageup</gaia:Panel>
        <gaia:Panel runat="server" ID="PAGEDOWN" CssClass="keyBindings">
            pagedown</gaia:Panel>
        <gaia:Panel runat="server" ID="LEFT" CssClass="keyBindings">
            left</gaia:Panel>
        <gaia:Panel runat="server" ID="UP" CssClass="keyBindings">
            up</gaia:Panel>
        <gaia:Panel runat="server" ID="RIGHT" CssClass="keyBindings">
            right</gaia:Panel>
        <gaia:Panel runat="server" ID="DOWN" CssClass="keyBindings">
            down</gaia:Panel>
        <gaia:Panel runat="server" ID="F1" CssClass="keyBindings">
            f1</gaia:Panel>
        <gaia:Panel runat="server" ID="F2" CssClass="keyBindings">
            f2</gaia:Panel>
        <gaia:Panel runat="server" ID="F3" CssClass="keyBindings">
            f3</gaia:Panel>
        <gaia:Panel runat="server" ID="F4" CssClass="keyBindings">
            f4</gaia:Panel>
        <gaia:Panel runat="server" ID="F5" CssClass="keyBindings">
            f5</gaia:Panel>
        <gaia:Panel runat="server" ID="F6" CssClass="keyBindings">
            f6</gaia:Panel>
        <gaia:Panel runat="server" ID="F7" CssClass="keyBindings">
            f7</gaia:Panel>
        <gaia:Panel runat="server" ID="F8" CssClass="keyBindings">
            f8</gaia:Panel>
        <gaia:Panel runat="server" ID="F9" CssClass="keyBindings">
            f9</gaia:Panel>
        <gaia:Panel runat="server" ID="F10" CssClass="keyBindings">
            f10</gaia:Panel>
        <gaia:Panel runat="server" ID="F11" CssClass="keyBindings">
            f11</gaia:Panel>
        <gaia:Panel runat="server" ID="F12" CssClass="keyBindings">
            f12</gaia:Panel>
    </div>
    <div style="overflow: auto;">
        <h2>
            0-9 Digits</h2>
        <gaia:Panel runat="server" ID="PanelD0" CssClass="keyBindings">
            0</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelD1" CssClass="keyBindings">
            1</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelD2" CssClass="keyBindings">
            2</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelD3" CssClass="keyBindings">
            3</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelD4" CssClass="keyBindings">
            4</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelD5" CssClass="keyBindings">
            5</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelD6" CssClass="keyBindings">
            6</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelD7" CssClass="keyBindings">
            7</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelD8" CssClass="keyBindings">
            8</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelD9" CssClass="keyBindings">
            9</gaia:Panel>
    </div>
    <div style="overflow: auto;">
        <h2>
            A-Z Letters</h2>
        <gaia:Panel runat="server" ID="PanelA" CssClass="keyBindings">
            a</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelB" CssClass="keyBindings">
            b</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelC" CssClass="keyBindings">
            c</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelD" CssClass="keyBindings">
            d</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelE" CssClass="keyBindings">
            e</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelF" CssClass="keyBindings">
            f</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelG" CssClass="keyBindings">
            g</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelH" CssClass="keyBindings">
            h</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelI" CssClass="keyBindings">
            i</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelJ" CssClass="keyBindings">
            j</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelK" CssClass="keyBindings">
            k</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelL" CssClass="keyBindings">
            l</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelM" CssClass="keyBindings">
            m</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelN" CssClass="keyBindings">
            n</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelO" CssClass="keyBindings">
            o</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelP" CssClass="keyBindings">
            p</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelQ" CssClass="keyBindings">
            q</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelR" CssClass="keyBindings">
            r</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelS" CssClass="keyBindings">
            s</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelT" CssClass="keyBindings">
            t</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelU" CssClass="keyBindings">
            u</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelV" CssClass="keyBindings">
            v</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelW" CssClass="keyBindings">
            w</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelX" CssClass="keyBindings">
            x</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelY" CssClass="keyBindings">
            y</gaia:Panel>
        <gaia:Panel runat="server" ID="PanelZ" CssClass="keyBindings">
            z</gaia:Panel>
    </div>
    <br />
</asp:Content>
