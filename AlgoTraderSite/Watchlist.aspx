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
			<asp:Button ID="btnDeleteList" Text="Delete List" OnClick="btnDeleteList_Click" runat="server" onkeydown="return (event.keyCode!=13);" />
			<asp:TextBox ID="tbAddList" placeholder="Enter a list name" CssClass="watchlist-input" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
			<asp:Button ID="btnAddList" Text="Add List" OnClick="btnAddList_Click" runat="server" onkeydown="return (event.keyCode!=13);" />
			<asp:TextBox ID="tbAddToWatchList" placeholder="Enter a stock symbol" MaxLength="4" CssClass="watchlist-input" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
			<asp:Button ID="btnAddToWatchList" Text="Add to list" OnClick="btnAddToWatchList_Click" runat="server" onkeydown="return (event.keyCode!=13);" />
			<span id="statusMessage" runat="server"></span>
			<div id="WatchlistDiv" runat="server"></div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
