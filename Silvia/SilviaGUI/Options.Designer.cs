namespace SilviaGUI
{
    partial class Options
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxPlugIns = new System.Windows.Forms.ListBox();
            this.chkBoxDelayedLoad = new System.Windows.Forms.CheckBox();
            this.btnUnload = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSelectAvatar = new System.Windows.Forms.Button();
            this.chkBoxShowAvatar = new System.Windows.Forms.CheckBox();
            this.chkBoxShowGUIOnLaunch = new System.Windows.Forms.CheckBox();
            this.chkBoxStartOnBoot = new System.Windows.Forms.CheckBox();
            this.btnAccept = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnPluginVisibility = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnPluginVisibility);
            this.groupBox1.Controls.Add(this.listBoxPlugIns);
            this.groupBox1.Controls.Add(this.chkBoxDelayedLoad);
            this.groupBox1.Controls.Add(this.btnUnload);
            this.groupBox1.Location = new System.Drawing.Point(12, 144);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(442, 330);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PlugIn Settings";
            // 
            // listBoxPlugIns
            // 
            this.listBoxPlugIns.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxPlugIns.FormattingEnabled = true;
            this.listBoxPlugIns.IntegralHeight = false;
            this.listBoxPlugIns.ItemHeight = 25;
            this.listBoxPlugIns.Location = new System.Drawing.Point(6, 19);
            this.listBoxPlugIns.Name = "listBoxPlugIns";
            this.listBoxPlugIns.Size = new System.Drawing.Size(211, 304);
            this.listBoxPlugIns.TabIndex = 4;
            // 
            // chkBoxDelayedLoad
            // 
            this.chkBoxDelayedLoad.AutoSize = true;
            this.chkBoxDelayedLoad.Enabled = false;
            this.chkBoxDelayedLoad.Location = new System.Drawing.Point(223, 19);
            this.chkBoxDelayedLoad.Name = "chkBoxDelayedLoad";
            this.chkBoxDelayedLoad.Size = new System.Drawing.Size(200, 17);
            this.chkBoxDelayedLoad.TabIndex = 3;
            this.chkBoxDelayedLoad.Text = "Use Delayed Load (not implemented)";
            this.chkBoxDelayedLoad.UseVisualStyleBackColor = true;
            // 
            // btnUnload
            // 
            this.btnUnload.Enabled = false;
            this.btnUnload.Location = new System.Drawing.Point(223, 280);
            this.btnUnload.Name = "btnUnload";
            this.btnUnload.Size = new System.Drawing.Size(213, 43);
            this.btnUnload.TabIndex = 2;
            this.btnUnload.Text = "Unload";
            this.btnUnload.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(235, 480);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(125, 60);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.btnSelectAvatar);
            this.groupBox2.Controls.Add(this.chkBoxShowAvatar);
            this.groupBox2.Controls.Add(this.chkBoxShowGUIOnLaunch);
            this.groupBox2.Controls.Add(this.chkBoxStartOnBoot);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(442, 126);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 99);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(155, 20);
            this.textBox1.TabIndex = 3;
            // 
            // btnSelectAvatar
            // 
            this.btnSelectAvatar.Location = new System.Drawing.Point(167, 77);
            this.btnSelectAvatar.Name = "btnSelectAvatar";
            this.btnSelectAvatar.Size = new System.Drawing.Size(63, 43);
            this.btnSelectAvatar.TabIndex = 4;
            this.btnSelectAvatar.Text = "Select Avatar";
            this.btnSelectAvatar.UseVisualStyleBackColor = true;
            // 
            // chkBoxShowAvatar
            // 
            this.chkBoxShowAvatar.AutoSize = true;
            this.chkBoxShowAvatar.Checked = true;
            this.chkBoxShowAvatar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxShowAvatar.Location = new System.Drawing.Point(6, 76);
            this.chkBoxShowAvatar.Name = "chkBoxShowAvatar";
            this.chkBoxShowAvatar.Size = new System.Drawing.Size(173, 17);
            this.chkBoxShowAvatar.TabIndex = 2;
            this.chkBoxShowAvatar.Text = "Show Avatar (not implemented)";
            this.chkBoxShowAvatar.UseVisualStyleBackColor = true;
            // 
            // chkBoxShowGUIOnLaunch
            // 
            this.chkBoxShowGUIOnLaunch.AutoSize = true;
            this.chkBoxShowGUIOnLaunch.Checked = true;
            this.chkBoxShowGUIOnLaunch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxShowGUIOnLaunch.Location = new System.Drawing.Point(6, 42);
            this.chkBoxShowGUIOnLaunch.Name = "chkBoxShowGUIOnLaunch";
            this.chkBoxShowGUIOnLaunch.Size = new System.Drawing.Size(211, 17);
            this.chkBoxShowGUIOnLaunch.TabIndex = 1;
            this.chkBoxShowGUIOnLaunch.Text = "Show GUI on launch (not implemented)";
            this.chkBoxShowGUIOnLaunch.UseVisualStyleBackColor = true;
            // 
            // chkBoxStartOnBoot
            // 
            this.chkBoxStartOnBoot.AutoSize = true;
            this.chkBoxStartOnBoot.Location = new System.Drawing.Point(6, 19);
            this.chkBoxStartOnBoot.Name = "chkBoxStartOnBoot";
            this.chkBoxStartOnBoot.Size = new System.Drawing.Size(173, 17);
            this.chkBoxStartOnBoot.TabIndex = 0;
            this.chkBoxStartOnBoot.Text = "Start on boot (not implemented)";
            this.chkBoxStartOnBoot.UseVisualStyleBackColor = true;
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(104, 480);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(125, 60);
            this.btnAccept.TabIndex = 2;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // btnPluginVisibility
            // 
            this.btnPluginVisibility.Enabled = false;
            this.btnPluginVisibility.Location = new System.Drawing.Point(223, 231);
            this.btnPluginVisibility.Name = "btnPluginVisibility";
            this.btnPluginVisibility.Size = new System.Drawing.Size(213, 43);
            this.btnPluginVisibility.TabIndex = 5;
            this.btnPluginVisibility.Text = "Hide";
            this.btnPluginVisibility.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 547);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Options";
            this.Text = "Silvia Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkBoxShowAvatar;
        private System.Windows.Forms.CheckBox chkBoxShowGUIOnLaunch;
        private System.Windows.Forms.CheckBox chkBoxStartOnBoot;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUnload;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSelectAvatar;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.CheckBox chkBoxDelayedLoad;
        private System.Windows.Forms.ListBox listBoxPlugIns;
        private System.Windows.Forms.Button btnPluginVisibility;
    }
}