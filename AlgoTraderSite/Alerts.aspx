<%@ Page Language="C#" Title="Alerts" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Alerts.aspx.cs" Inherits="AlgoTraderSite.Alerts" %>

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
    <asp:ListView ID="AlertItemList" runat="server">
        <LayoutTemplate>
            <table runat="server" id="table1" >
              <tr runat="server" id="itemPlaceholder" ></tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr id="Tr1" runat="server">
              <td id="Td1" runat="server">
                <%-- Data-bound content. --%>
                  <asp:Label ID="SymbolLabel" runat="server" Text='<%#Eval("SymbolName") %>' />
                  <asp:Label ID="CodeLabel" runat="server" Text='<%#Eval("ResponseCode") %>' />
                  <asp:HyperLink ID="AcceptLink" runat="server" Text='Accept' NavigateUrl='<%#"Alerts.aspx?id=" + Eval("AlertId") + "&s=accept" %>' />
                  <asp:HyperLink ID="RejectLink" runat="server" Text='Reject' NavigateUrl='<%#"Alerts.aspx?id=" + Eval("AlertId") + "&s=reject" %>' />
               </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</asp:Content>

