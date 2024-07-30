/*map start*/
function Map() 
{
	if( this.status == 1 ) return;
	
	emit( this.PlayerId, {total : this.RMB, rechargeCount : 1} ); 
}

/*map end*/

/*reduce start*/
function Reduce(key, values) 
{
	var reduced = { total : 0, rechargeCount : 0 };

	values.forEach(function(val) {
		reduced.total += val.total;  
		reduced.rechargeCount += val.rechargeCount;  
	});

	return reduced; 
}

/*reduce end*/
