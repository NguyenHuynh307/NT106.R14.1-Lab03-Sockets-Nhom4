using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
namespace Bai3
{
    public partial class ClientForm : Form
    {

        public ClientForm()
        {
            InitializeComponent();
        }
        private TcpClient? tcpClient;
        private NetworkStream? ns;
        private void ClientForm_Load(object sender, EventArgs e)
        {

        }

        private void SetStatus(string text, bool connected)
        {
            lblStatus.ForeColor = connected ? UiKit.Good : UiKit.Muted;
            lblStatus.Text = text;
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            // Da ket noi roi thi khong tao them ket noi moi (tranh ro ri Socket)
            if (tcpClient != null)
            {
                MessageBox.Show("Đã kết nối Server rồi. Bấm Ngắt kết nối trước khi kết nối lại.");
                return;
            }

            string ip = txtIp.Text.Trim();

            if (!IPAddress.TryParse(ip, out IPAddress? ipAddress) || ipAddress == null)
            {
                MessageBox.Show("Địa chỉ IP không hợp lệ. Ví dụ: 127.0.0.1", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIp.Focus();
                txtIp.SelectAll();
                return;
            }

            if (!int.TryParse(txtPort.Text.Trim(), out int port) || port < 1 || port > 65535)
            {
                MessageBox.Show("Cổng phải là số nguyên từ 1 đến 65535.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPort.Focus();
                txtPort.SelectAll();
                return;
            }

            btnConnect.Enabled = false;
            SetStatus("Đang kết nối...", false);

            TcpClient newClient = new TcpClient();

            try
            {
                // Ket noi bat dong bo, toi da 5 giay: sai IP/cong thi bao loi, Form khong bi treo
                using CancellationTokenSource timeout =
                    new CancellationTokenSource(TimeSpan.FromSeconds(5));

                await newClient.ConnectAsync(ipAddress, port, timeout.Token);

                tcpClient = newClient;
                ns = newClient.GetStream();

                SetStatus("Đã kết nối tới " + ip + ":" + port, true);
            }
            catch (OperationCanceledException)
            {
                newClient.Close();
                SetStatus("Kết nối thất bại.", false);
                MessageBox.Show("Không kết nối được: quá thời gian chờ (5 giây). Kiểm tra IP, cổng và Server đã bấm Listen chưa.");
            }
            catch (Exception ex)
            {
                newClient.Close();
                SetStatus("Kết nối thất bại.", false);
                MessageBox.Show("Không kết nối được: " + ex.Message);
            }
            finally
            {
                btnConnect.Enabled = true;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcpClient == null || ns == null || !ns.CanWrite)
                {
                    MessageBox.Show("Vui lòng kết nối Server trước.");
                    return;
                }

                string message = txtMessage.Text.Trim();

                if (message == "")
                {
                    MessageBox.Show("Vui lòng nhập tin nhắn.");
                    return;
                }

                byte[] data = Encoding.UTF8.GetBytes(message + "\n");

                ns.Write(data, 0, data.Length);

                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi thất bại: " + ex.Message);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcpClient == null)
                {
                    MessageBox.Show("Chưa kết nối Server.");
                    return;
                }

                if (ns != null)
                {
                    ns.Close();
                    ns = null;
                }

                tcpClient.Close();
                tcpClient = null;

                SetStatus("Đã ngắt kết nối.", false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ngắt kết nối thất bại: " + ex.Message);
            }
        }

        // Dong Form thi dong ket noi de Server biet Client da roi
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try { ns?.Close(); } catch { }
            try { tcpClient?.Close(); } catch { }

            ns = null;
            tcpClient = null;

            base.OnFormClosing(e);
        }
    }
}
