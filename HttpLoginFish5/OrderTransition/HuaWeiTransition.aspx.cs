using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.OrderTransition
{
    // 华为的支付签名，预签名后，客户端再请求支付
    public partial class HuaWeiTransition : System.Web.UI.Page
    {
        public string PRIVATE_KEY = @"<RSAKeyValue><Modulus>yDhkLHLjHzRnioypp+bPRQozXYCVjQRPCO+KWhEOe4ZLcBda5gebiysIAteGit4bUbzKlkouBs+C5bdrM2RimtmB1oiRiLwduseKdlFlmmCzE/GOXjWMRvbCFZOgRlh5d4PN2ea1SUmeEBG/N6D3XWJ4SyqNFfWmT5c5Kxsi5AOor90wKlNkBrfpUHAvWZx5V+uA1LSkI9yG5Jtv+z+G5m+ffTNcR9FpzyaXHOu08BTXBmLjb9mOWdcMgrE1zhJFxK2yWe8ycVu2c8Ch+fqyos96LnXN1f7vnDTPCRiUQGFFlIJRG3mlRWSJMHZkQjljp7JlR5xnKbC/q8HwQKMWXw==</Modulus><Exponent>AQAB</Exponent><P>9h80SVpDd3d/OxNbf2+bZwyJ4a4BJisiahsyf+xzcKKgAaKSdtF9PIBsRpStYuPqPXOC2ucDLGNsvjcC6emkmHZJqP67emp5nRuz3K/lHF4LrELvPJ+nfNc6qBl5pPtBCYj7BIFfQ9nQ/SyBSgr73mBhY3ZklQf8F1+/6P1Sbqc=</P><Q>0EGRYKDPvtPOk6jp+LjJMvByNgIT8uQpx8mmFkBXtqeYXD2+AqkCiaUHDlJ0udaohmDhUeKl3Q0cp/Zi1yEJxl6v2rSYL0AT3LTctELEJS/m3yWPfiit1OxT6Ae20QoUfiL7qqCm/jC4/Z92QQdN79elSMxp4K0lOndY02+OCYk=</Q><DP>f1JPKy8tvaysvfPanocu/lUnMv9gCs2/pOJi8nhwk7EdUxs154+h2N2apbyeIY1EDPwnknR1WM4qYBB3SmiaxGBU5L66X66bMSI2npDD1jP0l+2cc2EoNNXmuzVKj4WG9JbPPFps3N3eVkpiq9oYx8ZWOyzxBYO27qSS08XQkfs=</DP><DQ>U70Jp8EBx8mDFvykl841mLX2NqE/RDtaUWtTzT6yBk/9g5mP/aHX02D4JVoidVdLDD2IXFS2PNw42ZDIzZgLRihsrZRQXS92zRgsZo8yL4HDY5bGzAOvYh5k3re7WvKiqtWGPGW8wBj3SYQFGMDHJdmgLFWFfUIh5hB3Nw10H4E=</DQ><InverseQ>nwTk7TTWcesL6sj5+FwGaEvNGeY5LUSZPgP6FLm+55UK3XX9SGktxc0aKhX7MgLfP4prSEIbJ2ansljQPpeJOYOc/wrWgFR+YaksjERuik1PC8UXpRbkg3IvgUfQ8k4gAEzBuNPZ20phV3bDFjOe01qFn9e/OPRhtudrmIdhVAo=</InverseQ><D>QQ/2GW2R+ZNtbBvui6WYyA4bB/DfaC40wqoYktEFHEXVIM0uGB7zwCMRHN9NlLW9Oe/4AyOC1mq7Vgum9dHSqxAwPXcul7jNjmMGTxF/S03OE5aE1X9gILVR93H7/LAGdLxrebobQf/+sakasqiHiZyUi5R9uVuUmjuBc80DJwWntVznGVapJcRCc9B3rDJETggLaj/AEcQfIud+ILU3VousMh+WZPL5FI2wX8stP4IP28iyMMoGfsvhV8lH3bCKnG04VaKoLTZahQr7FZ40+Xsbyk/SLLfoF71aJsEKhdR4d2dckLOa1Maty+FekVGxBVNPscx8gIyVO6xqL4+IGQ==</D></RSAKeyValue>";

        protected void Page_Load(object sender, EventArgs e)
        {
            HuaWeiOrderInfoSign obj = new HuaWeiOrderInfoSign();
            obj.PrivateKey = PRIVATE_KEY;
            string sign = obj.getSign(Request);
            Response.ContentType = "text/plain";
            Response.Write(sign);
        }
    }
}