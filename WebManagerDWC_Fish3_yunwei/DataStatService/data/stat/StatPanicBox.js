/*map start*/
function Map() {

	var val = { box1:0, box2:0, box3:0 };
	val['box' + this.boxId] = 1;
	emit( 1, val );
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {
	
	var reduced = { box1:0, box2:0, box3:0 };

	values.forEach(function(val) {
		reduced.box1 += val.box1; 
		reduced.box2 += val.box2; 
		reduced.box3 += val.box3; 
	});

	return reduced;	
}
/*reduce end*/
