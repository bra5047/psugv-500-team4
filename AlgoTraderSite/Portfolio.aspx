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
				<div class="panel-watchlist">
					<asp:RadioButtonList runat="server" ID="radioLists" AutoPostBack="true" EnableViewState="true">
						<asp:ListItem Text="Portfolio"></asp:ListItem>
						<asp:ListItem Text="Transaction History"></asp:ListItem>
					</asp:RadioButtonList>
				</div>
				<div class="maincontent">
					<div id="inputGroupLeft" class="input-group float-left">
						<asp:Button ID="btnExpand" CssClass="ExpandAll" OnClientClick="return false" Text="Expand All" Width="200" runat="server" />
					</div>
					<div class="input-group float-right">
						<asp:RadioButtonList runat="server" ID="radioSortType" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" EnableViewState="true">
							<asp:ListItem Value="namea" Text="&#xe00d;" title="Sort by name (ascending)"></asp:ListItem>
							<asp:ListItem Value="named" Text="&#xe00e;" title="Sort by name (descending)"></asp:ListItem>
							<asp:ListItem Value="pricea" Text="&#xe011;" title="Sort by price (lowest)"></asp:ListItem>
							<asp:ListItem Value="priced" Text="&#xe012;" title="Sort by price (highest)"></asp:ListItem>
							<asp:ListItem Value="gaina" Text="&#xe00f;" title="Sort by highest gain"></asp:ListItem>
							<asp:ListItem Value="gaind" Text="&#xe010;" title="Sort by highest loss"></asp:ListItem>
							<asp:ListItem Value="gainperca" Text="&#xe00f;" title="Sort by highest gain %"></asp:ListItem>
							<asp:ListItem Value="gainpercd" Text="&#xe010;" title="Sort by highest loss %"></asp:ListItem>
						</asp:RadioButtonList>
					</div><div runat="server" id="statusMessage"></div>
					<div runat="server" id="emptyDiv" class="watchlist-empty"></div>
					<div id="PortfolioDiv" style="width:100%" runat="server"></div>
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	
	
	
	
	
</asp:Content>