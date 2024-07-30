using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

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
            ServiceMgr.getInstance().init();
            initConfirm();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void initConfirm()
        {
            WorkThreadConfirm obj = ServiceMgr.getInstance().getSys<WorkThreadConfirm>((int)SType.ServerType_Confirm);
            if (obj != null)
            {
                obj.addConfirmObj(ConfirmType.Confirm_QQGame, new QQGameConfirm());
            }

            obj.startWork();
        }
    }
}
