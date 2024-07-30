/*map start*/
function Map() {

	var val = { playerCount:1 };
	
	if( this['joinCount'] )
	{
		emit( this['joinCount'], val );
	}
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	
	var reduced = { playerCount:0 };

	values.forEach(function(val) {
		reduced.playerCount += val.playerCount; 
	});

	return reduced;	
}
/*reduce end*/
