using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace XXL.Model
{
    class StateUnit
    {
        public StateUnit(
            Dictionary<string, Block> dictOrigin,
            Dictionary<string, List<Block>> childs,
            string key,
            int score,
            int x, 
            int y,
            List<string> parentList,
            int parentScore)
        {
            this.dictOrigin = dictOrigin;
            this.groups = childs;
            this.Key = key;
            this.Score = score+parentScore;
            Point = new Point(x, y);
            ListLog.AddRange(parentList);
        }

        string key;
        int score;
        Point point;
        Dictionary<string, Block> dictOrigin;
        Dictionary<string, List<Block>> groups;
        List<string> listLog = new List<string>();

        public int Score
        {
            get
            {
                return score;
            }

            private set
            {
                score = value;
            }
        }

        public Point Point
        {
            get
            {
                return point;
            }

            private set
            {
                point = value;
            }
        }

        public string Key
        {
            get
            {
                return key;
            }

            private set
            {
                key = value;
            }
        }

        public Dictionary<string, Block> DictOrigin
        {
            get
            {
                return dictOrigin;
            }

            private set
            {
                dictOrigin = value;
            }
        }

       

        public Dictionary<string, List<Block>> Groups
        {
            get
            {
                return groups;
            }

            private set
            {
                groups = value;
            }
        }

        public List<string> ListLog
        {
            get
            {
                return listLog;
            }

            set
            {
                listLog = value;
            }
        }
    }
}
