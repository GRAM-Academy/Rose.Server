using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rose.Client;

namespace RoseBench
{
    public partial class FormSchemeView : Form
    {
        private const int Category_Scheme = 0;
        private const int Category_Collection = 1;

        private ContextMenu _menuTreeView = new ContextMenu();


        public static string SelectedScheme { get; private set; } = "";
        public static string SelectedCollection { get; private set; } = "";





        public FormSchemeView()
        {
            InitializeComponent();
        }


        private void OnLoad(object sender, EventArgs e)
        {
            _menuTreeView.MenuItems.Add(new MenuItem("Refresh", Click_Refresh));
            _menuTreeView.MenuItems.Add("-");
            _menuTreeView.MenuItems.Add(new MenuItem("Create Scheme", Click_CreateScheme));
            _menuTreeView.MenuItems.Add(new MenuItem("Drop Scheme", Click_DropScheme));
            _menuTreeView.MenuItems.Add("-");
            _menuTreeView.MenuItems.Add(new MenuItem("Create Collection", Click_CreateCollection));
            _menuTreeView.MenuItems.Add(new MenuItem("Drop Collection", Click_DropCollection));
            _menuTreeView.Popup += (s2, e2) =>
            {
                var node = _tvScheme.SelectedNode;
                if (node == null)
                {
                    _menuTreeView.MenuItems[0].Enabled = true;

                    _menuTreeView.MenuItems[2].Enabled = true;
                    _menuTreeView.MenuItems[3].Enabled = false;

                    _menuTreeView.MenuItems[5].Enabled = false;
                    _menuTreeView.MenuItems[6].Enabled = false;
                }
                else if ((int)node.Tag == Category_Scheme)
                {
                    _menuTreeView.MenuItems[0].Enabled = true;

                    _menuTreeView.MenuItems[2].Enabled = true;
                    _menuTreeView.MenuItems[3].Enabled = true;

                    _menuTreeView.MenuItems[5].Enabled = true;
                    _menuTreeView.MenuItems[6].Enabled = false;
                }
                else if ((int)node.Tag == Category_Collection)
                {
                    _menuTreeView.MenuItems[0].Enabled = true;

                    _menuTreeView.MenuItems[2].Enabled = false;
                    _menuTreeView.MenuItems[3].Enabled = false;

                    _menuTreeView.MenuItems[5].Enabled = true;
                    _menuTreeView.MenuItems[6].Enabled = true;
                }
            };

            _tvScheme.ImageList = new ImageList();
            _tvScheme.ImageList.Images.Add(Properties.Resources.Scheme);
            _tvScheme.ImageList.Images.Add(Properties.Resources.Collection);
            _tvScheme.ContextMenu = _menuTreeView;

            Click_Refresh(_tvScheme, null);
        }


        private void Click_Refresh(object sender, EventArgs e)
        {
            _tvScheme.Nodes.Clear();

            FormMain.SetStatus("Refreshing scheme list...");
            Global.API.SchemeList(Global.UrlAPI, (result) =>
            {
                if (result.ResultCode != RoseResult.Ok)
                {
                    FormMain.SetStatusRed(result.Message);
                    return;
                }


                var schemeList = result.Response.GetProperty("result").Value;
                foreach (var item in schemeList)
                {
                    var name = (string)item.GetProperty("schemeName").Value;
                    var node = _tvScheme.Nodes.Add(name, name, 0, 0);
                    node.Tag = Category_Scheme;
                }


                FormMain.SetStatus("Refreshing collection list...");
                RefreshCollection(_tvScheme.Nodes[0]);
            });
        }


