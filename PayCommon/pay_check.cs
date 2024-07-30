using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Xml;
using System.IO;
using System.Security.Cryptography;   

// 支付验签
public class PayCheck
{
    public delegate bool isJoinSign(object obj);
    public delegate bool isJoinSign2(string key, string value);

    // 空值不参与验签
    public string getWaitSignStrByAsc(Dictionary<string, object> data,
                                      List<string> excludeKey = null,
                                      isJoinSign join = null)
    {
        var descData = from s in data
                       orderby s.Key ascending
                       select s;

        string value = "";
        StringBuilder sbuilder = new StringBuilder();
        bool first = true;
        foreach (var d in descData)
        {
            if (excludeKey != null && excludeKey.Contains(d.Key))
                continue;

            if (join != null)
            {
                bool res = join(d.Value);
                if (!res)
                {
                    continue;
                }
                if (d.Value == null)
                {
                    value = "";
                }
                else
                {
                    value = Convert.ToString(d.Value);
                }
            }
            else
            {
                if (d.Value == null)
                {
                    continue;
                }
                else
                {
                    value = Convert.ToString(d.Value);
                }

                if (value == "")
                    continue;
            }
           
            if (first)
            {
                first = false;
                sbuilder.AppendFormat("{0}={1}", d.Key, value);
            }
            else
            {
                sbuilder.AppendFormat("&{0}={1}", d.Key, value);
            }
        }

        return sbuilder.ToString();
    }

    // 空值不参与验签
    public string getWaitSignStrByAsc2(Dictionary<string, object> data,
                                      List<string> excludeKey = null,
                                      isJoinSign2 join = null)
    {
        string[] keyList = data.Keys.ToArray();
        Array.Sort(keyList, string.CompareOrdinal);
        string value = "";
        StringBuilder sbuilder = new StringBuilder();
        bool first = true;
        foreach (var key in keyList)
        {
            if (excludeKey != null && excludeKey.Contains(key))
                continue;

            value = Convert.ToString(data[key]);
            if (join != null)
            {
                bool res = join(key, value);
                if (!res)
                {
                    continue;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }
            }

            if (first)
            {
                first = false;
                sbuilder.AppendFormat("{0}={1}", key, value);
            }
            else
            {
                sbuilder.AppendFormat("&{0}={1}", key, value);
            }
        }

        return sbuilder.ToString();
    }

    // 可指定分隔符号
    public string getWaitSignStrByAsc3(Dictionary<string, object> data,
                                        string split = "&",
                                      List<string> excludeKey = null,
                                      isJoinSign2 join = null)
    {
        string[] keyList = data.Keys.ToArray();
        Array.Sort(keyList, string.CompareOrdinal);
        string value = "";
        StringBuilder sbuilder = new StringBuilder();
        bool first = true;
        foreach (var key in keyList)
        {
            if (excludeKey != null && excludeKey.Contains(key))
                continue;

            value = Convert.ToString(data[key]);
            if (join != null)
            {
                bool res = join(key, value);
                if (!res)
                {
                    continue;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }
            }

            if (first)
            {
                first = false;
                sbuilder.AppendFormat("{0}{1}", key, value);
            }
            else
            {
                sbuilder.AppendFormat("{2}{0}{1}", key, value, split);
            }
        }

        return sbuilder.ToString();
    }

    public string getVivoWaitSigned(Dictionary<string, object> data,
                                    List<string> excludeKey,
                                    string appKey)
    {
        string waitStr = getWaitSignStrByAsc(data, excludeKey);
        return waitStr + "&" + Helper.getMD5(appKey);
    }

    // 返回微信待加密串
    public string getWeiXinWaitSigned(Dictionary<string, object> data,
                                   List<string> excludeKey,
                                   string appKey)
    {
        string waitStr = getWaitSignStrByAsc(data, excludeKey);
        return waitStr + "&key=" + appKey;
    }

    bool isJoinMeizuSign(object obj)
    {
        if (obj == null)
            return false;

        return true;
    }

    // 魅族单机
    public string getMeizuSingleWaitSignStr(Dictionary<string, object> inputData, List<string> excludeKey)
    {
        string str = "";
        str = getWaitSignStrByAsc(inputData, excludeKey, isJoinMeizuSign);
        str += ":" + PayTable.MEIZU_APP_SECRET;
        return str;
    }

    public string getMeizuSingleWaitSignStr(Dictionary<string, object> inputData, List<string> excludeKey, string appSecret)
    {
        string str = "";
        str = getWaitSignStrByAsc(inputData, excludeKey, isJoinMeizuSign);
        str += ":" + appSecret;
        return str;
    }

