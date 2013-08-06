<%@ Page Title="Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="AlgoTraderSite.Settings" %>

<asp:Content runat="server" ID="HeadContent" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		window.onload = function () {
			printValue('LblFirstDuck', '<%=duck1 %>');
			printValue('LblSecondDuck', '<%=duck2 %>');
			printValue('LblThirdDuck', '<%=duck3 %>');
			printValue('LblMovingAvg', '<%=movingavg %>');
		}

		function printValue(labelid, value) {
			var prefix = "";
			var units = "";

			if (labelid === "LblFirstDuck") {
				prefix = "First Duck:";
				units = "minutes";

				if (value / 60 === 1) {
					units = "minute";
				}

				value = value / 60;
			} else if (labelid === "LblSecondDuck") {
				prefix = "Second Duck:";
				units = "hours";

				if (value / 3600 === 1) {
					units = "hour";
				}

				value = value / 3600;
			} else if (labelid === "LblThirdDuck") {
				prefix = "Third Duck:";

				units = "hours";

				if (value / 3600 === 1) {
					units = "hour";
				}

				value = value / 3600;
			} else {
				prefix = "Moving Average Window:";
			}

			var label = document.getElementById(labelid);
			label.innerText = prefix + " " + value + " " + units;
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
						<input runat="server" id="InputName" type="text" placeholder="Enter your name" class="settings-input" required />
						<div style="height: 20px;"></div>
						<div class="settings-label">Email:</div>
						<input runat="server" id="InputEmail" type="email" placeholder="Enter your email address" class="settings-input" disabled="disabled" required />
						<div class="settings-spacer"></div>
						<div class="settings-heading">Strategy</div>

						<div id="LblFirstDuck" class="settings-label-nowidth">First Duck: </div>
						<input runat="server" id="InputFirstDuck" type="range" min="300" max="43200" step="300" onchange="printValue('LblFirstDuck', this.value)" />

						<div id="LblSecondDuck" class="settings-label-nowidth">Second Duck: </div>
						<input runat="server" id="InputSecondDuck" type="range" min="3600" max="86400" step="3600" onchange="printValue('LblSecondDuck', this.value)" />

						<div id="LblThirdDuck" class="settings-label-nowidth">Third Duck: </div>
						<input runat="server" id="InputThirdDuck" type="range" min="14400" max="604800" step="14400" onchange="printValue('LblThirdDuck', this.value)" />

						<div id="LblMovingAvg" class="settings-label-nowidth">Moving Average Window: </div>
						<input runat="server" id="InputAvgWindow" type="range" min="10" max="60" step="10" onchange="printValue('LblMovingAvg', this.value)"  />
					</div>
					<div class="settings-spacer"></div>
					<asp:Button runat="server" ID="btnSave" Text="Save Changes" CssClass="settings-save" OnClick="btnSave_Click" />
					<asp:Button runat="server" ID="btnResetDefault" Text="Reset to Default" />
					<div runat="server" id="statusMessage"></div>
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
