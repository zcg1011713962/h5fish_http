using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Data.OleDb;
using System.Drawing;

public class ParamGraphExport
{
    public int m_gameId;

    public int m_roomId;

    public DateTime StartTime { set; get; }

    public DateTime EndTime { set; get; }

    // 输出
    public string m_fileName;
}

public class GraphExport
{
    static OnlineMgr m_mgr;
    public const string DATE_TIME24 = "yyyy-MM-dd-HH-mm-ss";
    static string m_exportDir;

    static GraphExport()
    {
        m_exportDir = HttpRuntime.BinDirectory + "..\\excel";
        m_mgr = new OnlineMgr();
        m_mgr.RRDToolPath = Path.Combine(HttpRuntime.BinDirectory, DefCC.RRD_TOOL_PATH);
        m_mgr.init();
    }

    public OpRes export(ParamGraphExport param, GMUser user)
    {
        bool res = false;
        var rrd = m_mgr.getRRd(param.m_gameId, param.m_roomId);
        if (rrd != null)
        {
            GraphParam p = new GraphParam();
            p.StartTime = param.StartTime;
            p.EndTime = param.EndTime;
            p.OuputPng = genFileName(param, user);
            p.LineColor = Color.Red;
            p.Title = getTitle(param);
            res = rrd.graph(p);
        }

        return res ? OpRes.opres_success : OpRes.op_res_failed;
    }

    string genFileName(ParamGraphExport param,GMUser user)
    {
        string f = string.Format("{0}_{1}_{2}_{3}_{4}.png",
            param.m_gameId, param.m_roomId,
            param.StartTime.ToString(DATE_TIME24),
            param.EndTime.ToString(DATE_TIME24),
            new Random().Next());

        string f1 = Path.Combine(m_exportDir, user.m_user);
        if (!Directory.Exists(f1))
        {
            Directory.CreateDirectory(f1);
        }

        param.m_fileName = Path.Combine("/excel/", user.m_user + "/", f);
        f = Path.Combine(f1, f);

        return f;
    }

    string getTitle(ParamGraphExport param)
    {
        if (param.m_gameId == 0)
        {
            return "总体";
        }
        else if (param.m_gameId == 1 && param.m_roomId == 0)
        {
            return "捕鱼总体";
        }
        else if (param.m_gameId == 1)
        {
            return "捕鱼" + StrName.s_fishRoomName[param.m_roomId - 1];
        }
        return StrName.getGameName3(param.m_gameId);
    }
}

