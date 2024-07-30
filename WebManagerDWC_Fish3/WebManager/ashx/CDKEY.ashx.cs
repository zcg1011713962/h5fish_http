using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebManager.ashx
{
    public class CDKEY : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            RightMgr.getInstance().opCheck(RightDef.OTHER_CD_KEY, context.Session, context.Response);
            GMUser user = (GMUser)context.Session["user"];

            string retStr = "";
            ParamCDKEY param = new ParamCDKEY();
            param.m_op = Convert.ToInt32(context.Request.Form["op"]);
            switch (param.m_op)
            {
                case 1:
                    {
                        param.m_pici = context.Request.Form["pici"];
                        param.m_count = context.Request.Form["count"];
                        param.m_deadTime = context.Request.Form["validDays"];
                        param.m_giftId = context.Request.Form["giftId"];
                        OpRes res = user.doDyop(param, DyOpType.opTypeGiftCode);

                        Dictionary<string, object> jd = new Dictionary<string, object>();
                        jd.Add("result", OpResMgr.getInstance().getResultString(res));
                        jd.Add("curPici", CountMgr.getInstance().getCurId(CountMgr.GIFT_KEY, false) + 1);
                        retStr = ItemHelp.genJsonStr(jd);
                    }
                    break;
                case 2:
                    {
                        OpRes res = user.doDyop(param, DyOpType.opTypeGiftCode);
                        List<Dictionary<string, object>> retList = (List<Dictionary<string, object>>)user.getDyopResult(DyOpType.opTypeGiftCode);
                        Dictionary<string, object> jd = new Dictionary<string, object>();
                        jd.Add("piciList", retList);
                        retStr = ItemHelp.genJsonStr(jd);
                    }
                    break;
                case 4: // 获取礼包内容
                    {
                        var allData = M_CDKEY_GiftCFG.getInstance().getAllData();
                        Dictionary<string, object> jdGift = new Dictionary<string, object>();
                        foreach (var d in allData)
                        {
                            jdGift.Add(d.Key.ToString(), d.Value.m_name);
                        }
                        Dictionary<string, object> piciId = new Dictionary<string, object>();
                        piciId.Add("key", CountMgr.getInstance().getCurId(CountMgr.GIFT_KEY, false) + 1);

                        Dictionary<string, object> jd = new Dictionary<string, object>();
                        jd.Add("gift", jdGift);
                        jd.Add("curPici", piciId);
                        retStr = ItemHelp.genJsonStr(jd);
                    }
                    break;
                case 5: // 导出cdkey
                    {
                        string pici = context.Request.Form["pici"];
                        ExportMgr mgr = user.getSys<ExportMgr>(SysType.sysTypeExport);
                        OpRes res = mgr.doExport(pici, ExportType.exportTypeCDKEY, user);
                        retStr = OpResMgr.getInstance().getResultString(res);
                    }
                    break;
                case 6: // 启用，禁用状态
                    {
                        param.m_pici = context.Request.Form["pici"];
                        param.m_deadTime = context.Request.Form["enable"];
                        OpRes res = user.doDyop(param, DyOpType.opTypeGiftCode);
                        retStr = OpResMgr.getInstance().getResultString(res);
                    }
                    break;
                case 7: // 修改
                    {
                        param.m_pici = context.Request.Form["pici"];
                        param.m_deadTime = context.Request.Form["validDays"];
                        param.m_giftId = context.Request.Form["giftId"];

                        OpRes res = user.doDyop(param, DyOpType.opTypeGiftCode);
                        retStr = OpResMgr.getInstance().getResultString(res);
                    }
                    break;
                case 8: // 通知服务器重载数据
                    {
                        param.m_pici = context.Request.Form["pici"];
                        OpRes res = user.doDyop(param, DyOpType.opTypeGiftCode);
                        retStr = OpResMgr.getInstance().getResultString(res);
                    }
                    break;
                case 9:
                    {
                        param.m_pici = context.Request.Form["cdkey"];
                        OpRes res = user.doDyop(param, DyOpType.opTypeGiftCode);
                        string str = OpResMgr.getInstance().getResultString(res);
                        Dictionary<string, object> ret = null;
                        if (res == OpRes.opres_success)
                        {
                            List<Dictionary<string, object>> retList = 
                                (List<Dictionary<string, object>>)user.getDyopResult(DyOpType.opTypeGiftCode);
                            ret = retList[0];
                            if (ret.ContainsKey("useTime"))
                            {
                                string useTime = Convert.ToDateTime(ret["useTime"]).ToLocalTime().ToString();
                                ret.Remove("useTime");
                                ret.Add("useTime", useTime);
                            }
                            if (ret.ContainsKey("genTime"))
                            {
                                string genTime = Convert.ToDateTime(ret["genTime"]).ToLocalTime().ToShortDateString();
                                ret.Remove("genTime");
                                ret.Add("genTime", genTime);
                            }
                            ret["cdkey"] = param.m_pici;
                        }
                        else
                        {
                            ret = new Dictionary<string, object>();
                        }
                        ret.Add("result", (int)res);
                        ret.Add("resultStr", str);

                        retStr = ItemHelp.genJsonStr(ret);
                    }
                    break;
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(retStr);
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