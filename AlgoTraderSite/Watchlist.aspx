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
				<div class="panel-left">
					<div class="input-group">
						<asp:TextBox runat="server" ID="tbAddList" placeholder="Add a new list" CssClass="watchlist-input" Width="60%" MaxLength="20" onfocus="this.select()" onmouseup="return false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
						<asp:Button runat="server" ID="btnAddList" Text="&#xe00b;" ToolTip="Add to list" CssClass="symbol-button" OnClick="btnAddList_Click" onkeydown="return (event.keyCode!=13);" /><br />
					</div>
					<asp:RadioButtonList runat="server" ID="radioLists" DataValueField="ListName" DataTextField="ListName" OnSelectedIndexChanged="radioLists_SelectedIndexChanged" AutoPostBack="true" EnableViewState="true"></asp:RadioButtonList>
				</div>
				<div class="maincontent">
					<div id="InputGroup" class="input-group" runat="server">
						<div id="inputGroupLeft" class="float-left" runat="server">
							<asp:TextBox runat="server" ID="tbAddToWatchList" placeholder="Add a stock" MaxLength="4" CssClass="watchlist-input" onfocus="this.select()" onmouseup="return false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
							<asp:Button runat="server" ID="btnAddToWatchList" Text="Add to list" ToolTip="Add to list" OnClick="btnAddToWatchList_Click" onkeydown="return (event.keyCode!=13)" />
							<asp:Button runat="server" ID="btnDeleteList" Text="&#xe014;" ToolTip="Delete list" CssClass="delete symbol-button" OnClick="btnDeleteList_Click" onkeydown="return (event.keyCode!=13)" />
						</div>
						<div class="float-right">
							<asp:RadioButtonList runat="server" ID="radioSortType" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="radioSortType_SelectedIndexChanged" AutoPostBack="true" EnableViewState="true">
								<asp:ListItem Value="namea" Text="&#xe00d;" title="Sort by name (a-z)"></asp:ListItem>
								<asp:ListItem Value="named" Text="&#xe00e;" title="Sort by name (z-a)"></asp:ListItem>
								<asp:ListItem Value="pricea" Text="&#xe011;" title="Sort by price (lowest)"></asp:ListItem>
								<asp:ListItem Value="priced" Text="&#xe012;" title="Sort by price (highest)"></asp:ListItem>
								<asp:ListItem Value="gaina" Text="&#xe00f;" title="Sort by highest gain"></asp:ListItem>
								<asp:ListItem Value="gaind" Text="&#xe010;" title="Sort by highest loss"></asp:ListItem>
								<asp:ListItem Value="gainperca" Text="&#xe00f;" title="Sort by highest gain %"></asp:ListItem>
								<asp:ListItem Value="gainpercd" Text="&#xe010;" title="Sort by highest loss %"></asp:ListItem>
							</asp:RadioButtonList>
						</div>
					</div>


					<div runat="server" id="statusMessage"></div>
					<div runat="server" id="emptyDiv" class="watchlist-empty"></div>
					<div runat="server" id="WatchlistDiv"></div>
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
