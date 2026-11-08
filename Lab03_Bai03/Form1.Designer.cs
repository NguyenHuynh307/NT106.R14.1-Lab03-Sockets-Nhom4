namespace Bai3
{
    partial class Form1
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
            btnOpenServer = new Button();
            btnOpenClient = new Button();
            SuspendLayout();
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = UiKit.Ink;
            lblTitle.Location = new Point(32, 24);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Bài 3 · TCP 1 Server - 1 Client";
            //
            // lblHint
            //
            lblHint.AutoSize = true;
            lblHint.Font = new Font("Segoe UI", 10F);
            lblHint.ForeColor = UiKit.Muted;
            lblHint.Location = new Point(34, 66);
            lblHint.Name = "lblHint";
            lblHint.Text = "Mở Server và bấm Listen trước, sau đó mở Client để kết nối (cổng 8080).";
            //
            // btnOpenServer
            //
            btnOpenServer.Location = new Point(34, 118);
            btnOpenServer.Name = "btnOpenServer";
            btnOpenServer.Size = new Size(236, 64);
            btnOpenServer.TabIndex = 0;
            btnOpenServer.Text = "Mở TCP Server";
            btnOpenServer.Click += btnOpenServer_Click;
            UiKit.Primary(btnOpenServer);
            //
            // btnOpenClient
            //
            btnOpenClient.Location = new Point(290, 118);
            btnOpenClient.Name = "btnOpenClient";
            btnOpenClient.Size = new Size(236, 64);
            btnOpenClient.TabIndex = 1;
            btnOpenClient.Text = "Mở TCP Client";
            btnOpenClient.Click += btnOpenClient_Click;
            UiKit.Secondary(btnOpenClient);
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = UiKit.Page;
            ClientSize = new Size(560, 220);
            Controls.Add(lblTitle);
            Controls.Add(lblHint);
            Controls.Add(btnOpenServer);
            Controls.Add(btnOpenClient);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bài 3 · TCP";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblHint;
        private Button btnOpenServer;
        private Button btnOpenClient;
    }
}
