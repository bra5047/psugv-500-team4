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
				<div class="card">
					<div class="heading">Watchlists</div>
					<div class="content">AlgoTrader's Watchlists allow you to track stocks you're interested in.</div>
				</div>
				<div class="card">
					<div class="heading">Portfolio</div>
					<div class="content">Track and manage your entire stock portfolio through AlgoTrader.</div>
				</div>
			</div>
			<div class="alert-wrapper" runat="server">
				<div class="heading">Alerts</div>
				<div runat="server" id="AlertBox" class="content">No new alerts.</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
