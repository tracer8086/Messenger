namespace MessengerClient.GUI.Classes
{
    partial class ChatForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatForm));
            this.chatWindow = new System.Windows.Forms.RichTextBox();
            this.userList = new System.Windows.Forms.ListView();
            this.messageBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.logOutButton = new System.Windows.Forms.Button();
            this.enableMicrophoneButton = new System.Windows.Forms.Button();
            this.disableMicrophoneButton = new System.Windows.Forms.Button();
            this.enableOutputButton = new System.Windows.Forms.Button();
            this.disableOutputButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chatWindow
            // 
            this.chatWindow.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.chatWindow.Location = new System.Drawing.Point(39, 31);
            this.chatWindow.Name = "chatWindow";
            this.chatWindow.ReadOnly = true;
            this.chatWindow.Size = new System.Drawing.Size(597, 213);
            this.chatWindow.TabIndex = 0;
            this.chatWindow.Text = "";
            // 
            // userList
            // 
            this.userList.Location = new System.Drawing.Point(654, 31);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(121, 213);
            this.userList.TabIndex = 1;
            this.userList.UseCompatibleStateImageBehavior = false;
            this.userList.View = System.Windows.Forms.View.List;
            // 
            // messageBox
            // 
            this.messageBox.Location = new System.Drawing.Point(39, 260);
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(736, 20);
            this.messageBox.TabIndex = 2;
            this.messageBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.messageBox_KeyPress);
            // 
            // sendButton
            // 
            this.sendButton.BackColor = System.Drawing.SystemColors.HotTrack;
            this.sendButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.sendButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.sendButton.Location = new System.Drawing.Point(39, 296);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 23);
            this.sendButton.TabIndex = 3;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = false;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // logOutButton
            // 
            this.logOutButton.BackColor = System.Drawing.SystemColors.HotTrack;
            this.logOutButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.logOutButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.logOutButton.Location = new System.Drawing.Point(700, 296);
            this.logOutButton.Name = "logOutButton";
            this.logOutButton.Size = new System.Drawing.Size(75, 23);
            this.logOutButton.TabIndex = 4;
            this.logOutButton.Text = "Log out";
            this.logOutButton.UseVisualStyleBackColor = false;
            this.logOutButton.Click += new System.EventHandler(this.logOutButton_Click);
            // 
            // enableMicrophoneButton
            // 
            this.enableMicrophoneButton.BackColor = System.Drawing.SystemColors.HotTrack;
            this.enableMicrophoneButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.enableMicrophoneButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.enableMicrophoneButton.Location = new System.Drawing.Point(244, 295);
            this.enableMicrophoneButton.Name = "enableMicrophoneButton";
            this.enableMicrophoneButton.Size = new System.Drawing.Size(108, 23);
            this.enableMicrophoneButton.TabIndex = 5;
            this.enableMicrophoneButton.Text = "Enable microphone";
            this.enableMicrophoneButton.UseVisualStyleBackColor = false;
            this.enableMicrophoneButton.Click += new System.EventHandler(this.enableMicrophoneButton_Click);
            // 
            // disableMicrophoneButton
            // 
            this.disableMicrophoneButton.BackColor = System.Drawing.SystemColors.HotTrack;
            this.disableMicrophoneButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.disableMicrophoneButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.disableMicrophoneButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.disableMicrophoneButton.Location = new System.Drawing.Point(455, 295);
            this.disableMicrophoneButton.Name = "disableMicrophoneButton";
            this.disableMicrophoneButton.Size = new System.Drawing.Size(113, 23);
            this.disableMicrophoneButton.TabIndex = 6;
            this.disableMicrophoneButton.Text = "Disable microphone";
            this.disableMicrophoneButton.UseVisualStyleBackColor = false;
            this.disableMicrophoneButton.Click += new System.EventHandler(this.disableMicrophoneButton_Click);
            // 
            // enableOutputButton
            // 
            this.enableOutputButton.BackColor = System.Drawing.SystemColors.HotTrack;
            this.enableOutputButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.enableOutputButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.enableOutputButton.Location = new System.Drawing.Point(244, 324);
            this.enableOutputButton.Name = "enableOutputButton";
            this.enableOutputButton.Size = new System.Drawing.Size(108, 23);
            this.enableOutputButton.TabIndex = 7;
            this.enableOutputButton.Text = "Enable sound";
            this.enableOutputButton.UseVisualStyleBackColor = false;
            this.enableOutputButton.Click += new System.EventHandler(this.enableOutputButton_Click);
            // 
            // disableOutputButton
            // 
            this.disableOutputButton.BackColor = System.Drawing.SystemColors.HotTrack;
            this.disableOutputButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.disableOutputButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.disableOutputButton.Location = new System.Drawing.Point(455, 324);
            this.disableOutputButton.Name = "disableOutputButton";
            this.disableOutputButton.Size = new System.Drawing.Size(113, 23);
            this.disableOutputButton.TabIndex = 8;
            this.disableOutputButton.Text = "Disable sound";
            this.disableOutputButton.UseVisualStyleBackColor = false;
            this.disableOutputButton.Click += new System.EventHandler(this.disableOutputButton_Click);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(813, 376);
            this.Controls.Add(this.disableOutputButton);
            this.Controls.Add(this.enableOutputButton);
            this.Controls.Add(this.disableMicrophoneButton);
            this.Controls.Add(this.enableMicrophoneButton);
            this.Controls.Add(this.logOutButton);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.messageBox);
            this.Controls.Add(this.userList);
            this.Controls.Add(this.chatWindow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChatForm";
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox chatWindow;
        private System.Windows.Forms.ListView userList;
        private System.Windows.Forms.TextBox messageBox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Button logOutButton;
        private System.Windows.Forms.Button enableMicrophoneButton;
        private System.Windows.Forms.Button disableMicrophoneButton;
        private System.Windows.Forms.Button enableOutputButton;
        private System.Windows.Forms.Button disableOutputButton;
    }
}