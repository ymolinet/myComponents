<%@ Page
    Language="C#"
    AutoEventWireup="true"
    MasterPageFile="~/Core.master"
    Inherits="Gaia.WebWidgets.Samples.Aspects.AspectResizable.ResizableChart.Default" 
    Title="Resizable Chart Sample" Codebehind="Default.aspx.cs" %>

<asp:Content
    ID="Content1"
    ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">

    <h1>Gaia Ajax: Resizable Chart Example</h1>
    <p>
        Here we have created an <em>Ajax Bar Chart</em> by combining some Gaia Panels together with some Ajax Aspects. Try
        to drag either the interest or the loan terms left or right and watch as the Yearly Balance Chart updates. The 
        Ajax Chart here is created purely with HTML elements like Ajax Panels and Ajax Labels. Then what binds it together
        to make them resizable are the AspectResizable. One of the real "gems" in Gaia are the Aspects which can be combined
        together with other widgets and together expand on the functionality of existing Ajax Controls. Try to drag the
        green bars (Annual Interest and Loan Terms) to the right and watch as everything updates live.
    </p>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <h3>Loan Calculator</h3>
    Loan Amount?
    <br />
    <gaia:TextBox 
        ID="LoanAmount"
        runat="server"
        Text="100000"                        
        Width="150"
        AutoPostBack="true"
        OnTextChanged="amount_Changed"
        KeyChangeEventsInterval="200"
        KeyChangeEvents="true" />

    <div>
        Annual Interest Rate (%)
        <gaia:Panel 
            ID="AnnualInterestRateBox"
            style="width:50px;border:solid 1px Black;background-color:#5e5;"
            runat="server">
            <gaia:Label 
                runat="server" 
                ID="AnnualInterestRate" 
                Text="10%" />
        </gaia:Panel>
        <small>Use mouse to resize</small>
    </div>
    <br />

    <div>
        Loan Term (months)
        <gaia:Panel 
            ID="LoanTermBox"
            style="width:50px;border:solid 1px Black;background-color:#5e5;"
            runat="server">
            <gaia:Label runat="server" ID="LoanTerm" Text="120" />
        </gaia:Panel>
        <small>Use mouse to resize</small>
    </div>

    <table style="font-size: 1.2em;">
        <tr>
            <td>Total Payment</td>
            <td>
                <strong>
                    <gaia:Label ID="TotalPayment" runat="server" />
                </strong>
            </td>
        </tr>
        <tr>
            <td>Monthly Payment</td>
            <td>
                <strong>
                    <gaia:Label ID="MonthlyPayment" runat="server" />
                </strong>
            </td>
        </tr>
    </table>         
      
    <h3>Yearly Balance Chart</h3>
    <gaia:Panel ID="ChartPanel" runat="server" />
    <br style="clear:both;" />
    <hr />
    <div class="features">
        <h2>Features</h2>
        <ul>
            <li><strong>Flexible</strong> - Build Ajax Bar Charts exactly as you wish</li>
            <li><strong>XHTML</strong> - Of course</li>
            <li><strong>Great User Experience</strong> - Users may experiment with values in a very visual interface</li>
            <li><strong>Ultra flexible</strong> - Combine with any other Ajax Widgets including <a href="../../../BasicControls/DynamicImage/Overview/Default.aspx">our Ajax Dynamic Image</a></li>
            <li><strong>Richness</strong> - More than 10 different aspects are available. Everything from Draggable, Droppable, Key-listeners, Hover aspects etc</li>
        </ul>
    </div>

</asp:Content>
