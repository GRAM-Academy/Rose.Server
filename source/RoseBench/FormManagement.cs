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
    public partial class FormManagement : Form
    {
        public FormManagement()
        {
            InitializeComponent();

            _cbSelect_OrderBy.SelectedIndex = 0;
            _btnValidate.Click += (s, e) =>
            {
                if (_tabControl.SelectedIndex == 1) { Global.JsonValidate(_tbInsert_Data); }
                if (_tabControl.SelectedIndex == 2) { Global.JsonValidate(_tbUpdate_Data); }
            };

            OnSelectCommandTab(_tabControl, null);
        }


        private void Click_Send(object sender, EventArgs e)
        {
            if (FormSchemeView.SelectedCollection == "")
            {
                MessageBox.Show("Select a collection.", "Check");
                return;
            }

            if (_tabControl.SelectedIndex == 0)
                DoSelect(FormSchemeView.SelectedScheme, FormSchemeView.SelectedCollection);

            if (_tabControl.SelectedIndex == 1)
            {
                if (Global.JsonValidate(_tbInsert_Data) == true)
                    DoInsert(FormSchemeView.SelectedScheme, FormSchemeView.SelectedCollection);
            }

            if (_tabControl.SelectedIndex == 2)
            {
                if (Global.JsonValidate(_tbUpdate_Data) == true)
                    DoUpdate(FormSchemeView.SelectedScheme, FormSchemeView.SelectedCollection);
            }

            if (_tabControl.SelectedIndex == 3)
                DoDelete(FormSchemeView.SelectedScheme, FormSchemeView.SelectedCollection);
        }


        private void OnSelectCommandTab(object sender, EventArgs e)
        {
            if (_tabControl.SelectedIndex == 1 ||
                _tabControl.SelectedIndex == 2)
            {
                _btnValidate.Visible = true;
            }
            else
                _btnValidate.Visible = false;
        }
    }
}
