using System.Drawing;
using System.Windows.Forms;

namespace Bai3
{
    /// <summary>Màu và kiểu nút dùng chung cho các form của bài, giữ giao diện đơn giản và đồng nhất.</summary>
    internal static class UiKit
    {
        public static readonly Color Page = Color.FromArgb(244, 246, 249);
        public static readonly Color Blue = Color.FromArgb(37, 99, 160);
        public static readonly Color Ink = Color.FromArgb(32, 41, 56);
        public static readonly Color Muted = Color.FromArgb(100, 112, 130);
        public static readonly Color Good = Color.FromArgb(22, 128, 80);
        public static readonly Color Warn = Color.FromArgb(190, 110, 10);

        public static void Primary(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = false;
            button.BackColor = Blue;
            button.ForeColor = Color.White;
            button.Cursor = Cursors.Hand;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button.FlatAppearance.BorderSize = 0;
        }

        public static void Secondary(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = false;
            button.BackColor = Color.White;
            button.ForeColor = Ink;
            button.Cursor = Cursors.Hand;
            button.Font = new Font("Segoe UI", 10F);
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Color.FromArgb(190, 198, 210);
        }

        /// <summary>Cột cuối của ListView luôn giãn hết phần còn lại, không bị chữ bẻ dòng.</summary>
        public static void FillLastColumn(ListView list)
        {
            void Fit()
            {
                if (list.Columns.Count == 0)
                {
                    return;
                }
                int used = 0;
                for (int i = 0; i < list.Columns.Count - 1; i++)
                {
                    used += list.Columns[i].Width;
                }
                int width = list.ClientSize.Width - used - SystemInformation.VerticalScrollBarWidth;
                list.Columns[list.Columns.Count - 1].Width = Math.Max(120, width);
            }
            list.SizeChanged += (sender, e) => Fit();
            Fit();
        }
    }
}
