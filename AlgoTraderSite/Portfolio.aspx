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
	<asp:GridView ID="PortfolioGrid" AllowSorting="true" runat="server" AutoGenerateColumns="false">
		<Columns>
			<asp:TemplateField HeaderText="" ItemStyle-Width="5%">
				<ItemTemplate>
					<asp:Label runat="server">+</asp:Label>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:BoundField HeaderText="Company" DataField="SymbolName" />
			<asp:BoundField HeaderText="Quantity" DataField="Quantity" />
			<asp:BoundField HeaderText="Price" DataField="Price" DataFormatString="{0:C}" />
			<asp:BoundField HeaderText="Status" DataField="Status" />
			<asp:ButtonField HeaderText="Actions" ButtonType="Button" Text="Buy/Sell" />
		</Columns>
	</asp:GridView>



	<asp:TreeView ID="PortfolioTree" runat="server">
    </asp:TreeView>
	<div id="PortfolioDiv" style="width:100%" runat="server">

	</div>
	<!--<asp:Table ID="PortfolioTable" Width="100%" runat="server">
	</asp:Table>-->
</asp:Content>