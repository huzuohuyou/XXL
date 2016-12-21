using BIMT.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XXL.Core;
using XXL.Model;

namespace ConsoleView
{

    class ProxyOutputView : BaseFormView
    {
        public ProxyOutputView() { }
        XXLService service;
        public void Do()
        {
            GamePanel.OriginDict.Clear();
            service = new XXLService(this);
            service.InitSolution();
            service.InitStepDict();
            Thread t = new Thread(service.DoWork);
            t.Start();
        }

        public override void SetView(int count, string value)
        {
            Console.Write(string.Format("{0}: {1} \n", DateTime.Now.ToString("HH:mm:ss"), value));
        }
    }
}
