using BIMT.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXL.Model;

namespace XXL.Core
{
    public class XXLService : BaseService
    {
        public static List<int> result = new List<int>();
        int[,] demo = new int[10,10] { { 0,1,0,1,1,0,1,0,3,1 }
                                     , { 0,1,2,1,3,0,2,0,4,1 }
                                     , { 0,2,0,1,3,2,2,4,1,1 }
                                     , { 0,2,3,1,0,2,2,4,4,1 }
                                     , { 4,2,3,1,0,2,2,4,4,1 }
                                     , { 0,2,3,1,0,2,0,0,1,4 }
                                     , { 1,2,1,1,0,1,0,2,4,4 }
                                     , { 0,1,1,0,0,1,0,4,1,4 }
                                     , { 0,1,3,1,4,3,3,4,0,4 }
                                     , { 4,1,0,1,0,4,3,3,0,4 } };
        //List<string> list = new List<string>();
        Dictionary<string, Block> dictGroup = new Dictionary<string, Block>();
        public static List<int> scorelist = new List<int>();
        public int sumScore = 0;
        public GamePanel gamePanel= GamePanel.GetInstance();
        public XXLService(IViewCallback view,int m,int n) : base(view)
        {
            this.m = m;
            this.n = n;
        }

        public void InitDict()
        {
            SendMessage("game init ...");
            Random r = new Random();
            GamePanel.OriginDict.Clear();
            for (int y   = 0; y < 10; y++)
            {
                string row = string.Empty;
                for (int x = 0; x < 10;x++)
                {
                    int kind = demo[9-y, x];// r.Next(0, 4);
                    GamePanel.OriginDict.Add(x.ToString() + y.ToString(), new Block(kind, x, y));
                    row += kind.ToString()+" ";
                }
                SendMessage(row);
            }
        }

        public void DrawGame()
        {
            SendMessage("=======================================");
            for (int y = 0; y < 10; y++)
            {
                string row = string.Empty;
                for (int x = 0; x < 10; x++)
                {
                    string key = x.ToString() + y.ToString();
                    if (GamePanel.OriginDict.Keys.Contains(key) && GamePanel.OriginDict[key] != null)
                    {
                        row += GamePanel.OriginDict[x.ToString() + y.ToString()].kind + " ";
                    }
                    else
                    {
                        row += "* ";
                    }
                }
                SendMessage(row);
            }
        }

        //public void Add(string obj)
        //{
        //    foreach (var item in list)
        //    {
        //        if (item == obj)
        //        {
        //            return;
        //        }
        //    }
        //    list.Add(obj);
        //}
        int m, n;

        public void DoWork()
        {
            //for (int i = 0; i < 3; i++)
            //{
            //    for (int j = 0; j < 3; j++)
            //    {
                    InitDict();
                    sumScore = 0;
                    StartGame(3, 3);
                    SendMessage(string.Format("-----------------------------------------------------起始点({1}，{2}) 总分：{0}", sumScore,0,0));
            //    }
            //}
            //result.Sort();
            //for (int i = 0; i < result.Count; i++)
            //{
            //    SendMessage(result[i].ToString());
            //}
        }

        //public void StartGame(int m, int n)
        //{
        //    string key = m.ToString() + n.ToString();
        //    if (!GamePanel.OriginDict.Keys.Contains(key) ||GamePanel.OriginDict[key] == null)
        //    {
        //        return ;
        //    }
        //    for (int y = n; y < 10; y++)
        //    {
        //        for (int x = m; x < 10; x++)
        //        {
        //            if (GamePanel.OriginDict[x.ToString()+y.ToString()]==null|| GamePanel.OriginDict[x.ToString() + y.ToString()].IsNullBlock())
        //            {
        //                continue;
        //            }                    
        //            dictGroup.Clear();
        //            Expand(x, y);
        //            if (dictGroup.Keys.Count > 1)
        //            {
        //                int count = Destory();
        //                Refresh();
        //                CalculationScore(count);
        //                DrawGame();
        //                StartGame(x, y);
        //            }
        //            else
        //            {
        //                dictGroup.Clear();
        //                m++;
        //                StartGame(m, n);
        //            }

        //        }
        //        n++;
        //        m = 0;
        //        StartGame(m, n);
        //    }

        //}

        public void StartGame(int m, int n)
        {
            if (IsCleanPanel())
            {
                InitDict();
                for (int y = n; y < 10; y++)
                {
                    for (int x = m; x < 10; x++)
                    {
                        m++;
                        StartGame(x, y);
                    }
                    n++;
                }
            }
            for (int y =0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (GamePanel.OriginDict[x.ToString() + y.ToString()] == null || GamePanel.OriginDict[x.ToString() + y.ToString()].IsNullBlock())
                    {
                        continue;
                    }
                    dictGroup.Clear();
                    Expand(x, y);
                    if (dictGroup.Keys.Count > 1)
                    {
                        int count = Destory();
                        Refresh();
                        CalculationScore(count);
                        DrawGame();
                        StartGame(x, y);
                    }
                    //else
                    //{
                    //    dictGroup.Clear();
                    //    m++;
                    //    StartGame(m, n);
                    //}
                }
                //n++;
                //m = 0;
                //StartGame(m, n);
            }
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

