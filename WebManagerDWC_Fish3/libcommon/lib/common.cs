using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.Net;
using System.Collections.Specialized;

public struct Exp
{
    public const string DATE_TIME = @"^\s*(\d{4})/(\d{1,2})/(\d{1,2})\s*$";
    public const string DATE_TIME1 = @"^\s*(\d{4})/(\d{1,2})/(\d{1,2})\s+(\d{1,2})$";
    public const string DATE_TIME2 = @"^\s*(\d{4})/(\d{1,2})/(\d{1,2})\s+(\d{1,2}):(\d{1,2})$";
    public const string DATE_TIME3 = @"^\s*(\d{4})-(\d{1,2})-(\d{1,2})\s+(0\d{1}|1\d{1}|2[0-3]):([0-5]\d{1})\s*$"; //2018-02-08 00:00
    //包括时分秒
    public const string DATE_TIME4 = @"^\s*(\d{4})-(\d{1,2})-(\d{1,2})\s+(0\d{1}|1\d{1}|2[0-3]):([0-5]\d{1}):([0-5]\d{1})\s*$"; //2018-02-08 00:00:00

    public const string DATE_TIME5 = @"^\s*(\d{4})-(\d{1,2})-(\d{1,2})\s*$";  //2018-02-08
    public const string DATE_TIME6 = @"^\s*(\d{4})-(\d{1,2})-(\d{1,2})\s*(,\s*(\d{4})-(\d{1,2})-(\d{1,2})\s*)?\s*$";  //2020-02-08 [, 2020-02-10]

    // 以空格相隔的两个数字
    public const string TWO_NUM_BY_SPACE = @"^\s*(-?\d+)\s+(-?\d+)\s*$";
    // 以空格相隔的两个数字
    public const string TWO_NUM_BY_SPACE_SEQ = @"^(\s*(\d+)\s+(\d+)\s*)(;\s*(\d+)\s+(\d+)\s*)+$";
    // 匹配IP地址
    public const string IP_ADDRESS = @"\s*(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\s*$";
    public const string IP_ADDRESS1 = @"\s*(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5})\s*$";

    public const string ACCOUNT_RULE = @"^[0-9a-zA-Z]{6,14}$";

    public const string PWD_RULE = @"^[a-zA-Z0-9]{6,14}$";
    // 匹配多个客服段
    public const string SERVICE_HELP_M = @"^([1-4],[^#,]+,[^#,]+)(#[1-4],[^#,]+,[^#,]+)+$";
    // 匹配一个客服段
    public const string SERVICE_HELP1 = @"^([1-4],[^#,]+,[^#,]+)$";
    public const string NUM_LIST_BY_BLANK = @"\s*(-{0,1}\d+)(\s+-{0,1}\d+)+\s*$";

    public const string SINGLE_NUM = @"\s*\d+\s*$";

    //以冒号隔开的两个正整数
    public const string TWO_NUM_BY_COLON = @"^\s*(\d+)\s*(:)\s*(\d+)\s*$";

    //以逗号隔开的两个正整数
    public const string TWO_NUM_BY_COMMA = @"^\s*(\d+)\s*(,)\s*(\d+)\s*$";

    //6-10位长数字字母密码
    public const string PWD_RULE_AIR_DROP = @"^[0-9a-zA-Z]{6,10}$";
}

public class URLParam
{
    public string m_url = "";
    public string m_text = "";
    public string m_key = "";
    public string m_value = "";
    public string m_className = "";
    public string m_target;

    // 额外的url参数
    public Dictionary<string, object> m_exUrlParam = null;

    public void clearExParam()
    {
        if (m_exUrlParam != null)
        {
            m_exUrlParam.Clear();
        }
    }

    public void addExParam(string key, object v)
    {
        if (m_exUrlParam == null)
        {
            m_exUrlParam = new Dictionary<string, object>();
        }
        m_exUrlParam.Add(key, v);
    }
}

