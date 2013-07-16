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

	//$('.symbol-button.lightbox').click(function () {
	//	$('.lightbox-wrapper, .lightbox-content').animate({ 'opacity': '.50' }, 300, 'linear');
	//	$('.lightbox-content').animate({ 'opacity': '1.00' }, 300, 'linear');
	//	$('.lightbox-wrapper, .lightbox-content').css('display', 'block');
	//});

	//$('.lightbox-close').click(function () {
	//	close_box();
	//});

	//$('.lightbox-wrapper').click(function () {
	//	close_box();
	//});
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

function close_box() {
	$('.lightbox-wrapper, .lightbox-content').animate({ 'opacity': '0' }, 300, 'linear', function () {
		$('.lightbox-wrapper, .lightbox-content').css('display', 'none');
	});
}