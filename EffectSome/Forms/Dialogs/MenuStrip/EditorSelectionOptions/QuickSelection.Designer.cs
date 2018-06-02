namespace EffectSome
{
    partial class QuickSelection
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
            this.groupIDs1 = new System.Windows.Forms.ListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.groupIDs2 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // groupIDs1
            // 
            this.groupIDs1.ColumnWidth = 20;
            this.groupIDs1.Enabled = false;
            this.groupIDs1.FormattingEnabled = true;
            this.groupIDs1.Location = new System.Drawing.Point(260, 22);
            this.groupIDs1.MultiColumn = true;
            this.groupIDs1.Name = "groupIDs1";
            this.groupIDs1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.groupIDs1.Size = new System.Drawing.Size(131, 30);
            this.groupIDs1.TabIndex = 13;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(205, 17);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "Select triggers which affect this object";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(12, 35);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(242, 17);
            this.checkBox2.TabIndex = 15;
            this.checkBox2.Text = "Select objects that belong to the same groups";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(12, 58);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(235, 17);
            this.checkBox3.TabIndex = 16;
            this.checkBox3.Text = "Select triggers that target the chosen groups";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(12, 81);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(216, 17);
            this.checkBox4.TabIndex = 17;
            this.checkBox4.Text = "Select objects with the same parameters";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // groupIDs2
            // 
            this.groupIDs2.ColumnWidth = 25;
            this.groupIDs2.Enabled = false;
            this.groupIDs2.FormattingEnabled = true;
            this.groupIDs2.Location = new System.Drawing.Point(260, 58);
            this.groupIDs2.MultiColumn = true;
            this.groupIDs2.Name = "groupIDs2";
            this.groupIDs2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.groupIDs2.Size = new System.Drawing.Size(131, 30);
            this.groupIDs2.TabIndex = 18;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button1.Location = new System.Drawing.Point(12, 104);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(379, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Select!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // QuickSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 139);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupIDs2);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.groupIDs1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "QuickSelection";
            this.Text = "Quick Selection";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QuickSelection_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox groupIDs1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.ListBox groupIDs2;
        private System.Windows.Forms.Button button1;
    }
}