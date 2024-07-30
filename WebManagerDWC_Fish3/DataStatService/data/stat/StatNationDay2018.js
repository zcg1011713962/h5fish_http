/*map start*/
function Map() {

	var val = { lottery1:0, lottery2:0 };
	val['lottery' + this.lotteryId] = 1;
	emit( 1, val );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	
	var reduced = { lottery1:0, lottery2:0 };

	values.forEach(function(val) {
		reduced.lottery1 += val.lottery1; 
		reduced.lottery2 += val.lottery2; 
	});

	return reduced;	
}
/*reduce end*/
