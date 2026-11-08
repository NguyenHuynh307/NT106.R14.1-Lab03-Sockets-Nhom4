using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Lab03.Menu;

/// <summary>Một bài thực hành hiển thị trong menu và cách tìm file .exe đã build của bài đó.</summary>
internal sealed class BaiEntry
{
    public BaiEntry(int number, string title, string protocol, string owner, string description, Color accent, params string[] exeNames)
    {
        Number = number;
        Title = title;
        Protocol = protocol;
        Owner = owner;
        Description = description;
        Accent = accent;
        ExeNames = exeNames;
    }

    public int Number { get; }
    public string Title { get; }
    public string Protocol { get; }
    public string Owner { get; }
    public string Description { get; }
    public Color Accent { get; }
    public string[] ExeNames { get; }

    /// <summary>Khớp tên file .exe theo danh sách tên đã biết, hoặc theo mẫu "Bai1", "Bai01"... đứng riêng một số.</summary>
    public bool Matches(string fileName)
    {
        if (ExeNames.Contains(fileName, StringComparer.OrdinalIgnoreCase))
        {
            return true;
        }
        return System.Text.RegularExpressions.Regex.IsMatch(fileName, "bai0?" + Number + "(?!\\d)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    }
}

/// <summary>
/// Menu chính của Lab 3: mỗi thẻ mở một bài (mỗi bài là một project riêng).
/// Menu chạy file .exe của từng project nên không phụ thuộc vào mã nguồn của bài đó.
/// </summary>
internal sealed class MenuForm : Form
{
    private static readonly Color Page = Color.FromArgb(244, 246, 249);
    private static readonly Color Ink = Color.FromArgb(32, 41, 56);
    private static readonly Color Muted = Color.FromArgb(100, 112, 130);
    private static readonly Color Line = Color.FromArgb(218, 223, 231);
    private static readonly Color Good = Color.FromArgb(22, 128, 80);
    private static readonly Color Warn = Color.FromArgb(190, 110, 10);

    // Tên file .exe (không có đuôi) có thể gặp của từng bài; thêm tên mới vào đây nếu project được đặt tên khác.
    private static readonly BaiEntry[] Entries =
    {
        new BaiEntry(1, "Bài 1 · UDP Client - Server", "UDP", "Nguyễn Quốc Nhật Minh",
            "Gửi và nhận tin nhắn qua UDP, có hỗ trợ tiếng Việt.", Color.FromArgb(37, 99, 160), "Lab03_Bai01", "Bai1_Lab3"),
        new BaiEntry(2, "Bài 2 · Lắng nghe Telnet", "TCP · cổng 8080", "Nguyễn Quốc Nhật Minh",
            "Server lắng nghe và hiển thị từng dòng lệnh telnet gửi tới.", Color.FromArgb(14, 134, 140), "Lab03_Bai02", "Bai2_Lab3"),
        new BaiEntry(3, "Bài 3 · TCP 1 Server - 1 Client", "TCP · cổng 8080", "Hoàng Anh",
            "Client có 3 chức năng: Kết nối, Gửi tin và Ngắt kết nối.", Color.FromArgb(46, 125, 80), "Lab03_Bai03", "Bai3_Lab3"),
        new BaiEntry(4, "Bài 4 · Quản lý phòng vé", "TCP · cổng 8084", "Huỳnh Thanh Nguyên",
            "Nhiều quầy cùng đặt vé, ghế đã đặt ở quầy này sẽ khóa ở các quầy khác.", Color.FromArgb(200, 120, 20), "Lab03_Bai04"),
        new BaiEntry(5, "Bài 5 · Hôm nay ăn gì?", "TCP · cổng 8085 · SQLite", "Huỳnh Thanh Nguyên",
            "Chọn món ăn ngẫu nhiên, dữ liệu món ăn được lưu bằng SQLite tại Server.", Color.FromArgb(150, 80, 160), "Lab03_Bai05"),
        new BaiEntry(6, "Bài 6 · Chat Room", "TCP · cổng 8080", "Hoàng Anh",
            "Chat chung, nhắn riêng, gửi ảnh (.jpg, .png) và file (.txt).", Color.FromArgb(190, 65, 100), "Lab03_Bai06", "Bai6_Lab3"),
    };

    private readonly string solutionRoot;
    private readonly Dictionary<int, Label> statusLabels = new Dictionary<int, Label>();

    public MenuForm()
    {
        solutionRoot = FindSolutionRoot();

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = new Font("Segoe UI", 10F);
        Text = "Lab 3 · Lập trình Socket trong C#";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(980, 620);
        MinimumSize = new Size(860, 560);
        BackColor = Page;

        Controls.Add(BuildBody());
        Controls.Add(BuildHeader());
        Controls.Add(BuildFooter());

        RefreshStatuses();
    }

    private Control BuildHeader()
    {
        Panel header = new Panel { Dock = DockStyle.Top, Height = 84, BackColor = Color.White };
        Label title = new Label
        {
            Text = "Lab 3 · Lập trình Socket trong C#",
            AutoSize = true,
            ForeColor = Ink,
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            Location = new Point(28, 12)
        };
        Label sub = new Label
        {
            Text = "NT106.R14.1 · Nhóm 4 · Chọn một bài để mở",
            AutoSize = true,
            ForeColor = Muted,
            Location = new Point(30, 52)
        };
        header.Controls.Add(title);
        header.Controls.Add(sub);
        header.Paint += (sender, e) =>
        {
            using (Pen pen = new Pen(Line))
            {
                e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
            }
        };
        return header;
    }

    private Control BuildFooter()
    {
        Panel footer = new Panel { Dock = DockStyle.Bottom, Height = 64, BackColor = Color.White };

        Label ip = new Label
        {
            Text = "IP máy này: " + LocalAddresses(),
            AutoSize = true,
            ForeColor = Ink,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Location = new Point(28, 10)
        };
        Label hint = new Label
        {
            Text = "Demo LAN: máy làm Server mở bài trước, máy làm Client nhập IP máy Server.",
            AutoSize = true,
            ForeColor = Muted,
            Font = new Font("Segoe UI", 9F),
            Location = new Point(28, 34)
        };

        Button refresh = new Button { Text = "Kiểm tra lại", Size = new Size(130, 36), Anchor = AnchorStyles.Top | AnchorStyles.Right };
        refresh.FlatStyle = FlatStyle.Flat;
        refresh.UseVisualStyleBackColor = false;
        refresh.BackColor = Color.White;
        refresh.ForeColor = Ink;
        refresh.Cursor = Cursors.Hand;
        refresh.FlatAppearance.BorderColor = Color.FromArgb(190, 198, 210);
        refresh.Location = new Point(footer.Width - refresh.Width - 28, 14);
        refresh.Click += (sender, e) => RefreshStatuses();
        footer.Resize += (sender, e) => refresh.Left = footer.ClientSize.Width - refresh.Width - 28;

        footer.Paint += (sender, e) =>
        {
            using (Pen pen = new Pen(Line))
            {
                e.Graphics.DrawLine(pen, 0, 0, footer.Width, 0);
            }
        };

        footer.Controls.Add(ip);
        footer.Controls.Add(hint);
        footer.Controls.Add(refresh);
        return footer;
    }

    private Control BuildBody()
    {
        TableLayoutPanel grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 2,
            Padding = new Padding(20, 16, 20, 16),
            BackColor = Page
        };
        for (int c = 0; c < 3; c++)
        {
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / 3F));
        }
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

