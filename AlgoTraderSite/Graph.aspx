<%@ Page Title="Graph" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Graph.aspx.cs" Inherits="AlgoTraderSite.Graph" %>

<asp:Content runat="server" ID="HeadContent" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
	<script type="text/javascript">
		$(function () {
			var name = '<%=symbolName%>'
			var m1 = '<%=m1%>'
			var m2 = '<%=m2%>'
			var m3 = '<%=m3%>'

			//var data = '<%=plot.Select(x => new object[] { x.DateMilliseconds, x.Price })%>'
			//var json = <%=this.javaSerial.Serialize(plot)%>

			//alert(json);
			//var datagroup = JSON.stringify({myarray: [<%=datapoints%>]});
			//var datagroup = <%=this.javaSerial.Serialize(plot)%>
			//var datagroup = JSON.stringify({ myArray: {<%=plot%> } });

			//var d = new Date("July 21, 1983 01:15:00:526")
			//			var datapoints = [[d.valueOf(), 52.37],
			//[1153180800000, 52.90],
			//[1153267200000, 54.10],
			//[1153353600000, 60.50],
			//[1153440000000, 60.72],
			//[1153699200000, 61.42],
			//[1153785600000, 61.93],
			//[1153872000000, 63.87],
			//[1153958400000, 63.40],
			//[1154044800000, 65.59],
			//[1154304000000, 67.96]];

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
								text: '<%=m1Label%>'
							}
						}, {
							value: m2,
							color: '#ff9494',
							dashStyle: 'shortdash',
							width: 2,
							label: {
								text: '<%=m2Label%>'
							}
						}, {
							value: m3,
							color: '#f1db7b',
							dashStyle: 'shortdash',
							width: 2,
							label: {
								text: '<%=m3Label%>'
							}
						}]
					},

					series: [{
						name: name,
						data: data,
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
	<asp:Button runat="server" Text="Back" OnClick="btnClick_Back" />
	<div id="container" style="min-width: 500px; height: 70vh"></div>
    <div id="debug"><%=m1Label%>: <%=m1%><br /><%=m2Label%>: <%=m2%><br /><%=m3Label%>: <%=m3 %></div>
</asp:Content>
