/*map start*/
function Map() {
	if( this.roomId == undefined ) return;

	emit( this.roomId, { count:1 } );
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
