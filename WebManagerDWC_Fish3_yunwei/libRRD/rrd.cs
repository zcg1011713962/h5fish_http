using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHAWK;
using System.IO;
using System.Drawing;

public class RRDOnlinePerson
{
    static DateTime s_time1970 = new DateTime(1970, 1, 1);

    public string DbFileName { set; get; }

    public DateTime StartTime { set; get; }

    public string RRDToolPath { set; get; }

    RRD m_rrd;

    public RRD createDbFile()
    {
        string rt = Path.Combine(RRDToolPath, "rrdtool.exe");
        if (!File.Exists(rt))
        {
            return null;
        }

        NHawkCommand.Instance.RRDCommandPath = rt;

        string file = Path.GetFileName(DbFileName);
        m_rrd = new RRD(DbFileName, getSeconds(StartTime));
        m_rrd.step = 60;
        m_rrd.addDS(new DS("online", DS.TYPE.GAUGE, 60 * 5, 0, 10000000));
        m_rrd.addRRA(new RRA(RRA.CF.AVERAGE, 0.5, 1, 24 * 60 * 30 * 3));

        if (!File.Exists(DbFileName))
        {
            try
            {
                m_rrd.create(true);
            }
            catch (System.Exception ex)
            {
            }
        }

        return m_rrd;
    }

    public void insertData(DateTime upTime, int onlineNum)
    {
        if (m_rrd != null)
        {
            try
            {
                m_rrd.update(getSeconds(upTime), new object[] { onlineNum });
            }
            catch (System.Exception ex)
            {
            }
        }
    }

    public bool graph(GraphParam param)
    {
        bool res = false;

        if (m_rrd != null)
        {
            try
            {
                GRAPH gr = new GRAPH(param.OuputPng,
                getSeconds(param.StartTime).ToString(),
                getSeconds(param.EndTime).ToString());

                if (!string.IsNullOrEmpty(param.Title))
                {
                    gr.title = param.Title;
                }
                
                gr.yaxislabel = "onlinenum";
                gr.width = 1280;
                gr.height = 800;

                gr.addDEF(new DEF("myonline", m_rrd, "online", RRA.CF.AVERAGE));
                gr.addGELEM(new LINE(1, "myonline", param.LineColor, "online player count", false));
                gr.graph();
                res = true;
            }
            catch (System.Exception ex)
            {
            }
        }

        return res;
    }

    public static int getSeconds(DateTime s)
    {
        TimeSpan span = s - s_time1970;
        return Math.Abs((int)span.TotalSeconds) - 8 * 3600;
    }
}

public class GraphParam
{
    public DateTime StartTime { set; get; }

    public DateTime EndTime { set; get; }

    public string OuputPng { set; get; }

    public Color LineColor { set; get; }

    public string Title { set; get; }
}









