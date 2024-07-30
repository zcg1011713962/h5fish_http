/*map start*/
function Map() {

	var count = 0;
	if( undefined != this.joinCount )
	{
		count = this.joinCount;
	}
	var obj = { joinCount : count, joinPerson : 1 };
	emit( 1, obj ); 
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	
	var reduced = { joinCount : 0, joinPerson : 0 };
	
	values.forEach(function(val) {
		reduced.joinCount += val.joinCount;
		reduced.joinPerson += val.joinPerson;
	});

	return reduced;	
}
/*reduce end*/
