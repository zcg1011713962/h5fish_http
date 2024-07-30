/*map start*/
function Map() {
	emit( this.gameLevel, { playerId:this.playerId, count:1 } );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	var result = { playerId:0, count:0 }
	var tmp = {};
	values.forEach(function(val) {
		if( tmp[ val.playerId ] == null )
		{
			tmp[ val.playerId ] = true;
			result.count = result.count + 1;
		}
	});

	return result;	
}
/*reduce end*/
