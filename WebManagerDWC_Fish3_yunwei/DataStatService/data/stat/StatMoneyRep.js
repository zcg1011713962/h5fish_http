/*map start*/
function Map() {

	var playerList = [ 12069, 10004456 ];
    var all_v = { gold:0, dragon:0};
	var fact_v = { gold:0, dragon:0};
	var big_v = { gold:0, dragon:0};
	var val = { "all" : all_v, "fact" : fact_v, "big" : big_v };
	
	all_v.gold = this.gold;
	all_v.dragon = this.dragonBall;
	if( this.is_robot == false )
	{
		var res = isSatify( this , playerList );
		if( res )
		{
			 fact_v.gold = this.gold;
			 fact_v.dragon = this.dragonBall;
			 
			 if( this.VipLevel >= 7 )
			 {
				big_v.gold = this.gold;
				big_v.dragon = this.dragonBall;
			 }
		}
	}
	
	emit( 1, val );
	
	function isSatify( obj, arrPlayerList )
	{
		var pid = obj.player_id;
		var inList = false;
		for( var i = 0; i < arrPlayerList.length; i++ )
		{
			if( pid == arrPlayerList[ i ]  )
			{
				inList = true;
				break;
			}
		}
		
		return !inList;
	}
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	
	 var all_v = { gold:0, dragon:0};
	var fact_v = { gold:0, dragon:0};
	var big_v = { gold:0, dragon:0};
	var reduced = { "all" : all_v, "fact" : fact_v, "big" : big_v };

	values.forEach(function(val) {
		reduced.all.gold += val.all.gold; 
		reduced.all.dragon += val.all.dragon; 
		
		reduced.fact.gold += val.fact.gold; 
		reduced.fact.dragon += val.fact.dragon; 
		
		reduced.big.gold += val.big.gold; 
		reduced.big.dragon += val.big.dragon; 
	});

	return reduced;	
}
/*reduce end*/




















