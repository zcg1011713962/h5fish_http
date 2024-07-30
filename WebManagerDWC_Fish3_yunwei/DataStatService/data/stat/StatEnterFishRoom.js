/*map start*/
function Map() {

	if( this.loginDay == undefined ) return;
	if( this.loginDay != 0 ) return;

	var val = { count : 1};
		
	emit( 1, val  );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	
	var reduced = { count:0};
	
	values.forEach(function(val) {
		
		reduced.count  += val.count;
	});

	return reduced;	
}
/*reduce end*/




















