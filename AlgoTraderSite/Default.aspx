<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AlgoTraderSite._Default" %>

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
	<asp:UpdatePanel runat="server" ID="UpdatePanel1">
		<ContentTemplate>
			
			<div class="card-wrapper">
				<div runat="server" id="greeting" class="home-greeting">Welcome back, <%=username %>.</div>
				<%--<div class="home-heading">Summary</div>
				<div class="home-summarystat"><strong>Portfolio Items:</strong> <%=numPortfolio %></div>
				<div class="home-summarystat"><strong>Watchlist Items:</strong> <%=numWatchlist %></div>
				<div class="home-summarystat"></div>
				<div class="home-heading">Portfolio</div>
				<div class="card gain">
					<div class="heading">% gain here</div>
					<div class="content">SYMBOL (company name)</div>
				</div>
				<div class="card loss">
					<div class="heading">% loss here</div>
					<div class="content">SYMBOL (company name)</div>
				</div>
				<div class="home-heading">Watchlist</div>
				<div class="card gain">
					<div class="heading">% gain here</div>
					<div class="content">SYMBOL (company name)</div>
				</div>
				<div class="card loss">
					<div class="heading">% loss here</div>
					<div class="content">SYMBOL (company name)</div>
				</div>--%>
			</div>
			<div class="alert-wrapper" runat="server">
				<div class="heading">Alerts</div>
				<div runat="server" id="AlertBox" class="content">No new alerts.</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
