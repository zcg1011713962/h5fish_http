using System;
using System.IO;
using System.Text;
using System.Web;

public class CLOG
{
    public static void Info(string message)
    {
        write(message);
    }

    public static void Info(string message, params object[] args)
    {
        write(message, args);
    }

    private static void write(string msg)
    {
        try
        {
            string file = getfileName();
            string time = DateTime.Now.ToString();
            StreamWriter sw = new StreamWriter(file, true, Encoding.Default);
            sw.WriteLine(time + " " + msg);
            sw.Close();
        }
        catch (System.Exception ex)
        {
        }
    }

    private static void write(string msg, params object[] args)
    {
        try
        {
            string file = getfileName();
            string time = DateTime.Now.ToString();
            StreamWriter sw = new StreamWriter(file, true, Encoding.Default);
            sw.WriteLine(time + " " + msg, args);
            sw.Close();
        }
        catch (System.Exception ex)
        {	
        }
    }

    private static string getfileName()
    {
        DateTime dt = DateTime.Now;
        string f = Convert.ToString(dt.Year) + "_" + Convert.ToString(dt.Month) + "_" + Convert.ToString(dt.Day) + "_" + dt.Hour + ".txt";

        string dir = HttpRuntime.BinDirectory + "..\\log\\";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        f = dir + f;
        return f;
    }
}