public class Tool
{
    public static StringBuilder m_textBuilder = new StringBuilder();
    // 每天多少秒
    public const long SECONDS_EACH_DAY = 24 * 3600;
    // 每天包含的百纳秒数
    public const long TICKS_EACH_DAY = SECONDS_EACH_DAY * 10000000;

    // 生成时间
    public static long genTime(Match min, bool isaddoneday)
    {
        GroupCollection c = min.Groups;
        int y = Convert.ToInt32(c[1].Value);
        int m = Convert.ToInt32(c[2].Value);
        int d = Convert.ToInt32(c[3].Value);
        DateTime t = new DateTime(y, m, d);
        if (isaddoneday)
        {
            t = t.AddDays(1);
        }
        return t.Ticks;
    }

    private static bool genTime(Match match, bool isadd, int date_type, ref DateTime resTime)
    {
        GroupCollection c = match.Groups;
        int y = Convert.ToInt32(c[1].Value);
        if (y > 9999)
            return false;

        int m = Convert.ToInt32(c[2].Value);
        if (m < 1 || m > 12)
            return false;

        int d = Convert.ToInt32(c[3].Value);
        switch (date_type)
        {
            case 1:  // 日期
                {
                    DateTime t = new DateTime(y, m, d);
                    if (isadd)
                    {
                        t = t.AddDays(1);
                    }
                    resTime = t;
                    return true;
                }
            case 2: // 日期 时
                {
                    int hh = Convert.ToInt32(c[4].Value);
                    if (hh >= 0 && hh <= 23)
                    {
                        DateTime t = new DateTime(y, m, d, hh, 0, 0);
                        if (isadd)
                        {
                            t = t.AddHours(1);
                        }
                        resTime = t;
                        return true;
                    }
                }
                break;
            case 3: // 日期 时 分
                {
                    int hh = Convert.ToInt32(c[4].Value);
                    int mm = Convert.ToInt32(c[5].Value);
                    if (hh >= 0 && hh <= 23 && mm >= 0 && mm <= 59)
                    {
                        DateTime t = new DateTime(y, m, d, hh, mm, 0);
                        if (isadd)
                        {
                            t = t.AddMinutes(1);
                        }
                        resTime = t;
                        return true;
                    }
                }
                break;
        }

        return false;
    }

    // 构造一个超链接
    public static string genHyperlink(URLParam[] param)
    {
        clearTextBuilder();

        if (param == null)
            return "";

        foreach (URLParam p in param)
        {
            if (p.m_url != "")
            {
                m_textBuilder.Append("<li class=\"page\">");
                m_textBuilder.Append("<a ");

                //m_textBuilder.Append(" style=");
               // m_textBuilder.Append("\"");
                //m_textBuilder.Append("border:1px solid black;");
               // m_textBuilder.Append("padding : 5px 5px 5px 5px;");
               // m_textBuilder.Append("text-decoration : none;");
               // m_textBuilder.Append("font-size:medium;");
               // m_textBuilder.Append("\"");

                m_textBuilder.Append(" href=");
                m_textBuilder.Append("\"");
                m_textBuilder.Append(p.m_url);

                m_textBuilder.Append("?");
                m_textBuilder.Append(p.m_key);
                m_textBuilder.Append("=");
                m_textBuilder.Append(p.m_value);

                // 添加额外的url参数
                if (p.m_exUrlParam != null)
                {
                    foreach (var urlp in p.m_exUrlParam)
                    {
                        m_textBuilder.Append('&');
                        m_textBuilder.Append(urlp.Key);
                        m_textBuilder.Append("=");
                        m_textBuilder.Append(Convert.ToString(urlp.Value));
                    }
                }
                m_textBuilder.Append("\"");
                
                m_textBuilder.Append(">");
                m_textBuilder.Append(p.m_text);
                //m_textBuilder.Append("</a>");
            }
            else
            {
                m_textBuilder.Append("<li class=\"active\">");
                m_textBuilder.Append("<a>");
                m_textBuilder.Append(p.m_text);
            }
            m_textBuilder.Append("</a>");
            m_textBuilder.Append("</li>");

            m_textBuilder.Append("  ");
        }
        return m_textBuilder.ToString();
    }

