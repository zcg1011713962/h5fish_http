/*map start*/
function Map() {
	
	var  val = { missCount:0, levelCount:1 };
	
	if( this.missCount == 0)
	{
		val.missCount = 1;
	}
	
	emit( this.playerLv,  val  );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	var  reduce = { missCount:0, levelCount:0 };
	
	values.forEach(function(val) {
		reduce.missCount += val.missCount;
		reduce.levelCount += val.levelCount;
		
	});

	return reduce;	
}
/*reduce end*/
