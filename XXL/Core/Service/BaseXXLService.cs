using BIMT.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XXL.Model;

namespace XXL.Core.Service
{
    public abstract class BaseXXLService : BaseService, IService
    {

        public Dictionary<string, Block> dictGroup = new Dictionary<string, Block>();
        public int sumScore = 0;
        public GamePanel gamePanel = GamePanel.GetInstance();
        public BaseXXLService(IViewCallback view) : base(view)
        {
        }

        int[,] demo = new int[10, 10] { { 0,1,0,1,1,0,1,0,3,1 }
                                     , { 0,1,2,1,3,0,2,0,4,1 }
                                     , { 0,2,0,1,3,2,2,4,1,1 }
                                     , { 0,2,3,1,0,2,2,4,4,1 }
                                     , { 4,2,3,1,0,2,2,4,4,1 }
                                     , { 0,2,3,1,0,2,0,0,1,4 }
                                     , { 1,2,1,1,0,1,0,2,4,4 }
                                     , { 0,1,1,0,0,1,0,4,1,4 }
                                     , { 0,1,3,1,4,3,3,4,0,4 }
                                     , { 4,1,1,1,0,4,3,3,0,4 } };
        //int[,] demo = new int[10, 10] { { 0,1,0,1,1,0,1,0,0,1 }
        //                             , { 0,1,2,1,3,0,2,0,0,1 }
        //                             , { 0,2,0,1,3,2,2,4,0,1 }
        //                             , { 0,2,3,1,0,2,2,4,0,1 }
        //                             , { 4,2,3,1,0,2,2,4,0,1 }
        //                             , { 0,2,3,1,0,2,0,0,0,4 }
        //                             , { 1,2,1,1,0,1,0,2,0,4 }
        //                             , { 0,1,1,0,0,1,0,4,0,4 }
        //                             , { 0,1,3,1,4,3,3,4,0,4 }
        //                             , { 4,1,0,1,0,4,3,3,0,4 } };

        public void InitDict()
        {
            Dictionary<string, Block> init = new Dictionary<string, Block>();
            SendMessage("game init ...");
            //GamePanel.OriginDict.Clear();
            for (int y = 0; y < 10; y++)
            {
                string row = string.Empty;
                for (int x = 0; x < 10; x++)
                {
                    int kind = demo[9 - y, x];// r.Next(0, 4);
                    init.Add(x.ToString() + y.ToString(), new Block(kind, x, y,gamePanel));
                    row += kind.ToString() + " ";
                }
                SendMessage(row);
            }
            gamePanel.OriginDict = init;// = new GamePanel(init);
        }
        //Dictionary<string, Block>()
        public Dictionary<string, Block> GetOriginDict()
        {
            Dictionary<string, Block> origin = new Dictionary<string, Block>();
            for (int y = 0; y < 10; y++)
            {
                string row = string.Empty;
                for (int x = 0; x < 10; x++)
                {
                    int kind = demo[9 - y, x];
                    origin.Add(x.ToString() + y.ToString(), new Block(kind, x, y, gamePanel));
                    row += kind.ToString() + " ";
                }
            }
            gamePanel.OriginDict = origin;
            return gamePanel.OriginDict;
        }
        public bool IsCleanPanel()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (HasNeighbor(i, j))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Dictionary<string, Block> Destory(Dictionary<string, Block> origin, List<Block> listGroup)
        {
            if (listGroup.Count > 1)
            {
                foreach (Block item in listGroup)
                {
                    if (origin.Keys.Contains(item.GetKey()) && origin[item.GetKey()] != null)
                    {
                        //origin[item]=null;
                        origin[item.GetKey()] = origin[item.GetKey()].SetDisabled();
                    }
                }
            }
            return origin;
        }

        public int Destory(Dictionary<string, Block> dictGroup)
        {
            Dictionary<string, Block> origin = gamePanel.OriginDict;
            if (dictGroup.Keys.Count > 1)
            {
                foreach (string item in dictGroup.Keys)
                {
                    if (origin.Keys.Contains(item))
                    {
                        //origin[item]=null;
                        origin[item] = origin[item].SetDisabled();
                    }
                }
                return dictGroup.Keys.Count;
            }
            dictGroup.Clear();
            return 0;
        }

        public int Destory()
        {
            return Destory(dictGroup);
        }

        public void DrawGame()
        {
            DrawGame(gamePanel.OriginDict);
        }

        public void DrawGame(Dictionary<string, Block> origin)
        {
            for (int y = 0; y < 10; y++)
            {
                string row = string.Empty;
                for (int x = 0; x < 10; x++)
                {
                    string key = x.ToString() + y.ToString();
                    if (origin.Keys.Contains(key) && origin[key] != null&& origin[x.ToString() + y.ToString()].kind!=-1)
                    {
                        row += origin[x.ToString() + y.ToString()].kind + " ";
                    }
                    else
                    {
                        row += "* ";
                    }
                }
                SendMessage(row);
            }
        }

        public abstract void Expand(int i, int j);

        public void MessageGroup(Block block, string message)
        {
            if (!dictGroup.Keys.Contains(block.GetKey()))
            {
                //SendMessage(message);
            }
        }

        public void Refresh()
        {
            RefreshRow();
            RefreshColumn();

        }

