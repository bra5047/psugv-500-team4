$(document).ready(function (e) {
	//$("table").css("background-color", "red");
	$(".toggle").click(function (e) {
		var $target = $(e.target).parents("table:first"); // target = button -> cell -> row -> table
		$target = $target.find(".TradeRow");

		if ($target.css("visibility", "hidden")) {
			$target.css("visibility", "visible");
			$target.stop(true, true).animate({ height: "toggle", opacity: "toggle" }, 'fast');
		}
	});
});