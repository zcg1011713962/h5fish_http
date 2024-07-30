/*map start*/
function Map() {

	if( this.roomId == undefined ) return;

	emit( this.TurretLevel + "_" + this.roomId, { playerList:1} );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	var result = { playerList:0 };
	var i = 0;
	values.forEach(function(val) {
	
		result.playerList += val.playerList;
		/*for( i = 0; i < val.playerList.length; i++ )
		{
			if( !_isExist( result.playerList, val.playerList[i] ) )
			{
				result.playerList.push( val.playerList[i]  );
			}
		}*/
	
	});

	return result;	
	
	function _isExist( arr, id )
	{
		var i = 0;
		for( i = 0; i < arr.length; i++ )
		{
		    if( arr[ i ] == id )
				return true;
		}
		
		return false;
	}
}
/*reduce end*/
