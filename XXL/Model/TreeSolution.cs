using BIMT.Util.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XXL.Model
{
    class TreeSolution
    {
        private static TreeSolution solution;
        public List<MLTree<int>> solutionList = new List<MLTree<int>>();
        //private static MLTree<int> tree;//= new MLTree<string>();
        private TreeSolution() { }

        public static TreeSolution GetInstance()
        {
            if (solution == null)
            {
                solution = new TreeSolution();
            }
            return solution;
        }

        //public void SetRoot(Dictionary<string, Block> dict)
        //{
        //    MLNode<int> pt = new MLNode<int>(dict.Count);
        //    if (tree != null)
        //    {
        //        tree.Head = pt;
        //    }
        //}
    }
}
