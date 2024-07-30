/*map start*/
function Map() {
	
	var items = [ "item5","item8","item9","item16","item17","item18","item19",
	"item20","item21","item23", "item24","item25","item26","item27","item52", "item72", "useLockCount", "useFrozeCount", "useViolentCount", "useCallCount"]

	var  val = {count:1};
	for( var i= 0; i < items.length; ++i)
	{
		if(this[items[i]] != undefined)
		{
			val[items[i]] = this[items[i]];
		}
		else
		{
			val[items[i]] = 0;
		}
	}
	
	emit( this.turretLevel + "_" + this.type,  val  );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	var  reduce = {count:0};
	
	var items = [ "item5","item8","item9","item16","item17","item18","item19",
	"item20","item21","item23", "item24","item25","item26","item27","item52", "item72","useLockCount", "useFrozeCount", "useViolentCount", "useCallCount"]
	for( var i= 0; i < items.length; ++i)
	{
		reduce[items[i]] = 0;
	}

	values.forEach(function(val) {
		reduce.count += val.count;
		
		for( var i= 0; i < items.length; ++i)
		{
			reduce[items[i]] += val[ items[i] ];
		}
	});

	return reduce;	
}
/*reduce end*/
