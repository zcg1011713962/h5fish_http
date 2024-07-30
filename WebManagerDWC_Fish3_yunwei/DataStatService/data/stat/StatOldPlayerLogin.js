/*map start*/
function Map() {
	
	if( this.channel == undefined )
		return;
		
	var val = { count : 1 };
	emit( this.playerId + "_" + this.channel, val ); 
}

/*map end*/

/*reduce start*/
function Reduce(key, values) 
{
	var reduced = { count : 0 };
	return reduced;	
}
/*reduce end*/
