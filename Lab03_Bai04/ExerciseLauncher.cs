namespace Lab03.Bai04;

internal sealed class ExerciseLauncher : Form
{
    private readonly TextBox serverAddress = new() { Text = "127.0.0.1", Width = 210 };
    private readonly NumericUpDown port = new() { Minimum = 1, Maximum = 65535, Value = 8084, Width = 100 };
    private readonly Label status = new() { AutoSize = true, Text = "TCP · mặc định cổng 8084", ForeColor = Color.DimGray };

    public ExerciseLauncher()
    {
        Text = "Lab 3 · Bài 4 · Quản lý vé xem phim";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(510, 260);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var title = new Label { Text = "BÀI 4 · ĐẶT VÉ PHIM QUA TCP", Font = new Font("Segoe UI", 15, FontStyle.Bold), AutoSize = true, Location = new Point(30, 25) };
        var addressLabel = new Label { Text = "IP máy chủ", AutoSize = true, Location = new Point(34, 90) };
        serverAddress.Location = new Point(120, 86);
        var portLabel = new Label { Text = "Cổng", AutoSize = true, Location = new Point(347, 90) };
        port.Location = new Point(390, 86);
        var serverButton = new Button { Text = "MỞ SERVER", Location = new Point(34, 140), Size = new Size(205, 48) };
        var clientButton = new Button { Text = "MỞ CLIENT", Location = new Point(258, 140), Size = new Size(205, 48) };
        serverButton.Click += (_, _) => new SeatServerWindow((int)port.Value).Show(this);
        clientButton.Click += (_, _) =>
        {
            SeatClientSettings.Host = serverAddress.Text.Trim();
            SeatClientSettings.Port = (int)port.Value;
            try
            {
                _ = SeatRemote.GetSnapshot();
                new Lab02.Lab02_Bai05().Show(this);
                status.Text = "Đã kết nối tới " + SeatClientSettings.Host + ":" + SeatClientSettings.Port;
            }
            catch (Exception ex) { MessageBox.Show(this, ex.Message, "Không kết nối được", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        };
        status.Location = new Point(34, 210);
        Controls.AddRange(new Control[] { title, addressLabel, serverAddress, portLabel, port, serverButton, clientButton, status });
    }
}
