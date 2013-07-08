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
					<div class="input-group">
						<asp:TextBox runat="server" ID="tbAddList" placeholder="Enter a list name" CssClass="watchlist-input" Width="60%" MaxLength="20" onfocus="this.select()" onmouseup="return false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
						<asp:Button runat="server" ID="btnAddList" Text="&#xe00d;" ToolTip="Add to list" CssClass="symbol-button" OnClick="btnAddList_Click" onkeydown="return (event.keyCode!=13);" /><br />
					</div>
					<asp:RadioButtonList runat="server" ID="radioLists" DataValueField="ListName" DataTextField="ListName" OnSelectedIndexChanged="radioLists_SelectedIndexChanged" AutoPostBack="true" EnableViewState="true"></asp:RadioButtonList>
				</div>
				<div class="maincontent">
					<div id="inputGroupLeft" class="input-group float-left" runat="server">
						<asp:TextBox runat="server" ID="tbAddToWatchList" placeholder="Enter a stock symbol" MaxLength="4" CssClass="watchlist-input" onfocus="this.select()" onmouseup="return false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
						<asp:Button runat="server" ID="btnAddToWatchList" Text="Add to list" ToolTip="Add to list" OnClick="btnAddToWatchList_Click" onkeydown="return (event.keyCode!=13)" />
						<asp:Button runat="server" ID="btnDeleteList" Text="&#xe00c;" ToolTip="Delete list" CssClass="delete symbol-button" OnClick="btnDeleteList_Click" onkeydown="return (event.keyCode!=13)" />
					</div>
					<div class="input-group float-right">
						<asp:RadioButtonList runat="server" ID="radioSortType" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="radioSortType_SelectedIndexChanged" AutoPostBack="true" EnableViewState="true">
							<asp:ListItem Value="namea" Text="&#xe001;" title="Sort by name (ascending)"></asp:ListItem>
							<asp:ListItem Value="named" Text="&#xe002;" title="Sort by name (descending)"></asp:ListItem>
							<asp:ListItem Value="pricea" Text="&#xe013;" title="Sort by price (lowest)"></asp:ListItem>
							<asp:ListItem Value="priced" Text="&#xe014;" title="Sort by price (highest)"></asp:ListItem>
							<asp:ListItem Value="gaina" Text="&#xe011;" title="Sort by highest gain"></asp:ListItem>
							<asp:ListItem Value="gaind" Text="&#xe012;" title="Sort by highest loss"></asp:ListItem>
							<asp:ListItem Value="gainperca" Text="&#xe011;" title="Sort by highest gain %"></asp:ListItem>
							<asp:ListItem Value="gainpercd" Text="&#xe012;" title="Sort by highest loss %"></asp:ListItem>
						</asp:RadioButtonList>
						<%--<asp:Button runat="server" ID="btnSortName" Text="&#xe001;" ToolTip="Sort by name" CssClass="symbol-button" />
						<asp:Button runat="server" ID="btnSortPrice" Text="&#xe000;" ToolTip="Sort by price" CssClass="symbol-button" />
						<asp:Button runat="server" ID="btnSortGain" Text="&#xe011;" ToolTip="Sort by gain/loss" CssClass="symbol-button" />--%>
					</div>
					<div runat="server" id="statusMessage"></div>
					<div runat="server" id="emptyDiv" class="watchlist-empty"></div>
					<div runat="server" id="WatchlistDiv"></div>
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
