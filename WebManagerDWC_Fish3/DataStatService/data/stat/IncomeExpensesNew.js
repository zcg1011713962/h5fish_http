/*map start*/
function Map() 
{
	var gameIds = [0,1,2,4,6,10,12];
	var retVal = {};
	for(var i = 0; i < gameIds.length; i++)
	{
		retVal[gameIds[i]] = genItemDetail();
	}

	var curGame = retVal[this.gameId];
	if(curGame == undefined)
		return;

	var curItem = curGame[this.itemId];
	if(curItem == undefined)
		return;

	var curReason = curItem[this.reason];
	if(curReason == undefined)
		return;

	var delta = 0;

	delta = this.newValue-this.oldValue; 

	if(delta > 0)
	{
		curReason.income = delta;
	}
	else
	{
		curReason.outlay = -delta;
	}

	emit(1, retVal);

	function genItemDetail()
	{
		var items = [1, 2, 11, 14];
		var obj={};

		for(var i = 0; i < items.length; i++)
		{
			var id = items[i];
			obj[id] = genDetail();
		}
		return obj;
	}

	function genDetail()
	{
		var detailCount = 55;
		var obj={};
		for(var i = 1; i <= detailCount; i++)
		{
			obj[i] = {income:0, outlay:0};
		}
		return obj;
	}
}

/*map end*/

/*reduce start*/
function Reduce(key, values) 
{
	var gameIds = [0,1,2,4,6,10,12];
	var retVal = {};
	for(var i = 0; i < gameIds.length; i++)
	{
		retVal[gameIds[i]] = genItemDetail();
	}

	values.forEach(function(val) {

	for(var gid in val)
	{
		var inGame = val[gid];
		var outGame = retVal[gid];

		for(var itemId in inGame)
		{
			var inItem = inGame[itemId];
			var outItem = outGame[itemId];

			for(var r in inItem)
			{
				 outItem[r].income += inItem[r].income;
				 outItem[r].outlay += inItem[r].outlay;
			}
		}
	}
	});

	return retVal; 

	function genItemDetail()
	{
		var items = [1, 2, 11, 14];
		var obj={};

		for(var i = 0; i < items.length; i++)
		{
			var id = items[i];
			obj[id] = genDetail();
		}
		return obj;
	}

	function genDetail()
	{
		var detailCount = 55;
		var obj={};
		for(var i = 1; i <= detailCount; i++)
		{
			obj[i] = {income:0, outlay:0};
		}
		return obj;
	}
}

/*reduce end*/
