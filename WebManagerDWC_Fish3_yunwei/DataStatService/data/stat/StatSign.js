/*map start*/
function Map() {
	
	if( this.signCount == undefined )
		return;
		
	var val = { count : 1 };
	emit( this.signCount, val ); 
}

/*map end*/

/*reduce start*/
function Reduce(key, values) 
{
	var reduced = { count : 0 };
	values.forEach(function(val) {
		reduced.count += val.count; 
	});
	return reduced;	
}
/*reduce end*/
