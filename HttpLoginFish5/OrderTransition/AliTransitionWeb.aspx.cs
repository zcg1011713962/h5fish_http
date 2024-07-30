using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.OrderTransition
{
    // 支付宝web支付
    public partial class AliTransitionWeb : System.Web.UI.Page
    {
        //public const string ALI_APP_ID = "2021001161687578";
        //public const string ALI_PRIVATE_KEY = @"<RSAKeyValue><Modulus>mKnIzdNVMdCEnji4J55oDdC4NXezZsYpm5tBJ4eyOS/mxWNnFHcYw8cXre7cqkEEvZ7zJ3UxUafqo+r3sylY+cuMfwKHFGaV2RQpwbM0I3Y91OeuXAA4ji8YQj4SvmS592Ua+r6wdyVzLtouhtLkMXfmrhKyVaYIWLedWBAulsn/dHWG1NBPZQel5GSBEWsd5dTepOh9P/SLVpcmXLDzeBSUfBXzofL4lH7HXsCN+udqbJTHmN7HZdDhGwqfMHT67p5hPOw21cuMqCbWXc0NQGIYV2qcGp9tUGCSyQ7QCBT0ikKTRLpk/c1yw4rv0PXiTmvCAKnBS2hV4xxbK7mEtQ==</Modulus><Exponent>AQAB</Exponent><P>/IhLCmnPXZ9MIQ5WfD/puKTF9lfyvLB8m7Im+mTDaCjvh6CvVlZyOr2U38Vh8qdQyss/vR/KcDAGLIUeQtaEc+dVDUKiKWCSl8qKWDozU3TvrXsgWqFSYfKMglnjnmamG1Cg0LDv9RjhXBvNC26DQFWkRuZRx9tCXWalslIBAiM=</P><Q>msJt3ap9PcO5sec2TgaHYxrgPQeTiXZaB64WCg325JRlhB77XdIAbTzpUfXawfEthPyLKCf8UE0fXJR5eAW/33d/SvjI353koLcGW5hv5Rt8+xi9S21UMM6bJ/w6rLh7HRvvD2K8FXRUHPUR2R8PjDHs5YpNmAdjR6fIKajUr0c=</Q><DP>jftXLYimFT5OADvedkc88hp6TvHNwTb9KFC2x4tFrldtrPS0ADfkS5BxloqUcmiN7SbvNDcei4sEvZ0ukWeo2r2SvTzcjaUFZqByvf4jA2Y4p3IVk78l4XoMc/F0H4gZFjxz3kHM+CG/+xiKZUYCN3avQUmXa2WkX30p5iNi+78=</DP><DQ>XEp00It6stnFJOX4yaE1HhIKBfs9re1plUjpFcfsI7anr5n2V6YD4SFBF0Kh2aTle3hL2H+4BX9oo4JbygrLuQ1/WQtyZ4C2tm1PmlIW8K9q4ieHw7KIUa70cm0F5LkDmoFtGGTOO5ErFDXGFhBi6j1fxCetTGujdjkFsmpfo8M=</DQ><InverseQ>9YcXXqMyKz7E9aUlfkScF8tKfqYfjjPKtsv7fVABm1SYrO7Ap7Ck33lZO7jBmtKyAWrQQkyyI8ubErMUp/ZcgPq6atAzizzGg92FsqGZexp/jddlU6h+zkRW1cnCpHCg6NSa26OF+jctbRxsxQGqydrp3kSWSoaoF0bw9G8loFI=</InverseQ><D>K6V3HKmMFPbpw5axx9n5I+IFmHvFld+X674ijZu1L7ZrQwndYbN/KBIWygqdmiuGoHP+Tsypsg/Q/NZjxRxQLPKuE230kreVpf+mxJNol67plKI8bl89zCDYza+Tik2gAYQth3zXVOknGkEUxkUrKp1+HUyJIgWbI0ozuQa/Wyqm5S/hL2c4Is7EsLaJs1RxItt66hZfbFXpiLxSL+Lt3jx+amg7OJCf+reqEJrUntcv3+c8RjUZXJIIUjm33/4dGfYIRx7YrVSVzoZkD+eAwGHqdz0o51lZm1MI2ju7tCJjKRZ7Ns5EuyYoJJYdjkGY60Z5NjpRiTMIfXX6NU8djQ==</D></RSAKeyValue>";
        //public const string ALI_PUBLIC_KEY = @"MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBALTTiyu2WJDpxvfP496aRtJnFzrloB3jFYj2826EGxhCMBjZDkKapdrY5vkzSsMqPnyBqCsa31GR6jIJtl/JRUSmE9gmaDqlxXJ3aiSullANU3zR/BOmQVonbmhVQGnCizzV0tmCRa1jGrxsJHS19Dk6NIdrCiWdh3M2QlVmKBfxAgMBAAECgYAWk0qOvIc2IFmc2rGCOxSYdBJsYfqpgI5RuTMPGyMe1jSXBZJIMnJ+bhH4DrVIxF9kv/M03nf/AQ6SDLBeKQyinXjwrrpnTxEp2fFmJauof1nZt3WwY4Aow8FaYABYZ3SpHh37Q9+b9pkFdlh2Nj+66464EPgZdloy54RDnD3DeQJBAOzR2jboCL23U50T/1UwNo4CzWMwavJerOkZJ00/fVbebyUZQxrTQ3XQfGz4qLm+1gtzE7tNn2Enz0Xf8DVFzTMCQQDDeL3OHRKihJyIwqaJNplPLtxSV4WfZ+TAXPJkmBhrBBRgl8DF2OtEDXyHJuX+pp02lsldAVhKZrwaNh5Wsx5LAkA3QSNT6kGX2j1VCgRqIOypp7e6K+LYGATqAidsW6Ln8NAn7MP+b0pvI6zUVBQx+nfAhiIVcp/8MCipWf2WwGmHAkBjY8JGyhuOjRU2qJqbDCr5yx71s1DbE62JbflF0twflexyjNbVAo3lhWH7KnkpeThY6GSsqKFm+0PLpBbbCKpvAkAYq5D/3YHr4Pfp+zcqgMAelqbJrDOvZhZDJqoWvwJtQEBcY8aP8R9lpPvIksGejA5Ty4JUx/RIzqIFUkvhFmiE";

        //public const string ALI_PRIVATE_KEY = @"<RSAKeyValue><Modulus>iMH5ap3ZtpT6OjrFki6yMObiiWfgxjwo7AVcPukygeN8nBqwYFkaY3hYJMSy0yUXyw/8oN6Iz/r7GIsWXz3KGAx7xO8zLUgEZa5MPB2JR6vuC27R1soM1wdo4NSSV0UK2d50IsUoxScTYfavYRSQ2FSjo4wI187cjYpP/84DImODkCtk17RlPSUrBs3xUrR2AtLNdzjCnHpMofcCbZsMqCq8t0eDK1DATrcVy5yJCec67YU0r0EpF5t51Tw7JQB9GPfb4BSmB2JCsKywZpEODUwbJLYAXVHcmIMptTpjgjcgNGKp7jxuAltlmfDEJLrMEVcEfKmVvZwBa9mnKV8/dQ==</Modulus><Exponent>AQAB</Exponent><P>2/TwwHHyNiY28aPQSEg2IK3XS4SYESGslRLIW9tnzoD/rbPqyccfDbb3aqJohNM+1zpBKWhG0Mb6jLpSE12LfYw/JtFg3yfG/bHn3ZNY6X/nT3lVWunAQwyXvvXAslPXbuTFVEbTeLropPKIm0z2QTyxvVvmduZmdUS0qG/YS1M=</P><Q>nyrhcUIYd/pj2xKjTCrg0R83fl02b+pWR36xIPlUpafxNpmIaPV2Ael2AQUS44/cdWQeXvEBJS1Y86XiIJPFAeKESoMYaeZSSJd27Mqu2s7flGv2l79Bf/xalE3gBMFEyy0ylY+qf/UN21hFivAOwDfTGT4FR4YKCMw4f/DJORc=</Q><DP>UxfnU2w4akhpfUO7TXMqL5I8wSSoJeCox/A+jKUIRDTrYf2T9wIoMxApy7jq7zK2jKxPLYtdJUXmJP/GPdaa+aTfvpRemi72t6RbyZL+hcdx5M4bdqrTnQDG2rcQQs31lskJ2tqezP8fICEXhkO+y8pYAwcSu1wy3J5F+VO3TE0=</DP><DQ>CmHusMeh9vmTV/AATPCjF6EUqp6D9Yrws9s0zLTW67Nnzar4NfiFCRzkgK6HC+cPd2zfekv4SieKHOKePIfHGxdej8m7ZvUKOwf+TvXtsAI8nV7ph/6G1EVu1yu5SDYfOa2qFg14Dr66d6trZePA4pMBpyzfhUneD7LaoKXhSmM=</DQ><InverseQ>hqFCeyoBNErFZ5SKW0IOl5KaaoLO71oiM5vaNZvE0hJUvzWLKzFP26SAERHJlnF1fSvpVTG4UjE+xnsZI5YsqyG9eAyfv2iAskSAtMwzFIxUZNp8Brle/zB7Y4LPW1AzJ4E18HwF6klojslx5Z+jBFpgT6RdinHKgPHmLO+11eU=</InverseQ><D>A8Q751jTnzi5L5Ngz6bjl380bA5lXXBYvC9lnAH2NyeZzf7aLr0lgenVm6WjOzeBUUFudsvKPtgtxD5IbnUYNNNbMVBZO30JSfej99/kYQn2M3Bo7R6BfgahN9gC885d/yQmVLLTbaKm4QLXT7guSRcDrsRzleYHowpVK7lz0qWaj+WVB/1roqiY8mOavTnsY0KzypCzn5P6jSm33T78OjzQnhGP6YzXz+UNf5M9//oL5TgnZ/LGGsmnsDZ5ZE4LkX2CFbhAtkjxUrMNi9ykIFAw9lkoaXfvTEnz7gsKebLM9dO02LKRLqTCwAw3ByO1od4UD3OlE4I8SuvKjiLTnQ==</D></RSAKeyValue>";
        //public const string ALI_PUBLIC_KEY = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAiMH5ap3ZtpT6OjrFki6yMObiiWfgxjwo7AVcPukygeN8nBqwYFkaY3hYJMSy0yUXyw/8oN6Iz/r7GIsWXz3KGAx7xO8zLUgEZa5MPB2JR6vuC27R1soM1wdo4NSSV0UK2d50IsUoxScTYfavYRSQ2FSjo4wI187cjYpP/84DImODkCtk17RlPSUrBs3xUrR2AtLNdzjCnHpMofcCbZsMqCq8t0eDK1DATrcVy5yJCec67YU0r0EpF5t51Tw7JQB9GPfb4BSmB2JCsKywZpEODUwbJLYAXVHcmIMptTpjgjcgNGKp7jxuAltlmfDEJLrMEVcEfKmVvZwBa9mnKV8/dQIDAQAB";

        public const string ALI_APP_ID = "2019082966655948";
        public const string ALI_PRIVATE_KEY = @"<RSAKeyValue><Modulus>mKnIzdNVMdCEnji4J55oDdC4NXezZsYpm5tBJ4eyOS/mxWNnFHcYw8cXre7cqkEEvZ7zJ3UxUafqo+r3sylY+cuMfwKHFGaV2RQpwbM0I3Y91OeuXAA4ji8YQj4SvmS592Ua+r6wdyVzLtouhtLkMXfmrhKyVaYIWLedWBAulsn/dHWG1NBPZQel5GSBEWsd5dTepOh9P/SLVpcmXLDzeBSUfBXzofL4lH7HXsCN+udqbJTHmN7HZdDhGwqfMHT67p5hPOw21cuMqCbWXc0NQGIYV2qcGp9tUGCSyQ7QCBT0ikKTRLpk/c1yw4rv0PXiTmvCAKnBS2hV4xxbK7mEtQ==</Modulus><Exponent>AQAB</Exponent><P>/IhLCmnPXZ9MIQ5WfD/puKTF9lfyvLB8m7Im+mTDaCjvh6CvVlZyOr2U38Vh8qdQyss/vR/KcDAGLIUeQtaEc+dVDUKiKWCSl8qKWDozU3TvrXsgWqFSYfKMglnjnmamG1Cg0LDv9RjhXBvNC26DQFWkRuZRx9tCXWalslIBAiM=</P><Q>msJt3ap9PcO5sec2TgaHYxrgPQeTiXZaB64WCg325JRlhB77XdIAbTzpUfXawfEthPyLKCf8UE0fXJR5eAW/33d/SvjI353koLcGW5hv5Rt8+xi9S21UMM6bJ/w6rLh7HRvvD2K8FXRUHPUR2R8PjDHs5YpNmAdjR6fIKajUr0c=</Q><DP>jftXLYimFT5OADvedkc88hp6TvHNwTb9KFC2x4tFrldtrPS0ADfkS5BxloqUcmiN7SbvNDcei4sEvZ0ukWeo2r2SvTzcjaUFZqByvf4jA2Y4p3IVk78l4XoMc/F0H4gZFjxz3kHM+CG/+xiKZUYCN3avQUmXa2WkX30p5iNi+78=</DP><DQ>XEp00It6stnFJOX4yaE1HhIKBfs9re1plUjpFcfsI7anr5n2V6YD4SFBF0Kh2aTle3hL2H+4BX9oo4JbygrLuQ1/WQtyZ4C2tm1PmlIW8K9q4ieHw7KIUa70cm0F5LkDmoFtGGTOO5ErFDXGFhBi6j1fxCetTGujdjkFsmpfo8M=</DQ><InverseQ>9YcXXqMyKz7E9aUlfkScF8tKfqYfjjPKtsv7fVABm1SYrO7Ap7Ck33lZO7jBmtKyAWrQQkyyI8ubErMUp/ZcgPq6atAzizzGg92FsqGZexp/jddlU6h+zkRW1cnCpHCg6NSa26OF+jctbRxsxQGqydrp3kSWSoaoF0bw9G8loFI=</InverseQ><D>K6V3HKmMFPbpw5axx9n5I+IFmHvFld+X674ijZu1L7ZrQwndYbN/KBIWygqdmiuGoHP+Tsypsg/Q/NZjxRxQLPKuE230kreVpf+mxJNol67plKI8bl89zCDYza+Tik2gAYQth3zXVOknGkEUxkUrKp1+HUyJIgWbI0ozuQa/Wyqm5S/hL2c4Is7EsLaJs1RxItt66hZfbFXpiLxSL+Lt3jx+amg7OJCf+reqEJrUntcv3+c8RjUZXJIIUjm33/4dGfYIRx7YrVSVzoZkD+eAwGHqdz0o51lZm1MI2ju7tCJjKRZ7Ns5EuyYoJJYdjkGY60Z5NjpRiTMIfXX6NU8djQ==</D></RSAKeyValue>";
        public const string ALI_PUBLIC_KEY = @"MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBALTTiyu2WJDpxvfP496aRtJnFzrloB3jFYj2826EGxhCMBjZDkKapdrY5vkzSsMqPnyBqCsa31GR6jIJtl/JRUSmE9gmaDqlxXJ3aiSullANU3zR/BOmQVonbmhVQGnCizzV0tmCRa1jGrxsJHS19Dk6NIdrCiWdh3M2QlVmKBfxAgMBAAECgYAWk0qOvIc2IFmc2rGCOxSYdBJsYfqpgI5RuTMPGyMe1jSXBZJIMnJ+bhH4DrVIxF9kv/M03nf/AQ6SDLBeKQyinXjwrrpnTxEp2fFmJauof1nZt3WwY4Aow8FaYABYZ3SpHh37Q9+b9pkFdlh2Nj+66464EPgZdloy54RDnD3DeQJBAOzR2jboCL23U50T/1UwNo4CzWMwavJerOkZJ00/fVbebyUZQxrTQ3XQfGz4qLm+1gtzE7tNn2Enz0Xf8DVFzTMCQQDDeL3OHRKihJyIwqaJNplPLtxSV4WfZ+TAXPJkmBhrBBRgl8DF2OtEDXyHJuX+pp02lsldAVhKZrwaNh5Wsx5LAkA3QSNT6kGX2j1VCgRqIOypp7e6K+LYGATqAidsW6Ln8NAn7MP+b0pvI6zUVBQx+nfAhiIVcp/8MCipWf2WwGmHAkBjY8JGyhuOjRU2qJqbDCr5yx71s1DbE62JbflF0twflexyjNbVAo3lhWH7KnkpeThY6GSsqKFm+0PLpBbbCKpvAkAYq5D/3YHr4Pfp+zcqgMAelqbJrDOvZhZDJqoWvwJtQEBcY8aP8R9lpPvIksGejA5Ty4JUx/RIzqIFUkvhFmiE";

        // 测试
        //public const string NOTIFY_URL = "http://114.86.95.190:12140/AliWebPay.aspx";
        //public const string RETURN_URL = "http://114.86.95.190:12140/AliWebPayResult.aspx";

        // 正式
        public const string NOTIFY_URL = "http://123.207.170.249:26013/AliWebPay.aspx";
        public const string RETURN_URL = "http://123.207.170.249:26013/AliWebPayResult.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            CAliTransitionWeb obj = new CAliTransitionWeb();
            obj.AppId = ALI_APP_ID;
            obj.AppPrivateKey = ALI_PRIVATE_KEY;
            obj.AppPublicKey = ALI_PUBLIC_KEY;
            obj.NotifyUrl = NOTIFY_URL;
            obj.ReturnUrl = RETURN_URL;

            obj.OrderId = Request.QueryString["orderId"];
            obj.PayCode = Request.QueryString["payCode"];
            obj.TotalAmount = Request.QueryString["amount"];
            obj.Subject = Request.QueryString["productName"];

            string str = obj.getHtml();
            Response.Write(str);
        }
    }
}

