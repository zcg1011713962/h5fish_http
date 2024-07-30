/*map start*/
function Map() {
	
    if (this.itemid == undefined ||
        this.itemCount == undefined)
        return;

	var val = {totalOutlay:0, correspondingGold:0, 
			   index0:0, index1:0, index2:0, index3:0, index4:0, index5:0, lotteryCount:1};
	val.totalOutlay = this.gold;
	if(this.lotteryId != undefined)
	{
		var it = (this.lotteryId - 1) % 6;
		val['index' + it] = 1;
	}
	
	switch(this.itemid)
	{
	case 1:
		val.correspondingGold = this.itemCount;
		break;
	case 2:
		val.correspondingGold = this.itemCount * 1000;
		break;
	case 11:
		val.correspondingGold = this.itemCount * 100;
		break;
	case 14:
		val.correspondingGold = this.itemCount * 5000;
		break;
	}
	emit(this.starlvl, val); 
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	
	var reduced = {totalOutlay:0, correspondingGold:0, 
			      index0:0, index1:0, index2:0, index3:0, index4:0, index5:0, lotteryCount:0};
	var it = 0;
	
	values.forEach(function(val) {
		reduced.totalOutlay   += val.totalOutlay; 
		reduced.correspondingGold += val.correspondingGold; 
		reduced.lotteryCount += val.lotteryCount;
		
		for(it = 0; it < 6; it++)
		{
			reduced['index' + it] += val['index' + it];
		}
	});

	return reduced;	
}
/*reduce end*/
