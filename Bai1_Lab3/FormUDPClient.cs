using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string remoteIP = tbIP.Text.Trim();
                string portStr = tbPort.Text.Trim();
                string message = rtbMessage.Text;
                if (string.IsNullOrEmpty(remoteIP) || string.IsNullOrEmpty(portStr))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ địa chỉ IP và Port!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                IPAddress parsedIP;
                if (!IPAddress.TryParse(remoteIP, out parsedIP))
                {
                    MessageBox.Show("Địa chỉ IP không hợp lệ! Vui lòng nhập lại (Ví dụ: 127.0.0.1)", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbIP.Focus();
                    tbIP.SelectAll();
                    return;
                }
                int remotePort;
                if (!int.TryParse(portStr, out remotePort) || remotePort < 0 || remotePort > 65535)
                {
                    MessageBox.Show("Port phải là một số nguyên từ 0 đến 65535!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbPort.Focus();
                    tbPort.SelectAll();
                    return;
                }
                using (UdpClient udpClient = new UdpClient())
                {
                    byte[] sendBytes = Encoding.UTF8.GetBytes(message);
                    udpClient.Send(sendBytes, sendBytes.Length, parsedIP.ToString(), remotePort);
                    MessageBox.Show("Gửi thông điệp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
