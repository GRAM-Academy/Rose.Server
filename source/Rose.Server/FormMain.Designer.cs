namespace Rose.Server
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._tbLog = new System.Windows.Forms.TextBox();
            this._btnStartStop = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _tbLog
            // 
            this._tbLog.BackColor = System.Drawing.Color.White;
            this._tbLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._tbLog.Location = new System.Drawing.Point(13, 81);
            this._tbLog.MaxLength = 1048576;
            this._tbLog.Multiline = true;
            this._tbLog.Name = "_tbLog";
            this._tbLog.ReadOnly = true;
            this._tbLog.Size = new System.Drawing.Size(502, 242);
            this._tbLog.TabIndex = 91;
            this._tbLog.TabStop = false;
            // 
            // _btnStartStop
            // 
            this._btnStartStop.BackColor = System.Drawing.Color.LightYellow;
            this._btnStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnStartStop.Location = new System.Drawing.Point(13, 13);
            this._btnStartStop.Name = "_btnStartStop";
            this._btnStartStop.Size = new System.Drawing.Size(142, 53);
            this._btnStartStop.TabIndex = 100;
            this._btnStartStop.Text = "Start";
            this._btnStartStop.UseVisualStyleBackColor = false;
            this._btnStartStop.Click += new System.EventHandler(this.Click_StartStop);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightYellow;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(177, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 53);
            this.button1.TabIndex = 101;
            this.button1.Text = "Initialize Storage";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(541, 344);
            this.Controls.Add(this.button1);
            this.Controls.Add(this._btnStartStop);
            this.Controls.Add(this._tbLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ROSE";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox _tbLog;
        private System.Windows.Forms.Button _btnStartStop;
        private System.Windows.Forms.Button button1;
    }
}

