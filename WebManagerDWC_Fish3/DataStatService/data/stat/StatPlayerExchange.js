/*map start*/
function Map() {
	
	//if(this.itemId == undefined ) return;
	if(this.status !=4) return;
	//if(this.isReceive == false ) return;
	if(this.playerId == undefined) return;

	var  val = {money:0};
	val.money = getMoney(this);

	emit( this.playerId, val );

	function getMoney(obj)
	{
		var elems = {
			// 30元话费
			"id_7" : 30,  
			// 爱奇艺季卡
			"id_8" : 50,
			// 优酷视频季卡
			"id_9" : 50,
			// 100元京东卡
			"id_10": 100,
			// 爱奇艺季卡
			"id_11" : 50,
			// 优酷视频季卡
			"id_12" : 50,
			// 50元京东卡
			"id_15" : 50,
			// 50元话费
			"id_22" : 50,
			// 30元话费
			"id_29" : 30,
			// 50元话费
			"id_30" : 50,
			// 100元京东卡
			"id_31" : 100,
			// 200元京东卡
			"id_32" : 200,
			// 100元京东卡
			"id_39" : 100,
			// 200元京东卡
			"id_40" : 200,
			// 100元京东卡
			"id_41" : 100,
			// 50元京东卡
			"id_45" : 50
		};

		var v = elems[ "id_" + obj.chgId];
		if( v == undefined) return 0;
		
		return v;
	}
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	var  reduce = {money:0};

	values.forEach(function(val) {
		reduce.money += val.money;
	});

	return reduce;	
}
/*reduce end*/