        private void RefreshCollection(TreeNode schemeNode)
        {
            if (schemeNode == null)
            {
                FormMain.SetStatus("Ready");
                return;

            }

            Global.API.CollectionList(Global.UrlAPI, schemeNode.Text, (result) =>
            {
                if (result.ResultCode != RoseResult.Ok)
                {
                    FormMain.SetStatusRed(result.Message);
                    return;
                }

                var collectionList = result.Response.GetProperty("result").Value;
                foreach (var item in collectionList)
                {
                    var name = (string)item.GetProperty("name").Value;
                    var justInCache = (bool)item.GetProperty("justInCache").Value;
                    var node = schemeNode.Nodes.Add(name, name, 1, 1);
                    node.ForeColor = (justInCache ? Color.Blue : Color.Black);
                    node.Tag = Category_Collection;
                }

                schemeNode.ExpandAll();
                RefreshCollection(schemeNode.NextNode);
            });
        }


        private void Click_CreateScheme(object sender, EventArgs e)
        {
            SimpleInputBox dialog = new SimpleInputBox();
            dialog.Title = "Create new scheme";
            dialog.Description = "Input new scheme name.";
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.ShowDialog();

            if (dialog.IsOk)
            {
                FormMain.SetStatus($"Creating '{dialog.InputText}' scheme.");
                Global.API.CreateScheme(Global.UrlAPI, dialog.InputText, (result) =>
                {
                    if (result.ResultCode != RoseResult.Ok)
                    {
                        MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                    Click_Refresh(_tvScheme, null);
                    FormMain.SetStatus("Ready");
                });
            }
        }


        private void Click_DropScheme(object sender, EventArgs e)
        {
            var selectedNode = _tvScheme.SelectedNode;
            if (selectedNode == null)
                return;


            var ret = MessageBox.Show($"Would you like to drop '{selectedNode.Text}' scheme?",
                                      "Drop Scheme",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ret == DialogResult.No)
                return;


            FormMain.SetStatus($"Requesting drop '{selectedNode.Text}' scheme.");
            Global.API.DropScheme(Global.UrlAPI, selectedNode.Text, (result) =>
            {
                if (result.ResultCode != RoseResult.Ok)
                {
                    MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                Click_Refresh(_tvScheme, null);
                FormMain.SetStatus("Ready");
            });
        }


        private void Click_CreateCollection(object sender, EventArgs e)
        {
            var selectedNode = _tvScheme.SelectedNode;
            if (selectedNode == null)
                return;

            SimpleInputBox dialog = new SimpleInputBox();
            dialog.Title = "Create new collection";
            dialog.Description = "Input new collection name.";
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.ShowDialog();

            if (dialog.IsOk)
            {
                string schemeName = selectedNode.Text;
                FormMain.SetStatus($"Creating '{dialog.InputText}' collection.");
                Global.API.CreateCollection(Global.UrlAPI, schemeName, dialog.InputText, (result) =>
                {
                    if (result.ResultCode != RoseResult.Ok)
                    {
                        MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                    Click_Refresh(_tvScheme, null);
                    FormMain.SetStatus("Ready");
                });
            }
        }


        private void Click_DropCollection(object sender, EventArgs e)
        {
            var selectedNode = _tvScheme.SelectedNode;
            if (selectedNode == null)
                return;


            var ret = MessageBox.Show($"Would you like to drop '{selectedNode.Text}' collection?",
                                      "Drop Collection",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ret == DialogResult.No)
                return;


            string schemeName = selectedNode.Parent.Text;
            FormMain.SetStatus($"Requesting drop '{selectedNode.Text}' scheme.");
            Global.API.DropCollection(Global.UrlAPI, schemeName, selectedNode.Text, (result) =>
            {
                if (result.ResultCode != RoseResult.Ok)
                {
                    MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                Click_Refresh(_tvScheme, null);
                FormMain.SetStatus("Ready");
            });
        }


        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _tvScheme.SelectedNode = e.Node;
        }


        private void NodeSelected(object sender, TreeViewEventArgs e)
        {
            var node = _tvScheme.SelectedNode;
            if (node == null)
            {
                SelectedScheme = "";
                SelectedCollection = "";
                return;
            }


            if ((int)node.Tag == Category_Scheme)
            {
                SelectedScheme = node.Text;
                SelectedCollection = "";
            }
            if ((int)node.Tag == Category_Collection)
            {
                SelectedScheme = node.Parent.Text;
                SelectedCollection = node.Text;
            }
        }
    }
}
