
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai6
{
    public partial class ClientForm : Form
    {
        private const int MAX_FILE_SIZE = 5 * 1024 * 1024;

        private TcpClient? tcpClient;
        private NetworkStream? networkStream;
        private StreamReader? reader;
        private StreamWriter? writer;

        private readonly SemaphoreSlim sendLock =
            new SemaphoreSlim(1, 1);

        private CancellationTokenSource? receiveCts;

        // Giao dien
        private TextBox txtIp = null!;
        private TextBox txtUsername = null!;
        private TextBox txtReceiver = null!;
        private TextBox txtMessage = null!;

        private NumericUpDown numPort = null!;

        private Button btnConnect = null!;
        private Button btnDisconnect = null!;
        private Button btnSend = null!;
        private Button btnSendFile = null!;

        private RichTextBox chatBox = null!;
        private FlowLayoutPanel imagePanel = null!;
        private ListBox listParticipants = null!;
        private Label lblStatus = null!;

        private readonly List<Image> previewImages =
            new List<Image>();

        public ClientForm()
        {
            InitializeComponent();
            CreateInterface();

            FormClosing += ClientForm_FormClosing;
            FormClosed += ClientForm_FormClosed;
        }

        // =====================================
        // CREATE INTERFACE
        // =====================================

        private void CreateInterface()
        {
            Text = "Bài 6 · Chat Room · Client";
            ClientSize = new Size(1100, 700);
            MinimumSize = new Size(960, 640);
            StartPosition = FormStartPosition.CenterScreen;

            Label lblTitle = new Label();
            lblTitle.Text = "TCP CHAT ROOM";
            lblTitle.Font = new Font("Arial", 18, FontStyle.Bold);
            lblTitle.Location = new Point(20, 15);
            lblTitle.AutoSize = true;

            Label lblIp = new Label();
            lblIp.Text = "Server IP:";
            lblIp.Location = new Point(20, 70);
            lblIp.AutoSize = true;

            txtIp = new TextBox();
            txtIp.Text = "127.0.0.1";
            txtIp.Location = new Point(20, 95);
            txtIp.Width = 150;

            Label lblPort = new Label();
            lblPort.Text = "Port:";
            lblPort.Location = new Point(190, 70);
            lblPort.AutoSize = true;

            numPort = new NumericUpDown();
            numPort.Minimum = 1;
            numPort.Maximum = 65535;
            numPort.Value = 8080;
            numPort.Location = new Point(190, 95);
            numPort.Width = 100;

            Label lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new Point(310, 70);
            lblUsername.AutoSize = true;

            txtUsername = new TextBox();
            txtUsername.Location = new Point(310, 95);
            txtUsername.Width = 150;

            btnConnect = new Button();
            btnConnect.Text = "Connect";
            btnConnect.Location = new Point(480, 92);
            btnConnect.Size = new Size(100, 35);
            btnConnect.Click += BtnConnect_Click;

            btnDisconnect = new Button();
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.Location = new Point(590, 92);
            btnDisconnect.Size = new Size(110, 35);
            btnDisconnect.Enabled = false;
            btnDisconnect.Click += BtnDisconnect_Click;

            lblStatus = new Label();
            lblStatus.Text = "Status: Disconnected";
            lblStatus.Location = new Point(20, 136);
            lblStatus.AutoSize = true;

            Label lblChat = new Label();
            lblChat.Text = "CHAT MESSAGES";
            lblChat.Location = new Point(20, 168);
            lblChat.AutoSize = true;
            lblChat.Font = new Font("Arial", 10, FontStyle.Bold);

            chatBox = new RichTextBox();
            chatBox.Location = new Point(20, 194);
            chatBox.Size = new Size(650, 400);
            chatBox.ReadOnly = true;
            chatBox.Font = new Font("Arial", 11);
            chatBox.Anchor = AnchorStyles.Top |
                             AnchorStyles.Bottom |
                             AnchorStyles.Left |
                             AnchorStyles.Right;

            Label lblParticipants = new Label();
            lblParticipants.Text = "ONLINE USERS";
            lblParticipants.Location = new Point(690, 168);
            lblParticipants.AutoSize = true;
            lblParticipants.Font = new Font("Arial", 10, FontStyle.Bold);

            listParticipants = new ListBox();
            listParticipants.Location = new Point(690, 194);
            listParticipants.Size = new Size(370, 105);
            listParticipants.Anchor = AnchorStyles.Top |
                                     AnchorStyles.Left |
                                     AnchorStyles.Right;

            Label lblImages = new Label();
            lblImages.Text = "IMAGE PREVIEW";
            lblImages.Location = new Point(690, 316);
            lblImages.AutoSize = true;
            lblImages.Font = new Font("Arial", 10, FontStyle.Bold);

            imagePanel = new FlowLayoutPanel();
            imagePanel.Location = new Point(690, 342);
            imagePanel.Size = new Size(370, 252);
            imagePanel.AutoScroll = true;
            imagePanel.FlowDirection = FlowDirection.TopDown;
            imagePanel.WrapContents = false;
            imagePanel.BorderStyle = BorderStyle.FixedSingle;
            imagePanel.Anchor = AnchorStyles.Top |
                                AnchorStyles.Bottom |
                                AnchorStyles.Left |
                                AnchorStyles.Right;

            Label lblReceiver = new Label();
            lblReceiver.Text = "Receiver (blank = group chat):";
            lblReceiver.Location = new Point(20, 608);
            lblReceiver.AutoSize = true;

            txtReceiver = new TextBox();
            txtReceiver.Location = new Point(20, 634);
            txtReceiver.Width = 220;

            Label lblMessage = new Label();
            lblMessage.Text = "Message:";
            lblMessage.Location = new Point(260, 608);
            lblMessage.AutoSize = true;

            txtMessage = new TextBox();
            txtMessage.Location = new Point(260, 634);
            txtMessage.Width = 580;

            btnSend = new Button();
            btnSend.Text = "Send";
            btnSend.Location = new Point(855, 632);
            btnSend.Size = new Size(90, 35);
            btnSend.Enabled = false;
            btnSend.Click += BtnSend_Click;

            btnSendFile = new Button();
            btnSendFile.Text = "Send File";
            btnSendFile.Location = new Point(955, 632);
            btnSendFile.Size = new Size(105, 35);
            btnSendFile.Enabled = false;
            btnSendFile.Click += BtnSendFile_Click;

            // chat box giãn hết bề ngang phần bên trái, cột bên phải giữ nguyên độ rộng và bám lề phải
            chatBox.Width = 650;
            lblParticipants.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblImages.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            listParticipants.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            imagePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            chatBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

            lblReceiver.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtReceiver.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSendFile.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            Controls.Add(lblTitle);
            Controls.Add(lblIp);
            Controls.Add(txtIp);
            Controls.Add(lblPort);
            Controls.Add(numPort);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(btnConnect);
            Controls.Add(btnDisconnect);
            Controls.Add(lblStatus);
            Controls.Add(lblChat);
            Controls.Add(chatBox);
            Controls.Add(lblParticipants);
            Controls.Add(listParticipants);
            Controls.Add(lblImages);
            Controls.Add(imagePanel);
            Controls.Add(lblReceiver);
            Controls.Add(txtReceiver);
            Controls.Add(lblMessage);
            Controls.Add(txtMessage);
            Controls.Add(btnSend);
            Controls.Add(btnSendFile);
        }

        // =====================================
        // CONNECT
        // =====================================

        private async void BtnConnect_Click(
            object? sender, EventArgs e)
        {
            if (tcpClient != null)
                return;

            string ip = txtIp.Text.Trim();
            string username = txtUsername.Text.Trim();
            int port = (int)numPort.Value;

            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show("Please enter Server IP.");
                return;
            }

            if (username.Length == 0 || username.Length > 20)
            {
                MessageBox.Show("Username must contain 1-20 characters.");
                return;
            }

            btnConnect.Enabled = false;

            TcpClient newClient = new TcpClient();

            try
            {
                await newClient.ConnectAsync(ip, port);

                tcpClient = newClient;
                networkStream = newClient.GetStream();

                reader = new StreamReader(
                    networkStream,
                    new UTF8Encoding(false),
                    false,
                    1024,
                    true
                );

                writer = new StreamWriter(
                    networkStream,
                    new UTF8Encoding(false),
                    1024,
                    true
                );

                writer.AutoFlush = true;

                receiveCts = new CancellationTokenSource();

                ChatPacket packet = new ChatPacket
                {
                    Type = "join",
                    From = username
                };

                await SendPacketAsync(packet);

                lblStatus.Text = "Status: Connected as " + username;

                btnDisconnect.Enabled = true;
                btnSend.Enabled = true;
                btnSendFile.Enabled = true;

                AppendChat("Connecting as " + username + "...");

                _ = ReceiveMessagesAsync(
                    receiveCts.Token,
                    newClient,
                    reader
                );
            }
            catch (Exception ex)
            {
                CleanupConnection();
                newClient.Close();

                btnConnect.Enabled = true;

                MessageBox.Show("Connection failed: " + ex.Message);
            }
        }

        // =====================================
        // SEND PACKET
        // =====================================

        private async Task SendPacketAsync(ChatPacket packet)
        {
            await sendLock.WaitAsync();

            try
            {
                StreamWriter currentWriter = writer
                    ?? throw new InvalidOperationException(
                        "Client is not connected."
                    );

                string json = JsonSerializer.Serialize(packet);

                await currentWriter.WriteLineAsync(json);
            }
            finally
            {
                sendLock.Release();
            }
        }

        // =====================================
        // SEND TEXT
        // =====================================

        private async void BtnSend_Click(
            object? sender, EventArgs e)
        {
            string message = txtMessage.Text.Trim();
            string receiver = txtReceiver.Text.Trim();

            if (string.IsNullOrWhiteSpace(message))
                return;

            if (tcpClient == null || writer == null)
            {
                MessageBox.Show("Please connect to Server first.");
                return;
            }

            try
            {
                ChatPacket packet;

                if (receiver.Length == 0)
                {
                    packet = new ChatPacket
                    {
                        Type = "all",
                        Message = message
                    };

                    await SendPacketAsync(packet);

                    AppendChat("You (Group): " + message);
                }
                else
                {
                    packet = new ChatPacket
                    {
                        Type = "private",
                        To = receiver,
                        Message = message
                    };

                    await SendPacketAsync(packet);
                }

                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                AppendChat("Send error: " + ex.Message);
            }
        }

        // =====================================
        // SEND FILE
        // =====================================

        private async void BtnSendFile_Click(
            object? sender, EventArgs e)
        {
            if (tcpClient == null || writer == null)
            {
                MessageBox.Show("Please connect to Server first.");
                return;
            }

            using OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Choose file to send";
            dialog.Filter =
                "Supported files|*.jpg;*.jpeg;*.png;*.txt|" +
                "All files|*.*";

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                string filePath = dialog.FileName;
                string fileName = Path.GetFileName(filePath);
                string extension =
                    Path.GetExtension(fileName).ToLowerInvariant();

                if (!IsSupportedFile(extension))
                {
                    MessageBox.Show("Unsupported file type.");
                    return;
                }

                FileInfo info = new FileInfo(filePath);

                if (info.Length > MAX_FILE_SIZE)
                {
                    MessageBox.Show("Maximum file size is 5 MB.");
                    return;
                }

                byte[] fileBytes =
                    await File.ReadAllBytesAsync(filePath);

                if (IsImage(extension) && !IsValidImage(fileBytes))
                {
                    MessageBox.Show("Invalid image file.");
                    return;
                }

                string fileData =
                    Convert.ToBase64String(fileBytes);

                string receiver = txtReceiver.Text.Trim();

                ChatPacket packet = new ChatPacket
                {
                    Type = "file",
                    To = receiver,
                    FileName = fileName,
                    FileData = fileData
                };

                await SendPacketAsync(packet);

                if (receiver.Length == 0)
                {
                    AppendChat("You shared file with everyone: " + fileName);
                }
                else
                {
                    AppendChat("You sent file to " + receiver + ": " + fileName);
                }

                if (IsImage(extension))
                {
                    AddImagePreview("You: " + fileName, fileBytes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Send file failed: " + ex.Message);
            }
        }

        // =====================================
        // RECEIVE MESSAGES
        // =====================================

        private async Task ReceiveMessagesAsync(
            CancellationToken token,
            TcpClient currentClient,
            StreamReader currentReader)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    string? line =
                        await currentReader.ReadLineAsync(token);

                    if (line == null)
                        break;

                    ChatPacket? packet =
                        JsonSerializer.Deserialize<ChatPacket>(line);

                    if (packet != null)
                        HandlePacket(packet);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (IOException)
            {
            }
            catch (SocketException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                AppendChat("Receive error: " + ex.Message);
            }
            finally
            {
                RunOnUi(() =>
                {
                    if (ReferenceEquals(tcpClient, currentClient))
                    {
                        CleanupConnection();

                        lblStatus.Text = "Status: Disconnected";

                        btnConnect.Enabled = true;
                        btnDisconnect.Enabled = false;
                        btnSend.Enabled = false;
                        btnSendFile.Enabled = false;
                        UpdateParticipants(Array.Empty<string>());

                        AppendChat("Disconnected from Server.");
                    }
                });
            }
        }

        // =====================================
        // HANDLE PACKET
        // =====================================

        private void UpdateParticipants(string[] usernames)
        {
            RunOnUi(() =>
            {
                listParticipants.BeginUpdate();
                try
                {
                    listParticipants.Items.Clear();
                    listParticipants.Items.AddRange(usernames);
                }
                finally
                {
                    listParticipants.EndUpdate();
                }
            });
        }

        private void HandlePacket(ChatPacket packet)
        {
            switch (packet.Type)
            {
                case "system":
                    AppendChat("[SYSTEM] " + packet.Message);
                    break;

                case "error":
                    AppendChat("[ERROR] " + packet.Message);
                    break;

                case "all":
                    AppendChat(packet.From + ": " + packet.Message);
                    break;

                case "private":
                    AppendChat(
                        "[PRIVATE] " + packet.From + ": " + packet.Message
                    );
                    break;

                case "users":
                    UpdateParticipants(packet.Users ?? Array.Empty<string>());
                    break;

                case "file":
                    RunOnUi(() => HandleIncomingFile(packet));
                    break;
            }
        }

        // =====================================
        // HANDLE RECEIVED FILE
        // =====================================

        private void HandleIncomingFile(ChatPacket packet)
        {
            try
            {
                string fileName =
                    Path.GetFileName(packet.FileName ?? "");

                string extension =
                    Path.GetExtension(fileName).ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(fileName) ||
                    !IsSupportedFile(extension))
                {
                    AppendChat("Unsupported incoming file.");
                    return;
                }

                string encodedData = packet.FileData ?? "";

                int maxBase64Length =
                    ((MAX_FILE_SIZE + 2) / 3) * 4;

                if (encodedData.Length > maxBase64Length)
                {
                    AppendChat("Incoming file exceeds 5 MB.");
                    return;
                }

                byte[] fileBytes =
                    Convert.FromBase64String(encodedData);

                if (fileBytes.Length > MAX_FILE_SIZE)
                {
                    AppendChat("Incoming file exceeds 5 MB.");
                    return;
                }

                if (IsImage(extension))
                {
                    if (!IsValidImage(fileBytes))
                    {
                        AppendChat("Invalid image received: " + fileName);
                        return;
                    }

                    AddImagePreview(
                        packet.From + ": " + fileName,
                        fileBytes
                    );
                }

                string fileType =
                    string.IsNullOrWhiteSpace(packet.To)
                    ? "Group file"
                    : "Private file";

                AppendChat(
                    "[" + fileType + "] Received from " +
                    packet.From + ": " + fileName
                );

                DialogResult answer = MessageBox.Show(
                    this,
                    "Received file: " + fileName +
                    "\nDo you want to save this file?",
                    "Incoming file",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (answer == DialogResult.Yes)
                {
                    SaveIncomingFile(fileName, fileBytes);
                }
            }
            catch (FormatException)
            {
                AppendChat("Invalid Base64 file data.");
            }
            catch (Exception ex)
            {
                AppendChat("File error: " + ex.Message);
            }
        }

        // =====================================
        // SAVE FILE
        // =====================================

        private void SaveIncomingFile(
            string fileName,
            byte[] fileBytes)
        {
            using SaveFileDialog dialog = new SaveFileDialog();

            dialog.Title = "Save received file";
            dialog.FileName = fileName;
            dialog.Filter =
                "Supported files|*.jpg;*.jpeg;*.png;*.txt|" +
                "All files|*.*";

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            File.WriteAllBytes(dialog.FileName, fileBytes);

            AppendChat("Saved file: " + dialog.FileName);
        }

        // =====================================
        // IMAGE PREVIEW
        // =====================================

        private void AddImagePreview(
            string title,
            byte[] imageBytes)
        {
            try
            {
                using MemoryStream ms = new MemoryStream(imageBytes);

                using Image original = Image.FromStream(
                    ms,
                    false,
                    true
                );

                if (original.Width > 10000 ||
                    original.Height > 10000)
                {
                    AppendChat("Image dimensions are too large: " + title);
                    return;
                }

                Bitmap bitmap = new Bitmap(original);

                previewImages.Add(bitmap);

                Panel item = new Panel();
                item.Size = new Size(325, 205);

                Label label = new Label();
                label.Text = title;
                label.Location = new Point(5, 5);
                label.Size = new Size(310, 30);
                label.AutoEllipsis = true;

                PictureBox picture = new PictureBox();
                picture.Location = new Point(5, 38);
                picture.Size = new Size(310, 160);
                picture.SizeMode = PictureBoxSizeMode.Zoom;
                picture.BorderStyle = BorderStyle.FixedSingle;
                picture.Image = bitmap;

                item.Controls.Add(label);
                item.Controls.Add(picture);

                imagePanel.Controls.Add(item);
                imagePanel.ScrollControlIntoView(item);
            }
            catch
            {
                AppendChat("Cannot display image: " + title);
            }
        }

        // =====================================
        // FILE VALIDATION
        // =====================================

        private bool IsSupportedFile(string extension)
        {
            return extension == ".jpg" ||
                   extension == ".jpeg" ||
                   extension == ".png" ||
                   extension == ".txt";
        }

        private bool IsImage(string extension)
        {
            return extension == ".jpg" ||
                   extension == ".jpeg" ||
                   extension == ".png";
        }

        private bool IsValidImage(byte[] data)
        {
            try
            {
                using MemoryStream ms = new MemoryStream(data);

                using Image image = Image.FromStream(
                    ms,
                    false,
                    true
                );

                return image.Width > 0 &&
                       image.Height > 0 &&
                       image.Width <= 10000 &&
                       image.Height <= 10000;
            }
            catch
            {
                return false;
            }
        }

        // =====================================
        // DISPLAY CHAT
        // =====================================

        private void AppendChat(string message)
        {
            RunOnUi(() =>
            {
                chatBox.AppendText(
                    DateTime.Now.ToString("HH:mm:ss") +
                    "  " + message +
                    Environment.NewLine
                );

                chatBox.SelectionStart = chatBox.TextLength;
                chatBox.ScrollToCaret();
            });
        }

        // =====================================
        // UI THREAD
        // =====================================

        private void RunOnUi(Action action)
        {
            if (IsDisposed ||
                Disposing ||
                !IsHandleCreated)
            {
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(action);
                }
                catch (ObjectDisposedException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
            else
            {
                action();
            }
        }

        // =====================================
        // DISCONNECT
        // =====================================

        private void BtnDisconnect_Click(
            object? sender, EventArgs e)
        {
            CleanupConnection();

            lblStatus.Text = "Status: Disconnected";

            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            btnSend.Enabled = false;
            btnSendFile.Enabled = false;
            UpdateParticipants(Array.Empty<string>());

            AppendChat("You disconnected.");
        }

        private void CleanupConnection()
        {
            receiveCts?.Cancel();

            try { reader?.Dispose(); } catch { }
            try { writer?.Dispose(); } catch { }
            try { networkStream?.Dispose(); } catch { }
            try { tcpClient?.Close(); } catch { }

            reader = null;
            writer = null;
            networkStream = null;
            tcpClient = null;

            receiveCts?.Dispose();
            receiveCts = null;
        }

        // =====================================
        // FORM CLOSING
        // =====================================

        private void ClientForm_FormClosing(
            object? sender, FormClosingEventArgs e)
        {
            CleanupConnection();
        }

        private void ClientForm_FormClosed(
            object? sender, FormClosedEventArgs e)
        {
            foreach (Image image in previewImages)
            {
                image.Dispose();
            }

            previewImages.Clear();
        }
    }
}
