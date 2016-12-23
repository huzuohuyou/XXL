using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XXL.Model
{
    class GameFrame
    {
        private Dictionary<string, Block> originDict = new Dictionary<string, Block>();
        public GameFrame(Dictionary<string, Block> OriginDict) {
            this.OriginDict = OriginDict;
        }

        public Dictionary<string, Block> OriginDict
        {
            get
            {
                return originDict;
            }

            private set
            {
                originDict = value;
            }
        }
    }
}
