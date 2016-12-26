using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XXL.Model
{
    public class Block
    {
        GamePanel panel;
        public int kind;
        public Point location;

        public GamePanel Panel
        {
            get
            {
                return panel;
            }

            set
            {
                panel = value;
            }
        }


        public Block GetSameBlock()
        {
            //return new Block(block.kind, block.location.X, block.location.Y, block.Panel);

            return new Block(this.kind, this.location.X, this.location.Y, this.panel);
        }

        //public static readonly Block NullBlock = new Block(-1, -1, -1);
        public Block(int kind, int x, int y,GamePanel panel)
        {
            this.Panel = panel;
            this.kind = kind;
            //if (x < 0 || x > 9 || y < 0 || y > 9)
            //{
            //    throw new Exception("illegal point~ ");
            //}
            location = new Point(x, y);
        }

       
        public Block GetSameKindBlock(Dictionary<string, Block> dictOrigin, int x, int y)
        {
            string key = x.ToString() + y.ToString();
            if (dictOrigin.Keys.Contains(key))
            {
                Block block = dictOrigin[key];// GamePanel.GetInstance().GetBlock(dictOrigin, x.ToString() + y.ToString());
                if (block != null && block.kind == this.kind && kind != -1)
                {
                    return block;
                }
            }
            return null;
        }

        public Block GetSameKindBlock(Point location)
        {
            int x = location.X;
            int y = location.Y;
            Block block = Panel.GetBlock(x.ToString() + y.ToString());
            if (block != null && block.kind == this.kind)
            {
                return block;
            }
            return null;
        }

        public Block GetSameKindBlock(int x, int y)
        {
            return GetSameKindBlock(Panel.OriginDict, x, y);
        }

     
        public Block GetSameGroupLeftBlock()
        {
            int x = location.X - 1;
            int y = location.Y;
            return GetSameKindBlock(x, y);
        }

        public Block GetSameGroupRightBlock()
        {
            int x = location.X + 1;
            int y = location.Y;
            return GetSameKindBlock(x, y);
        }

        public Block GetSameGroupUpBlock()
        {
            int x = location.X;
            int y = location.Y + 1;
            return GetSameKindBlock(x, y);
        }

        public Block GetSameGroupDownBlock()
        {
            int x = location.X;
            int y = location.Y - 1;
            return GetSameKindBlock(x, y);
        }



                     
        public Block GetSameGroupLeftBlock(Dictionary<string, Block> dictOrigin)
        {
            int x = location.X - 1;
            int y = location.Y;
            return GetSameKindBlock(dictOrigin,x, y);
        }

        public Block GetSameGroupRightBlock(Dictionary<string, Block> dictOrigin)
        {
            int x = location.X + 1;
            int y = location.Y;
            return GetSameKindBlock(dictOrigin, x, y);
        }

        public Block GetSameGroupUpBlock(Dictionary<string, Block> dictOrigin)
        {
            int x = location.X;
            int y = location.Y + 1;
            return GetSameKindBlock(dictOrigin, x, y);
        }

        public Block GetSameGroupDownBlock(Dictionary<string, Block> dictOrigin)
        {
            int x = location.X;
            int y = location.Y - 1;
            return GetSameKindBlock(dictOrigin, x, y);
        }

        public bool IsNullBlock()
        {
            if (kind == -1)
            {
                return true;
            }
            return false;
        }

        public Block GoDown()
        {
            location.Y = location.Y - 1;
            return new Block(this.kind,this.location.X,this.location.Y,Panel);// panel.GetBlock(this.GetKey());
        }

        public Block GoLeft()
        {
            location.X = location.X - 1;
            return new Block(this.kind, this.location.X, this.location.Y, Panel); //panel.GetBlock(this.GetKey());
        }

        public Block GetUpBlock()
        {
            return Panel.GetBlock(location.X.ToString() + (location.Y + 1).ToString());
        }

        public Block GetRightBlock()
        {
            int x = location.X + 1;
            int y = location.Y ;
            return Panel.GetBlock(x.ToString() + y.ToString());
        }

        public string GetKey()
        {
            return location.X.ToString() + location.Y.ToString();
        }

        public Block SetDisabled()
        {
            this.kind = -1;
            return this;
        }
    }
}
