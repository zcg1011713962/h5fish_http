/*map start*/
function Map() {

	var task1 = { finish:0, recv:0 };
	var task2 = { finish:0, recv:0 };
	var task3 = { finish:0, recv:0 };
	var task = { 1:task1, 2:task2, 3:task3 };
	
	for( var k = 1; k <= 3; k++ )
	{
		var p = 'p' + k;
		var t = 't' + k;
		
		var r = setTask( this[p], this[t], task[k] );
		if( r )
		{
			var p1 = this[p];
			if( p1 < 30)
			{
				p1 = 29;
			}
			emit( p1 + '_' + k , task[k] );
		}
	}
	
	function setTask( p, t, task )
	{
		var result = false;
		if( p != undefined && t != undefined )
		{
			if( t == 1 )
			{
				task.finish = 1;
				result = true;
			}
			else if( t == 2 )
			{
				task.finish = 1;
				task.recv = 1;
				result = true;
			}
		}
		
		return result;
	}
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	
	var reduced = {  finish:0, recv:0  };

	values.forEach(function(val) {
		reduced.finish += val.finish; 
		reduced.recv += val.recv; 
	});

	return reduced;	
}
/*reduce end*/




















