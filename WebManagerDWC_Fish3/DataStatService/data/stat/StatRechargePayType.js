/*map start*/
function Map() 
{
 if(this.PayType!='WechatSubscription')
  return;
  
 emit( this.PayType, {total: this.RMB, rechargeCount : 1, acc:this.Account, rechargePerson:1} );
}


/*map end*/

/*reduce start*/

function Reduce(key, values) 
{
 var reduced = {total: 0, rechargeCount : 0, acc:'1111', rechargePerson:0};
 var accs={};
 var count = 0;
 
 values.forEach(function(val) {
   reduced.total += val.total;  
   reduced.rechargeCount += val.rechargeCount;  
   if(accs[val.acc]==undefined)
   {
  count++;
  accs[val.acc]=true;
   }
   
  });
 reduced.rechargePerson=count;
 return reduced; 
}

/*reduce end*/
