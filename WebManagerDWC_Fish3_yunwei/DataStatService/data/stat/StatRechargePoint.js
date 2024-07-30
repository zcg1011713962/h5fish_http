/*map start*/
function Map() {

	var val = { rechargeCount:1, rmbSum:this.RMB };
	
	emit( this.PayCode, val );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	
	var reduced = { rechargeCount:0, rmbSum:0 };

	values.forEach(function(val) {
		reduced.rechargeCount += val.rechargeCount; 
		reduced.rmbSum += val.rmbSum; 
	});

	return reduced;	
}
/*reduce end*/
