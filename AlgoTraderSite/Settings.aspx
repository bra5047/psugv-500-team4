<%@ Page Title="Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="AlgoTraderSite.Settings" %>

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
	<div class="onoffswitch">
		<input type="checkbox" name="onoffswitch" class="onoffswitch-checkbox" id="myonoffswitch" checked>
		<label class="onoffswitch-label" for="myonoffswitch">
			<div class="onoffswitch-inner"></div>
			<div class="onoffswitch-switch"></div>
		</label>
	</div>
</asp:Content>
