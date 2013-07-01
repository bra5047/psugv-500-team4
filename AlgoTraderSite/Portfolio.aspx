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
	<div id="Status" runat="server"></div>
	<asp:Button UseSubmitBehavior="false" CssClass="ExpandAll" OnClientClick="return false" Text="Expand All" Width="200" runat="server" />
	<div id="PortfolioDiv" style="width:100%" runat="server"></div>
</asp:Content>