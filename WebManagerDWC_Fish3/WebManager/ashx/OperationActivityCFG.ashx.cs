using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Web.UI.HtmlControls;

namespace WebManager.ashx
{
    /// <summary>
    /// OperationActivityCFG 的摘要说明
    /// </summary>
    public class OperationActivityCFG : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            GMUser user = (GMUser)context.Session["user"];

            int itemId = Convert.ToInt32(context.Request.Form["itemId"]);
            string startTime = context.Request.Form["startTime"].Trim();
            string endTime = context.Request.Form["endTime"].Trim();
            Match m1 = Regex.Match(startTime, Exp.DATE_TIME4);
            Match m2 = Regex.Match(endTime, Exp.DATE_TIME4);
            OpRes res = OpRes.op_res_failed;
            if (m1.Success && m2.Success)
            {
                //startTime < endTime
                DateTime time1 = Convert.ToDateTime(startTime);
                DateTime time2 = Convert.ToDateTime(endTime);
                if (DateTime.Compare(time1, time2) >= 0)
                {
                    res = OpRes.op_res_param_not_valid;
                }
                else { 
                    //写入XML
                    string path = HttpRuntime.BinDirectory + "..\\" + "data";
                    string file = Path.Combine(path, "M_ActivityCFG.xml");
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);

                    XmlNode node = doc.SelectSingleNode("/Root");

                    for (node = node.FirstChild; node != null; node = node.NextSibling)
                    {
                        int id = Convert.ToInt32(node.Attributes["ID"].Value);
                        if (id == itemId) 
                        {
                            node.Attributes["StartTime"].Value = startTime;
                            node.Attributes["EndTime"].Value = endTime;

                            res = OpRes.opres_success;
                            break;
                        }
                        res = OpRes.op_res_failed;
                    }
                    //如果修改成功 保存
                    if (res == OpRes.opres_success)
                    {
                        doc.Save(file);
                        OpLogMgr.getInstance().addLog(LogType.LOG_TYPE_OPERATION_ACTIVITY_CFG,
                                new LogOperationActivityCFGEdit(itemId, startTime, endTime), user);
                    }
                }
            }
            else {
                res = OpRes.op_res_param_not_valid;
            }

            string str = OpResMgr.getInstance().getResultString(res);

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