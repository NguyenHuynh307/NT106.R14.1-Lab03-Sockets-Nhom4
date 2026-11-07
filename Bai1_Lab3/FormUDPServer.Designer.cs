namespace Bai1_Lab3
{
    partial class FormUDPServer
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
            lblPort = new Label();
            tbPort = new MaskedTextBox();
            btnListen = new Button();
            lblReceivedMessages = new Label();
            listViewReceivedMessages = new ListView();
            SuspendLayout();
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPort.Location = new Point(47, 39);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(44, 25);
            lblPort.TabIndex = 0;
            lblPort.Text = "Port";
            // 
            // tbPort
            // 
            tbPort.Location = new Point(97, 40);
            tbPort.Name = "tbPort";
            tbPort.Size = new Size(204, 27);
            tbPort.TabIndex = 1;
            // 
            // btnListen
            // 
            btnListen.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnListen.Location = new Point(594, 40);
            btnListen.Name = "btnListen";
            btnListen.Size = new Size(159, 54);
            btnListen.TabIndex = 2;
            btnListen.Text = "Listen";
            btnListen.UseVisualStyleBackColor = true;
            btnListen.Click += btnListen_Click;
            // 
            // lblReceivedMessages
            // 
            lblReceivedMessages.AutoSize = true;
            lblReceivedMessages.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblReceivedMessages.Location = new Point(47, 87);
            lblReceivedMessages.Name = "lblReceivedMessages";
            lblReceivedMessages.Size = new Size(164, 25);
            lblReceivedMessages.TabIndex = 3;
            lblReceivedMessages.Text = "Received messages";
            // 
            // listViewReceivedMessages
            // 
            listViewReceivedMessages.Location = new Point(47, 135);
            listViewReceivedMessages.Name = "listViewReceivedMessages";
            listViewReceivedMessages.Size = new Size(704, 284);
            listViewReceivedMessages.TabIndex = 4;
            listViewReceivedMessages.UseCompatibleStateImageBehavior = false;
            // 
            // FormUDPServer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 452);
            Controls.Add(listViewReceivedMessages);
            Controls.Add(lblReceivedMessages);
            Controls.Add(btnListen);
            Controls.Add(tbPort);
            Controls.Add(lblPort);
            MaximizeBox = false;
            Name = "FormUDPServer";
            Text = "UDP Server";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPort;
        private MaskedTextBox tbPort;
        private Button btnListen;
        private Label lblReceivedMessages;
        private ListView listViewReceivedMessages;
    }
}