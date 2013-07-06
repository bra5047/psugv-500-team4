<%@ Page Title="Watchlist" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Watchlist.aspx.cs" Inherits="AlgoTraderSite.WatchListPage" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
	<section class="featured">
		<div class="content-wrapper">
			<hgroup class="title">
				<h1><%: Title %></h1>
			</hgroup>
		</div>
	</section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<div class="panel-wrapper">
				<div class="panel-watchlist">
					<label class="header">Watchlists</label><br />
					<asp:RadioButtonList ID="radioLists" DataValueField="ListName" DataTextField="ListName" OnSelectedIndexChanged="radioLists_SelectedIndexChanged" AutoPostBack="true" EnableViewState="true" runat="server"></asp:RadioButtonList>
					<asp:TextBox ID="tbAddList" placeholder="Enter a list name" CssClass="watchlist-input" Width="80%" MaxLength="20" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
					<asp:Button ID="btnAddList" Text="+ Add List" OnClick="btnAddList_Click" runat="server" onkeydown="return (event.keyCode!=13);" />
				</div>
				<div class="maincontent">
					<div class="input-group">
						<asp:TextBox ID="tbAddToWatchList" placeholder="Enter a stock symbol" MaxLength="4" CssClass="watchlist-input" onfocus="this.select()" onmouseup="return false" onkeydown="return (event.keyCode!=13);" runat="server"></asp:TextBox>
						<asp:Button ID="btnAddToWatchList" Text="Add to list" OnClick="btnAddToWatchList_Click" runat="server" onkeydown="return (event.keyCode!=13);" />
						<asp:Button ID="btnDeleteList" Text="Delete List" CssClass="delete" OnClick="btnDeleteList_Click" runat="server" onkeydown="return (event.keyCode!=13);" />
					</div>
					<div id="statusMessage" runat="server"></div>
					<div id="WatchlistDiv" runat="server"></div>
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
