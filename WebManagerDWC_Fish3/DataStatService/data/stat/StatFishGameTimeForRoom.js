/*map start*/
function Map() {
	//if( this.channel == undefined) return;
	
	var onlineTime = {
		room1: 0, room2: 0, room3: 0, room4: 0, room5: 0, room6: 0, room7: 0, room8: 0, room9: 0,
		room10: 0, room11: 0,

		count1: 0, count2: 0, count3: 0, count4: 0, count5: 0, count6: 0, count7: 0, count8: 0, count9: 0,
	    count10: 0, count11: 0};
	
	var i = 1;
	for(; i <= 11; i++ )
	{
		if( this["game1_" + i] != undefined )
		{
			onlineTime["room"+i] = this["game1_" + i];
			
			if( this["game1_" + i] > 0 )
			{
				onlineTime["count"+i] = 1;
			}
		}
	}
	
	if( this.channel != undefined)
	{
		emit( this.channel,  onlineTime  );
	}
	
	emit(  "allChannel",  onlineTime  );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	var onlineTime = {
		room1: 0, room2: 0, room3: 0, room4: 0, room5: 0, room6: 0, room7: 0, room8: 0, room9: 0,
		room10: 0, room11: 0,
		count1: 0, count2: 0, count3: 0, count4: 0, count5: 0, count6: 0, count7: 0, count8: 0, count9: 0,
		count10: 0, count11: 0};
	
	values.forEach(function(val) {
		onlineTime.count1 += val.count1;
		onlineTime.count2 += val.count2;
		onlineTime.count3 += val.count3;
		onlineTime.count4 += val.count4;
		onlineTime.count5 += val.count5;
		onlineTime.count6 += val.count6;
		onlineTime.count7 += val.count7;
		onlineTime.count8 += val.count8;
		onlineTime.count9 += val.count9;
		onlineTime.count11 += val.count11;

		onlineTime.room1 += val.room1;
		onlineTime.room2 +=  val.room2;
		onlineTime.room3 +=  val.room3;
		onlineTime.room4 +=  val.room4;
		onlineTime.room5 +=  val.room5;
		onlineTime.room6 +=  val.room6;
		onlineTime.room7 +=  val.room7;
		onlineTime.room8 +=  val.room8;
		onlineTime.room9 += val.room9;
		onlineTime.room11 += val.room11;
	});

	return onlineTime;	
}
/*reduce end*/
