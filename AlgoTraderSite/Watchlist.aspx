<%@ Page Title="Watchlist" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Watchlist.aspx.cs" Inherits="AlgoTraderSite.WatchListPage" %>

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
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent" EnableViewState="true">
	<div id="statusMessage" runat="server"></div>
    <asp:Label runat="server" Text="List: " ID="lblWatchLists"></asp:Label><asp:DropDownList runat="server" ID="ddlistWatchLists" EnableViewState="true" OnSelectedIndexChanged="ddlistWatchLists_SelectedIndexChanged"></asp:DropDownList>
	<asp:TextBox runat="server" ID="tbAddToWatchList"></asp:TextBox>
	<asp:Button runat="server" ID="btnAddToWatchList" Text="Add to Watch List" OnClick="btnAddToWatchList_Click"/>
	<asp:Table runat="server" ID="tblWatchList" EnableViewState="true" Width="100%"></asp:Table>
</asp:Content>
