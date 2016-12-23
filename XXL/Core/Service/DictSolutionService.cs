using BIMT.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using XXL.Model;

namespace XXL.Core.Service
{
    class DictSolutionService : BaseXXLService
    {
       

        public DictSolutionService(IViewCallback view) : base(view)
        {
        }

        public override void Expand(int i, int j)
        {

            if (!HasNeighbor(i, j))
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

        

       
       
       

        Dictionary<string, Point> dictSolution = new Dictionary<string, Point>();
        public void InitSolution()
        {
            //SendMessage("解决方案初始化.............");
            for (int step = 0; step < 100; step++)
            {
                for (int choice = 0; choice < 100; choice++)
                {
                    int y = choice / 10;
                    int x = choice % 10;
                    dictSolution.Add(string.Format("{0}_{1}", step, choice), new Point(x, y));
                }
            }
        }
        int choice = 0;
        public Point GetNextStep(int step)
        {
            if (step == 99)
            {
                choice = 0;
            }
            else
            {
                choice++;
            }
            return dictSolution[string.Format("{0}_{1}", step, choice)];
        }

        public static Dictionary<int, int> dictStepPower = new Dictionary<int, int>();
        public void InitStepDict()
        {
            for (int i = 0; i < 100; i++)
            {
                dictStepPower.Add(i, 0);
            }

        }
        public static bool once = true;
        public void DoStep()
        {
            once = true;
            DoStep(0);
        }

        public void JumpStep(int count)
        {
            for (int i = 0; i < count - 1; i++)
            {
                once = true;
                DoStep(0);
            }
        }

        public void DoStep(int i)
        {
            if (i == 100)
            {
                return;
            }
            if (once)
            {
                dictStepPower[i] = ++dictStepPower[i];
            }
            once = false;
            if (dictStepPower[i] == 100)
            {
                dictStepPower[i] = 0;
                dictStepPower[i + 1] = ++dictStepPower[i + 1];
            }
            DoStep(++i);
        }




        public void StartGame(int m, int n)
        {
            string key = m.ToString() + n.ToString();
            if (!GamePanel.OriginDict.Keys.Contains(key) || GamePanel.OriginDict[key] == null)
            {
                return;
            }
            for (int y = n; y < 10; y++)
            {
                for (int x = m; x < 10; x++)
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
                    else
                    {
                        dictGroup.Clear();
                        m++;
                        StartGame(m, n);
                    }

                }
                n++;
                m = 0;
                StartGame(m, n);
            }

        }



        public override void StartGame()
        {
            if (IsCleanPanel())
            {
                SendMessage("GAME OVER!");
            }
            StartGame(0,0);

        }

        public void CalculationScore(int count)
        {
            sumScore += count * count;
            //SendMessage(string.Format("计算得分:{0} 总分：{1}", count * count, sumScore));
            dictGroup.Clear();
        }
    }
}
