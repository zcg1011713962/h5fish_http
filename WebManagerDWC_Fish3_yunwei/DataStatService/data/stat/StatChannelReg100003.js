/*map start*/
function Map() {
	
	if( this.regeditCount == undefined ) return;
	
	var val = { count:this.regeditCount };
	emit( 1, val );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	var  reduce = { count:0 };

	values.forEach(function(val) {
		reduce.count += val.count;
	});

	return reduce;
}
/*reduce end*/
