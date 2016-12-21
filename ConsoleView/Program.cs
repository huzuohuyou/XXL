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
    class Program
    {
        
        static void Main(string[] args)
        {
            ProxyOutputView service = new ProxyOutputView();
            service.Do();
        }

       
    }
}
