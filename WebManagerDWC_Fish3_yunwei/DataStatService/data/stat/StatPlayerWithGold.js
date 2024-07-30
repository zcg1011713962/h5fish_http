/*map start*/
function Map() {
	
	var  val = {gold:this.gold + this.itemValue, count:1};

	emit( this.turretLevel + "_" + this.type,  val  );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	var  reduce = {gold:0,count:0};

	values.forEach(function(val) {
		reduce.gold += val.gold;
		reduce.count += val.count;
	});

	return reduce;	
}
/*reduce end*/