        public void CalculationScore(int count)
        {
            sumScore += count * count;
            SendMessage(string.Format("计算得分:{0} 总分：{1}", count * count, sumScore));
            dictGroup.Clear();
        }

        public void MessageGroup(Block block, string message)
        {
            if (!dictGroup.Keys.Contains(block.GetKey()))
            {
                SendMessage(message);
            }
        }

        public bool HasNeighbor(int i, int j)
        {
            Block start = GamePanel.GetInstance().GetBlock(i.ToString() + j.ToString());
            if (start != null)
            {
                //MessageGroup(start, string.Format("set origin  ({0},{1}) kind:{2} in group", start.location.X.ToString(), start.location.Y.ToString(), start.kind));
                Block left = start.GetSameGroupLeftBlock();
                Block up = start.GetSameGroupUpBlock();
                Block right = start.GetSameGroupRightBlock();
                Block down = start.GetSameGroupDownBlock();
                if (   left  != null
                    || up    != null
                    || right != null
                    || down  != null)
                {
                    return true;
                }
                return false;
            }
            return false;
        }


        public void Expand(int i, int j)
        {
            if (!HasNeighbor(i,j))
            {
                return;
            }
            Block start = GamePanel.GetInstance().GetBlock(i.ToString() + j.ToString());
            if (start != null)
            {
                MessageGroup(start, string.Format("set origin  ({0},{1}) kind:{2} in group", start.location.X.ToString(), start.location.Y.ToString(), start.kind));
                Block left = start.GetSameGroupLeftBlock();
                Block up = start.GetSameGroupUpBlock();
                Block right = start.GetSameGroupRightBlock();
                Block down = start.GetSameGroupDownBlock();
                //string key = left.location.X.ToString() + left.location.Y.ToString();
                if (left != null && !dictGroup.Keys.Contains(left.location.X.ToString() + left.location.Y.ToString()))
                {
                    MessageGroup(left, string.Format("add left    ({0},{1}) in group", left.location.X.ToString(), left.location.Y.ToString(), left.kind));
                    dictGroup.Add(left.location.X.ToString() + left.location.Y.ToString(), left);
                    Expand(left.location.X, left.location.Y);
                }
                if (up != null && !dictGroup.Keys.Contains(up.location.X.ToString() + up.location.Y.ToString()))
                {
                    MessageGroup(up, string.Format("add up      ({0},{1}) in group", up.location.X.ToString(), up.location.Y.ToString(), up.kind));
                    dictGroup.Add(up.location.X.ToString() + up.location.Y.ToString(), up);
                    Expand(up.location.X, up.location.Y);
                }
                if (right != null && !dictGroup.Keys.Contains(right.location.X.ToString() + right.location.Y.ToString()))
                {
                    MessageGroup(right, string.Format("add right   ({0},{1}) in group", right.location.X.ToString(), right.location.Y.ToString(), right.kind));
                    dictGroup.Add(right.location.X.ToString() + right.location.Y.ToString(), right);
                    Expand(right.location.X, right.location.Y);
                }
                if (down != null && !dictGroup.Keys.Contains(down.location.X.ToString() + down.location.Y.ToString()))
                {
                    MessageGroup(down, string.Format("add down    ({0},{1}) in group", down.location.X.ToString(), down.location.Y.ToString(), down.kind));
                    dictGroup.Add(down.location.X.ToString() + down.location.Y.ToString(), down);
                    Expand(down.location.X, down.location.Y);
                }
                return;
            }
            return;


        }

        public int Destory()
        {
            Dictionary<string, Block> origin = GamePanel.OriginDict;
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

        public void Refresh()
        {
            RefreshRow();
            RefreshColumn();
        }

        public void RefreshRow()
        {
            Dictionary<string, Block> origin = GamePanel.OriginDict;
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
                            origin[upkey] = new Block(-1, 0, 0);// origin[upkey].SetDisabled();
                        }
                    }
                    else
                    {
                        if (origin[key] != null && origin[key].IsNullBlock())
                        {
                            origin[key] = null;
                        }
                        //if ((origin[key] != null && origin[key].IsNullBlock()) || origin[key] == null)
                        //{
                        //origin[key] = null;
                        //}
                    }

                }
                while (GamePanel.GetInstance().IsClearColumn(x))
                {
                    x++;
                    if (x > 9)
                    {
                        break;
                    }
                }
            }
        }

        public void RefreshColumn()
        {
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


    }
}
