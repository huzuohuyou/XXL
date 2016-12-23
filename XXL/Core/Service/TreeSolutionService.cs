using BIMT.Util;
using BIMT.Util.Serialiaze;
using BIMT.Util.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using XXL.Model;

namespace XXL.Core.Service
{
    class TreeSolutionService : BaseXXLService
    {
        Dictionary<string, string> dictLog = new Dictionary<string, string>();
        List<Block> listGroup = new List<Block>();
        static Dictionary<string, List<Block>> dictPanelGroup = new Dictionary<string, List<Block>>();
        TreeSolution solution;
        MLTree<StateUnit> tree;// = new MLTree<int>();
        public TreeSolutionService(IViewCallback view) : base(view)
        {
            solution = TreeSolution.GetInstance();
        }

        public void InitGroupDict()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Expand(j, i);
                    string key = GetGroupKey(dictGroup);
                    List<Block> listGroup = GetGroupValues(dictGroup);
                    if (!dictPanelGroup.Keys.Contains(key) && key.Length > 0 && listGroup.Count > 1)
                    {
                        dictPanelGroup.Add(key, listGroup);
                    }
                    dictGroup.Clear();
                }
            }
        }

        public Dictionary<string, List<Block>> InitGroupList(Dictionary<string, Block> origin)
        {
            Dictionary<string, List<Block>> temp = new Dictionary<string, List<Block>>();
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (origin[x.ToString() + y.ToString()] == null)
                    {
                        continue;
                    }
                    if (y==-1)
                    {
                        int sss = 0;
                    }
                    Expand(origin, x, y);
                    string key = GetGroupKey(dictGroup);
                    List<Block> listGroup = GetGroupValues(dictGroup);
                    if (!temp.Keys.Contains(key) && key.Length > 0)
                    {
                        //SendMessage(string.Format("--------------------------------Add group:{0}", key));
                        temp.Add(key, listGroup);
                    }
                    else
                    {
                        key = string.Empty;
                    }

                    dictGroup.Clear();
                }
            }
            return temp;
        }

        Dictionary<string, Dictionary<string, Block>> dictOriginBase = new Dictionary<string, Dictionary<string, Block>>();

        public void InitTree(MLNode<StateUnit> parent)
        {
            int index = 0;

            foreach (string key in parent.Data.Groups.Keys)
            {
                List<Block> list = parent.Data.Groups[key];
                MLNode<StateUnit> child = new MLNode<StateUnit>(list.Count);
                tree.Insert(child, parent, index);
                index++;
                if (list.Count == 1)
                {
                    child.Data = new StateUnit(null, null, key, list.Count > 1 ? list.Count * list.Count : 0, list[0].location.X, list[0].location.Y);
                    continue;
                }
                else
                {//继续扩展
                    Dictionary<string, Block> dictOrigin = DeserialiazeClass.Deserialize<Dictionary<string, Block>>( SerialiazeClass.Serialiaze( new Dictionary<string, Block>(parent.Data.DictOrigin)));
                    dictOrigin = Destory(dictOrigin, list);
                    dictOrigin = Refresh(dictOrigin);
                    SendMessage(string.Format("===================================（{0},{1}）", list[0].location.X, list[0].location.Y));

                    Dictionary<string, List<Block>> childs = InitGroupList(dictOrigin);
                    child.Data = new StateUnit(dictOrigin, childs, key, list.Count > 1 ? list.Count * list.Count : 0, list[0].location.X, list[0].location.Y);
                    DrawGame(dictOrigin);
                    //break;
                    //dictOriginBase.Add(,temp);
                    //InitTree(child);

                    Thread.Sleep(1000);
                    //DrawGame(temp);

                    //InitTree(temp, refresh, child);
                }
            }
        }
        List<int> result = new List<int>();
        int sum = 0;
        public void CalScore(MLNode<StateUnit> parent)
        {
            if (parent == null)
            {
                return;
            }
            string s = string.Format("pick:({0},{1}) key:{2}\n", parent.Data.Point.X, parent.Data.Point.Y, parent.Data.Key);
            File.AppendAllText("D:\\result.txt", s);
            sum += parent.Data.Score;
            foreach (MLNode<StateUnit> item in parent.Childs)
            {
                if (parent.Childs[0] == null)
                {
                    //SendMessage(string.Format("计算完成:{0}....", sum));
                    string finish = string.Format("{0}  >>>>>>>>>>>>>>>>>>>>>>>>>>>>score:{1}\n", DateTime.Now.ToString("HH:mm:ss"), sum);
                    File.AppendAllText("D:\\result.txt", finish);
                    //sum = 0;
                }
                else
                {
                    //if (item != null)
                    //{
                    CalScore(item);
                    //}
                }
            }
        }

        public string GetGroupKey(Dictionary<string, Block> dict)
        {
            string key = string.Empty;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (dict.Keys.Contains(x.ToString() + y.ToString()))
                    {
                        key += x.ToString() + y.ToString();
                    }
                }
            }
            return key;
        }

       

        public string GetNextKey()
        {
            foreach (string key in dictPanelGroup.Keys)
            {
                if (!dictLog.Keys.Contains(key) && key.Length > 2)
                {
                    dictLog.Add(key, key);
                    return key;
                }
            }
            return string.Empty;
        }

        public List<Block> GetGroupValues(Dictionary<string, Block> dict)
        {
            List<Block> list = new List<Block>();
            foreach (string item in dict.Keys)
            {
                list.Add(dict[item]);
            }
            return list;
        }

        public override void Expand(int i, int j)
        {
            Expand(gamePanel.OriginDict, i, j);
        }

        public void Expand(Dictionary<string, Block> dictOrigin, int i, int j)
        {
            
            Block start = dictOrigin.Keys.Contains(i.ToString() + j.ToString())? dictOrigin[i.ToString() + j.ToString()] : null;// GamePanel.GetInstance().GetBlock(dictOrigin, i.ToString() + j.ToString());
            if ( start != null&&start.kind!=-1)
            {
                if (!dictGroup.Keys.Contains(start.location.X.ToString() + start.location.Y.ToString()))
                {
                    MessageGroup(start, string.Format("set origin  ({0},{1}) kind:{2} in group", start.location.X.ToString(), start.location.Y.ToString(), start.kind));
                    dictGroup.Add(start.location.X.ToString() + start.location.Y.ToString(), start);
                }
                Block left = start.GetSameGroupLeftBlock(dictOrigin);
                Block up = start.GetSameGroupUpBlock(dictOrigin);
                Block right = start.GetSameGroupRightBlock(dictOrigin);
                Block down = start.GetSameGroupDownBlock(dictOrigin);
                //string key = left.location.X.ToString() + left.location.Y.ToString();
                if (left != null && !dictGroup.Keys.Contains(left.location.X.ToString() + left.location.Y.ToString()))
                {
                    MessageGroup(left, string.Format("add left    ({0},{1}) in group", left.location.X.ToString(), left.location.Y.ToString(), left.kind));
                    dictGroup.Add(left.location.X.ToString() + left.location.Y.ToString(), left);
                    Expand(dictOrigin, left.location.X, left.location.Y);
                }
                if (up != null && !dictGroup.Keys.Contains(up.location.X.ToString() + up.location.Y.ToString()))
                {
                    MessageGroup(up, string.Format("add up      ({0},{1}) in group", up.location.X.ToString(), up.location.Y.ToString(), up.kind));
                    dictGroup.Add(up.location.X.ToString() + up.location.Y.ToString(), up);
                    Expand(dictOrigin, up.location.X, up.location.Y);
                }
                if (right != null && !dictGroup.Keys.Contains(right.location.X.ToString() + right.location.Y.ToString()))
                {
                    MessageGroup(right, string.Format("add right   ({0},{1}) in group", right.location.X.ToString(), right.location.Y.ToString(), right.kind));
                    dictGroup.Add(right.location.X.ToString() + right.location.Y.ToString(), right);
                    Expand(dictOrigin, right.location.X, right.location.Y);
                }
                if (down != null && !dictGroup.Keys.Contains(down.location.X.ToString() + down.location.Y.ToString()))
                {
                    MessageGroup(down, string.Format("add down    ({0},{1}) in group", down.location.X.ToString(), down.location.Y.ToString(), down.kind));
                    dictGroup.Add(down.location.X.ToString() + down.location.Y.ToString(), down);
                    Expand(dictOrigin, down.location.X, down.location.Y);
                }
                return;
            }
            return;
            
        }

        public void StartAt(int x, int y)
        {
            Dictionary<string, Block> origin = gamePanel.OriginDict;// GetOriginDict();
            Expand(origin, x, y);
            //Expand(origin, 1, 0);
            string key = GetGroupKey(dictGroup);
            List<Block> list = GetGroupValues(dictGroup);
            origin = Destory(origin, GetGroupValues(dictGroup));
            origin = Refresh(origin);
            SendMessage(string.Format("=================================({0},{1})",x,y));
            DrawGame(origin);
            dictGroup.Clear();
            Dictionary<string, List<Block>> childs = InitGroupList(origin);
            tree = new MLTree<StateUnit>();
            MLNode<StateUnit> head = new MLNode<StateUnit>(childs.Keys.Count);
            head.Data =new StateUnit(origin, childs, key, list.Count > 1 ? list.Count * list.Count : 0,list[0].location.X,list[0].location.Y);

            tree.Head = head;
            InitTree(tree.Head);
            CalScore(tree.Head);
            tree.Clear();
        }

        public void StartAt(int x, int y,int index) {
            SendMessage(string.Format("Tree:{0}", index));
            string s = string.Format("{0}: tree:{1} start at({2},{3})", DateTime.Now.ToString("HH:mm:ss"), index, x, y);
            File.AppendAllText("D:\\result.txt", s);
            StartAt(x,y);
        }
        public override void StartGame()
        {
            InitGroupDict();
            //int index = 0;
            //foreach (string key in dictPanelGroup.Keys)
            //{
            //    int x = int.Parse(key.ToArray()[0].ToString());
            //    int y = int.Parse(key.ToArray()[1].ToString());
            //    StartAt(x, y,index);
            //    index++;
            //}
            StartAt(8, 0, 0);
        }
    }
}
