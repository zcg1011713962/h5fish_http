using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    /// <summary>
    /// OPerationGameCtrl 的摘要说明
    /// </summary>
    public class OPerationGameCtrl : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.OP_OPERATION_GAME_CTRL, context.Session, context.Response);

            ParamQuery param = new ParamQuery();

            param.m_param = context.Request.Form["id"];
            param.m_op = Convert.ToInt32(context.Request.Form["op"]);

            GMUser user = (GMUser)context.Session["user"];
            OpRes res = user.doDyop(param, DyOpType.opTypeOPerationGameCtrl);

            string str = "";

            //p.m_op  0 添加 1删除 2查看
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("op", param.m_op);

            switch (param.m_op)
            {
                case 2:
                    {
                        List<OperationGameCtrlItem> itemList =
                            (List<OperationGameCtrlItem>)user.getSys<DyOpMgr>(SysType.sysTypeDyOp).getResult(DyOpType.opTypeOPerationGameCtrl);
                        data.Add("buffList", BaseJsonSerializer.serialize(itemList));
                        str = ItemHelp.genJsonStr(data);
                    }
                    break;
                case 1:
                    {
                        data.Add("result", (int)res);
                        data.Add("m_id", param.m_param);
                        str = ItemHelp.genJsonStr(data);
                    }
                    break;
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(str);
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