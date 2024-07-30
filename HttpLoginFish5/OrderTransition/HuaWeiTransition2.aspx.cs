using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpLogin.OrderTransition
{
    // com.yoyang.fishh5.huawei
    // 华为的另外一个包
    public partial class HuaWeiTransition2 : System.Web.UI.Page
    {
        public string PRIVATE_KEY = @"<RSAKeyValue><Modulus>hiWnQD4whNLySq0yOaRE3CGyQ7CbCscCwM7iprcmzpf9fXa2kuBA5YLvA88ft67LQfvaOBiJMAMOwJ9h5c9i6+tPc5Rqqz+BL72OKQ9itIHoTlx4bAI4p2Z9Zrkssld6UR/8oRAhNutIBc5eWENfsSnpsmTYzyqJUig9cQxuvE0HNlL5BqmKy8HIP7Ku3Mz5WRoKgGc6sZwLxeSMqYG8ylqR+U3pTY3didqVGSsRG9PHx/kXN8QoZUv8FlKHBRVLCOkGWfhAsayJ6DKb7LKpx+C3mS58Cem8Gpo/YJldOit7dVe4CVF4shquCCNt/fzID5FYzBE4Ze8NgPHtLGZ6zw==</Modulus><Exponent>AQAB</Exponent><P>wYwc8FvFk/xWMR7Ld2qe+T+KlBHpbMYoYaa9BOHZkIU/0eIxMySCwVYHi8/uyl7EnWqyYKxDv39b4iZPHkQq5WblH3FZ0S62GLtH+3gcecedSw/PTs5fKSks+Ffl2jJwdeyMBqMbzebyWml2WsYiRsl5jFioV016Jh835ieqLdU=</P><Q>sW7PtaQBMsIm1Cn098k0JrMy3OHWM0r5fzstRP/+znSc47nJx5h0STg9QCqwXv4tsVdOpwyVCHJ87imaZfZF1g6x5SqSr4lIm84yMsiRMU4hNGq9zKwbxR4a5+sXipp7MiN80dMBOQDPHWwkciZyC/zh8SfkQzuwov6e69H2xBM=</Q><DP>DC9kCwXcFIsIaiNEMAp4Y7zKXrQZ/A0lsbArqkkFx4F8GRyieFPOH5no87ZtlDQPMNHs1QVDZqUABebfiEpGYRbvSeF7BB5qvHfd+kj/kfMtRfKC3LVFbHP5LtFp2Sjfx6zSzAAs2fO2Qlbtap2avtYkXt+eCZEJJalFfan1ntk=</DP><DQ>D09nft3q+I1GOnJwX90tsdUuo16dVAm5I6BglJ6eRwRK8V97qMxf5F2E8397fobiPHYzJDjytyrh2s8eMoQgnIriEMsuoxTuffs526Xw2VdLiDFZtUp4jva9DZv8iQoxbS8gux5A8cE/HePR9UOZ/azRdjSh/qcs47Oi1OhmKZ8=</DQ><InverseQ>Za7hJR4Cf4JGAoUSGMLAqsmKppCNRJJgM3Dvq8miRu5yBHsVIDYXWN7mGWFWMt0LwBdc3F8ou1PP3BVGUThjW6V9vaRPMC82vTAvnHBnPzJYsQqAVUqIhiyZLRQmRqnoBwuGkLB1A7Qcurfc8obfOR3LULRAuCf6C8dsT+CV49k=</InverseQ><D>g14OSqBcPUai1bSEceRHNtecziL9wfEf5hbz6FWFoF1BdYMyyeToSJoPXMiSA/wdunFbwwFsH2NvJ/0tDrMn6WbXuJUIXMlkxo63z8fsXl61Uz/FAeiu+Ggqt9TxS3jVJOrRKB02maSRNHZi98aKyGEGJZgSpdW/AoC7oqCNNMJC2GSdPCoH2gKTdufGpcB/QHnAOPjNOQhKRlWG3iYi5BDNI2saiufYl12Yxdw2RzxgmxLQgfcYzhZkMPoXLME4hQmTATxadLTiNc8gP4vorCNMl9prSCs3TxHkaYebIcmWSCiUU4DRSlpQ8+3O5dBIwzgFmA+31EEzw2YBHZhKOQ==</D></RSAKeyValue>";

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