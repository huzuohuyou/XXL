using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace XXL.Model
{
    class Step
    {
        public Step(string key,int score, int x, int y)
        {
            this.Key = key;
            this.Score = score;
            Point = new Point(x, y);
        }
        string key;
        int score;
        Point point;

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
    }
}
