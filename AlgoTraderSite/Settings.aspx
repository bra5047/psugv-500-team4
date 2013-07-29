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
	<asp:UpdatePanel runat="server" ID="UpdatePanel1">
		<ContentTemplate>
			<%--<div class="onoffswitch">
		<input type="checkbox" name="onoffswitch" class="onoffswitch-checkbox" id="myonoffswitch" checked>
		<label class="onoffswitch-label" for="myonoffswitch">
			<div class="onoffswitch-inner"></div>
			<div class="onoffswitch-switch"></div>
		</label>
	</div>--%>
			<div class="panel-wrapper">
				<div class="panel-left">
					<asp:RadioButtonList runat="server" ID="radioLists" OnSelectedIndexChanged="radioLists_SelectedIndexChanged" AutoPostBack="true" EnableViewState="true">
						<asp:ListItem Text="General" Selected="True"></asp:ListItem>
						<asp:ListItem Text="Alerts"></asp:ListItem>
					</asp:RadioButtonList>
				</div>
				<div class="maincontent" runat="server">
					<div id="contentDiv" runat="server"></div>
					<asp:Button runat="server" ID="btnSave" Text="Save Changes" />
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
