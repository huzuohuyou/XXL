using BIMT.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XXL.Core.Service;
using XXL.Model;

namespace XXL.Core
{
    public class XXLController 
    {
        BaseXXLService service;

        public XXLController(IViewCallback view)
        {
            service = new TreeSolutionService(view);
        }

        public void Init() {
            service.InitDict();
        }

        public void DoWork()
        {
            service.StartGame();
        }

    }
}
