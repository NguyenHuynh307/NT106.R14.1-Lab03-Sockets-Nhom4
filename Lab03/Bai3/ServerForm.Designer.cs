namespace Bai3
{
    partial class ServerForm
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
            btnListen = new Button();
            listViewCommand = new ListView();
            columnHeader1 = new ColumnHeader();
            SuspendLayout();
            // 
            // btnListen
            // 
            btnListen.Location = new Point(679, 23);
            btnListen.Name = "btnListen";
            btnListen.Size = new Size(109, 60);
            btnListen.TabIndex = 0;
            btnListen.Text = "Listen";
            btnListen.UseVisualStyleBackColor = true;
            btnListen.Click += btnListen_Click;
            // 
            // listViewCommand
            // 
            listViewCommand.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            listViewCommand.FullRowSelect = true;
            listViewCommand.GridLines = true;
            listViewCommand.Location = new Point(69, 78);
            listViewCommand.Name = "listViewCommand";
            listViewCommand.Size = new Size(614, 341);
            listViewCommand.TabIndex = 1;
            listViewCommand.UseCompatibleStateImageBehavior = false;
            listViewCommand.View = View.Details;
            listViewCommand.SelectedIndexChanged += listViewCommand_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Thông báo từ Server";
            columnHeader1.Width = 550;
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(listViewCommand);
            Controls.Add(btnListen);
            Name = "ServerForm";
            Text = "ServerForm";
            Load += ServerForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnListen;
        private ListView listViewCommand;
        private ColumnHeader columnHeader1;
    }
}