/*map start*/
function Map() {
	
	if(this.level == undefined || this.adv == undefined) return;

	var RANGE = [ {"start":1, "end":10}, 
		{"start":11, "end":20}, 
		{"start":21, "end":30}, 
		{"start":31, "end":40}, 
		{"start":41, "end":50}, 
		{"start":51, "end":60}, 
		{"start":61, "end":70}, 
		{"start":71, "end":80},
		{"start":81, "end":90},
		{"start":91, "end":100},
		{"start":101, "end":110},
		{"start":111, "end":120}
	];

	var len = RANGE.length;
	var  val = {};
	for( var i = 0; i < len; i++ )
	{
		val['index' + i] = 0;
	}

	var index = getIndex(RANGE, this.level);
	if( index >= 0 )
	{
		val['index' + index] = 1;
		emit( this.adv ? 1 :0, val );
	}

	function getIndex(range, curLevel)
	{
		var L = range.length;
		var i = 0;
		for( i = 0; i < L; i++)
		{
			if(curLevel >= range[i]["start"] && curLevel <= range[i]["end"])
				return i;
		}

		return -1;
	}
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	var  reduce = {};
	var i = 0;
	for( i = 0; i < 12; i++ )
	{
		reduce['index' + i] = 0;
	}

	values.forEach(function(val) {

		for( i = 0; i < 12; i++ )
		{
			reduce['index' +i ] += val['index' + i];
		}
	});

	return reduce;	
}
/*reduce end*/
