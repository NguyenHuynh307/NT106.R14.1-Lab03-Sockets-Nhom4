namespace Bai1_Lab3
{
    partial class FormDB
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblHint = new Label();
            btnUDPServer = new Button();
            btnUDPClient = new Button();
            SuspendLayout();
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = UiKit.Ink;
            lblTitle.Location = new Point(32, 24);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Bài 1 · UDP Client - Server";
            //
            // lblHint
            //
            lblHint.AutoSize = true;
            lblHint.Font = new Font("Segoe UI", 10F);
            lblHint.ForeColor = UiKit.Muted;
            lblHint.Location = new Point(34, 66);
            lblHint.Name = "lblHint";
            lblHint.Text = "Mở Server trước, sau đó mở Client để gửi tin nhắn.";
            //
            // btnUDPServer
            //
            btnUDPServer.Location = new Point(34, 118);
            btnUDPServer.Name = "btnUDPServer";
            btnUDPServer.Size = new Size(236, 64);
            btnUDPServer.TabIndex = 0;
            btnUDPServer.Text = "Mở UDP Server";
            btnUDPServer.Click += btnUDPServer_Click;
            UiKit.Primary(btnUDPServer);
            //
            // btnUDPClient
            //
            btnUDPClient.Location = new Point(290, 118);
            btnUDPClient.Name = "btnUDPClient";
            btnUDPClient.Size = new Size(236, 64);
            btnUDPClient.TabIndex = 1;
            btnUDPClient.Text = "Mở UDP Client";
            btnUDPClient.Click += btnUDPClient_Click;
            UiKit.Secondary(btnUDPClient);
            //
            // FormDB
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = UiKit.Page;
            ClientSize = new Size(560, 220);
            Controls.Add(lblTitle);
            Controls.Add(lblHint);
            Controls.Add(btnUDPServer);
            Controls.Add(btnUDPClient);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "FormDB";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bài 1 · UDP";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblHint;
        private Button btnUDPServer;
        private Button btnUDPClient;
    }
}
