namespace EffectSome
{
    partial class LevelOverview
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelOverview));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button6 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LevelFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelRevision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelBuildTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelSong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelObjects = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelGroups = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelTriggers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelAttempts = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.LevelFolder,
            this.LevelName,
            this.LevelRevision,
            this.LevelDescription,
            this.LevelBuildTime,
            this.LevelLength,
            this.LevelSong,
            this.LevelObjects,
            this.LevelGroups,
            this.LevelTriggers,
            this.LevelAttempts,
            this.LevelVersion,
            this.LevelID,
            this.LevelStatus});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 4;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(1125, 551);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseUp);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button6
            // 
            this.button6.Enabled = false;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button6.Location = new System.Drawing.Point(12, 569);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(1201, 23);
            this.button6.TabIndex = 12;
            this.button6.Text = "Apply Changes";
            this.toolTip1.SetToolTip(this.button6, "Apply all changes.");
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button9
            // 
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button9.Image = ((System.Drawing.Image)(resources.GetObject("button9.Image")));
            this.button9.Location = new System.Drawing.Point(1181, 50);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(32, 32);
            this.button9.TabIndex = 4;
            this.toolTip1.SetToolTip(this.button9, "Move the selected levels to the bottom of the list.");
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            this.button9.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button10.Image = ((System.Drawing.Image)(resources.GetObject("button10.Image")));
            this.button10.Location = new System.Drawing.Point(1181, 12);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(32, 32);
            this.button10.TabIndex = 3;
            this.toolTip1.SetToolTip(this.button10, "Move the selected levels to the top of the list.");
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            this.button10.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button8.Image = ((System.Drawing.Image)(resources.GetObject("button8.Image")));
            this.button8.Location = new System.Drawing.Point(1143, 50);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(32, 32);
            this.button8.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button8, "Move the selected levels down by one place.");
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            this.button8.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button7.Image = ((System.Drawing.Image)(resources.GetObject("button7.Image")));
            this.button7.Location = new System.Drawing.Point(1143, 12);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(32, 32);
            this.button7.TabIndex = 1;
            this.toolTip1.SetToolTip(this.button7, "Move the selected levels up by one place.");
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            this.button7.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button5.Image = ((System.Drawing.Image)(resources.GetObject("button5.Image")));
            this.button5.Location = new System.Drawing.Point(1143, 455);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(32, 32);
            this.button5.TabIndex = 7;
            this.toolTip1.SetToolTip(this.button5, "Create a new level. The new level will be placed at the top of the list.");
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            this.button5.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.Location = new System.Drawing.Point(1143, 531);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(32, 32);
            this.button4.TabIndex = 9;
            this.toolTip1.SetToolTip(this.button4, "Clone the selected levels. The cloned levels will be placed in the top of the lis" +
        "t in the order they appear.");
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            this.button4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.Location = new System.Drawing.Point(1143, 493);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(32, 32);
            this.button3.TabIndex = 8;
            this.toolTip1.SetToolTip(this.button3, "Delete the selected levels.");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            this.button3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(1181, 531);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(32, 32);
            this.button2.TabIndex = 11;
            this.toolTip1.SetToolTip(this.button2, "Export the selected levels to files.");
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(1181, 493);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 32);
            this.button1.TabIndex = 10;
            this.toolTip1.SetToolTip(this.button1, "Import levels from a file.");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button11
            // 
            this.button11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button11.Image = ((System.Drawing.Image)(resources.GetObject("button11.Image")));
            this.button11.Location = new System.Drawing.Point(1143, 202);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(32, 32);
            this.button11.TabIndex = 5;
            this.toolTip1.SetToolTip(this.button11, "Select all levels.");
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            this.button11.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button12
            // 
            this.button12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button12.Image = ((System.Drawing.Image)(resources.GetObject("button12.Image")));
            this.button12.Location = new System.Drawing.Point(1143, 240);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(32, 32);
            this.button12.TabIndex = 6;
            this.toolTip1.SetToolTip(this.button12, "Deselect all levels.");
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            this.button12.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button13
            // 
            this.button13.Enabled = false;
            this.button13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button13.Image = ((System.Drawing.Image)(resources.GetObject("button13.Image")));
            this.button13.Location = new System.Drawing.Point(1143, 126);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(32, 32);
            this.button13.TabIndex = 13;
            this.toolTip1.SetToolTip(this.button13, "Swap the two selected levels.");
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            this.button13.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button14
            // 
            this.button14.Enabled = false;
            this.button14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button14.Image = ((System.Drawing.Image)(resources.GetObject("button14.Image")));
            this.button14.Location = new System.Drawing.Point(1181, 202);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(32, 32);
            this.button14.TabIndex = 14;
            this.toolTip1.SetToolTip(this.button14, "Select all levels located above the last selected level.");
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            this.button14.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button15
            // 
            this.button15.Enabled = false;
            this.button15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button15.Image = ((System.Drawing.Image)(resources.GetObject("button15.Image")));
            this.button15.Location = new System.Drawing.Point(1181, 240);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(32, 32);
            this.button15.TabIndex = 15;
            this.toolTip1.SetToolTip(this.button15, "Select all levels located below the first selected level.");
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            this.button15.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            // 
            // button16
            // 
            this.button16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button16.Image = ((System.Drawing.Image)(resources.GetObject("button16.Image")));
            this.button16.Location = new System.Drawing.Point(1143, 278);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(32, 32);
            this.button16.TabIndex = 16;
            this.toolTip1.SetToolTip(this.button16, "Invert the current selection.");
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Import levels from files";
            // 
            // Selected
            // 
            this.Selected.HeaderText = "";
            this.Selected.Name = "Selected";
            this.Selected.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Selected.Width = 22;
            // 
            // LevelFolder
            // 
            this.LevelFolder.HeaderText = "Folder";
            this.LevelFolder.Name = "LevelFolder";
            this.LevelFolder.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelFolder.Width = 45;
            // 
            // LevelName
            // 
            this.LevelName.HeaderText = "Name";
            this.LevelName.MaxInputLength = 32;
            this.LevelName.Name = "LevelName";
            this.LevelName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelName.Width = 150;
            // 
            // LevelRevision
            // 
            this.LevelRevision.HeaderText = "Rev.";
            this.LevelRevision.MaxInputLength = 3;
            this.LevelRevision.Name = "LevelRevision";
            this.LevelRevision.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelRevision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelRevision.Width = 40;
            // 
            // LevelDescription
            // 
            this.LevelDescription.HeaderText = "Description";
            this.LevelDescription.MaxInputLength = 256;
            this.LevelDescription.Name = "LevelDescription";
            this.LevelDescription.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelDescription.Width = 283;
            // 
            // LevelBuildTime
            // 
            this.LevelBuildTime.HeaderText = "Build Time";
            this.LevelBuildTime.Name = "LevelBuildTime";
            this.LevelBuildTime.ReadOnly = true;
            this.LevelBuildTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelBuildTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelBuildTime.Width = 75;
            // 
            // LevelLength
            // 
            this.LevelLength.HeaderText = "Length";
            this.LevelLength.Name = "LevelLength";
            this.LevelLength.ReadOnly = true;
            this.LevelLength.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelLength.Width = 52;
            // 
            // LevelSong
            // 
            this.LevelSong.HeaderText = "Song";
            this.LevelSong.MaxInputLength = 7;
            this.LevelSong.Name = "LevelSong";
            this.LevelSong.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelSong.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelSong.Width = 50;
            // 
            // LevelObjects
            // 
            this.LevelObjects.HeaderText = "Objects";
            this.LevelObjects.Name = "LevelObjects";
            this.LevelObjects.ReadOnly = true;
            this.LevelObjects.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelObjects.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelObjects.Width = 51;
            // 
            // LevelGroups
            // 
            this.LevelGroups.HeaderText = "Groups";
            this.LevelGroups.Name = "LevelGroups";
            this.LevelGroups.ReadOnly = true;
            this.LevelGroups.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelGroups.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelGroups.Width = 50;
            // 
            // LevelTriggers
            // 
            this.LevelTriggers.HeaderText = "Triggers";
            this.LevelTriggers.Name = "LevelTriggers";
            this.LevelTriggers.ReadOnly = true;
            this.LevelTriggers.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelTriggers.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelTriggers.Width = 53;
            // 
            // LevelAttempts
            // 
            this.LevelAttempts.HeaderText = "Attempts";
            this.LevelAttempts.Name = "LevelAttempts";
            this.LevelAttempts.ReadOnly = true;
            this.LevelAttempts.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelAttempts.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelAttempts.Width = 63;
            // 
            // LevelVersion
            // 
            this.LevelVersion.HeaderText = "Version";
            this.LevelVersion.MaxInputLength = 7;
            this.LevelVersion.Name = "LevelVersion";
            this.LevelVersion.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelVersion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelVersion.Width = 51;
            // 
            // LevelID
            // 
            this.LevelID.HeaderText = "ID";
            this.LevelID.MaxInputLength = 9;
            this.LevelID.Name = "LevelID";
            this.LevelID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelID.Width = 62;
            // 
            // LevelStatus
            // 
            this.LevelStatus.HeaderText = "Status";
            this.LevelStatus.MaxInputLength = 50;
            this.LevelStatus.Name = "LevelStatus";
            this.LevelStatus.ReadOnly = true;
            this.LevelStatus.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelStatus.Width = 56;
            // 
            // LevelOverview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1225, 604);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LevelOverview";
            this.Text = "Level Overview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LevelOverview_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCheck);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelRevision;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelBuildTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelSong;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelObjects;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelGroups;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelTriggers;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelAttempts;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelID;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelStatus;
    }
}