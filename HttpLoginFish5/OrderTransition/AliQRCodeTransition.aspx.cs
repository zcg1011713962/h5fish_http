using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace HttpLogin.OrderTransition
{
    // 支付宝二维码支付
    public partial class AliQRCodeTransition : System.Web.UI.Page
    {
        public const string ALI_PRIVATE_KEY = @"<RSAKeyValue><Modulus>mKnIzdNVMdCEnji4J55oDdC4NXezZsYpm5tBJ4eyOS/mxWNnFHcYw8cXre7cqkEEvZ7zJ3UxUafqo+r3sylY+cuMfwKHFGaV2RQpwbM0I3Y91OeuXAA4ji8YQj4SvmS592Ua+r6wdyVzLtouhtLkMXfmrhKyVaYIWLedWBAulsn/dHWG1NBPZQel5GSBEWsd5dTepOh9P/SLVpcmXLDzeBSUfBXzofL4lH7HXsCN+udqbJTHmN7HZdDhGwqfMHT67p5hPOw21cuMqCbWXc0NQGIYV2qcGp9tUGCSyQ7QCBT0ikKTRLpk/c1yw4rv0PXiTmvCAKnBS2hV4xxbK7mEtQ==</Modulus><Exponent>AQAB</Exponent><P>/IhLCmnPXZ9MIQ5WfD/puKTF9lfyvLB8m7Im+mTDaCjvh6CvVlZyOr2U38Vh8qdQyss/vR/KcDAGLIUeQtaEc+dVDUKiKWCSl8qKWDozU3TvrXsgWqFSYfKMglnjnmamG1Cg0LDv9RjhXBvNC26DQFWkRuZRx9tCXWalslIBAiM=</P><Q>msJt3ap9PcO5sec2TgaHYxrgPQeTiXZaB64WCg325JRlhB77XdIAbTzpUfXawfEthPyLKCf8UE0fXJR5eAW/33d/SvjI353koLcGW5hv5Rt8+xi9S21UMM6bJ/w6rLh7HRvvD2K8FXRUHPUR2R8PjDHs5YpNmAdjR6fIKajUr0c=</Q><DP>jftXLYimFT5OADvedkc88hp6TvHNwTb9KFC2x4tFrldtrPS0ADfkS5BxloqUcmiN7SbvNDcei4sEvZ0ukWeo2r2SvTzcjaUFZqByvf4jA2Y4p3IVk78l4XoMc/F0H4gZFjxz3kHM+CG/+xiKZUYCN3avQUmXa2WkX30p5iNi+78=</DP><DQ>XEp00It6stnFJOX4yaE1HhIKBfs9re1plUjpFcfsI7anr5n2V6YD4SFBF0Kh2aTle3hL2H+4BX9oo4JbygrLuQ1/WQtyZ4C2tm1PmlIW8K9q4ieHw7KIUa70cm0F5LkDmoFtGGTOO5ErFDXGFhBi6j1fxCetTGujdjkFsmpfo8M=</DQ><InverseQ>9YcXXqMyKz7E9aUlfkScF8tKfqYfjjPKtsv7fVABm1SYrO7Ap7Ck33lZO7jBmtKyAWrQQkyyI8ubErMUp/ZcgPq6atAzizzGg92FsqGZexp/jddlU6h+zkRW1cnCpHCg6NSa26OF+jctbRxsxQGqydrp3kSWSoaoF0bw9G8loFI=</InverseQ><D>K6V3HKmMFPbpw5axx9n5I+IFmHvFld+X674ijZu1L7ZrQwndYbN/KBIWygqdmiuGoHP+Tsypsg/Q/NZjxRxQLPKuE230kreVpf+mxJNol67plKI8bl89zCDYza+Tik2gAYQth3zXVOknGkEUxkUrKp1+HUyJIgWbI0ozuQa/Wyqm5S/hL2c4Is7EsLaJs1RxItt66hZfbFXpiLxSL+Lt3jx+amg7OJCf+reqEJrUntcv3+c8RjUZXJIIUjm33/4dGfYIRx7YrVSVzoZkD+eAwGHqdz0o51lZm1MI2ju7tCJjKRZ7Ns5EuyYoJJYdjkGY60Z5NjpRiTMIfXX6NU8djQ==</D></RSAKeyValue>";
        public const string ALI_PUBLIC_KEY = @"MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBALTTiyu2WJDpxvfP496aRtJnFzrloB3jFYj2826EGxhCMBjZDkKapdrY5vkzSsMqPnyBqCsa31GR6jIJtl/JRUSmE9gmaDqlxXJ3aiSullANU3zR/BOmQVonbmhVQGnCizzV0tmCRa1jGrxsJHS19Dk6NIdrCiWdh3M2QlVmKBfxAgMBAAECgYAWk0qOvIc2IFmc2rGCOxSYdBJsYfqpgI5RuTMPGyMe1jSXBZJIMnJ+bhH4DrVIxF9kv/M03nf/AQ6SDLBeKQyinXjwrrpnTxEp2fFmJauof1nZt3WwY4Aow8FaYABYZ3SpHh37Q9+b9pkFdlh2Nj+66464EPgZdloy54RDnD3DeQJBAOzR2jboCL23U50T/1UwNo4CzWMwavJerOkZJ00/fVbebyUZQxrTQ3XQfGz4qLm+1gtzE7tNn2Enz0Xf8DVFzTMCQQDDeL3OHRKihJyIwqaJNplPLtxSV4WfZ+TAXPJkmBhrBBRgl8DF2OtEDXyHJuX+pp02lsldAVhKZrwaNh5Wsx5LAkA3QSNT6kGX2j1VCgRqIOypp7e6K+LYGATqAidsW6Ln8NAn7MP+b0pvI6zUVBQx+nfAhiIVcp/8MCipWf2WwGmHAkBjY8JGyhuOjRU2qJqbDCr5yx71s1DbE62JbflF0twflexyjNbVAo3lhWH7KnkpeThY6GSsqKFm+0PLpBbbCKpvAkAYq5D/3YHr4Pfp+zcqgMAelqbJrDOvZhZDJqoWvwJtQEBcY8aP8R9lpPvIksGejA5Ty4JUx/RIzqIFUkvhFmiE";

        // 测试回调
        const string PAY_NOTIFY_URL_TEST = "http://114.86.94.110:12140/AliQRPay.aspx";
        // 正式回调
        const string PAY_NOTIFY_URL = "http://123.207.170.249:26013/AliQRPay.aspx";
        public bool IS_TEST = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            AliQRCode obj = new AliQRCode();
            obj.NotifyURL = IS_TEST ? PAY_NOTIFY_URL_TEST : PAY_NOTIFY_URL;
            obj.AppId = "2019082966655948";
            obj.MchId = "2088921100168084";
            obj.AppPrivateKey = ALI_PRIVATE_KEY;
            obj.AppPublicKey = ALI_PUBLIC_KEY;

            string str = obj.setUpParam(Request);
            if (obj.OpcodeResult != 0) // 获取参数不成功，直接返回错误
            {
                Response.ContentType = "text/plain";
                Response.Write(str);
                return;
            }

            string codeUrl = obj.getCodeUrl();
            MemoryStream ms = obj.genQR(codeUrl);
            Response.ContentType = "image/png";
            Response.BinaryWrite(ms.GetBuffer());
            Response.End();
        }
    }
}