$(document).ready(function () {
	// NAV
	setNavCss();

	// PORTFOLIO
	$(".toggle").click(function (e) {
		var $target = $(e.target).parents("table:first"); // target = button -> cell -> row -> table
		var $toggle = $target.find(".toggle");
		var $traderow = $target.find(".TradeRow");

		if ($traderow.css("visibility") === "hidden") {
			$traderow.css("visibility", "visible");
		}
		if ($toggle.prop("value") === "+") {
			$toggle.prop("value", "-");
		} else {
			$toggle.prop("value", "+");
		}
		$traderow.stop(true, true).animate({ height: "toggle", opacity: "toggle" }, 250);
	});

	$(".ExpandAll").click(function (e) {
		$target = $(".ExpandAll");
		var value = "";

		if ($target.prop("value") === "Expand All") {
			value = "Collapse All";
			$(".toggle[value='+']").trigger('click');
		} else {
			value = "Expand All";
			$(".toggle[value='-']").trigger('click');
		}
		$target.prop("value", value);
	});
});

function setNavCss() {
	var page = $("hgroup").children().text();
	var tab = 0;
	if (page === "Welcome") {
		tab = 1;
	} else if (page === "Watchlist") {
		tab = 2;
	} else if (page === "My Portfolio") {
		tab = 3;
	} else if (page === "Settings") {
		tab = 4;
	} else {
		tab = 0;
	}
	$("ul li:nth-child(" + tab + ")").children("a").addClass("selected");
}