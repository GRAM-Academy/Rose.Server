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
    public partial class FormMain : Form
    {
        private Dictionary<FormType, Form> _forms = new Dictionary<FormType, Form>();
        private FormSchemeView _formSchemeView = new FormSchemeView();
        private static FormMain _instance;





        public FormMain()
        {
            InitializeComponent();

            _instance = this;
            Global.Initialize();
            Global.Load();


            {
                _formSchemeView.Text = "";
                _formSchemeView.TopLevel = false;
                _formSchemeView.ControlBox = false;
                _formSchemeView.FormBorderStyle = FormBorderStyle.None;
                _formSchemeView.Size = _panel1.Size;
                _formSchemeView.Show();
                _panel1.Controls.Add(_formSchemeView);
            }

            _forms[FormType.Management] = new FormManagement();
            _forms[FormType.Query] = new FormQuery();
            _forms[FormType.Setting] = new FormSetting();

            foreach (var form in _forms.Select(v => v.Value))
            {
                form.Text = "";
                form.TopLevel = false;
                form.ControlBox = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Size = _panel2.Size;
                form.Hide();
                _panel2.Controls.Add(form);
            }


            this.Size = new Size(1024, 768);
        }


        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            Global.Release();
        }


        private void ShowForm(FormType type)
        {
            foreach (var item in _forms)
            {
                if (item.Key == type)
                    item.Value.Show();
                else
                    item.Value.Hide();
            }
        }


        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            _formSchemeView.Size = _panel1.Size;
            foreach (var form in _forms.Select(v => v.Value))
                form.Size = _panel2.Size;
        }


        #region Setting status text
        public static void SetStatus(string text)
        {
            WinFormHelper.TryInvoke(_instance._lbStatus, () =>
            {
                _instance._lbStatus.Text = text;
                _instance._lbStatus.ForeColor = Color.White;
                _instance._lbStatus.BackColor = Color.SteelBlue;
            });
        }


        public static void SetStatus(string format, params object[] args)
        {
            WinFormHelper.TryInvoke(_instance._lbStatus, () =>
            {
                string text = string.Format(format, args);
                _instance._lbStatus.Text = text;
                _instance._lbStatus.ForeColor = Color.White;
                _instance._lbStatus.BackColor = Color.SteelBlue;
            });
        }


        public static void SetStatusRed(string text)
        {
            WinFormHelper.TryInvoke(_instance._lbStatus, () =>
            {
                _instance._lbStatus.Text = text;
                _instance._lbStatus.ForeColor = Color.White;
                _instance._lbStatus.BackColor = Color.Firebrick;
            });
        }


        public static void SetStatusRed(string format, params object[] args)
        {
            WinFormHelper.TryInvoke(_instance._lbStatus, () =>
            {
                string text = string.Format(format, args);
                _instance._lbStatus.Text = text;
                _instance._lbStatus.ForeColor = Color.White;
                _instance._lbStatus.BackColor = Color.Firebrick;
            });
        }
        #endregion


        private void Click_Management(object sender, EventArgs e)
        {
            ShowForm(FormType.Management);
        }


        private void Click_Query(object sender, EventArgs e)
        {
            ShowForm(FormType.Query);
        }


        private void Click_Setting(object sender, EventArgs e)
        {
            ShowForm(FormType.Setting);
        }
    }
}
