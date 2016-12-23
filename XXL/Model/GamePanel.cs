﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XXL.Model
{
    public class GamePanel
    {
        private Dictionary<string, Block> originDict = new Dictionary<string, Block>();
        public GamePanel(Dictionary<string, Block> OriginDict)
        {
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