    public static string genHyperlink(URLParam param,bool isDeal=false)
    {
        if (param == null)
            return "";

        StringBuilder textBuilder = new StringBuilder();

        textBuilder.Append("<a ");
        textBuilder.Append("href=");
        textBuilder.Append(param.m_url);

        if (!string.IsNullOrEmpty(param.m_key))
        {
            textBuilder.Append("?");
            textBuilder.Append(param.m_key);
            textBuilder.Append("=");
            textBuilder.Append(param.m_value);

            if (param.m_exUrlParam != null)
            {
                foreach (var urlp in param.m_exUrlParam)
                {
                    textBuilder.Append('&');
                    textBuilder.Append(urlp.Key);
                    textBuilder.Append("=");
                    textBuilder.Append(Convert.ToString(urlp.Value));
                }
            }
        }

        if(isDeal) //当为true时，设置颜色为绿色
            textBuilder.Append(" style=\" color: green \"");

        textBuilder.Append(" class=");
        textBuilder.Append("\"" + param.m_className + "\"");
        if (!string.IsNullOrEmpty(param.m_target))
        {
            textBuilder.Append(" target=");
            textBuilder.Append("\"" + param.m_target + "\"");
        }
        textBuilder.Append(">");
        textBuilder.Append(param.m_text);
        textBuilder.Append("</a>");
        return textBuilder.ToString();
    }

    public static void clearTextBuilder()
    {
        m_textBuilder.Remove(0, m_textBuilder.Length);
    }

    // 将时间串cur_time分隔，分隔成两个整数时间值，返回true表示成功
    public static bool splitTimeStr(string cur_time, ref DateTime minTime, ref DateTime maxTime)
    {
        // 首先分隔时间串
        string[] str = Tool.split(cur_time, '-');
        if (str.Length == 2)
        {
            Match m1 = null, m2 = null;
            int type1 = parseTimeStr(str[0], ref m1);
            if (type1 == 0)
                return false;
            int type2 = parseTimeStr(str[1], ref m2);
            if (type2 == 0)
                return false;

            bool res = Tool.genTime(m1, false, type1, ref minTime);
            if (!res)
                return false;

            res = Tool.genTime(m2, true, type2, ref maxTime);
            if (!res)
                return false;

            if (minTime >= maxTime)
            {
                return false;
            }
        }
        else if (str.Length == 1)
        {
            Match m1 = null;
            int type1 = parseTimeStr(str[0], ref m1);
            if (type1 == 0)
                return false;

            bool res = Tool.genTime(m1, false, type1, ref minTime);
            if (!res)
                return false;

            res = Tool.genTime(m1, true, type1, ref maxTime);
            if (!res)
                return false;
        }
        else
        {
            return false;
        }
        return true;
    }

    // 解析一个时间点，返回true成功
    // format为0不限格式，
    public static bool splitTimeStr(string curTime, ref DateTime resultTime, int format = 0)
    {
        Match m1 = null;
        int type1 = parseTimeStr(curTime, ref m1);
        if (type1 == 0)
            return false;

        if (format > 0)
        {
            if (type1 != format)
                return false;
        }

        bool res = genTime(m1, false, type1, ref resultTime);
        if (!res)
            return false;

        return true;
    }

    // 返回复选框
    public static string getChecked(bool issel)
    {
        return issel ? "checked=\"true\"" : "";
    }

    // 返回一个checkbox html串，issel表示是否选中
    public static string getCheckBoxHtml(string name, string value, bool issel, string text = "")
    {
        string str = "<input type= \"checkbox\" name=" + "\"" + name + "\"" + getChecked(issel) + " value= " + "\"" + value + "\"" + " runat=\"server\" />" + text;
        return str;
    }

