namespace Lab03.Bai05;

internal sealed class ExerciseLauncher : Form
{
    private readonly TextBox serverAddress = new() { Text = "127.0.0.1", Width = 210 };
    private readonly NumericUpDown port = new() { Minimum = 1, Maximum = 65535, Value = 8085, Width = 100 };
    private readonly Label status = new() { AutoSize = true, Text = "SQLite tập trung tại server · TCP/8085", ForeColor = Color.DimGray };

    public ExerciseLauncher()
    {
        Text = "Lab 3 · Bài 5 · Hôm nay ăn gì?";
        StartPosition = FormStartPosition.CenterScreen; ClientSize = new Size(510, 260);
        FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false;
        var title = new Label { Text = "BÀI 5 · HÔM NAY ĂN GÌ? · TCP", Font = new Font("Segoe UI", 15, FontStyle.Bold), AutoSize = true, Location = new Point(30, 25) };
        var addressLabel = new Label { Text = "IP máy chủ", AutoSize = true, Location = new Point(34, 90) };
        serverAddress.Location = new Point(120, 86);
        var portLabel = new Label { Text = "Cổng", AutoSize = true, Location = new Point(347, 90) };
        port.Location = new Point(390, 86);
        var serverButton = new Button { Text = "MỞ SERVER", Location = new Point(34, 140), Size = new Size(205, 48) };
        var clientButton = new Button { Text = "MỞ CLIENT", Location = new Point(258, 140), Size = new Size(205, 48) };
        serverButton.Click += (_, _) => new FoodServerWindow((int)port.Value).Show(this);
        clientButton.Click += (_, _) =>
        {
            Lab02.Bai06Data.FoodNetworkOptions.Host = serverAddress.Text.Trim();
            Lab02.Bai06Data.FoodNetworkOptions.Port = (int)port.Value;
            Lab02.Bai06Data.FoodNetworkOptions.IsClient = true;
            try
            {
                using var probe = new System.Net.Sockets.TcpClient();
                probe.Connect(Lab02.Bai06Data.FoodNetworkOptions.Host, Lab02.Bai06Data.FoodNetworkOptions.Port);
                new Lab02.Lab02_Bai06().Show(this);
                status.Text = "Đã kết nối tới " + Lab02.Bai06Data.FoodNetworkOptions.Host + ":" + Lab02.Bai06Data.FoodNetworkOptions.Port;
            }
            catch (Exception ex) { MessageBox.Show(this, ex.Message, "Không kết nối được", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        };
        status.Location = new Point(34, 210);
        Controls.AddRange(new Control[] { title, addressLabel, serverAddress, portLabel, port, serverButton, clientButton, status });
    }
}
