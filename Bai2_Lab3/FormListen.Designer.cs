namespace Bai2_Lab3
{
    partial class FormListen
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
            btnListen = new Button();
            listViewMessage = new ListView();
            SuspendLayout();
            // 
            // btnListen
            // 
            btnListen.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnListen.Location = new Point(590, 28);
            btnListen.Name = "btnListen";
            btnListen.Size = new Size(167, 45);
            btnListen.TabIndex = 0;
            btnListen.Text = "Listen";
            btnListen.UseVisualStyleBackColor = true;
            btnListen.Click += btnListen_Click;
            // 
            // listViewMessage
            // 
            listViewMessage.Location = new Point(51, 95);
            listViewMessage.Name = "listViewMessage";
            listViewMessage.Size = new Size(706, 337);
            listViewMessage.TabIndex = 1;
            listViewMessage.UseCompatibleStateImageBehavior = false;
            // 
            // FormListen
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 464);
            Controls.Add(listViewMessage);
            Controls.Add(btnListen);
            Name = "FormListen";
            Text = "Lab03_Bai02";
            ResumeLayout(false);
        }

        #endregion

        private Button btnListen;
        private ListView listViewMessage;
    }
}
