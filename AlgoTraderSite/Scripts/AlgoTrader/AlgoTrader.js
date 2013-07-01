$(document).ready(function (e) {

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