    // 返回一个checkbox html串，issel表示是否选中
    public static string getRadioHtml(string name, string value, bool issel, string text = "")
    {
        string str = "<input type= \"radio\" name=" + "\"" + name + "\"" + getChecked(issel) + " value= " + "\"" + value + "\"" + " runat=\"server\" />" + text;
        return str;
    }

    public static string getTextBoxHtml(string id, string value)
    {
        string str = "<input type= \"text\" id=" + "\"" + id + "\"" + " value=" + "\"" + value + "\"" + " />";
        return str;
    }

    // 根据符号ch对串str进行拆分
    public static string[] split(string str, char ch, StringSplitOptions op = StringSplitOptions.None)
    {
        char[] sp = { ch };
        string[] arr = str.Split(sp, op);
        return arr;
    }

    //将字符串按每个字符分割为数组
    public static int[] strSplit(string str,int len)
    {
        int[] arr=new int[len];
        for (int i = 0; i<str.Length;i++ )
        {
           arr[i]=Convert.ToInt32(str.Substring(i,1));
        }
        return arr;
    }

    private static int parseTimeStr(string time_str, ref Match m)
    {
        m = Regex.Match(time_str, Exp.DATE_TIME);
        if (!m.Success)
        {
            m = Regex.Match(time_str, Exp.DATE_TIME1);
            if (!m.Success)
            {
                m = Regex.Match(time_str, Exp.DATE_TIME2);
                if (m.Success)
                {
                    return 3;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 2;
            }
        }
        return 1;
    }

    /*
     *  解析Item串列表，返回true成功
     *  item_str 可以为空，此时返回true，但out_items保持不变
     */
    public static bool parseItemList(string itemStr, List<ParamItem> out_items, bool isEmptyStrValid = true)
    {
        if (!isItemListValid(itemStr, isEmptyStrValid))
            return false;

        parseItem(itemStr, out_items);
        return true;
    }

    /**
     *  解析由空格相隔的数字串，存于结果outList中， 成功返回true
     */
    public static bool parseNumList(string numStr, List<int> outList = null)
    {
        Match m = Regex.Match(numStr, Exp.NUM_LIST_BY_BLANK);
        if (!m.Success)
        {
            m = Regex.Match(numStr, Exp.SINGLE_NUM);
            if (!m.Success)
                return false;

            if (outList != null)
            {
                outList.Add(Convert.ToInt32(numStr));
            }
            return true;
        }

        if (outList != null)
        {
            string[] arr = Tool.split(numStr, ' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arr.Length; i++)
            {
                int id = Convert.ToInt32(arr[i]);
                outList.Add(id);
            }
        }
        
        return true;
    }

    /* 
     * 道具串列表是否合法
     * isEmptyStrValid 空串是否合法
     */
    public static bool isItemListValid(string itemStr, bool isEmptyStrValid = true)
    {
        if (itemStr == "" && isEmptyStrValid)
            return true;

        Match match = Regex.Match(itemStr, Exp.TWO_NUM_BY_SPACE);
        if (!match.Success)
        {
            match = Regex.Match(itemStr, Exp.TWO_NUM_BY_SPACE_SEQ);
            if (!match.Success)
            {
                return false;
            }
        }
        return true;
    }

    // 判定两个数字是否合法，以空格相隔
    public static bool isTwoNumValid(string itemStr, bool isEmptyStrValid = false)
    {
        if (itemStr == "" && isEmptyStrValid)
            return true;

        Match match = Regex.Match(itemStr, Exp.TWO_NUM_BY_SPACE);
        if (!match.Success)
        {
            return false;
        }
        return true;
    }

    // 分隔串，没考虑合法性
    public static void parseItem(string str, List<ParamItem> result)
    {
        string[] arr = str.Split(';');
        int i = 0;
        for (; i < arr.Length; i++)
        {
            string[] tmp = Tool.split(arr[i], ' ', StringSplitOptions.RemoveEmptyEntries);
            ParamItem item = new ParamItem();
            item.m_itemId = Convert.ToInt32(tmp[0]);
            item.m_itemCount = Convert.ToInt32(tmp[1]);
            result.Add(item);
        }
    }

    // 从字典内解析道具列表
    public static bool parseItemFromDic(Dictionary<string, object> dic, List<ParamItem> result)
    {
        if (result == null || dic == null) 
            return false;

        int i = 0;
        for (; i < dic.Count; i++)
        {
            Dictionary<string, object> tmp = dic[i.ToString()] as Dictionary<string, object>;
            ParamItem item = new ParamItem();
            item.m_itemId = Convert.ToInt32(tmp["itemId"]);
            item.m_itemCount = Convert.ToInt32(tmp["itemCount"]);
            result.Add(item);
        }
        return true;
    }

    // 从字典内解析道具列表
    public static bool parseItemFromDicNew(Dictionary<string, object> dic, ParamItem item)
    {
        if (item == null || dic == null)
            return false;

        item.m_itemId = Convert.ToInt32(dic["item_id"]);
        item.m_itemCount = Convert.ToInt32(dic["item_count"]);
        return true;
    }

    // 从字典内解析道具列表
    public static bool parseItemFromDicNew1(Dictionary<string, object> dic, checkmapItem item)
    {
        if (item == null || dic == null)
            return false;

        item.m_checkId = Convert.ToInt32(dic["check_id"]);
        item.m_checkValue = Convert.ToInt32(dic["check_value"]);
        return true;
    }

    // 从字典内解析道具列表
    public static bool parseItemFromDicMail(Dictionary<string, object> dic, List<mailGiftItem> result)
    {
        if (result == null || dic == null)
            return false;
        int i = 0;
        for (; i < dic.Count; i++)
        {
            Dictionary<string, object> tmp = dic[i.ToString()] as Dictionary<string, object>;
            mailGiftItem item = new mailGiftItem();
            item.m_giftId = Convert.ToInt32(tmp["giftId"]);
            item.m_count = Convert.ToInt32(tmp["count"]);
            if (tmp.ContainsKey("receive")) 
            {
                item.m_receive = Convert.ToBoolean(tmp["receive"]);
            }
            result.Add(item);
        }
        return true;
    }

    // 从字典内解析道具列表   添加同步道具失败
    public static bool parseItemFromDicError(Dictionary<string, object> dic, List<giftErrorItem> result)
    {
        if (result == null || dic == null)
            return false;
        int i = 0;
        for (; i < dic.Count; i++)
        {
            Dictionary<string, object> tmp = dic[i.ToString()] as Dictionary<string, object>;
            giftErrorItem item = new giftErrorItem();
            item.m_gameItemId = Convert.ToInt32(tmp["gameItemId"]);
            item.m_mainItemId = Convert.ToInt32(tmp["mainItemId"]);
            item.m_count = Convert.ToInt32(tmp["count"]);
            result.Add(item);
        }
        return true;
    }

    // 返回时间串
    public static string getTimeStr(int seconds)
    {
        TimeSpan span = new TimeSpan(0, 0, seconds);
        if (span.Days > 0)
        {
            return string.Format("[{0}]天[{1}]小时[{2}]分[{3}]秒", span.Days, span.Hours, span.Minutes, span.Seconds);
        }
        if (span.Hours > 0)
        {
            return string.Format("[{0}]小时[{1}]分[{2}]秒", span.Hours, span.Minutes, span.Seconds);
        }
        return string.Format("[{0}]分[{1}]秒", span.Minutes, span.Seconds);
    }

    // MD5加密
    public static string getMD5Hash(String input)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] res = md5.ComputeHash(Encoding.Default.GetBytes(input), 0, input.Length);
        return BitConverter.ToString(res).Replace("-", "");
    }