        for (int i = 0; i < Entries.Length; i++)
        {
            grid.Controls.Add(BuildCard(Entries[i]), i % 3, i / 3);
        }
        return grid;
    }

    private Control BuildCard(BaiEntry entry)
    {
        Panel card = new Panel { Dock = DockStyle.Fill, Margin = new Padding(8), BackColor = Color.White, Padding = new Padding(22, 16, 16, 14) };
        card.Paint += (sender, e) =>
        {
            using (Pen border = new Pen(Line))
            {
                e.Graphics.DrawRectangle(border, 0, 0, card.Width - 1, card.Height - 1);
            }
            using (SolidBrush bar = new SolidBrush(entry.Accent))
            {
                e.Graphics.FillRectangle(bar, 0, 0, 6, card.Height);
            }
        };

        TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, BackColor = Color.Transparent };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

        Label title = new Label { Text = entry.Title, AutoSize = true, ForeColor = Ink, Font = new Font("Segoe UI", 12.5F, FontStyle.Bold), Margin = new Padding(0, 2, 0, 2) };
        Label meta = new Label { Text = entry.Protocol + "  ·  " + entry.Owner, AutoSize = true, ForeColor = entry.Accent, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Margin = new Padding(0, 0, 0, 8) };
        Label desc = new Label { Text = entry.Description, Dock = DockStyle.Fill, ForeColor = Muted, Font = new Font("Segoe UI", 10F) };

        Panel bottom = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) };
        Button open = new Button { Text = "Mở bài " + entry.Number, Dock = DockStyle.Right, Width = 116, Margin = new Padding(0) };
        open.FlatStyle = FlatStyle.Flat;
        open.FlatAppearance.BorderSize = 0;
        open.UseVisualStyleBackColor = false;
        open.BackColor = entry.Accent;
        open.ForeColor = Color.White;
        open.Cursor = Cursors.Hand;
        open.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        Label status = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
        bottom.Controls.Add(status);
        bottom.Controls.Add(open);

        BaiEntry captured = entry;
        open.Click += (sender, e) => OpenBai(captured);

        layout.Controls.Add(title, 0, 0);
        layout.Controls.Add(meta, 0, 1);
        layout.Controls.Add(desc, 0, 2);
        layout.Controls.Add(bottom, 0, 3);
        card.Controls.Add(layout);

        statusLabels[entry.Number] = status;
        return card;
    }

    // ------------------------------------------------------------------ tìm và chạy bài

    private void RefreshStatuses()
    {
        foreach (BaiEntry entry in Entries)
        {
            bool found = FindExe(entry) != null;
            statusLabels[entry.Number].Text = found ? "● Sẵn sàng" : "● Chưa build";
            statusLabels[entry.Number].ForeColor = found ? Good : Warn;
        }
    }

    private void OpenBai(BaiEntry entry)
    {
        string exe = FindExe(entry);
        if (exe == null)
        {
            MessageBox.Show(this, "Chưa có bản build của " + entry.Title + ".\r\nTrong Visual Studio chọn Build → Build Solution, rồi bấm Kiểm tra lại.", "Chưa build", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo(exe)
            {
                WorkingDirectory = Path.GetDirectoryName(exe),
                UseShellExecute = true
            });
        }
        catch (Exception ex) when (ex is System.ComponentModel.Win32Exception || ex is InvalidOperationException || ex is IOException)
        {
            MessageBox.Show(this, "Không mở được " + entry.Title + ":\r\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>Đi ngược từ thư mục chạy lên đến thư mục chứa file .sln hoặc .slnx.</summary>
    private static string FindSolutionRoot()
    {
        DirectoryInfo dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            if (dir.GetFiles("*.sln").Length > 0 || dir.GetFiles("*.slnx").Length > 0)
            {
                return dir.FullName;
            }
            dir = dir.Parent;
        }
        return null;
    }

    /// <summary>Tìm file .exe mới build nhất của bài, chỉ quét trong thư mục bin của từng project.</summary>
    private string FindExe(BaiEntry entry)
    {
        if (solutionRoot == null)
        {
            return null;
        }

        string best = null;
        DateTime bestTime = DateTime.MinValue;

        foreach (string projectDir in Directory.EnumerateDirectories(solutionRoot))
        {
            string name = Path.GetFileName(projectDir);
            if (name.StartsWith(".", StringComparison.Ordinal) || name.StartsWith("_", StringComparison.Ordinal) ||
                name.Equals("Report", StringComparison.OrdinalIgnoreCase) || name.Equals("Lab03_Menu", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // project có thể nằm trực tiếp dưới thư mục gốc hoặc lồng thêm một cấp (Bai1_Lab3\Bai1_Lab3)
            List<string> projectDirs = new List<string> { projectDir };
            foreach (string inner in Directory.EnumerateDirectories(projectDir))
            {
                string innerName = Path.GetFileName(inner);
                if (!innerName.Equals("bin", StringComparison.OrdinalIgnoreCase) && !innerName.Equals("obj", StringComparison.OrdinalIgnoreCase) && !innerName.StartsWith(".", StringComparison.Ordinal))
                {
                    projectDirs.Add(inner);
                }
            }

            foreach (string candidate in projectDirs)
            {
                string bin = Path.Combine(candidate, "bin");
                if (!Directory.Exists(bin))
                {
                    continue;
                }

                foreach (string exe in Directory.EnumerateFiles(bin, "*.exe", SearchOption.AllDirectories))
                {
                    string fileName = Path.GetFileNameWithoutExtension(exe);
                    if (!entry.Matches(fileName))
                    {
                        continue;
                    }

                    DateTime written = File.GetLastWriteTime(exe);
                    if (written > bestTime)
                    {
                        bestTime = written;
                        best = exe;
                    }
                }
            }
        }

        return best;
    }

    private static string LocalAddresses()
    {
        try
        {
            List<string> list = new List<string>();
            foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    list.Add(address.ToString());
                }
            }
            return list.Count > 0 ? string.Join("   ", list) : "(không đọc được)";
        }
        catch (SocketException)
        {
            return "(không đọc được)";
        }
    }
}
