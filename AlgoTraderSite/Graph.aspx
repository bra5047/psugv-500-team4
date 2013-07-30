<%@ Page Title="Graph" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Graph.aspx.cs" Inherits="AlgoTraderSite.Graph" %>

<asp:Content runat="server" ID="HeadContent" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
	<script type="text/javascript">
		$(function () {
			var name = '<%=symbolName%>'
			var m1 = '<%=m1%>'
			var m2 = '<%=m2%>'
			var m3 = '<%=m3%>'
			var datapoints = <%=data%>

			$.getJSON('http://www.highcharts.com/samples/data/jsonp.php?filename=' + name.toLowerCase() + '-c.json&callback=?', function (data) {
				// Create the chart
				$('#container').highcharts('StockChart', {

					rangeSelector: {
						selected: 1
					},

					title: {
						text: name
					},

					yAxis: {
						title: {
							text: 'Price'
						},
						plotLines: [{
							value: m1,
							color: '#99cc00',
							dashStyle: 'shortdash',
							width: 2,
							label: {
								text: ''
							}
						}, {
							value: m2,
							color: '#ff9494',
							dashStyle: 'shortdash',
							width: 2,
							label: {
								text: ''
							}
						}, {
							value: m3,
							color: '#f1db7b',
							dashStyle: 'shortdash',
							width: 2,
							label: {
								text: ''
							}
						}]
					},

					series: [{
						name: name,
						data: datapoints,
						tooltip: {
							valueDecimals: 2
						}
					}]
				});
			});

		});

	</script>
</asp:Content>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
	<section class="featured">
		<div class="content-wrapper">
			<hgroup class="title">
				<h1><%: Title %> of <%: symbolName %></h1>
			</hgroup>
		</div>
	</section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
	<div class="graph-wrapper">
		<asp:Button runat="server" Text="Back" OnClick="btnClick_Back" />
		<div class="legend" style="background-color: #99cc00"></div> <span><%=m1Label%> (<%=String.Format("{0:C}", m1)%>)</span>
		<div class="legend" style="background-color: #ff9494"></div> <span><%=m2Label%> (<%=String.Format("{0:C}", m2)%>)</span>
		<div class="legend" style="background-color: #f1db7b"></div> <span><%=m3Label%> (<%=String.Format("{0:C}", m3)%>)</span>
	</div>
	<div id="container" style="min-width: 500px; height: 70vh"></div>
</asp:Content>
