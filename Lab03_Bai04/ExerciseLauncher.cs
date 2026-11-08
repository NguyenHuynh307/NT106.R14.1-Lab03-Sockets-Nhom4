using System.Net;
using System.Net.Sockets;

namespace Lab03.Bai04;

/// <summary>Màn hình khởi chạy: nhập IP/cổng rồi mở Server hoặc Client. Bố cục co giãn theo DPI nên không bị che chữ.</summary>
internal sealed class ExerciseLauncher : Form
{
    private readonly TextBox serverAddress = new() { Text = "127.0.0.1", Anchor = AnchorStyles.Left | AnchorStyles.Right };
    private readonly NumericUpDown port = new() { Minimum = 1024, Maximum = 65535, Value = 8084, Width = 120, Anchor = AnchorStyles.Left };
    private readonly Label status = new() { AutoSize = true, Text = "TCP · mặc định cổng 8084", ForeColor = Color.DimGray, Anchor = AnchorStyles.Left };

    public ExerciseLauncher()
    {
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = new Font("Segoe UI", 10F);
        Text = "Lab 3 · Bài 4 · Quản lý vé xem phim";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        ClientSize = new Size(620, 320);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(24, 18, 24, 18),
            ColumnCount = 4,
            RowCount = 4
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var title = new Label { Text = "BÀI 4 · ĐẶT VÉ PHIM QUA TCP", Font = new Font("Segoe UI", 15F, FontStyle.Bold), AutoSize = true, Anchor = AnchorStyles.Left };
        layout.Controls.Add(title, 0, 0);
        layout.SetColumnSpan(title, 4);

        layout.Controls.Add(new Label { Text = "IP máy chủ", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 3, 12, 3) }, 0, 1);
        layout.Controls.Add(serverAddress, 1, 1);
        layout.Controls.Add(new Label { Text = "Cổng", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(16, 3, 8, 3) }, 2, 1);
        layout.Controls.Add(port, 3, 1);

        var serverButton = new Button { Text = "MỞ SERVER", Dock = DockStyle.Fill, Margin = new Padding(3, 8, 8, 8) };
        var clientButton = new Button { Text = "MỞ CLIENT", Dock = DockStyle.Fill, Margin = new Padding(8, 8, 3, 8) };
        layout.Controls.Add(serverButton, 0, 2);
        layout.SetColumnSpan(serverButton, 2);
        layout.Controls.Add(clientButton, 2, 2);
        layout.SetColumnSpan(clientButton, 2);

        layout.Controls.Add(status, 0, 3);
        layout.SetColumnSpan(status, 4);
        Controls.Add(layout);

        serverButton.Click += (_, _) =>
        {
            try
            {
                // Server mở thành cửa sổ riêng, không gắn owner để có thể chuyển qua lại với Client
                new SeatServerWindow((int)port.Value).Show();
                status.Text = "Server đã chạy ở cổng " + (int)port.Value + ". Client máy khác nhập IP LAN hiển thị trong cửa sổ Server.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Không mở được Server (cổng đang bị chương trình khác dùng?):\r\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        clientButton.Click += (_, _) =>
        {
            string host = serverAddress.Text.Trim();
            if (host.Length == 0)
            {
                MessageBox.Show(this, "Vui lòng nhập IP của máy chạy Server.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int serverPort = (int)port.Value;
            UseWaitCursor = true;
            try
            {
                SeatClientSettings.Host = host;
                SeatClientSettings.Port = serverPort;
                _ = SeatRemote.GetSnapshot();   // thử nối trước, lỗi sẽ báo ngay thay vì mở form rỗng
                new Lab02.Lab02_Bai05().Show();
                status.Text = "Đã kết nối tới " + host + ":" + serverPort;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Không kết nối được", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
            }
        };
    }
}
