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
using XXL.Core.Service;

namespace XXL
{
    public partial class frmMain : BaseFormView
    {
        BaseXXLService service;
        public frmMain()
        {
            InitializeComponent();
           
        }

        public override void SetView(int count, string value)
        {
            richTextBox1.Text = string.Format("{0}: {1} \n", DateTime.Now.ToString("HH:mm:ss"), value) + richTextBox1.Text;
            //richTextBox1.Font = new Font(new FontFamily("Courier New"),10.5f);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            service = new TreeSolutionService(this);
            service.InitDict();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(service.StartGame);
            t.Start();
        }

    }
}
