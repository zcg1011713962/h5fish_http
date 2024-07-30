/*map start*/
function Map() {

	var val = { joinCount:0, giveoutGold:0, matchTime:0, personCount:1, winCount:0 };
	var i = 0;
	var LevelCount = 10;
	
	if( this.joinCount )
	{
		val.joinCount = this.joinCount;
	}
	
	if( this.winCount )
	{
		val.winCount = this.winCount;
	}
	
	for( i = 1; i < LevelCount; i++ )
	{
		if( this['recvGoldLevel_' + i] )
		{
			val.giveoutGold += this['recvGoldLevel_' + i];
		}
		
		if( this['baoji_' + i] )
		{
			val[ 'baoji_' + i ] = this['baoji_' + i];
		}
		else
		{
			val[ 'baoji_' + i ] = 0;
		}
	}
	
	if( this['matchTime'] )
	{
		val.matchTime = this['matchTime'];
	}
	
	emit( 1, val );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	
	var reduced = { joinCount:0, giveoutGold:0, matchTime:0, personCount:0, winCount:0 };
	var LevelCount = 10;
	var i = 1; 
	for( ; i < LevelCount; i++ )
	{
		reduced[ 'baoji_' + i ] = 0;
	}
	
	values.forEach(function(val) {
		reduced.joinCount   += val.joinCount; 
		reduced.giveoutGold += val.giveoutGold; 
		reduced.matchTime += val.matchTime; 
		reduced.personCount += val.personCount; 
		reduced.winCount += val.winCount; 
		
		for( i = 1; i < LevelCount; i++ )
		{
			reduced[ 'baoji_' + i ] += val[ 'baoji_' + i ];
		}
	});

	return reduced;	
}
/*reduce end*/
