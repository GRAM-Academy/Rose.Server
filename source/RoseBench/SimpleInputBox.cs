using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoseBench
{
    public partial class SimpleInputBox : Form
    {
        public string Title { set { base.Text = value; } }
        public string Description { set { _lbDescription.Text = value; } }
        public string InputText { get { return _tbInput.Text; } }
        public bool IsOk { get; private set; }





        public SimpleInputBox()
        {
            InitializeComponent();
        }


        private void Click_Ok(object sender, EventArgs e)
        {
            IsOk = true;
            Close();
        }


        private void Click_Cancel(object sender, EventArgs e)
        {
            IsOk = false;
            Close();
        }
    }
}