        public Dictionary<string, Block> Refresh(Dictionary<string, Block> dictOrigin)
        {
            dictOrigin = RefreshRow(dictOrigin);
            dictOrigin = RefreshColumn(dictOrigin);
            return dictOrigin;
        }

        public void RefreshRow()
        {
            RefreshRow(gamePanel.OriginDict);
        }

        public Dictionary<string, Block> RefreshRow(Dictionary<string, Block> origin)
        {
            for (int x = 0; x < 10;)
            {
                for (int y = 0; y < 10; y++)
                {
                    string key = x.ToString() + y.ToString();
                    string upkey = x.ToString() + (y + 1).ToString();
                    if (origin.Keys.Contains(upkey))
                    {
                        if ((origin[key] != null && origin[key].IsNullBlock()) || origin[key] == null)
                        {
                            Block up = origin[upkey];
                            if (up != null)
                            {
                                up = up.GoDown();
                            }
                            origin[key] = up;
                            origin[upkey] = new Block(-1, 0, 0,gamePanel);// origin[upkey].SetDisabled();
                        }
                    }
                    else
                    {
                        if (origin[key] != null && origin[key].IsNullBlock())
                        {
                            origin[key] = null;
                        }
                    }

                }
                while (gamePanel.IsClearColumn(origin, x))
                {
                    x++;
                    if (x > 9)
                    {
                        break;
                    }
                }
            }
            return origin;
        }

        public void RefreshColumn()
        {
            RefreshColumn(gamePanel.OriginDict);
            //Dictionary<string, Block> origin = GamePanel.OriginDict;
            //for (int x = 0; x < 10; x++)
            //{
            //    if (!origin.Keys.Contains(x.ToString() + "0"))
            //    {
            //        for (int i = x + 1; i < 10; i++)
            //        {
            //            for (int y = 0; y < 10; y++)
            //            {
            //                string key = i.ToString() + y.ToString();
            //                if (origin.Keys.Contains(key))
            //                {
            //                    origin[key] = origin[key].GoLeft();
            //                }
            //            }
            //        }
            //    }
            //}
        }

        public bool NeedRefreshColumn(Dictionary<string, Block> origin, int x)
        {
            for (int i = x; i < 10; i++)
            {
                StringBuilder sbKey = new StringBuilder(4);
                sbKey.Append(i);
                sbKey.Append(0);
                if (origin[sbKey.ToString()] != null)
                {
                    return true;
                }
            }
            return false;
        }

        public Dictionary<string, Block> RefreshColumn(Dictionary<string, Block> origin)
        {
            //return origin;
            //Dictionary<string, Block> origin = GamePanel.OriginDict;
            for (int x = 0; x < 9; x++)
            {
               
                StringBuilder sbKey = new StringBuilder(4);
                sbKey.Append(x);
                sbKey.Append(0);
                if (origin[sbKey.ToString()] == null)
                {
                    if (!NeedRefreshColumn(origin,x))
                    {
                        return origin;
                    }
                    int count = MoveCount(origin);
                    if (count==0)
                    {
                        continue;
                    }
                    //DrawGame(origin);
                    for (int i = x + 1; i < 10; i++)
                    {
                        for (int y = 0; y < 10; y++)
                        {
                            string keyRight = new StringBuilder(i.ToString(), 4).Append(y).ToString(); // i.ToString() + y.ToString();
                            string keyLeft = new StringBuilder((i - count).ToString(), 4).Append(y).ToString();// (i - count).ToString() + y.ToString();
                            if (origin[keyRight] != null)
                            {
                                Block right = origin[keyRight];
                                if (right != null)
                                {
                                    right = right.GoLeft();
                                }
                                origin[keyLeft] = right;/// origin[i.ToString() + y.ToString()];
                                origin[keyRight] = null;// new Block(-1, 0, 0,gamePanel); ;
                            }
                        }
                    }
                }
            }
            return origin;
        }

        public bool IsCleanRight(Dictionary<string, Block> origin, int x)
        {
            for (int i = x + 1; i < 10; i++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (null != origin[i.ToString() + y.ToString()])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public int MoveCount(Dictionary<string, Block> origin)
        {
            for (int i = 0; i < 10; i++)
            {
                StringBuilder sbnull = new StringBuilder(4);
                sbnull.Append(i);
                sbnull.Append(0);
                if (origin[sbnull.ToString()] == null)
                {
                    for (int j = i; j < 10; j++)
                    {
                        StringBuilder sbnunull = new StringBuilder(4);
                        sbnunull.Append(j);
                        sbnunull.Append(0);
                        if (origin[sbnunull.ToString()] != null)
                        {
                            return j - i;
                        }
                    }
                }
            }
            return 0;
        }

        public bool HasNeighbor(int i, int j)
        {
            Block start = gamePanel.GetBlock(i.ToString() + j.ToString());
            if (start != null)
            {
                //MessageGroup(start, string.Format("set origin  ({0},{1}) kind:{2} in group", start.location.X.ToString(), start.location.Y.ToString(), start.kind));
                Block left = start.GetSameGroupLeftBlock();
                Block up = start.GetSameGroupUpBlock();
                Block right = start.GetSameGroupRightBlock();
                Block down = start.GetSameGroupDownBlock();
                if (left != null
                    || up != null
                    || right != null
                    || down != null)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public abstract void StartGame();
    }
}
