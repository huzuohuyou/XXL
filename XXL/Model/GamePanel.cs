using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XXL.Model
{
    public class GamePanel
    {
        private static GamePanel gamePanel;
        private Dictionary<string, Block> originDict = new Dictionary<string, Block>();

        private GamePanel(Dictionary<string, Block> OriginDict)
        {
            this.OriginDict = OriginDict;
        }
        private GamePanel()
        {
            //this.OriginDict = OriginDict;
        }


        public Dictionary<string, Block> OriginDict
        {
            get
            {
                return originDict;
            }

             set
            {
                originDict = value;
            }
        }

        public static GamePanel GetInstance() {
            if (gamePanel==null)
            {
                gamePanel = new GamePanel();
            }
            return gamePanel;
        }



        public Block GetBlock(string key)
        {
            return GetBlock(OriginDict,key) ;
        }

        public Block GetBlock( Dictionary<string, Block> OriginDict,string key)
        {
            if (OriginDict.Keys.Contains(key))
            {
                return OriginDict[key];
            }
            return null;
        }

        public bool IsClearColumn(int x)
        {
            return IsClearColumn(OriginDict,x);
        }

        public bool IsClearColumn(Dictionary<string, Block> OriginDict, int x)
        {
            for (int y = 0; y < 10; y++)
            {
                StringBuilder sb = new StringBuilder(4);
                sb.Append(x);
                sb.Append(y);
                //string key = x.ToString() + y.ToString();
                if (OriginDict.Keys.Contains(sb.ToString()))
                {
                    if (OriginDict[sb.ToString()] != null && OriginDict[sb.ToString()].IsNullBlock())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
