namespace Bai1_Lab3
{
    partial class FormUDPClient
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
            lblIP = new Label();
            tbIP = new MaskedTextBox();
            lblPort = new Label();
            tbPort = new MaskedTextBox();
            lblMessage = new Label();
            btnSend = new Button();
            rtbMessage = new RichTextBox();
            SuspendLayout();
            // 
            // lblIP
            // 
            lblIP.AutoSize = true;
            lblIP.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblIP.Location = new Point(127, 37);
            lblIP.Name = "lblIP";
            lblIP.Size = new Size(133, 25);
            lblIP.TabIndex = 1;
            lblIP.Text = "IP Remote host";
            // 
            // tbIP
            // 
            tbIP.Location = new Point(49, 74);
            tbIP.Name = "tbIP";
            tbIP.Size = new Size(301, 27);
            tbIP.TabIndex = 2;
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPort.Location = new Point(587, 37);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(44, 25);
            lblPort.TabIndex = 3;
            lblPort.Text = "Port";
            // 
            // tbPort
            // 
            tbPort.Location = new Point(450, 74);
            tbPort.Name = "tbPort";
            tbPort.Size = new Size(301, 27);
            tbPort.TabIndex = 4;
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMessage.Location = new Point(49, 126);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(82, 25);
            lblMessage.TabIndex = 5;
            lblMessage.Text = "Message";
            // 
            // btnSend
            // 
            btnSend.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSend.Location = new Point(48, 396);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(189, 42);
            btnSend.TabIndex = 7;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // rtbMessage
            // 
            rtbMessage.Location = new Point(49, 165);
            rtbMessage.Name = "rtbMessage";
            rtbMessage.Size = new Size(702, 212);
            rtbMessage.TabIndex = 8;
            rtbMessage.Text = "";
            // 
            // FormUDPClient
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(rtbMessage);
            Controls.Add(btnSend);
            Controls.Add(lblMessage);
            Controls.Add(tbPort);
            Controls.Add(lblPort);
            Controls.Add(tbIP);
            Controls.Add(lblIP);
            Name = "FormUDPClient";
            Text = "UDP Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblIP;
        private MaskedTextBox tbIP;
        private Label lblPort;
        private MaskedTextBox tbPort;
        private Label lblMessage;
        private Button btnSend;
        private RichTextBox rtbMessage;
    }
}