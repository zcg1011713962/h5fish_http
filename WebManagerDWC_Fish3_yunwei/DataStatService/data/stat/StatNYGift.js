/*map start*/
function Map() {

	var MAX_BUY_COUNT = 18;
	
	for( var i = 1; i <= 3; i++ )
	{
		var obj = getMap( this, i );
		if( obj != undefined )
		{
			emit( i, obj ); 
		}
	}

	function getMap( obj, id )
	{
		var buyCount = obj[ 'gift_' + id ];
		if( buyCount == undefined )
		{
			return undefined;
		}
		
		var giftCount = {};
		var i = 1;
		
		for( i = 1; i <= buyCount; i++ )
		{
			giftCount[ 'count_' + i ] = 1;
		}
		for( i = buyCount + 1; i <= MAX_BUY_COUNT; i++ )
		{
			giftCount[ 'count_' + i ] = 0;
		}
		
		return giftCount;
	}
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	
	var MAX_BUY_COUNT = 18;
	var i = 0;
	var reduced = {};
	
	for( i = 1; i <= MAX_BUY_COUNT; i++ )
	{
		reduced[ 'count_' + i ] = 0;
	}
	
	values.forEach(function(val) {
	
		for( i = 1; i <= MAX_BUY_COUNT; i++ )
		{
			reduced[ 'count_' + i ] += val[ 'count_' + i ];
		}
	});

	return reduced;	
}
/*reduce end*/
