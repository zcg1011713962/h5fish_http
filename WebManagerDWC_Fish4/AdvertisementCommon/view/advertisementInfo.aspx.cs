using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace AdvertisementCommon.view
{
    public partial class advertisementInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string muid = Request.QueryString["muid"];
            string active_cb = Request.QueryString["active_cb"];
            //判断reg_cb是否为空
            string reg_cb = string.IsNullOrEmpty(Request.QueryString["reg_cb"]) ? "": Request.QueryString["reg_cb"];
            string ret = "";

            if (string.IsNullOrEmpty(muid)||string.IsNullOrEmpty(active_cb))
            {
                Dictionary<string, object> retdata = new Dictionary<string, object>();
                retdata.Add(RetResult.KEY_RESULT, RetResult.RET_PARAM_ERROR);
                ret = JsonHelper.genJson(retdata);
            }
            else
            {
                CMemoryBuffer buf = CommandBase.createBuf();
                BinaryWriter w = buf.Writer;
                w.Write(muid);
                w.Write(active_cb);
                w.Write(reg_cb);
                CommandAdvertisement cmd = new CommandAdvertisement();
                ret = cmd.execute(buf);
            }
        }
    }
}