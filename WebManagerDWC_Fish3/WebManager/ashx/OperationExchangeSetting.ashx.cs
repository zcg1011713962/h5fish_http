using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    public class OperationExchangeSetting : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_EXCHANGE_SETTING, context.Session, context.Response);

            GMUser user = (GMUser)context.Session["user"];

            Dictionary<string, object> retVal = new Dictionary<string, object>();

            do
            {
                int op = Convert.ToInt32(context.Request.Form["op"]);
                retVal.Add("op", op);

                var obj = new ExchangeSetting();
                if (obj == null || user == null)
                {
                    retVal.Add("resultStr", "unknown");
                    retVal.Add("result", 1);
                    break;
                }

                if (op == DefCC.OP_VIEW)
                {
                    obj.search(user, null);

                    var d = (Dictionary<string, object>)obj.getSearchResult();
                    retVal.Add("result", (int)OpRes.opres_success);
                    retVal.Add("data", d);
                }
                else if (op == DefCC.OP_MODIFY)
                {
                    int id = Convert.ToInt32(context.Request.Form["id"]);
                    int val = Convert.ToInt32(context.Request.Form["val"]);

                    CMemoryBuffer buf = new CMemoryBuffer();
                    buf.Writer.Write(id);
                    buf.Writer.Write(val);
                    OpRes code = obj.modify(user, buf);
                    retVal.Add("resultStr", OpResMgr.getInstance().getResultString(code));
                    retVal.Add("result", (int)code);
                    retVal.Add("id", id);
                }
            } while (false);

            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonHelper.ConvertToStr(retVal));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}