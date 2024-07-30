/*map start*/
function Map() {
 
 if(this.itemId == 14)
 {
  var val={dbGen:0, dbConsume:0};
  var delta = (this.newValue-this.oldValue);
  var isEmit = false;
  
  if(delta>0)
  {
   val.dbGen = delta;
   isEmit = true;
  }
  else if(delta < 0)
  {
   val.dbConsume = -delta;
   isEmit = true;
  }
  if(isEmit)
  {
   emit(1, val);
  }
 } 
}

/*map end*/

/*reduce start*/
function Reduce(key, values) {

 var reduced ={dbGen:0, dbConsume:0};

 values.forEach(function(val) {
  reduced.dbGen   += val.dbGen;  
  reduced.dbConsume += val.dbConsume; 
 });

 return reduced; 

}

/*reduce end*/