    public string getMeizuSingleWaitSignStr(string str1, List<string> excludeKey, string appSecret)
    {
        string str = str1;
        //str = getWaitSignStrByAsc(inputData, excludeKey, isJoinMeizuSign);
        str += ":" + appSecret;
        return str;
    }

    public byte[] SHA256Hash(string src)
    {
        byte[] waitArr = Encoding.UTF8.GetBytes(src);
        using (SHA256Managed alg = new SHA256Managed())
        {
            return alg.ComputeHash(waitArr);
        }

        return null;
    }

    public string toHex(byte[] arr)
    {
        if (arr == null) return "";

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < arr.Length; i++)
        {
            builder.Append(arr[i].ToString("x2"));
        }
        return builder.ToString();
    }

    public string signBySHA256WithRSA(string waitStr, string privateKey)
    {
        try
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);

                byte[] waitArr = Encoding.UTF8.GetBytes(waitStr);
                using (SHA256Managed alg = new SHA256Managed())
                {
                    byte[] signatureBytes = rsa.SignData(waitArr, alg);
                    return Convert.ToBase64String(signatureBytes);
                }
            }
        }
        catch (System.Exception ex)
        {
        }

        return "";
    }

    // 通过RSA2加密
    public string signByRSA2(string waitStr, string privateKey)
    {
        try
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                byte[] arr = Encoding.UTF8.GetBytes(waitStr);

                byte[] signatureBytes = rsa.SignData(arr, "SHA256");
                return Convert.ToBase64String(signatureBytes);
            }      
        }
        catch (System.Exception ex)
        {
        }
        return "";
    }

    public string signByRSA(string waitStr, string privateKey)
    {
        try
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            byte[] arr = Encoding.UTF8.GetBytes(waitStr);

            byte[] signatureBytes = rsa.SignData(arr, "SHA1");
            return Convert.ToBase64String(signatureBytes);
        }
        catch (System.Exception ex)
        {
        }
        return "";
    }

    public string signByRSAMd5(string content, string privateKey)
    {
        byte[] Data = Encoding.UTF8.GetBytes(content);
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(privateKey);
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] signData = rsa.SignData(Data, md5);
                return Convert.ToBase64String(signData);
            }
        }
    }

    public string signByHMACSHA256(string content, string privateKey)
    {
        byte[] Data = Encoding.UTF8.GetBytes(content);
        byte[] keyByte = Encoding.UTF8.GetBytes(privateKey);

        using (var hmacsha256 = new HMACSHA256(keyByte))
        {
            byte[] hashmessage = hmacsha256.ComputeHash(Data);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashmessage.Length; i++)
            {
                builder.Append(hashmessage[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public bool checkSignByRSA256PublicKey(string waitStr, string sign, string publicKey)
    {
        bool res = false;
        try
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);

                byte[] waitArr = Encoding.UTF8.GetBytes(waitStr);
                byte[] signArr = Convert.FromBase64String(sign);
                using (SHA256Managed alg = new SHA256Managed())
                {
                    res = rsa.VerifyData(waitArr, alg, signArr);
                }
            }
        }
        catch (System.Exception ex)
        {
        }

        return res;
    }
}

public class XmlGen
{
    public string genXML(string root, Dictionary<string, object> data)
    {
        MemoryStream ms = new MemoryStream();

        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.Encoding = new UTF8Encoding(false);
        settings.NewLineChars = Environment.NewLine;
        settings.OmitXmlDeclaration = true;

        XmlWriter writer = XmlWriter.Create(ms, settings);
        writer.WriteStartElement(root);

        foreach (var item in data)
        {
            writer.WriteStartElement(item.Key);
            writer.WriteString(Convert.ToString(item.Value));
            writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.Close();
        string xml = Encoding.UTF8.GetString(ms.ToArray());
        return xml;
    }

    public Dictionary<string, object> fromXmlString(string root, string xmlBody)
    {
        Dictionary<string, object> ret = new Dictionary<string, object>();
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlBody);

            XmlNode nodeRoot = doc.SelectSingleNode("/" + root);
            for (XmlNode node = nodeRoot.FirstChild; node != null; node = node.NextSibling)
            {
                if (node is XmlComment)
                    continue;

                if (node.FirstChild == null)
                {
                    ret.Add(node.Name, "");
                }
                else
                {
                    ret.Add(node.Name, node.FirstChild.Value);
                }
            }
        }
        catch (System.Exception ex)
        {
        }

        return ret;
    }
}

