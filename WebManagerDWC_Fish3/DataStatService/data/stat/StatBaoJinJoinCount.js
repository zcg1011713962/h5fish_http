/*map start*/
function Map() {
	emit( this.playerId, { count:1 } );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	var result = { count:0 }
	values.forEach(function(val) {
		result.count += val.count;
	});

	return result;	
}
/*reduce end*/
