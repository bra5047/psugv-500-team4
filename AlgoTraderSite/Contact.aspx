<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="AlgoTraderSite.Contact" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>

    <section class="contact">
        <header>
            <h3>Email:</h3>
        </header>
        <p>
            <span class="label">Adam Trinh:</span>
            <span><a href="mailto:aht5021@psu.edu">aht5021@psu.edu</a></span>
        </p>
        <p>
            <span class="label">Brian Armstrong:</span>
            <span>email@server.com</span>
        </p>
		<p>
			<span class="label">Bill Jones:</span>
			<span>email@server.com</span>
		</p>
    </section>

	<section class="contact">
        <header>
            <h3>Address:</h3>
        </header>
        <p>
            Penn State Great Valley<br />
			30 East Swedesford Road<br />
            Malvern, PA 19355
        </p>
    </section>

    <section class="contact">
        <header>
            <h3>Source:</h3>
        </header>
        <p>
            <span class="label">Github</span>
            <span><a href="https://github.com/bra5047/psugv-500-team4" target="_blank">https://github.com/bra5047/psugv-500-team4</a></span>
        </p>
    </section>
</asp:Content>