using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XXL.Model
{
    public class GamePanel
    {
        public static Dictionary<string, Block>  OriginDict = new Dictionary<string, Block>();
        
        private GamePanel()
        {
           
        }
        private static GamePanel instance;

        //public Dictionary<string, Block> OriginDict
        //{
        //    get
        //    {
        //        return originDict;
        //    }

        //    set
        //    {
        //        originDict = value;
        //    }
        //}



        public static GamePanel GetInstance()
        {
            if (instance == null)
            {
                instance = new GamePanel();
            }
            return instance;
        }

        public Block GetBlock(string key)
        {
            if (OriginDict.Keys.Contains(key))
            {
                return OriginDict[key];
            }
            return null;
        }

        public bool IsClearColumn(int x)
        {
            for (int y = 0; y < 10; y++)
            {
                string key = x.ToString() + y.ToString();
                if (OriginDict.Keys.Contains(key))
                {
                    if (OriginDict[key] != null && OriginDict[key].IsNullBlock())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
