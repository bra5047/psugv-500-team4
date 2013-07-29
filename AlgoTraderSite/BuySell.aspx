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
	<div class="buysell-wrapper">
		<div class="input-group">
			<asp:RadioButtonList ID="BuySellPicker" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
				<asp:ListItem Selected="True">Buy</asp:ListItem>
				<asp:ListItem>Sell</asp:ListItem>
			</asp:RadioButtonList>
		</div>
		<input id="QuantityBox" runat="server" style="width: 70px" type="number" min="0" required />
		<asp:Label ID="SymbolLabel" runat="server"></asp:Label>
		<asp:Label ID="FillerLabel" runat="server" Text="shares @ "></asp:Label>
		<asp:Label ID="PriceLabel" runat="server" Text="$0.00"></asp:Label>
		<asp:Label ID="EachLabel" runat="server" Text=" each"></asp:Label>
		<br />
		<asp:Button ID="ExecuteTrade" runat="server" Text="Confirm" OnClick="ExecuteTrade_Click" />
		<div>
			<asp:Label ID="ErrorMsg" runat="server" BackColor="#FF3300" ForeColor="White" Visible="False" Width="100%"></asp:Label>
		</div>
	</div>
</asp:Content>
