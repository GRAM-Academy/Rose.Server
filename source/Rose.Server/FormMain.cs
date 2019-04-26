using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rose.Server
{
    public partial class FormMain : Form
    {
        public static FormMain Instance { get; private set; }





        public FormMain()
        {
            InitializeComponent();

            FormClosed += OnFormClosed;
            _btnStartStop.Text = "Start";
            Instance = this;


            ServerMain.FinalizedHandler += () =>
            {
                Helper.InvokeAction(_btnStartStop, () =>
                {
                    _btnStartStop.Text = "Start";
                });
            };


            (new Aegis.Calculate.IntervalTimer("UpdateMainUI", 1000, () =>
            {
            })).Start();
        }


        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            ServerMain.StopServer();

            Aegis.Calculate.IntervalTimer.Timers["UpdateMainUI"]?.Dispose();
        }


        private void Click_StartStop(object sender, EventArgs e)
        {
            if (_btnStartStop.Text == "Start")
            {
                _tbLog.Text = "";
                _btnStartStop.Text = "Stop";
                ServerMain.StartServer(_tbLog);
            }
            else
            {
                _btnStartStop.Text = "Start";
                ServerMain.StopServer();
            }
        }


        public void SetServiceName(string serviceName)
        {
            Helper.InvokeAction(this, () =>
            {
                Text = serviceName;
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServerMain.CreateStorage();
        }
    }
}
