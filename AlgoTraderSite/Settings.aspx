<%@ Page Title="Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="AlgoTraderSite.Settings" %>

<asp:Content runat="server" ID="HeadContent" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		function printValue(sliderID, textbox) {
			var x = document.getElementById(textbox);
			var y = document.getElementById(sliderID);
			x.innerText = y.value;
		}
	</script>
</asp:Content>

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
						<asp:ListItem Text="Settings" Selected="True"></asp:ListItem>
					</asp:RadioButtonList>
				</div>
				<div class="maincontent" runat="server">
					<div id="contentDiv" runat="server">
						<div class="settings-heading">General</div>
						<div class="settings-label">Name:</div>
						<input runat="server" id="InputName" type="text" placeholder="Enter your name" class="settings-input" />
						<div style="height: 20px;"></div>
						<div class="settings-label">Email:</div>
						<input runat="server" id="InputEmail" type="email" placeholder="Enter your email address" class="settings-input" />
						<div class="settings-spacer"></div>
						<div class="settings-heading">Strategy</div>
						<div runat="server" id="LblFirstDuck" class="settings-label">First Duck: </div>
						<input runat="server" id="InputFirstDuck" type="range" min="300" max="43200" step="300" onchange="InputFirstDuck_ServerChange" />
						<input id="rangeValue1" type="text" size="2"/>
						<div runat="server" id="LblSecondDuck" class="settings-label">Second Duck: </div>
						<input runat="server" id="InputSecondDuck" type="range" min="3600" max="86400" step="3600" onchange="printValue('InputSecondDuck', 'LblSecondDuck', 'Second Duck:')" />
						<div runat="server" id="LblThirdDuck" class="settings-label">Third Duck: </div>
						<input runat="server" id="InputThirdDuck" type="range" min="14400" max="604800" step="14400" onchange="printValue('InputThirdDuck', 'LblThirdDuck', 'Third Duck:')" />
						<div runat="server" id="LblMovingAvg" class="settings-label">Moving Average Window: </div>
						<input runat="server" id="InputAvgWindow" type="range" min="10" max="60" step="10" />
					</div>
					<div class="settings-spacer"></div>
					<asp:Button runat="server" ID="btnSave" Text="Save Changes" CssClass="settings-save" />
					<asp:Button runat="server" ID="btnResetDefault" Text="Reset to Default" />
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
