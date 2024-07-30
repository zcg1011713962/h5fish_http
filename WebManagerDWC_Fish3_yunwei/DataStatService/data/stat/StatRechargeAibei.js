/*map start*/
function Map() 
{
	emit( this.channel_number, {total : this.money} ); 
}

/*map end*/

/*reduce start*/
function Reduce(key, values) 
{
	var reduced = { total : 0 };

	values.forEach(function(val) {
		reduced.total += val.total;  
	});

	return reduced; 
}

/*reduce end*/
