<%@ Page Title="TestStrategy" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="TestStrategyDetail.aspx.cs" Inherits="AlgoTraderSite.TestStrategyDetail" %>

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

		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>