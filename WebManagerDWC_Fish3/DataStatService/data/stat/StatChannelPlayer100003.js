/*map start*/
function Map() {
	
	if( this.playerId == undefined ) return;
	
	var gold = (this.win_gold == undefined) ? 0 : this.win_gold;
	gold = (this.room_01_wingold == undefined) ? gold : this.room_01_wingold;
	var money = (this.payamount == undefined) ? 0 : this.payamount;
    var money1 = (this.time_payamount == undefined) ? 0 : this.time_payamount;
	 money += money1;
	 
	var val = { totalPay:money, totalGold:gold };
	emit( this.playerId, val );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	var  reduce = { totalPay:0, totalGold:0 };

	values.forEach(function(val) {
		reduce.totalPay += val.totalPay;
		reduce.totalGold += val.totalGold;
	});

	return reduce;	
}
/*reduce end*/
