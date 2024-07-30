/*map start*/
function Map() {

	if( this.reason == undefined ) return;
	
	if(  !(this.reason == 9 || this.reason == 14 ) ) return;
	
	// 锁定 冰冻 狂暴 火力符文   瞄准符文  炮管符文  召唤 普通鱼雷   升级石
	var arr = [ "5", "8", "17", "11", "12", "13", "9", "23", "16" ];
	var val = { };
	var i = 0;
	for( i = 0; i < arr.length; ++i)
	{
		val[ arr[i] ] = 0;
	}
	
	if( val[ this.itemId + ''] != undefined )
	{
		val[ this.itemId + ''] = 1;
	}
		
	emit( this.playerId, val  );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	
	var arr = [ "5", "8", "17", "11", "12", "13", "9", "23", "16" ];
	var reduced = { };
	var i = 0;
	for( i = 0; i < arr.length; ++i)
	{
		reduced[ arr[i] ] = 0;
	}
	
	values.forEach(function(val) {
		
		for( i =0; i < arr.length; ++i)
		{
			reduced[ arr[i]  ]  += val[  arr[i]  ] ;
		}

	});

	return reduced;	
}
/*reduce end*/




















