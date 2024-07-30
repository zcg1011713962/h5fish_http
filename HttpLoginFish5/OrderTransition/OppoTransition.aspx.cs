using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.OrderTransition
{
    public partial class OppoTransition : System.Web.UI.Page
    {
        const string PRIVATE_KEY = @"<RSAKeyValue><Modulus>iDHKO8vBg0i5e2SW4crnRQR8ohPtkrgiIsGy81xXO5DUiyEdal6SQHUh0z49JsWCYoqUnmyl+nei6LM5EvtDF2YzwcCzx4OtYAtDwLU7uc+dU/YqN/EOMfdzSAWadejmnR32RtWVFWtGZzKJAbsrzR/fpOA0VObTAt+Q1Q0WWas=</Modulus><Exponent>AQAB</Exponent><P>zwOt/G3YYEzS3MeUqr7LgEzupYRyt/zGIiJSXSlCo+UmO+fNbFSgql292R73lSNMiDykCmyj3pYukPK94pyTsw==</P><Q>qGwM/PkON1OR4QQCrFl5Dh1RB4+kwukzSLEsq9kaq7yX9BBfQROHUvFn1D3J5p2YeLu93RSlpT1MzeYhYwGGKQ==</Q><DP>xKNOJTWCDxdqRGUgBoBtdVWeaoLBJZtp6QqJCg0jS+NiygyJyJkqdCnPJ+qOao+5Jr/TYkOH9OrbGLw7OoJYmQ==</DP><DQ>LOVQuQazEXCHjJIDHGplaKvL89YIt6RudUw1ekHPaERHxVmv6qsIAcY0RVGLRpxwDo8NV6evWFL07FYSRFAdsQ==</DQ><InverseQ>IOJBti2RZlw49idZTQat7jAZhGshhB16fdlCZB7QAVaIaa+9auLIFNzoww1zhXJ3xaK5kquM5lcQCK8PUp8XgQ==</InverseQ><D>W5f4pSxMEWnYbDbjDikyTgVNeIckcFbrhip0mhxRAltNxd/Yc4/0lerwe6vQ3oFYRcNjXgQ9v+uzoQHfcXePz3FLXi+s8VtvA5c/J7sK57GYk5zvh5dPR3CziSzvcSOq4K54BMleJbOd5O+WKgM8d61I5I5zg0XqCgw0K9Qhd/E=</D></RSAKeyValue>";
        //const string PRIVATE_KEY = @"<RSAKeyValue><Modulus>g4EKxSrvxSDjlJFnyoPDAjqngDgpeUo/STNX55b7J9dkGyEc91x+u1G6/U1oUkS2NFT+eYuyyTSlKbVyC/nac8y7W8O0lwrcrThUd+18DAly/g5Fwpq3gw1wkzsMQ8mMrvf5lEnykix4NDzKs7jkmQvhA0JgFKnQE/0bS2cz0SE=</Modulus><Exponent>AQAB</Exponent><P>vdIxSeLbAuQ//VHRgZSNxKGcTRZBUz/9N2Wgrsj0/DwKVeolPx5d4PhzsIgkDN3UVtdY1CHbVHrJ/an4LsYKeQ==</P><Q>sVn4+0g9Tf6HwwMpLtvOEreUJN10Hv2esCwr53prAW/EhO+itzvMazX1JJpPLaSWYDMS1qhV/wdPMf6Tn8RR6Q==</Q><DP>tCif4SlvhlIOLaaO+rZ8y0g7qapau+G6ue450EoowpFVme9OBJcUsOX+H2lHFfMgin9+7m9n1MlpltjQFcyeuQ==</DP><DQ>OJ4aiE3llFKfkLgfm5/8R6lnl+VCvuoEkQVo9rr0Ej9WI4JFFe33MnrFSewhPtb+UrO7Xd/bpFD3DWTSR/zqkQ==</DQ><InverseQ>Gg0fKnpRXs74aUkF7405RyGQJSYXxh38Efpbmhl9PLgh/LoS1LpzzDvM8myuBpqStKGcCek4LANsVySFzAJvug==</InverseQ><D>gUVQ+RGEMhxqm5l0ljeAc6iFEB4lZv39yE+YkihxLHEoDyM1/lQGoM1vK6H0cyrokfkbTxBs6C3ATof3XJ54hwRBxz3Em7bmWRTlKo/f9YzKoLySTC0F6JvpqwxCDuxUWvtGTu2+XB63bCSx2PRkP3dWnB5/UcN9VJaxvnar3gE=</D></RSAKeyValue>";
        const string CALL_BACK_URL = "http://123.207.170.249:26013/OppoPay.aspx";
        const string CALL_BACK_URL_TEST = "http://114.86.94.110:12140/OppoPay.aspx";
        const bool IS_TEST = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            OppoPreOrder obj = new OppoPreOrder();
            obj.AppId = "30437489";
            obj.PrivateKey = PRIVATE_KEY;
            obj.NotifyUrl = IS_TEST ? CALL_BACK_URL_TEST : CALL_BACK_URL;
            obj.AppKey = "aflGzv4p5Jc4480gc80sScg4S";

            string outstr = "";
            bool res = obj.loadData(Request, out outstr);
            if (!res)
            {
                Response.Write(outstr);
            }
            else
            {
                string str = obj.getOrder();
                Response.Write(str);
            }
        }
    }
}