    // 生成道具列表
    public static string genItemList(List<mailGiftItem> itemList)
    {
        if (itemList == null) return "";

        StringBuilder builder = new StringBuilder();

        for (int i = 0; i < itemList.Count; i++)
        {
            if (i == 0)
            {
                builder.AppendFormat("{0} {1}", itemList[i].m_giftId, itemList[i].m_count);
            }
            else
            {
                builder.AppendFormat(";{0} {1}", itemList[i].m_giftId, itemList[i].m_count);
            }
        }

        return builder.ToString();
    }
}

public class ParamItem
{
    public int m_itemId;
    public int m_itemCount;
}
public class mailGiftItem
{
    public int m_giftId;
    public int m_count;
    public bool m_receive;
}

public class giftErrorItem 
{
    public int m_gameItemId;
    public int m_mainItemId;
    public int m_count;
}

public class checkmapItem
{
    public int m_checkId;
    public int m_checkValue;
}

///////////////////////////////////////////////////////////////////////////////

public static class BaseJsonSerializer
{
    public static string serialize(object base_object)
    {
        JsonSerializer serializer = new JsonSerializer();
        StringWriter sw = new StringWriter();
        serializer.Serialize(new JsonTextWriter(sw), base_object);
        return sw.ToString();
    }

    public static T deserialize<T>(string json_string)
    {
        JsonReader reader = new JsonTextReader(new StringReader(json_string));
        JsonSerializer serializer = new JsonSerializer();
        return serializer.Deserialize<T>(reader);
    }
}

