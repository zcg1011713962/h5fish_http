using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishTool
{
    class Program
    {
        static void Main(string[] args)
        {
            LogMgr.init();
            ResMgr.getInstance().reload();
            var obj = new ParallelQuestTrans();
            obj.exec();

          //  Channel100003Update.getInstance();
          //  Channel100003Update.getInstance().output();
            System.Console.Read();
        }
    }
}
