using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

namespace WebManager.appaspx.stat
{
    public partial class StatServiceDumpTime : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string var = ReaderDirectory(@" ", " ");
            m_res.InnerHtml = var;
        }

        public static string ReaderDirectory(string path, string filter="") 
        {
            ///判断目录是否正确  
            if (string.IsNullOrEmpty(path)) 
                return string.Empty;

            StringBuilder content = new StringBuilder();
            ///保存读取的内容  
            ///如果过滤器（filter）为空，则默认读取所有文件的内容  
            if (string.IsNullOrEmpty(filter))
            {
                ReaderSubDirectory(path, filter, ref content);
            }
            else
            {   ///如果过滤器(filter)不为空,则获取被读取的文件  
                string[] filters = filter.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                ///读取每一个文件的内容  
                foreach (string fi in filters)
                {
                    ReaderSubDirectory(path, fi, ref content);
                }
            }
            return content.ToString();  
        }

         private static void ReaderSubDirectory(string path,string filter,ref StringBuilder content)  
        {   ///判断目录是否正确  
            if(string.IsNullOrEmpty(path)) 
                return;  

            ///创建目录信息的实例  
             DirectoryInfo parentDI = new DirectoryInfo(path);  
            ///读取当前目录及其子目录下的指定文件的内容， 都保存到content变量中  
            foreach(FileInfo fi in parentDI.GetFiles(filter,SearchOption.AllDirectories))  
            {  
                content.AppendLine();  
                content.Append(ReaderFile(fi.FullName));  
                content.AppendLine();  
            }  
        } 

        public static string ReaderFile(string path)  
        {  
           string fileData = string.Empty;  
           try 
            {   ///读取文件的内容  
                StreamReader reader = new   
                StreamReader(path,Encoding.Default);  
                fileData = reader.ReadToEnd();  
               reader.Close();  
           }  
          catch(Exception ex){
              throw new Exception(ex.Message,ex);
          }  ///抛出异常  
          return fileData;  
       } 
    }
}