/*
public class HttpPost
{
    /// <summary>
    /// 以Post 形式提交数据到 uri
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="files"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static byte[] Post(Uri uri, IEnumerable<UploadFile> files, NameValueCollection values = null)
    {
        string boundary = "-----------" + DateTime.Now.Ticks.ToString("x");
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.ContentType = "multipart/form-data; boundary=" + boundary;
        request.Method = "POST";
        request.KeepAlive = true;
        request.Credentials = CredentialCache.DefaultCredentials;
        MemoryStream stream = new MemoryStream();
        byte[] line = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
        byte[] endline = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
        //提交文本字段
        if (values != null)
        {
            string format = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";
            foreach (string key in values.Keys)
            {
                string s = string.Format(format, key, values[key]);
                byte[] data = Encoding.UTF8.GetBytes(s);
                stream.Write(data, 0, data.Length);
            }

            stream.Write(line, 0, line.Length);

        }

        //提交文件
        if (files != null)
        {
            string fformat = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";

            foreach (UploadFile file in files)
            {
                string s = string.Format(fformat, file.Name, file.Filename);
                byte[] data = Encoding.UTF8.GetBytes(s);
                stream.Write(line, 0, line.Length);
                stream.Write(data, 0, data.Length);
                stream.Write(file.Data, 0, file.Data.Length);
            }

            stream.Write(endline, 0, endline.Length);
        }

        request.ContentLength = stream.Length;
        Stream requestStream = request.GetRequestStream();
        stream.Position = 0L;
        stream.CopyTo(requestStream);
        stream.Close();
        requestStream.Close();

        using (var response = request.GetResponse())
        using (var responseStream = response.GetResponseStream())
        using (var mstream = new MemoryStream())
        {
            responseStream.CopyTo(mstream);
            return mstream.ToArray();
        }
    }
    private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
    public static byte[] Get(Uri uri)
    {
        byte[] bytes = null;
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();

            List<byte> lb = new List<byte>();
            int length = 1;
            byte[] temp = new byte[1024];
            while (length > 0)
            {
                length = dataStream.Read(temp, 0, 1024);
                for (int i = 0; i < length; i++)
                {
                    lb.Add(temp[i]);
                }
            }
            bytes = lb.ToArray();
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message + uri);
        }
        return bytes;
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    public class UploadFile
    {
        public UploadFile()
        {
            ContentType = "application/octet-stream";
        }
        public string Name { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
}
*/