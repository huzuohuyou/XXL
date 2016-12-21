using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BIMT.Util;
using XXL.Core;
using System.Threading;
using XXL.Model;

namespace XXL
{
    public partial class frmMain : BaseFormView
    {
        XXLService service;
        public frmMain()
        {
            InitializeComponent();
           
        }

        public override void SetView(int count, string value)
        {
            richTextBox1.Text = string.Format("{0}: {1} \n", DateTime.Now.ToString("HH:mm:ss"), value) + richTextBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GamePanel.OriginDict.Clear();
            service = new XXLService(this);
            service.InitSolution();
            service.InitStepDict();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(service.DoWork);
            t.Start();
            //XXLService service = new XXLService(this,0,0);
            //service.StartGame(0,0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int x = 1;
            int j = 0;
            if (x == 1)
            {
                j++;
            }
            if (x == 2)
            {
                j++;
            }
            if (x == 3)
            {
                j++;
            }

            j++;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < 999; i++)
            {
                service.DoStep();
            }
            Dictionary<int,int> dict =  XXLService.dictStepPower;

            //for (double  i = 0; i <1E+200; i++)
            //{
            //    double x = i;
            //}
            //MessageBox.Show("over");
        }
    }
}
