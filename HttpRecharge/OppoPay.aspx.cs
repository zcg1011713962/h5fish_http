using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpRecharge
{
    public partial class OppoPay : System.Web.UI.Page
    {
        const string PUBLIC_KEY = @"<RSAKeyValue><Modulus>m6loq9zDqwd1HwbqsLue7q8uo4x9iF8XF2YQzeP66NUXAAyuwWZA/KOLWBsxTnr6uKGYgw4RCVWFiLCCSR2qSWEtxMVR6qXt8XOwFEdAE4J3IMPuMCcD6szdPJRADmntufz25er5jBrqzo3DRsLZyzq8DvNTkPizGgb+a7AVZRU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        //const string PUBLIC_KEY = @"<RSAKeyValue><Modulus>iDHKO8vBg0i5e2SW4crnRQR8ohPtkrgiIsGy81xXO5DUiyEdal6SQHUh0z49JsWCYoqUnmyl+nei6LM5EvtDF2YzwcCzx4OtYAtDwLU7uc+dU/YqN/EOMfdzSAWadejmnR32RtWVFWtGZzKJAbsrzR/fpOA0VObTAt+Q1Q0WWas=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        // const string PUBLIC_KEY = @"<RSAKeyValue><Modulus>s/+eJJD6LIJnlGGeO/ehKox5jQIrJ65CihZ2Lwiem9r77AEcvkOc48mptnUHnxnjHd8CCQxsb3Whdd1a2wY4symGDfMbCmiD4pcjQzhU2qjpj5cIG4H+fx6NuJd5ciMSEMcbFVE1hyMLRfCE9KgSp6uR09UZyeLlYRHpkaCtEBE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        //public static string PUBLIC_KEY = "<RSAKeyValue><Modulus>pq3mCJD8FaLykfKyx1pRVcO2A35vbkDiSi/PUpresqVxlXawCipdMDWHN/43nAKD4tspQoX2tokbuYLpycjZqVmc5QjUSGrueX8RltFxLuedyIqazmXGsOTrmBpX+tIGvEE+qk6D9P3aYi/2W5jVDvZFIaLCIxLttrhYEfuXGMk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        protected void Page_Load(object sender, EventArgs e)
        {
            PayCallbackOppo obj = new PayCallbackOppo();
            obj.PublicKey = PUBLIC_KEY;
            obj.PayTableName = "oppo_pay";
            string str = obj.notifyPay(Request);
            Response.ContentType = "text/plain";
            Response.Write(str);
        }
    }
}