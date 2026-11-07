namespace Bai1_Lab3
{
    partial class FormDB
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnUDPServer = new Button();
            btnUDPClient = new Button();
            SuspendLayout();
            // 
            // btnUDPServer
            // 
            btnUDPServer.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUDPServer.Location = new Point(79, 108);
            btnUDPServer.Name = "btnUDPServer";
            btnUDPServer.Size = new Size(172, 81);
            btnUDPServer.TabIndex = 0;
            btnUDPServer.Text = "UDP Server";
            btnUDPServer.UseVisualStyleBackColor = true;
            btnUDPServer.Click += btnUDPServer_Click;
            // 
            // btnUDPClient
            // 
            btnUDPClient.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUDPClient.Location = new Point(399, 108);
            btnUDPClient.Name = "btnUDPClient";
            btnUDPClient.Size = new Size(172, 81);
            btnUDPClient.TabIndex = 1;
            btnUDPClient.Text = "UDP Client";
            btnUDPClient.UseVisualStyleBackColor = true;
            btnUDPClient.Click += btnUDPClient_Click;
            // 
            // FormDB
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(659, 306);
            Controls.Add(btnUDPClient);
            Controls.Add(btnUDPServer);
            Name = "FormDB";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dashboard";
            ResumeLayout(false);
        }

        #endregion

        private Button btnUDPServer;
        private Button btnUDPClient;
    }
}
