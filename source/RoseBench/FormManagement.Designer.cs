namespace RoseBench
{
    partial class FormManagement
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
            this._tbSelect_Where = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this._tbSelect_Result = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._tbSelect_RangeCount = new System.Windows.Forms.TextBox();
            this._tbSelect_RangeStart = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this._cbSelect_OrderBy = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this._tbSelect_SortKey = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._tbInsert_Data = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this._tbInsert_UniqueFor = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this._tbUpdate_Data = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this._tbUpdate_Where = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this._tbDelete_Where = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this._btnValidate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this._tbInsert_OnDuplicate = new System.Windows.Forms.TextBox();
            this._tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tbSelect_Where
            // 
            this._tbSelect_Where.Location = new System.Drawing.Point(86, 36);
            this._tbSelect_Where.Name = "_tbSelect_Where";
            this._tbSelect_Where.Size = new System.Drawing.Size(131, 22);
            this._tbSelect_Where.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Where";
            // 
            // _tabControl
            // 
            this._tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tabControl.Controls.Add(this.tabPage1);
            this._tabControl.Controls.Add(this.tabPage2);
            this._tabControl.Controls.Add(this.tabPage3);
            this._tabControl.Controls.Add(this.tabPage4);
            this._tabControl.Location = new System.Drawing.Point(12, 12);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(535, 390);
            this._tabControl.TabIndex = 27;
            this._tabControl.SelectedIndexChanged += new System.EventHandler(this.OnSelectCommandTab);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this._tbSelect_Result);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(527, 363);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Select";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // _tbSelect_Result
            // 
            this._tbSelect_Result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tbSelect_Result.BackColor = System.Drawing.Color.LightBlue;
            this._tbSelect_Result.Location = new System.Drawing.Point(47, 199);
            this._tbSelect_Result.Multiline = true;
            this._tbSelect_Result.Name = "_tbSelect_Result";
            this._tbSelect_Result.ReadOnly = true;
            this._tbSelect_Result.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._tbSelect_Result.Size = new System.Drawing.Size(430, 147);
            this._tbSelect_Result.TabIndex = 39;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.White;
            this.label12.Font = new System.Drawing.Font("Gulim", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(15, 159);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(87, 37);
            this.label12.TabIndex = 40;
            this.label12.Text = "Result";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Font = new System.Drawing.Font("Gulim", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(15, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 37);
            this.label9.TabIndex = 35;
            this.label9.Text = "Condition";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this._tbSelect_RangeCount);
            this.groupBox1.Controls.Add(this._tbSelect_Where);
            this.groupBox1.Controls.Add(this._tbSelect_RangeStart);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this._cbSelect_OrderBy);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this._tbSelect_SortKey);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(47, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 117);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            // 
            // _tbSelect_RangeCount
            // 
            this._tbSelect_RangeCount.Location = new System.Drawing.Point(358, 36);
            this._tbSelect_RangeCount.Name = "_tbSelect_RangeCount";
            this._tbSelect_RangeCount.Size = new System.Drawing.Size(50, 22);
            this._tbSelect_RangeCount.TabIndex = 32;
            // 
            // _tbSelect_RangeStart
            // 
            this._tbSelect_RangeStart.Location = new System.Drawing.Point(304, 36);
            this._tbSelect_RangeStart.Name = "_tbSelect_RangeStart";
            this._tbSelect_RangeStart.Size = new System.Drawing.Size(50, 22);
            this._tbSelect_RangeStart.TabIndex = 30;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Sort key";
            // 
            // _cbSelect_OrderBy
            // 
            this._cbSelect_OrderBy.BackColor = System.Drawing.Color.White;
            this._cbSelect_OrderBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbSelect_OrderBy.FormattingEnabled = true;
            this._cbSelect_OrderBy.Items.AddRange(new object[] {
            "Asc",
            "Desc"});
            this._cbSelect_OrderBy.Location = new System.Drawing.Point(304, 67);
            this._cbSelect_OrderBy.Name = "_cbSelect_OrderBy";
            this._cbSelect_OrderBy.Size = new System.Drawing.Size(104, 21);
            this._cbSelect_OrderBy.TabIndex = 28;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(250, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Range";
            // 
            // _tbSelect_SortKey
            // 
            this._tbSelect_SortKey.Location = new System.Drawing.Point(86, 67);
            this._tbSelect_SortKey.Name = "_tbSelect_SortKey";
            this._tbSelect_SortKey.Size = new System.Drawing.Size(131, 22);
            this._tbSelect_SortKey.TabIndex = 27;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(236, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Order by";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(527, 363);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Insert";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Gulim", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(5, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 37);
            this.label3.TabIndex = 42;
            this.label3.Text = "Data";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this._tbInsert_Data);
            this.groupBox2.Location = new System.Drawing.Point(48, 169);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(430, 144);
            this.groupBox2.TabIndex = 43;
            this.groupBox2.TabStop = false;
            // 
            // _tbInsert_Data
            // 
            this._tbInsert_Data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tbInsert_Data.Location = new System.Drawing.Point(28, 29);
            this._tbInsert_Data.Multiline = true;
            this._tbInsert_Data.Name = "_tbInsert_Data";
            this._tbInsert_Data.Size = new System.Drawing.Size(366, 85);
            this._tbInsert_Data.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("Gulim", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(15, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 37);
            this.label4.TabIndex = 41;
            this.label4.Text = "Condition";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this._tbInsert_OnDuplicate);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this._tbInsert_UniqueFor);
            this.groupBox6.Location = new System.Drawing.Point(47, 27);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(430, 112);
            this.groupBox6.TabIndex = 40;
            this.groupBox6.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "uniqueFor";
            // 
            // _tbInsert_UniqueFor
            // 
            this._tbInsert_UniqueFor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tbInsert_UniqueFor.Location = new System.Drawing.Point(100, 38);
            this._tbInsert_UniqueFor.Name = "_tbInsert_UniqueFor";
            this._tbInsert_UniqueFor.Size = new System.Drawing.Size(295, 22);
            this._tbInsert_UniqueFor.TabIndex = 23;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.groupBox4);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(527, 363);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Update";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.White;
            this.label13.Font = new System.Drawing.Font("Gulim", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(5, 128);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(87, 37);
            this.label13.TabIndex = 38;
            this.label13.Text = "Data";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this._tbUpdate_Data);
            this.groupBox4.Location = new System.Drawing.Point(48, 139);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(430, 144);
            this.groupBox4.TabIndex = 39;
            this.groupBox4.TabStop = false;
            // 
            // _tbUpdate_Data
            // 
            this._tbUpdate_Data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tbUpdate_Data.Location = new System.Drawing.Point(28, 29);
            this._tbUpdate_Data.Multiline = true;
            this._tbUpdate_Data.Name = "_tbUpdate_Data";
            this._tbUpdate_Data.Size = new System.Drawing.Size(366, 85);
            this._tbUpdate_Data.TabIndex = 23;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.White;
            this.label10.Font = new System.Drawing.Font("Gulim", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(15, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(103, 37);
            this.label10.TabIndex = 37;
            this.label10.Text = "Condition";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this._tbUpdate_Where);
            this.groupBox3.Location = new System.Drawing.Point(47, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(430, 91);
            this.groupBox3.TabIndex = 36;
            this.groupBox3.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(32, 38);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "Where";
            // 
            // _tbUpdate_Where
            // 
            this._tbUpdate_Where.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tbUpdate_Where.Location = new System.Drawing.Point(86, 35);
            this._tbUpdate_Where.Name = "_tbUpdate_Where";
            this._tbUpdate_Where.Size = new System.Drawing.Size(309, 22);
            this._tbUpdate_Where.TabIndex = 23;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label14);
            this.tabPage4.Controls.Add(this.groupBox5);
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(527, 363);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Delete";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.White;
            this.label14.Font = new System.Drawing.Font("Gulim", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(15, 14);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(103, 37);
            this.label14.TabIndex = 39;
            this.label14.Text = "Condition";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this._tbDelete_Where);
            this.groupBox5.Location = new System.Drawing.Point(47, 27);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(430, 91);
            this.groupBox5.TabIndex = 38;
            this.groupBox5.TabStop = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(32, 38);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(48, 13);
            this.label15.TabIndex = 22;
            this.label15.Text = "Where";
            // 
            // _tbDelete_Where
            // 
            this._tbDelete_Where.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tbDelete_Where.Location = new System.Drawing.Point(86, 35);
            this._tbDelete_Where.Name = "_tbDelete_Where";
            this._tbDelete_Where.Size = new System.Drawing.Size(305, 22);
            this._tbDelete_Where.TabIndex = 23;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Cornsilk;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(438, 420);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 36);
            this.button1.TabIndex = 28;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Click_Send);
            // 
            // _btnValidate
            // 
            this._btnValidate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnValidate.BackColor = System.Drawing.Color.Cornsilk;
            this._btnValidate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnValidate.Location = new System.Drawing.Point(308, 420);
            this._btnValidate.Name = "_btnValidate";
            this._btnValidate.Size = new System.Drawing.Size(109, 36);
            this._btnValidate.TabIndex = 37;
            this._btnValidate.Text = "Validate";
            this._btnValidate.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "onDuplicate";
            // 
            // _tbInsert_OnDuplicate
            // 
            this._tbInsert_OnDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tbInsert_OnDuplicate.Location = new System.Drawing.Point(100, 66);
            this._tbInsert_OnDuplicate.Name = "_tbInsert_OnDuplicate";
            this._tbInsert_OnDuplicate.Size = new System.Drawing.Size(295, 22);
            this._tbInsert_OnDuplicate.TabIndex = 25;
            // 
            // FormManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(559, 468);
            this.Controls.Add(this._btnValidate);
            this.Controls.Add(this.button1);
            this.Controls.Add(this._tabControl);
            this.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "FormManagement";
            this.Text = "FormManagement";
            this._tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox _tbSelect_Where;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox _tbSelect_RangeCount;
        private System.Windows.Forms.TextBox _tbSelect_RangeStart;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox _cbSelect_OrderBy;
        private System.Windows.Forms.TextBox _tbSelect_SortKey;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox _tbUpdate_Data;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox _tbUpdate_Where;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox _tbDelete_Where;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox _tbInsert_Data;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox _tbInsert_UniqueFor;
        private System.Windows.Forms.TextBox _tbSelect_Result;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button _btnValidate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _tbInsert_OnDuplicate;
    }
}