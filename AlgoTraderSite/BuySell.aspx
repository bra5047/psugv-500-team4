<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuySell.aspx.cs" Inherits="AlgoTraderSite.BuySell" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
        <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Buy or Sell</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:DropDownList ID="BuySellPicker" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
        <asp:ListItem Selected="True">Buy</asp:ListItem>
        <asp:ListItem>Sell</asp:ListItem>
    </asp:DropDownList>
    <asp:Literal ID="SymbolLabel" runat="server"></asp:Literal>
    <asp:TextBox ID="QuantityBox" runat="server" Width="60px"></asp:TextBox>
    <asp:Literal ID="FillerLabel" runat="server" Text=" @ "></asp:Literal>
    <asp:Literal ID="PriceLabel" runat="server" Text="$0.00"></asp:Literal>
    <asp:Button ID="ExecuteTrade" runat="server" Text="Execute" OnClick="ExecuteTrade_Click" />
</asp:Content>
