using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XXL.Core.Service
{
    interface IService
    {
        void Expand(int i, int j);
        int Destory();
        void Refresh();
        void DrawGame();
        void StartGame();
    }
}
