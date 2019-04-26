using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rose.Client;

namespace RoseBench
{
    public partial class FormManagement
    {
        private void SetResult(RoseResult result)
        {
            if (result.ResultCode == RoseResult.Ok)
            {
                _tbSelect_Result.Text = result.Response.GetProperty("result", false)?.Value.ToString() ?? "";
                FormMain.SetStatus("Processing time: {0:0.###} sec     {1}",
                    result.Response.GetProperty("processingTime").Value,
                    result.Message);
            }
            else
            {
                _tbSelect_Result.Text = result.Message;
                var exception = result.Response.GetProperty("exception", false);
                if (exception != null)
                    _tbSelect_Result.Text += "\r\n";

                FormMain.SetStatusRed(result.Message);
            }
        }


        private void DoSelect(string scheme, string collection)
        {
            string where = (_tbSelect_Where.Text == "" ? null : _tbSelect_Where.Text);
            string sortKey = (_tbSelect_SortKey.Text == "" ? null : _tbSelect_SortKey.Text);
            int rangeStart, rangeCount;


            if (int.TryParse(_tbSelect_RangeStart.Text, out rangeStart) == false)
            {
                _tbSelect_RangeStart.Text = "0";
                rangeStart = 0;
            }
            if (int.TryParse(_tbSelect_RangeCount.Text, out rangeCount) == false)
            {
                _tbSelect_RangeCount.Text = "1000";
                rangeCount = 1000;
            }


            if (_cbSelect_OrderBy.SelectedIndex == 0)
                sortKey += " asc";
            else
                sortKey += " desc";


            FormMain.SetStatus("Requesting select command.");

            _tbSelect_Result.Text = "";
            Global.API.Select(
                Global.UrlAPI, $"{scheme}.{collection}",
                where, sortKey,
                rangeStart, rangeCount,
                (result) =>
                {
                    SetResult(result);
                });
        }


        private void DoInsert(string scheme, string collection)
        {
            string uniqueFor = (_tbInsert_UniqueFor.Text == "" ? null : _tbInsert_UniqueFor.Text);
            string onDuplicate = (_tbInsert_OnDuplicate.Text == "" ? null : _tbInsert_OnDuplicate.Text);
            var data = Newtonsoft.Json.Linq.JToken.Parse(_tbInsert_Data.Text);


            FormMain.SetStatus("Requesting insert command.");

            _tbSelect_Result.Text = "";
            Global.API.Insert(
                Global.UrlAPI, $"{scheme}.{collection}",
                uniqueFor, onDuplicate, data,
                (result) =>
                {
                    SetResult(result);
                });
        }


        private void DoUpdate(string scheme, string collection)
        {
            string where = (_tbUpdate_Where.Text == "" ? null : _tbUpdate_Where.Text);
            var data = Newtonsoft.Json.Linq.JToken.Parse(_tbUpdate_Data.Text);


            FormMain.SetStatus("Requesting update command.");

            _tbSelect_Result.Text = "";
            Global.API.Update(
                Global.UrlAPI, $"{scheme}.{collection}",
                where, data,
                (result) =>
                {
                    SetResult(result);
                });
        }


        private void DoDelete(string scheme, string collection)
        {
            string where = (_tbDelete_Where.Text == "" ? null : _tbDelete_Where.Text);


            FormMain.SetStatus("Requesting delete command.");

            _tbSelect_Result.Text = "";
            Global.API.Delete(
                Global.UrlAPI, $"{scheme}.{collection}",
                where,
                (result) =>
                {
                    SetResult(result);
                });
        }
    }
}
