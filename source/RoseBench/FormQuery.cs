using Newtonsoft.Json.Linq;
using Rose.Client;
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
    public partial class FormQuery : Form
    {
        public FormQuery()
        {
            InitializeComponent();

            _btnValidate.Click += (s, e) => { Global.JsonValidate(_tbRequest); };
        }


        private void FormShown(object sender, EventArgs e)
        {
            _cbURL.Items.Clear();
            _cbURL.Items.Add(Global.UrlAPI);

            _cbTemplate.Items.Clear();
            _cbTemplate.Items.Add("hello");
            _cbTemplate.Items.Add("select");
            _cbTemplate.Items.Add("insert");
            _cbTemplate.Items.Add("update");
            _cbTemplate.Items.Add("delete");
            _cbTemplate.DrawItem += ComboBoxItem_CenterAlign;
        }


        //  ComboBox Item 가운데 정렬
        private void ComboBoxItem_CenterAlign(object sender, DrawItemEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            e.DrawBackground();
            if (e.Index >= 0)
            {
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                Brush brush = new SolidBrush(cb.ForeColor);
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    brush = SystemBrushes.HighlightText;

                e.Graphics.DrawString(cb.Items[e.Index].ToString(), cb.Font, brush, e.Bounds, sf);
            }
        }


        private void SetResult(RoseResult result)
        {
            if (result.ResultCode == RoseResult.Ok)
            {
                if (_cbShowOnlyResult.Checked)
                    _tbResponse.Text = result.Response.GetProperty("result", false)?.Value.ToString(Newtonsoft.Json.Formatting.Indented) ?? "";
                else
                    _tbResponse.Text = result.Response.ToString(Newtonsoft.Json.Formatting.Indented);
                FormMain.SetStatus("Processing time: {0:0.##}sec     {1}",
                    result.Response.GetProperty("processingTime").Value,
                    result.Message);
            }
            else
            {
                _tbResponse.Text = result.Message;
                var exception = result.Response.GetProperty("exception", false);
                if (exception != null)
                    _tbResponse.Text += "\r\n";

                FormMain.SetStatusRed(result.Message);
            }
        }


        private void OnCommandSelected(object sender, EventArgs e)
        {
            string templateHello = "{\"cmd\":\"hello\"}";
            string templateSelect = "{\"cmd\":\"select\",\"collection\":\"{0}\",\"where\":\"\",\"sortKey\":\"\",\"range\":[0,1000]}";
            string templateInsert = "{\"cmd\":\"insert\",\"collection\":\"{0}\",\"uniqueFor\":\"\",\"onDuplicate\":\"ignore\",\"data\":{}}";
            string templateUpdate = "{\"cmd\":\"update\",\"collection\":\"{0}\",\"data\":{}}";
            string templateDelete = "{\"cmd\":\"delete\",\"collection\":\"{0}\",\"where\":\"\"}";
            string text = "";

            if (_cbTemplate.SelectedIndex == 0) text = JObject.Parse(templateHello).ToString();
            if (_cbTemplate.SelectedIndex == 1) text = JObject.Parse(templateSelect).ToString();
            if (_cbTemplate.SelectedIndex == 2) text = JObject.Parse(templateInsert).ToString();
            if (_cbTemplate.SelectedIndex == 3) text = JObject.Parse(templateUpdate).ToString();
            if (_cbTemplate.SelectedIndex == 4) text = JObject.Parse(templateDelete).ToString();

            _tbRequest.Text = text.Replace("{0}", $"{FormSchemeView.SelectedScheme}.{FormSchemeView.SelectedCollection}");
        }


        private void Click_Send(object sender, EventArgs e)
        {
            if (Global.JsonValidate(_tbRequest) == false)
                return;


            if (_cbURL.Text == null || _cbURL.Text == "")
            {
                FormMain.SetStatusRed("Please select target url.");
                return;
            }

            Global.API.SendAsync(_cbURL.Text, JToken.Parse(_tbRequest.Text), (result) =>
            {
                SetResult(result);
            });
        }
    }
}
