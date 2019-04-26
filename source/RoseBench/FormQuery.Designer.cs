namespace RoseBench
{
    partial class FormQuery
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
            this.button1 = new System.Windows.Forms.Button();
            this._tbResponse = new System.Windows.Forms.TextBox();
            this._tbRequest = new System.Windows.Forms.TextBox();
            this._btnValidate = new System.Windows.Forms.Button();
            this._cbURL = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._cbShowOnlyResult = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this._cbTemplate = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Cornsilk;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(233, 361);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 41);
            this.button1.TabIndex = 35;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Click_Send);
            // 
            // _tbResponse
            // 
            this._tbResponse.BackColor = System.Drawing.Color.MintCream;
            this._tbResponse.Font = new System.Drawing.Font("Gulim", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._tbResponse.Location = new System.Drawing.Point(386, 93);
            this._tbResponse.Multiline = true;
            this._tbResponse.Name = "_tbResponse";
            this._tbResponse.ReadOnly = true;
            this._tbResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._tbResponse.Size = new System.Drawing.Size(355, 262);
            this._tbResponse.TabIndex = 34;
            // 
            // _tbRequest
            // 
            this._tbRequest.Font = new System.Drawing.Font("Gulim", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._tbRequest.Location = new System.Drawing.Point(24, 63);
            this._tbRequest.Multiline = true;
            this._tbRequest.Name = "_tbRequest";
            this._tbRequest.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._tbRequest.Size = new System.Drawing.Size(355, 292);
            this._tbRequest.TabIndex = 33;
            // 
            // _btnValidate
            // 
            this._btnValidate.BackColor = System.Drawing.Color.Cornsilk;
            this._btnValidate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnValidate.Location = new System.Drawing.Point(24, 361);
            this._btnValidate.Name = "_btnValidate";
            this._btnValidate.Size = new System.Drawing.Size(146, 41);
            this._btnValidate.TabIndex = 36;
            this._btnValidate.Text = "Validate";
            this._btnValidate.UseVisualStyleBackColor = false;
            // 
            // _cbURL
            // 
            this._cbURL.BackColor = System.Drawing.Color.White;
            this._cbURL.FormattingEnabled = true;
            this._cbURL.Location = new System.Drawing.Point(60, 18);
            this._cbURL.Name = "_cbURL";
            this._cbURL.Size = new System.Drawing.Size(317, 21);
            this._cbURL.TabIndex = 37;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "URL";
            // 
            // _cbShowOnlyResult
            // 
            this._cbShowOnlyResult.AutoSize = true;
            this._cbShowOnlyResult.Location = new System.Drawing.Point(386, 63);
            this._cbShowOnlyResult.Name = "_cbShowOnlyResult";
            this._cbShowOnlyResult.Size = new System.Drawing.Size(130, 17);
            this._cbShowOnlyResult.TabIndex = 39;
            this._cbShowOnlyResult.Text = "Show only result";
            this._cbShowOnlyResult.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(383, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Template";
            // 
            // _cbTemplate
            // 
            this._cbTemplate.BackColor = System.Drawing.Color.White;
            this._cbTemplate.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this._cbTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbTemplate.FormattingEnabled = true;
            this._cbTemplate.Location = new System.Drawing.Point(453, 18);
            this._cbTemplate.Name = "_cbTemplate";
            this._cbTemplate.Size = new System.Drawing.Size(157, 23);
            this._cbTemplate.TabIndex = 26;
            this._cbTemplate.SelectedIndexChanged += new System.EventHandler(this.OnCommandSelected);
            // 
            // FormQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(793, 463);
            this.Controls.Add(this._cbShowOnlyResult);
            this.Controls.Add(this._cbURL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._btnValidate);
            this.Controls.Add(this.button1);
            this.Controls.Add(this._tbResponse);
            this.Controls.Add(this._tbRequest);
            this.Controls.Add(this._cbTemplate);
            this.Controls.Add(this.label5);
            this.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "FormQuery";
            this.Text = "Query";
            this.Shown += new System.EventHandler(this.FormShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox _tbResponse;
        private System.Windows.Forms.TextBox _tbRequest;
        private System.Windows.Forms.Button _btnValidate;
        private System.Windows.Forms.ComboBox _cbURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox _cbShowOnlyResult;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox _cbTemplate;
    }
}