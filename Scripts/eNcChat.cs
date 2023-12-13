using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;


namespace ChestnutHotel
{
    public partial class ChatForm : Form
    {
        bool alive = false;
        UdpClient client;
        public string userName;
        string lastSendedMessage;

        string key = "6qwXxJA6U-Lo8jWL";
        string vector = "LzgeH!CYqtp1xcRL";
        string salt = "u02c4#4!7xAvSctr";
        int iterations = 512;
        string IP = "239.101.20.91";
        int port = 46172;

        public ChatForm()
        {
            InitializeComponent();

        }

        public void Login() //Подключение
        {
            try
            {
                KeyPreview = true;
                chatTextBox.Text = "";

                myTimer.Interval = 3000;

                myTimer.Enabled = true;

                Chat.Visible = true;
                messageTextBox.Enabled = true;

                chatTextBox.Text = "";
                messageTextBox.Select();
                try
                {
                    client = new UdpClient(Convert.ToInt32(port));
                    client.JoinMulticastGroup(IPAddress.Parse(IP), 20);
                    Task receiveTask = new Task(ReceiveMessages);
                    receiveTask.Start();
                    chatTextBox.Text = "⯁Добро пожаловать, " + userName + ". Адрес: " + IP + ":" + port + ". Время: " + DateTime.Now.ToString("F") + ".\r\n⯁В поле 'ПУЛЬС' отображается время последнего обмена ЭХО-пакетами (необходимо для поддержания работы сети).\r\n\r\n";

                    string message = userName + " присоединился.";

                    message = Encrypt(message, key, salt, Convert.ToInt32(iterations), vector);

                    byte[] data = Encoding.Unicode.GetBytes(message);
                    client.Send(data, data.Length, IP, Convert.ToInt32(port));
                    sendButton.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("Ошибка отправки сообщения.", "Ошибка | Система связи \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch
            { MessageBox.Show("Ошибка отправки сообщения.", "Ошибка | Система связи \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            chatTextBox.ReadOnly = true;
        }


        private void ReceiveMessages() //Приемник сообщений
        {
            Pulse.BackColor = Color.LightGreen;
            alive = true;
            try
            {
                while (alive)
                {
                    bool showbadmessage = true;
                    bool techmessage = false;
                    IPEndPoint remoteIp = null;
                    byte[] data = client.Receive(ref remoteIp);
                    string message = Encoding.Unicode.GetString(data);
                    try
                    {

                        message = Decrypt(message, key, salt, Convert.ToInt32(iterations), vector);


                        if (message == (DateTime.Now.ToString("dd")))
                        {
                            techmessage = true;
                        }
                    }
                    catch
                    {

                        if (techmessage == false)
                            message = "❌ Расшифровка сообщения невозможна, настройки шифрования не совпадают ❌";
                        else { }


                    }
                    {
                        Invoke(new MethodInvoker(() =>
                            {
                                if (techmessage == false)
                                {
                                    if (showbadmessage == true)
                                    {
                                        string time = DateTime.Now.ToShortTimeString();
                                        chatTextBox.Text = chatTextBox.Text + time + " " + message + "\r\n";

                                        if (message == lastSendedMessage)
                                        { }
                                        else
                                        {
                                            NotifyIcon NI = new NotifyIcon();
                                            NI.BalloonTipText = message;
                                            NI.BalloonTipTitle = "Система связи \"Каштан\"";
                                            NI.BalloonTipIcon = ToolTipIcon.Info;
                                            NI.Icon = this.Icon;
                                            NI.Visible = true;
                                            NI.ShowBalloonTip(300);
                                            NI.Dispose();
                                        }

                                    }
                                }
                                else
                                {
                                    chatTextBox.Text = chatTextBox.Text;
                                    Pulse.Text = DateTime.Now.ToLongTimeString();
                                    Pulse.BackColor = Color.LightGreen;
                                }
                            }));
                    }

                }
            }
            catch (ObjectDisposedException)
            {
                if (!alive)
                    return;
                throw;
            }
            catch
            {
                MessageBox.Show("Ошибка приёма сообщения.", "Ошибка | Система связи \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void SendButton_Click(object sender, EventArgs e) //Отправка сообщений
        {
            if (messageTextBox.Text.Length == 0)
            { }
            else
            {
                try
                {
                    string message = String.Format(userName + ": " + messageTextBox.Text);
                    lastSendedMessage = message;

                    message = Encrypt(message, key, salt, Convert.ToInt32(iterations), vector);

                    byte[] data = Encoding.Unicode.GetBytes(message);
                    client.Send(data, data.Length, IP, Convert.ToInt32(port));
                    messageTextBox.Clear();
                }
                catch
                {
                    MessageBox.Show("Ошибка отправки сообщения.", "Ошибка | Система связи \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExitChat() //Выход из чата
        {
            try
            {
                string message = userName + " отключился.";

                message = Encrypt(message, key, salt, Convert.ToInt32(iterations), vector);

                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Send(data, data.Length, IP, Convert.ToInt32(port));
                alive = false;
                client.Close();
                sendButton.Enabled = false;
                messageTextBox.Enabled = false;
                messageTextBox.Clear();
                chatTextBox.Clear();
                Chat.Visible = false;
            }
            catch { MessageBox.Show("Ошибка выхода.", "Ошибка | Система связи \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendButton_Click(sender, e);
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            chatTextBox.Clear();
        }

        private void ChatTextBox_TextChanged(object sender, EventArgs e)
        {
            chatTextBox.SelectionStart = chatTextBox.Text.Length;
            chatTextBox.ScrollToCaret();
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                string message = (DateTime.Now.ToString("dd"));

                message = Encrypt(message, key, salt, Convert.ToInt32(iterations), vector);

                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Send(data, data.Length, IP, Convert.ToInt32(port));
            }
            catch
            { }
        }

        private void AuthorButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Система связи \"Каштан\" v.1.0\n\nАвтор идеи и разработчик:\n\n      Александр Наумов\n      073797@gmail.com\n                                        2021.", "Об авторе | Система связи \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static string Encrypt(string ishText, string pass, string sol, int passIter, string initVec)
        {
            try
            {
                string cryptographicAlgorithm = "SHA-512";
                int keySize = 256;
                if (string.IsNullOrEmpty(ishText))
                    return "";
                byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
                byte[] solB = Encoding.ASCII.GetBytes(sol);
                byte[] ishTextB = Encoding.UTF8.GetBytes(ishText);
                PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
                byte[] keyBytes = derivPass.GetBytes(keySize / 8);
                RijndaelManaged symmK = new RijndaelManaged();
                symmK.Mode = CipherMode.CBC;
                byte[] cipherTextBytes = null;
                using (ICryptoTransform encryptor = symmK.CreateEncryptor(keyBytes, initVecB))
                {
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(ishTextB, 0, ishTextB.Length);
                            cryptoStream.FlushFinalBlock();
                            cipherTextBytes = memStream.ToArray();
                            memStream.Close();
                            cryptoStream.Close();
                        }
                    }
                }
                symmK.Clear();
                return Convert.ToBase64String(cipherTextBytes);
            }
            catch
            {
                return "❌ Ошибка шифрования строки, переподключитесь к комнате ❌";
            }
        }
        public static string Decrypt(string ciphText, string pass, string sol, int passIter, string initVec)
        {
            try
            {
                string cryptographicAlgorithm = "SHA-512";
                int keySize = 256;
                if (string.IsNullOrEmpty(ciphText))
                    return "";
                byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
                byte[] solB = Encoding.ASCII.GetBytes(sol);
                byte[] cipherTextBytes = Convert.FromBase64String(ciphText);
                PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
                byte[] keyBytes = derivPass.GetBytes(keySize / 8);
                RijndaelManaged symmK = new RijndaelManaged();
                symmK.Mode = CipherMode.CBC;
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int byteCount = 0;
                using (ICryptoTransform decryptor = symmK.CreateDecryptor(keyBytes, initVecB))
                {
                    using (MemoryStream mSt = new MemoryStream(cipherTextBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(mSt, decryptor, CryptoStreamMode.Read))
                        {
                            byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            mSt.Close();
                            cryptoStream.Close();
                        }
                    }
                }
                symmK.Clear();
                return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
            }
            catch
            {
                return "❌ Расшифровка сообщения невозможна, настройки шифрования не совпадают ❌";
            }
        }

        private void ExitChatBTN_Click(object sender, EventArgs e)
        {
            ExitChat();
            this.Close();
        }
        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (alive)
                ExitChat();
        }
    }
}