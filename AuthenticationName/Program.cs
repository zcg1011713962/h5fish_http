using System;
using System.Windows.Forms;
using System.IO;

namespace ExportExcel
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            LogMgr.init();
            ResMgr.getInstance().init();
            CheckBase.init();
            ServiceMgr.getInstance().init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void test()
        {
            string str = File.ReadAllText("UpResult.txt");
            AuthInfoUpdataRep rep = new AuthInfoUpdataRep();
            rep.fromJsonStr(str);

            string str1 = File.ReadAllText("NameCheck.txt");
            AuthInfoCheckRep rep1 = new AuthInfoCheckRep();
            rep1.fromJsonStr(str1);
        }
    }
}
