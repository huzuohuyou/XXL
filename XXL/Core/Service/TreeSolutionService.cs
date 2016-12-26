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

        public Dictionary<string, Block> CloneDict(Dictionary<string, Block> origin)
        {
            Dictionary<string, Block> dict = new Dictionary<string, Block>();
            foreach (var item in origin.Keys)
            {
                dict.Add(item, origin[item] == null ? null : origin[item].GetSameBlock());
            }
            return dict;
        }


        public Dictionary<string, List<Block>> InitGroupList(Dictionary<string, Block> origin)
        {
            Dictionary<string, List<Block>> temp = new Dictionary<string, List<Block>>();
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (!CanExpand(origin, x, y))
                    {
                        continue;
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

        public bool Cleared(MLNode<StateUnit> parent) {
            foreach (var item in parent.Data.Groups.Keys)
            {
                if (item.Length > 2)
                {
                    return false;
                }
            }
            return true;
        }

        public void InitTree(MLNode<StateUnit> parent)
        {
            if (Cleared(parent))
            {
                //SendMessage(string.Format("===============ONE PATH====================）"));
                //SendMessageWithProcess(-1, string.Empty);
                return;
            }
            int index = 0;
            Dictionary<string, Block> panel = CloneDict(parent.Data.DictOrigin);
            foreach (string key in parent.Data.Groups.Keys)
            {
                if (key.Length == 2)
                {
                    continue;
                }
                List<Block> list = parent.Data.Groups[key];
                MLNode<StateUnit> child = new MLNode<StateUnit>(list.Count);
                tree.Insert(child, parent, index);
                index++;
                //if (list.Count == 1)
                //{
                //    child.Data = new StateUnit(null, null, key, list.Count > 1 ? list.Count * list.Count : 0, list[0].location.X, list[0].location.Y);
                //    continue;
                //}
                //else
                //{//继续扩展
                //GameFrame frame = new GameFrame(parent.Data.DictOrigin);
                Dictionary<string, Block> cloned = CloneDict(panel);
                //Dictionary<string, Block> dictOrigin = DeserialiazeClass.Deserialize<Dictionary<string, Block>>( SerialiazeClass.Serialiaze( new Dictionary<string, Block>(parent.Data.DictOrigin)));
                cloned = Destory(cloned, list);
                cloned = Refresh(cloned);
                //SendMessage(string.Format("===================================（{0},{1}）", list[0].location.X, list[0].location.Y));
                Dictionary<string, List<Block>> childs =  InitGroupList(cloned);
                child.Data = new StateUnit(
                    cloned,
                    childs,
                    key,
                    list.Count > 1 ? list.Count * list.Count : 0,
                    list[0].location.X,
                    list[0].location.Y,
                    parent.Data.ListLog,
                    parent.Data.Score);
                //DrawGame(cloned);
                //break;
                //dictOriginBase.Add(,temp);
                InitTree(child);
                Thread.Sleep(500);
                //DrawGame(temp);
                //InitTree(temp, refresh, child);
                //}
            }
        }
        List<int> result = new List<int>();
        int sum = 0;
        public string GetPath(List<string> list)
        {
            string path = string.Empty;
            foreach (var item in list)
            {
                path += "-" + item + "\n";
            }
            return path;
        }
        public void CalScore(MLNode<StateUnit> parent)
        {
            if (parent.Childs.Length == 0 || (parent.Childs.Length > 0 && parent.Childs[0] == null))
            {
                string s = string.Format("score:{0} \n path:{1} \n", parent.Data.Score, GetPath(parent.Data.ListLog));
                File.AppendAllText("D:\\result.txt", s);
            }
            foreach (MLNode<StateUnit> item in parent.Childs)
            {
                CalScore(item);
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


        public bool CanExpand(Dictionary<string, Block> dictOrigin, int i, int j)
        {
            Block start = dictOrigin.Keys.Contains(i.ToString() + j.ToString()) ? dictOrigin[i.ToString() + j.ToString()] : null;
            Block up = dictOrigin.Keys.Contains(i.ToString() + (j + 1).ToString()) ? dictOrigin[i.ToString() + (j + 1).ToString()] : null;
            Block down = dictOrigin.Keys.Contains(i.ToString() + (j - 1).ToString()) ? dictOrigin[i.ToString() + (j - 1).ToString()] : null;
            Block left = dictOrigin.Keys.Contains((i - 1).ToString() + j.ToString()) ? dictOrigin[(i - 1).ToString() + j.ToString()] : null;
            Block right = dictOrigin.Keys.Contains((i + 1).ToString() + j.ToString()) ? dictOrigin[(i + 1).ToString() + j.ToString()] : null;
            if (start != null
                && start.kind != -1
                && (up != null || down != null || right != null || left != null))
            {
                return true;
            }
            return false;
        }

        public void Expand(Dictionary<string, Block> dictOrigin, int i, int j)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                try
                {
                    sb.Append(i);
                    sb.Append(j);
                }
                catch (Exception)
                {

                    throw;
                }
               
              
                Block start = dictOrigin.Keys.Contains(sb.ToString()) ? dictOrigin[sb.ToString()] : null;// GamePanel.GetInstance().GetBlock(dictOrigin, i.ToString() + j.ToString());
                if (start != null && start.kind != -1)
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
            catch (Exception)
            {

                throw;
            }
           
            
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
            List<string> root = new List<string>();
            //root.Add(key);

            head.Data =new StateUnit(
                origin, 
                childs, 
                key, 
                list.Count > 1 ? list.Count * list.Count : 0,
                list[0].location.X,
                list[0].location.Y,
                root,
                0);

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
