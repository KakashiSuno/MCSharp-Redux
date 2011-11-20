namespace Minecraft_Server
{
    partial class Window
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
            this.txtLog = new System.Windows.Forms.TextBox();
            this.liClients = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtReports = new System.Windows.Forms.TextBox();
            this.lable1 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnKick = new System.Windows.Forms.Button();
            this.btnBan = new System.Windows.Forms.Button();
            this.btnKickban = new System.Windows.Forms.Button();
            this.btnBanIp = new System.Windows.Forms.Button();
            this.btnChangeRank = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.Location = new System.Drawing.Point(16, 36);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(428, 272);
            this.txtLog.TabIndex = 0;
            // 
            // liClients
            // 
            this.liClients.FormattingEnabled = true;
            this.liClients.Location = new System.Drawing.Point(450, 35);
            this.liClients.Name = "liClients";
            this.liClients.Size = new System.Drawing.Size(120, 420);
            this.liClients.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtReports);
            this.groupBox1.Location = new System.Drawing.Point(16, 315);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(249, 140);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reports";
            // 
            // txtReports
            // 
            this.txtReports.BackColor = System.Drawing.SystemColors.Window;
            this.txtReports.Location = new System.Drawing.Point(6, 19);
            this.txtReports.Multiline = true;
            this.txtReports.Name = "txtReports";
            this.txtReports.ReadOnly = true;
            this.txtReports.Size = new System.Drawing.Size(237, 115);
            this.txtReports.TabIndex = 0;
            // 
            // lable1
            // 
            this.lable1.AutoSize = true;
            this.lable1.Location = new System.Drawing.Point(13, 13);
            this.lable1.Name = "lable1";
            this.lable1.Size = new System.Drawing.Size(32, 13);
            this.lable1.TabIndex = 3;
            this.lable1.Text = "URL:";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(51, 10);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.ReadOnly = true;
            this.txtUrl.Size = new System.Drawing.Size(517, 20);
            this.txtUrl.TabIndex = 4;
            // 
            // btnKick
            // 
            this.btnKick.Location = new System.Drawing.Point(6, 19);
            this.btnKick.Name = "btnKick";
            this.btnKick.Size = new System.Drawing.Size(74, 23);
            this.btnKick.TabIndex = 5;
            this.btnKick.Text = "Kick";
            this.btnKick.UseVisualStyleBackColor = true;
            this.btnKick.Click += new System.EventHandler(this.btnKick_Click);
            // 
            // btnBan
            // 
            this.btnBan.Location = new System.Drawing.Point(93, 19);
            this.btnBan.Name = "btnBan";
            this.btnBan.Size = new System.Drawing.Size(74, 23);
            this.btnBan.TabIndex = 6;
            this.btnBan.Text = "Ban";
            this.btnBan.UseVisualStyleBackColor = true;
            this.btnBan.Click += new System.EventHandler(this.btnBan_Click);
            // 
            // btnKickban
            // 
            this.btnKickban.Location = new System.Drawing.Point(6, 48);
            this.btnKickban.Name = "btnKickban";
            this.btnKickban.Size = new System.Drawing.Size(74, 23);
            this.btnKickban.TabIndex = 7;
            this.btnKickban.Text = "Kickban";
            this.btnKickban.UseVisualStyleBackColor = true;
            this.btnKickban.Click += new System.EventHandler(this.btnKickban_Click);
            // 
            // btnBanIp
            // 
            this.btnBanIp.Location = new System.Drawing.Point(93, 48);
            this.btnBanIp.Name = "btnBanIp";
            this.btnBanIp.Size = new System.Drawing.Size(74, 23);
            this.btnBanIp.TabIndex = 8;
            this.btnBanIp.Text = "BanIP";
            this.btnBanIp.UseVisualStyleBackColor = true;
            this.btnBanIp.Click += new System.EventHandler(this.btnBanIp_Click);
            // 
            // btnChangeRank
            // 
            this.btnChangeRank.Location = new System.Drawing.Point(6, 77);
            this.btnChangeRank.Name = "btnChangeRank";
            this.btnChangeRank.Size = new System.Drawing.Size(161, 23);
            this.btnChangeRank.TabIndex = 9;
            this.btnChangeRank.Text = "Change Rank";
            this.btnChangeRank.UseVisualStyleBackColor = true;
            this.btnChangeRank.Click += new System.EventHandler(this.btnChangeRank_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnKick);
            this.groupBox2.Controls.Add(this.btnChangeRank);
            this.groupBox2.Controls.Add(this.btnBan);
            this.groupBox2.Controls.Add(this.btnBanIp);
            this.groupBox2.Controls.Add(this.btnKickban);
            this.groupBox2.Location = new System.Drawing.Point(271, 315);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(173, 140);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Player Controls";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 462);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Chat:";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(54, 459);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(514, 20);
            this.txtInput.TabIndex = 12;
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyDown);
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 485);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.lable1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.liClients);
            this.Controls.Add(this.txtLog);
            this.Name = "Window";
            this.Text = "Window";
            this.Load += new System.EventHandler(this.Window_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Window_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ListBox liClients;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtReports;
        private System.Windows.Forms.Label lable1;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnKick;
        private System.Windows.Forms.Button btnBan;
        private System.Windows.Forms.Button btnKickban;
        private System.Windows.Forms.Button btnBanIp;
        private System.Windows.Forms.Button btnChangeRank;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInput;
    }
}