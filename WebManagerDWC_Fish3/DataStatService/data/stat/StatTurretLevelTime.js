/*map start*/
function Map() {
	if( this.TurretLevel == undefined) return;
	if( this.channel == undefined) return;

	var  onlineTime = {gameTime:this.onlineTime,count:1};
	
	emit(  this.TurretLevel + "_" + this.channel,  onlineTime  );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	var  onlineTime = {gameTime:0,count:0};
	
	values.forEach(function(val) {
		onlineTime.gameTime += val.gameTime;
		onlineTime.count += val.count;
		
	});

	return onlineTime;	
}
/*reduce end*/
