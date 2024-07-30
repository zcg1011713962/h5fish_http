/*map start*/
function Map() {
	
	if(this.itemId == undefined ) return;
	if(this.status !=4) return;
	//if(this.isReceive == false ) return;
	if(this.playerId == undefined) return;

	var  val = {money:0};
	val.money = getMoney(this);

	emit( this.playerId, val );

	function getMoney(obj)
	{
		if(obj.itemId == 1002)
		{
			return 30;
		}
		else if(obj.itemId == 1003)
		{
			return 50;
		}
		else if(obj.itemId == 1004)
		{
			return 100;
		}
		else if(obj.itemId == 1025)
		{
			return 50;
		}
		return 0;
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
