using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Security;    
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace WxPayAPI
{
    public class DataSend
    {
        // text/json
        public string m_contentType;

        // m_str, m_data只取其一
        public string m_str;

        public byte[] m_data;

        public DataSend()
        {
            m_contentType = "text/json";
        }
    }

    /// <summary>
    /// http连接基础类，负责底层的http通信
    /// </summary>
    public class HttpService
    {
        private static string USER_AGENT =  "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)"; 

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }

        // timeout 单位秒
        public static string PostData(Action<HttpWebRequest, DataSend> cb, string url, bool isUseCert, int timeout)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            //LogMgr.info("post {0}", url);

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = USER_AGENT;
                request.Method = "POST";
                request.Timeout = timeout * 1000;
                //request.KeepAlive = true;
                request.ProtocolVersion = HttpVersion.Version10;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                DataSend sendmsg = new DataSend();
                if (cb != null)
                {
                    cb(request, sendmsg);
                }
                request.ContentType = sendmsg.m_contentType;
                byte[] data = null; // System.Text.Encoding.UTF8.GetBytes(xml);
                if (!string.IsNullOrEmpty(sendmsg.m_str))
                {
                    data = System.Text.Encoding.UTF8.GetBytes(sendmsg.m_str);
                }
                else
                {
                    data = sendmsg.m_data;
                }
                if (data != null)
                {
                    request.ContentLength = data.Length;
                }

                //是否使用证书
                if (isUseCert)
                {
                 //  string path = HttpContext.Current.Request.PhysicalApplicationPath;
                 //  X509Certificate2 cert = new X509Certificate2(path + WxPayConfig.GetConfig().GetSSlCertPath(), WxPayConfig.GetConfig().GetSSlCertPassword());
                 //  request.ClientCertificates.Add(cert);
                 //  Log.Debug("WxPayApi", "PostXml used cert");
                }

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                if (data != null)
                {
                    reqStream.Write(data, 0, data.Length);
                }
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                LogMgr.error(e.ToString());
            }
            catch (WebException e)
            {
                LogMgr.error(e.ToString());
            }
            catch (Exception e)
            {
                LogMgr.error(e.ToString());
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }
    }
}