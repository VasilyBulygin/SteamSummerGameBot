namespace SteamSummerGameBot
{
    partial class MainForm
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
            this.tbGameId = new System.Windows.Forms.TextBox();
            this.tbAccessToken = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.tbClickCount = new System.Windows.Forms.TextBox();
            this.lblAccessToken = new System.Windows.Forms.Label();
            this.lblGameId = new System.Windows.Forms.Label();
            this.lblClickCount = new System.Windows.Forms.Label();
            this.tbSleepTime = new System.Windows.Forms.TextBox();
            this.lblSleepTime = new System.Windows.Forms.Label();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.lblHpCaption = new System.Windows.Forms.Label();
            this.lblHp = new System.Windows.Forms.Label();
            this.lblSteamId = new System.Windows.Forms.Label();
            this.tbSteamId = new System.Windows.Forms.TextBox();
            this.btnConnectToSpecificGame = new System.Windows.Forms.Button();
            this.btnAddBot = new System.Windows.Forms.Button();
            this.clbBots = new System.Windows.Forms.CheckedListBox();
            this.tbGameIdToConnect = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnLeaveGame = new System.Windows.Forms.Button();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnStartSelected = new System.Windows.Forms.Button();
            this.btnRemoveBot = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSetGameId = new System.Windows.Forms.Button();
            this.btnJoinGame = new System.Windows.Forms.Button();
            this.btnRefreshGameId = new System.Windows.Forms.Button();
            this.btnLeaveSelected = new System.Windows.Forms.Button();
            this.ssStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbGameId
            // 
            this.tbGameId.Location = new System.Drawing.Point(295, 111);
            this.tbGameId.Name = "tbGameId";
            this.tbGameId.Size = new System.Drawing.Size(56, 20);
            this.tbGameId.TabIndex = 0;
            // 
            // tbAccessToken
            // 
            this.tbAccessToken.Location = new System.Drawing.Point(296, 5);
            this.tbAccessToken.Name = "tbAccessToken";
            this.tbAccessToken.Size = new System.Drawing.Size(220, 20);
            this.tbAccessToken.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(409, 139);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(107, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbClickCount
            // 
            this.tbClickCount.Location = new System.Drawing.Point(441, 55);
            this.tbClickCount.Name = "tbClickCount";
            this.tbClickCount.Size = new System.Drawing.Size(75, 20);
            this.tbClickCount.TabIndex = 3;
            this.tbClickCount.Text = "125";
            // 
            // lblAccessToken
            // 
            this.lblAccessToken.AutoSize = true;
            this.lblAccessToken.Location = new System.Drawing.Point(211, 8);
            this.lblAccessToken.Name = "lblAccessToken";
            this.lblAccessToken.Size = new System.Drawing.Size(79, 13);
            this.lblAccessToken.TabIndex = 4;
            this.lblAccessToken.Text = "Access Token:";
            // 
            // lblGameId
            // 
            this.lblGameId.AutoSize = true;
            this.lblGameId.Location = new System.Drawing.Point(212, 114);
            this.lblGameId.Name = "lblGameId";
            this.lblGameId.Size = new System.Drawing.Size(47, 13);
            this.lblGameId.TabIndex = 5;
            this.lblGameId.Text = "GameId:";
            // 
            // lblClickCount
            // 
            this.lblClickCount.AutoSize = true;
            this.lblClickCount.Location = new System.Drawing.Point(377, 58);
            this.lblClickCount.Name = "lblClickCount";
            this.lblClickCount.Size = new System.Drawing.Size(64, 13);
            this.lblClickCount.TabIndex = 6;
            this.lblClickCount.Text = "Click Count:";
            // 
            // tbSleepTime
            // 
            this.tbSleepTime.Location = new System.Drawing.Point(296, 55);
            this.tbSleepTime.Name = "tbSleepTime";
            this.tbSleepTime.Size = new System.Drawing.Size(75, 20);
            this.tbSleepTime.TabIndex = 7;
            this.tbSleepTime.Text = "1000";
            // 
            // lblSleepTime
            // 
            this.lblSleepTime.AutoSize = true;
            this.lblSleepTime.Location = new System.Drawing.Point(211, 58);
            this.lblSleepTime.Name = "lblSleepTime";
            this.lblSleepTime.Size = new System.Drawing.Size(63, 13);
            this.lblSleepTime.TabIndex = 8;
            this.lblSleepTime.Text = "Sleep Time:";
            // 
            // lbLog
            // 
            this.lbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(214, 172);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(369, 173);
            this.lbLog.TabIndex = 9;
            // 
            // lblHpCaption
            // 
            this.lblHpCaption.AutoSize = true;
            this.lblHpCaption.Location = new System.Drawing.Point(517, 115);
            this.lblHpCaption.Name = "lblHpCaption";
            this.lblHpCaption.Size = new System.Drawing.Size(24, 13);
            this.lblHpCaption.TabIndex = 10;
            this.lblHpCaption.Text = "Hp:";
            // 
            // lblHp
            // 
            this.lblHp.AutoSize = true;
            this.lblHp.Location = new System.Drawing.Point(547, 115);
            this.lblHp.Name = "lblHp";
            this.lblHp.Size = new System.Drawing.Size(52, 13);
            this.lblHp.TabIndex = 11;
            this.lblHp.Text = "               ";
            // 
            // lblSteamId
            // 
            this.lblSteamId.AutoSize = true;
            this.lblSteamId.Location = new System.Drawing.Point(212, 32);
            this.lblSteamId.Name = "lblSteamId";
            this.lblSteamId.Size = new System.Drawing.Size(51, 13);
            this.lblSteamId.TabIndex = 12;
            this.lblSteamId.Text = "SteamID:";
            // 
            // tbSteamId
            // 
            this.tbSteamId.Location = new System.Drawing.Point(296, 29);
            this.tbSteamId.Name = "tbSteamId";
            this.tbSteamId.Size = new System.Drawing.Size(220, 20);
            this.tbSteamId.TabIndex = 13;
            // 
            // btnConnectToSpecificGame
            // 
            this.btnConnectToSpecificGame.Location = new System.Drawing.Point(116, 297);
            this.btnConnectToSpecificGame.Name = "btnConnectToSpecificGame";
            this.btnConnectToSpecificGame.Size = new System.Drawing.Size(88, 23);
            this.btnConnectToSpecificGame.TabIndex = 15;
            this.btnConnectToSpecificGame.Text = "Connect";
            this.btnConnectToSpecificGame.UseVisualStyleBackColor = true;
            this.btnConnectToSpecificGame.Click += new System.EventHandler(this.btnConnectToSpecificGame_Click);
            // 
            // btnAddBot
            // 
            this.btnAddBot.Location = new System.Drawing.Point(7, 84);
            this.btnAddBot.Name = "btnAddBot";
            this.btnAddBot.Size = new System.Drawing.Size(19, 20);
            this.btnAddBot.TabIndex = 16;
            this.btnAddBot.Text = "Add";
            this.btnAddBot.UseVisualStyleBackColor = true;
            this.btnAddBot.Click += new System.EventHandler(this.btnAddBot_Click);
            // 
            // clbBots
            // 
            this.clbBots.FormattingEnabled = true;
            this.clbBots.Location = new System.Drawing.Point(32, 32);
            this.clbBots.Name = "clbBots";
            this.clbBots.Size = new System.Drawing.Size(172, 259);
            this.clbBots.TabIndex = 18;
            this.clbBots.SelectedIndexChanged += new System.EventHandler(this.clbBots_SelectedIndexChanged);
            // 
            // tbGameIdToConnect
            // 
            this.tbGameIdToConnect.Location = new System.Drawing.Point(32, 299);
            this.tbGameIdToConnect.Name = "tbGameIdToConnect";
            this.tbGameIdToConnect.Size = new System.Drawing.Size(78, 20);
            this.tbGameIdToConnect.TabIndex = 19;
            this.tbGameIdToConnect.Text = "0";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(296, 81);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(220, 20);
            this.tbName.TabIndex = 20;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(212, 88);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 21;
            this.lblName.Text = "Name:";
            // 
            // btnLeaveGame
            // 
            this.btnLeaveGame.Location = new System.Drawing.Point(215, 139);
            this.btnLeaveGame.Name = "btnLeaveGame";
            this.btnLeaveGame.Size = new System.Drawing.Size(107, 23);
            this.btnLeaveGame.TabIndex = 22;
            this.btnLeaveGame.Text = "Leave game";
            this.btnLeaveGame.UseVisualStyleBackColor = true;
            this.btnLeaveGame.Click += new System.EventHandler(this.btnLeaveGame_Click);
            // 
            // ssStatus
            // 
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slblStatus});
            this.ssStatus.Location = new System.Drawing.Point(0, 346);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(583, 22);
            this.ssStatus.TabIndex = 23;
            this.ssStatus.Text = "statusStrip1";
            // 
            // slblStatus
            // 
            this.slblStatus.Name = "slblStatus";
            this.slblStatus.Size = new System.Drawing.Size(45, 17);
            this.slblStatus.Text = "Готово";
            // 
            // btnStartSelected
            // 
            this.btnStartSelected.Location = new System.Drawing.Point(32, 322);
            this.btnStartSelected.Name = "btnStartSelected";
            this.btnStartSelected.Size = new System.Drawing.Size(172, 23);
            this.btnStartSelected.TabIndex = 24;
            this.btnStartSelected.Text = "Start Selected";
            this.btnStartSelected.UseVisualStyleBackColor = true;
            this.btnStartSelected.Click += new System.EventHandler(this.btnStartSelected_Click);
            // 
            // btnRemoveBot
            // 
            this.btnRemoveBot.Location = new System.Drawing.Point(7, 58);
            this.btnRemoveBot.Name = "btnRemoveBot";
            this.btnRemoveBot.Size = new System.Drawing.Size(19, 20);
            this.btnRemoveBot.TabIndex = 25;
            this.btnRemoveBot.Text = "Del";
            this.btnRemoveBot.UseVisualStyleBackColor = true;
            this.btnRemoveBot.Click += new System.EventHandler(this.btnRemoveBot_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(7, 32);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(19, 20);
            this.btnEdit.TabIndex = 26;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnSetGameId
            // 
            this.btnSetGameId.Location = new System.Drawing.Point(355, 111);
            this.btnSetGameId.Name = "btnSetGameId";
            this.btnSetGameId.Size = new System.Drawing.Size(25, 21);
            this.btnSetGameId.TabIndex = 27;
            this.btnSetGameId.Text = "Set";
            this.btnSetGameId.UseVisualStyleBackColor = true;
            this.btnSetGameId.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnJoinGame
            // 
            this.btnJoinGame.Location = new System.Drawing.Point(384, 111);
            this.btnJoinGame.Name = "btnJoinGame";
            this.btnJoinGame.Size = new System.Drawing.Size(24, 21);
            this.btnJoinGame.TabIndex = 28;
            this.btnJoinGame.Text = "Join game";
            this.btnJoinGame.UseVisualStyleBackColor = true;
            this.btnJoinGame.Click += new System.EventHandler(this.btnJoinGame_Click);
            // 
            // btnRefreshGameId
            // 
            this.btnRefreshGameId.Location = new System.Drawing.Point(412, 111);
            this.btnRefreshGameId.Name = "btnRefreshGameId";
            this.btnRefreshGameId.Size = new System.Drawing.Size(23, 21);
            this.btnRefreshGameId.TabIndex = 29;
            this.btnRefreshGameId.Text = "Refresh";
            this.btnRefreshGameId.UseVisualStyleBackColor = true;
            this.btnRefreshGameId.Click += new System.EventHandler(this.btnRefreshGameId_Click);
            // 
            // btnLeaveSelected
            // 
            this.btnLeaveSelected.Location = new System.Drawing.Point(32, 2);
            this.btnLeaveSelected.Name = "btnLeaveSelected";
            this.btnLeaveSelected.Size = new System.Drawing.Size(172, 23);
            this.btnLeaveSelected.TabIndex = 30;
            this.btnLeaveSelected.Text = "Leave Selected";
            this.btnLeaveSelected.UseVisualStyleBackColor = true;
            this.btnLeaveSelected.Click += new System.EventHandler(this.btnLeaveSelected_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 368);
            this.Controls.Add(this.btnLeaveSelected);
            this.Controls.Add(this.btnRefreshGameId);
            this.Controls.Add(this.btnJoinGame);
            this.Controls.Add(this.btnSetGameId);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnRemoveBot);
            this.Controls.Add(this.btnStartSelected);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.btnLeaveGame);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.tbGameIdToConnect);
            this.Controls.Add(this.clbBots);
            this.Controls.Add(this.btnAddBot);
            this.Controls.Add(this.btnConnectToSpecificGame);
            this.Controls.Add(this.tbSteamId);
            this.Controls.Add(this.lblSteamId);
            this.Controls.Add(this.lblHp);
            this.Controls.Add(this.lblHpCaption);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.lblSleepTime);
            this.Controls.Add(this.tbSleepTime);
            this.Controls.Add(this.lblClickCount);
            this.Controls.Add(this.lblGameId);
            this.Controls.Add(this.lblAccessToken);
            this.Controls.Add(this.tbClickCount);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.tbAccessToken);
            this.Controls.Add(this.tbGameId);
            this.Name = "MainForm";
            this.Text = "GameBot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fMainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fMainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbGameId;
        private System.Windows.Forms.TextBox tbAccessToken;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox tbClickCount;
        private System.Windows.Forms.Label lblAccessToken;
        private System.Windows.Forms.Label lblGameId;
        private System.Windows.Forms.Label lblClickCount;
        private System.Windows.Forms.TextBox tbSleepTime;
        private System.Windows.Forms.Label lblSleepTime;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.Label lblHpCaption;
        private System.Windows.Forms.Label lblHp;
        private System.Windows.Forms.Label lblSteamId;
        private System.Windows.Forms.TextBox tbSteamId;
        private System.Windows.Forms.Button btnConnectToSpecificGame;
        private System.Windows.Forms.Button btnAddBot;
        private System.Windows.Forms.CheckedListBox clbBots;
        private System.Windows.Forms.TextBox tbGameIdToConnect;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnLeaveGame;
        private System.Windows.Forms.StatusStrip ssStatus;
        private System.Windows.Forms.ToolStripStatusLabel slblStatus;
        private System.Windows.Forms.Button btnStartSelected;
        private System.Windows.Forms.Button btnRemoveBot;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSetGameId;
        private System.Windows.Forms.Button btnJoinGame;
        private System.Windows.Forms.Button btnRefreshGameId;
        private System.Windows.Forms.Button btnLeaveSelected;
    }
}

