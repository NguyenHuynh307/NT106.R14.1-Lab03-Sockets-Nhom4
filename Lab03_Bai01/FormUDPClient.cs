using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Bai1_Lab3
{
    public partial class FormUDPClient : Form
    {
        public FormUDPClient()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object? sender, EventArgs e)
        {
            string remoteIP = tbIP.Text.Trim();
            string portStr = tbPort.Text.Trim();
            string message = rtbMessage.Text;

            if (string.IsNullOrEmpty(remoteIP) || string.IsNullOrEmpty(portStr))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ địa chỉ IP và cổng.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IPAddress.TryParse(remoteIP, out IPAddress? parsedIP) || parsedIP == null)
            {
                MessageBox.Show("Địa chỉ IP không hợp lệ. Ví dụ: 127.0.0.1", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbIP.Focus();
                tbIP.SelectAll();
                return;
            }

            if (!int.TryParse(portStr, out int remotePort) || remotePort < 1 || remotePort > 65535)
            {
                MessageBox.Show("Cổng phải là số nguyên từ 1 đến 65535.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPort.Focus();
                tbPort.SelectAll();
                return;
            }

            if (message.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập nội dung tin nhắn.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbMessage.Focus();
                return;
            }

            try
            {
                using (UdpClient udpClient = new UdpClient())
                {
                    byte[] sendBytes = Encoding.UTF8.GetBytes(message);
                    udpClient.Send(sendBytes, sendBytes.Length, parsedIP.ToString(), remotePort);
                }
                lblStatus.ForeColor = UiKit.Good;
                lblStatus.Text = "Đã gửi lúc " + DateTime.Now.ToString("HH:mm:ss") + " tới " + parsedIP + ":" + remotePort;
            }
            catch (SocketException ex)
            {
                lblStatus.ForeColor = UiKit.Warn;
                lblStatus.Text = "Gửi thất bại.";
                MessageBox.Show("Không gửi được: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
