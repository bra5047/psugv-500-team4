<%@ Page Title="Watchlist" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Watchlist.aspx.cs" Inherits="AlgoTraderSite.WatchListPage" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
	<section class="featured">
		<div class="content-wrapper">
			<hgroup class="title">
				<h1><%: Title %></h1>
				<h2></h2>
			</hgroup>
		</div>
	</section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<span>Lists:</span>
			<asp:DropDownList ID="ddlistWatchLists" runat="server" EnableViewState="true" OnSelectedIndexChanged="ddlistWatchLists_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
			<asp:TextBox ID="Input" runat="server" placeholder="Enter a stock symbol" MaxLength="4" CssClass="watchlist-input"></asp:TextBox>
			<asp:Button runat="server" ID="btnAddToWatchList" Text="Add" OnClick="btnAddToWatchList_Click" />
			<span id="statusMessage" runat="server"></span>
			<div id="WatchlistDiv" runat="server"></div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
