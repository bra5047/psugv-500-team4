$(document).ready(function () {
	// NAV
	setNavCss();
});

function pageLoad(sender, args) {
	// PORTFOLIO
	$(".toggle").click(function (e) {
		var $target = $(e.target).parents("table:first"); // target = button -> cell -> row -> table
		var $toggle = $target.find(".toggle");
		var $traderow = $target.find(".TradeDiv");

		if ($traderow.css("visibility") === "hidden") {
			$traderow.css("visibility", "visible");
		}
		if ($toggle.hasClass("icon-plus-sign")) {
			$toggle.removeClass("icon-plus-sign");
			$toggle.addClass("icon-minus-sign");
		} else {
			$toggle.removeClass("icon-minus-sign");
			$toggle.addClass("icon-plus-sign");
		}
		$traderow.stop(true, true).animate({ height: "toggle", opacity: "toggle" }, 250);
	});

	$(".alert-table").click(function (e) {
		var $target = $(e.target).parents("table:first");
		var $actionrow = $target.find(".alert-actiondiv");

		if ($actionrow.css("visibility") === "hidden") {
			$actionrow.css("visibility", "visible");
		}
		$actionrow.stop(true, true).animate({ height: "toggle", opacity: "toggle" }, 250);
	});

	$(".alert-wrapper .heading").click(function (e) {
		$(".alert-wrapper .content").stop(true, true).animate({ height: "toggle", opacity: "toggle" }, 250);
	});

	$(".ExpandAll").click(function (e) {
		$target = $(".ExpandAll");
		var value = "";

		if ($target.prop("value") === "Expand All") {
			value = "Collapse All";
			$(".toggle.icon-plus-sign").trigger('click');
		} else {
			value = "Expand All";
			$(".toggle.icon-minus-sign").trigger('click');
		}
		$target.prop("value", value);
	});
};

function setNavCss() {
	var page = $("hgroup").children().text();
	var tab = 0;
	if (page === "Home") {
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