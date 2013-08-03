<%@ Page Title="My Portfolio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Portfolio.aspx.cs" Inherits="AlgoTraderSite.MyPortfolio" %>

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
					<asp:RadioButtonList runat="server" ID="radioLists" OnSelectedIndexChanged="radioLists_SelectedIndexChanged" AutoPostBack="true" EnableViewState="true">
						<asp:ListItem Text="Portfolio" Selected="True"></asp:ListItem>
						<asp:ListItem Text="Transaction History"></asp:ListItem>
					</asp:RadioButtonList>
					<div class="separator"></div>
					<label class="header">Available cash:</label><br />
					<asp:Label runat="server" ID="AvailableCash" CssClass="cash"></asp:Label>
					<br />
					<div class="input-group">
						<input runat="server" id="InputAddCash" type="number" min="0" max="999999" onfocus="this.select()" onmouseup="return false" />
						<asp:Button ID="BtnAddCash" runat="server" OnClick="btnAddCash_Click" Text="Add Cash" />
					</div>
				</div>
				<div class="maincontent">
					<div id="InputGroup" class="input-group" runat="server">
						<div id="inputGroupLeft" class="float-left" runat="server">
							<asp:Button ID="btnExpand" CssClass="ExpandAll" OnClientClick="return false" Text="Expand All" Width="200" runat="server" />
						</div>
						<div id="inputGroupRight" class="float-right" runat="server">
							<asp:RadioButtonList runat="server" ID="radioSortType" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="radioSortType_SelectedIndexChanged" AutoPostBack="true" EnableViewState="true">
								<asp:ListItem Value="namea" Text="&#xe00d;" title="Sort by name (a-z)"></asp:ListItem>
								<asp:ListItem Value="named" Text="&#xe00e;" title="Sort by name (z-a)"></asp:ListItem>
								<asp:ListItem Value="sharesd" Text="&#xe011;" title="Sort by total shares (highest)"></asp:ListItem>
								<asp:ListItem Value="sharesa" Text="&#xe012;" title="Sort by total shares (lowest)"></asp:ListItem>
								<asp:ListItem Value="valued" Text="&#xe00f;" title="Sort by value (highest)"></asp:ListItem>
								<asp:ListItem Value="valuea" Text="&#xe010;" title="Sort by value (lowest)"></asp:ListItem>
							</asp:RadioButtonList>
						</div>
					</div>
					<div runat="server" id="statusMessage"></div>
					<div runat="server" id="emptyDiv" class="watchlist-empty"></div>
					<div id="PortfolioDiv" style="width: 100%" runat="server"></div>
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
