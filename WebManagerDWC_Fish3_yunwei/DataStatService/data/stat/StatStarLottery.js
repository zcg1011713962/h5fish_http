/*map start*/
function Map() {
	
    if (this.itemid == undefined ||
        this.itemCount == undefined)
        return;

	var val = {totalOutlay:0, gold:0,gem:0,db:0,chip:0};
	
	val.totalOutlay = this.gold;
	switch(this.itemid)
	{
	case 1:
		val.gold = this.itemCount;
		break;
	case 2:
		val.gem = this.itemCount;
		break;
	case 11:
		val.chip = this.itemCount;
		break;
	case 14:
		val.db = this.itemCount;
		break;
	}
	emit(this.starlvl, val); 
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	
	var reduced = {totalOutlay:0, gold:0,gem:0,db:0,chip:0};

	values.forEach(function(val) {
		reduced.totalOutlay   += val.totalOutlay; 
		reduced.gold += val.gold; 
		reduced.gem += val.gem; 
		reduced.db += val.db; 
		reduced.chip += val.chip; 
	});

	return reduced;	
}
/*reduce end*/
