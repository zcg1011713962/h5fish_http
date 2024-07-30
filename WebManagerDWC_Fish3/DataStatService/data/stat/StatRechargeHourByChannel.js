/*map start*/
function Map() 
{
	if( this.status == 1 ) return;
	
	emit( this.channel_number, {total : this.RMB} ); 
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
