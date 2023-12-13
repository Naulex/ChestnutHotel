using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace ChestnutHotel
{
    public partial class NotepadForm : Form
    {
        string fileName;
        string pass;
        bool notifyShowed = false;
        bool isInterfaceHidden = false;
        bool isTopMost = false;
        bool isFirstTry = true;
        public NotepadForm()
        {
            InitializeComponent();
            KeyPreview = true;
            TopMost = false;
            NoteTextBox.Font = new Font(NoteTextBox.Font.FontFamily, 10);
            NotifyIcon.Visible = false;
            NotifyIcon.MouseClick += new MouseEventHandler(NotifyIcon_MouseDoubleClick);
            Resize += new EventHandler(Form1_Resize);
            infolabel.Text = "Запуск выполнен успешно";

        }
        private void PreventClosing()
        {
            try
            {
                if (сворачиватьАНеЗакрыватьToolStripMenuItem.Checked == true)
                {
                    if (WindowState == FormWindowState.Minimized)
                    {
                        if (notifyShowed == false)
                        {
                            ShowInTaskbar = false;
                            NotifyIcon.Visible = true;
                            NotifyIcon NI = new NotifyIcon();
                            NI.BalloonTipText = "Кликните по иконке в панели задач чтобы развернуть";
                            NI.BalloonTipTitle = "Справочная система \"Каштан\"";
                            NI.BalloonTipIcon = ToolTipIcon.Info;
                            NI.Icon = this.Icon;
                            NI.Visible = true;
                            NI.ShowBalloonTip(3000);
                            NI.Dispose();
                            notifyShowed = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Справочная система \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            PreventClosing();
        }
        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                NotifyIcon.Visible = false;
                ShowInTaskbar = true;
                WindowState = FormWindowState.Normal;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Справочная система \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.X && e.Control)
                {
                    try
                    {
                        FileAttributes attributes = File.GetAttributes(fileName);
                        if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                        {
                            this.Close();
                        }
                        else
                        {
                            if (NoteTextBox.Text.Length != 0)
                            {
                                DialogResult result = MessageBox.Show("Сохранить изменения в файле?", "Сохранение | Справочная система \"Каштан\"", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                                if (result == DialogResult.No)
                                {
                                    NotifyIcon.Dispose();
                                    this.Close();
                                }
                                if (result == DialogResult.Yes)
                                {
                                    SaveTextFile();
                                    this.Close();
                                }
                                if (result == DialogResult.Cancel)
                                {
                                    return;
                                }
                            }
                            else
                                this.Close();
                        }
                    }
                    catch
                    {
                        if (fileName == null)
                            this.Close();
                    }
                }
                if (e.KeyCode == Keys.L && e.Control)
                {
                    HideInterface();
                }
                if (e.KeyCode == Keys.Enter && e.Control)
                {
                    AddDateTime();
                }
                if (e.KeyCode == Keys.G && e.Control)
                {
                    if (FontSelector.ShowDialog() != DialogResult.Cancel)
                    {
                        NoteTextBox.SelectionFont = FontSelector.Font;
                    }
                }
                if (e.KeyCode == Keys.H && e.Control)
                {
                    if (ColorSelector.ShowDialog() != DialogResult.Cancel)
                    {
                        NoteTextBox.SelectionColor = ColorSelector.Color;
                    }
                }
                if (e.KeyCode == Keys.F && e.Control)
                {
                    if (TopMost == false)
                    {
                        TopMost = true;
                        поверхВсехОконToolStripMenuItem.Checked = true;
                        infolabel.Text = "Отображение поверх всех окон включено";
                        isTopMost = true;
                    }
                    else
                    {
                        TopMost = false;
                        поверхВсехОконToolStripMenuItem.Checked = false;
                        infolabel.Text = "Отображение поверх всех окон отключено";
                        isTopMost = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Справочная система \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddDateTime()
        {
            try
            {
                FileAttributes attributes = File.GetAttributes(fileName);
                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    infolabel.Text = "Невозможно добавить дату, так как файл открыт только для чтения";
                }
                else
                {
                    infolabel.Text = "Добавлены дата и время";
                    NoteTextBox.SelectedText = "\r\n" + Convert.ToString(DateTime.Now) + "\r\n=========================";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Справочная система \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FileAttributes attributes = File.GetAttributes(fileName);
                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    this.Close();
                }
                else
                {
                    if (NoteTextBox.Text.Length != 0)
                    {
                        DialogResult result = MessageBox.Show("Сохранить изменения в файле?", "Сохранение | Справочная система \"Каштан\"", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            NotifyIcon.Dispose();
                            this.Close();
                        }
                        if (result == DialogResult.Yes)
                        {
                            SaveTextFile();
                            this.Close();
                        }
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                    else
                        this.Close();
                }
            }
            catch
            {
                if (fileName == null)
                    this.Close();
            }
        }
        private static string GetHash(string text)
        {
            try
            {
                SHA512 sha512Hash = new SHA512Managed();
                byte[] sourceBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
                string rawTextHash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return rawTextHash;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Справочная система \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private void SaveAs()
        {
            try
            {
                if (SaveFile.ShowDialog() == DialogResult.Cancel)
                    return;
                fileName = SaveFile.FileName;
                if (Path.GetExtension(fileName) == ".txt")
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(fileName))
                        {
                            sw.WriteLine(NoteTextBox.Text, pass);
                        }
                        infolabel.Text = "Файл сохранён успешно";
                    }
                    catch
                    { infolabel.Text = "Ошибка сохранения файла. Возможно, файл имеет атрибут \"Только для чтения\""; }
                }
                else
                if (Path.GetExtension(fileName) == ".NoteN")
                {
                    try
                    {
                        string FullString = GetHash(NoteTextBox.Rtf) + "/NoteNSplitterNoteNAjFiS/" + Size.Height + "/NoteNSplitterNoteNAjFiS/" + Size.Width + "/NoteNSplitterNoteNAjFiS/" + BackColor.ToArgb() + "/NoteNSplitterNoteNAjFiS/" + создатьToolStripMenuItem.ForeColor.ToArgb() + "/NoteNSplitterNoteNAjFiS/" + NoteTextBox.Rtf + "/NoteNSplitterNoteNAjFiS/" + isInterfaceHidden + "/NoteNSplitterNoteNAjFiS/" + isTopMost;
                        var size = this.Size.Height;
                        var size2 = this.Size.Width;
                        var foncolor = this.BackColor.ToString();
                        var buttoncolor = this.создатьToolStripMenuItem.ForeColor.ToString();
                        pass = Microsoft.VisualBasic.Interaction.InputBox("Введите пароль, которым будет зашифрован файл. Оставьте пустым для стандартного шифрования.", "Ввод пароля | Справочная система \"Каштан\"");
                        if (pass.Length == 0)
                        { pass = "9ByGEunqAsE3H2VHjc5nLD3kXb087e"; }
                        using (StreamWriter sw = new StreamWriter(fileName))
                        {
                            sw.WriteLine(EncryptString(FullString, pass));
                        }
                        infolabel.Text = "Файл сохранён успешно";
                    }
                    catch
                    {
                        infolabel.Text = "Ошибка сохранения файла. Возможно, файл имеет атрибут \"Только для чтения\"";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Справочная система \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveTextFile()
        {
            try
            {
                if (Path.GetExtension(fileName) == ".txt")
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(fileName))
                        {
                            sw.WriteLine(NoteTextBox.Text, pass);
                        }
                        infolabel.Text = "Файл сохранён успешно";
                    }
                    catch
                    { infolabel.Text = "Ошибка сохранения файла. Возможно, файл имеет атрибут \"Только для чтения\""; }
                }
                else
                if (Path.GetExtension(fileName) == ".NoteN")
                {
                    try
                    {
                        bool interfaceSettings = false;
                        if (isInterfaceHidden == true)
                            interfaceSettings = false;
                        if (isInterfaceHidden == false)
                            interfaceSettings = true;
                        string FullString = GetHash(NoteTextBox.Rtf) + "/NoteNSplitterNoteNAjFiS/" + Size.Height + "/NoteNSplitterNoteNAjFiS/" + Size.Width + "/NoteNSplitterNoteNAjFiS/" + BackColor.ToArgb() + "/NoteNSplitterNoteNAjFiS/" + создатьToolStripMenuItem.ForeColor.ToArgb() + "/NoteNSplitterNoteNAjFiS/" + NoteTextBox.Rtf + "/NoteNSplitterNoteNAjFiS/" + interfaceSettings + "/NoteNSplitterNoteNAjFiS/" + isTopMost;
                        using (StreamWriter sw = new StreamWriter(fileName))
                        {
                            sw.WriteLine(EncryptString(FullString, pass));
                        }
                        infolabel.Text = "Файл сохранён успешно";
                    }
                    catch
                    {
                        infolabel.Text = "Ошибка сохранения файла. Возможно, файл имеет атрибут \"Только для чтения\"";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Справочная система \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public async void OpenTextFile(string fileName)
        {
            try
            {
                if (Path.GetExtension(fileName) == ".txt")
                {
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        NoteTextBox.Text = (await sr.ReadToEndAsync());
                    }
                    infolabel.Text = "Открыт текстовый файл";
                }
                else
                {
                    try
                    {
                        pass = "9ByGEunqAsE3H2VHjc5nLD3kXb087e";
                        string EncfileText = File.ReadAllText(fileName);
                        string DecFileText = DecryptString(EncfileText, pass);
                        String[] Data = DecFileText.Split(new string[] { "/NoteNSplitterNoteNAjFiS/" }, StringSplitOptions.RemoveEmptyEntries);
                        if (Data[0] == GetHash(Data[5]))
                            infolabel.Text = "Файл открыт успешно";
                        else
                        {
                            infolabel.Text = "Файл открыт, но контрольные суммы не совпадают";
                        }
                        infolabel.Enabled = true;
                        контрольнаяСумма1ToolStripMenuItem.Visible = true;
                        контрольнаяСумма2ToolStripMenuItem.Visible = true;
                        контрольнаяСумма1ToolStripMenuItem.Text = Data[0];
                        контрольнаяСумма2ToolStripMenuItem.Text = GetHash(Data[5]);
                        SetBackColor(Color.FromArgb(Convert.ToInt32(Data[3])));
                        SetButtonColor(Color.FromArgb(Convert.ToInt32(Data[4])));
                        this.Height = Convert.ToInt32(Data[1]);
                        this.Width = Convert.ToInt32(Data[2]);
                        if (Convert.ToBoolean(Data[6]) == true)
                        {
                            isInterfaceHidden = true;
                            HideInterface();
                        }
                        if (Convert.ToBoolean(Data[6]) == false)
                        {
                            isInterfaceHidden = false;
                            HideInterface();
                        }
                        if (Convert.ToBoolean(Data[7]) == true)
                        {
                            TopMost = true;
                            поверхВсехОконToolStripMenuItem.Checked = true;
                            isTopMost = true;
                        }
                        if (Convert.ToBoolean(Data[7]) == false)
                        {
                            TopMost = false;
                            поверхВсехОконToolStripMenuItem.Checked = false;
                            isTopMost = false;
                        }
                        NoteTextBox.Rtf = Data[5];
                    }
                    catch
                    {
                        string EncfileText = File.ReadAllText(fileName);
                        pass = Microsoft.VisualBasic.Interaction.InputBox("Введите пароль для расшифровки файла", "Ввод пароля | Справочная система \"Каштан\"");
                        string DecFileText = DecryptString(EncfileText, pass);
                        String[] Data = DecFileText.Split(new string[] { "/NoteNSplitterNoteNAjFiS/" }, StringSplitOptions.RemoveEmptyEntries);
                        if (Data[0] == GetHash(Data[5]))
                            infolabel.Text = "Файл открыт успешно";
                        else
                        {
                            infolabel.Text = "Файл открыт, но контрольные суммы не совпадают";
                        }
                        infolabel.Enabled = true;
                        контрольнаяСумма1ToolStripMenuItem.Visible = true;
                        контрольнаяСумма2ToolStripMenuItem.Visible = true;
                        контрольнаяСумма1ToolStripMenuItem.Text = Data[0];
                        контрольнаяСумма2ToolStripMenuItem.Text = GetHash(Data[5]);
                        SetBackColor(Color.FromArgb(Convert.ToInt32(Data[3])));
                        SetButtonColor(Color.FromArgb(Convert.ToInt32(Data[4])));
                        this.Height = Convert.ToInt32(Data[1]);
                        this.Width = Convert.ToInt32(Data[2]);
                        if (Convert.ToBoolean(Data[6]) == true)
                        {
                            isInterfaceHidden = true;
                            HideInterface();
                        }
                        if (Convert.ToBoolean(Data[6]) == false)
                        {
                            isInterfaceHidden = false;
                            HideInterface();
                        }
                        if (Convert.ToBoolean(Data[7]) == true)
                        {
                            TopMost = true;
                            поверхВсехОконToolStripMenuItem.Checked = true;
                            isTopMost = true;
                        }
                        if (Convert.ToBoolean(Data[7]) == false)
                        {
                            TopMost = false;
                            поверхВсехОконToolStripMenuItem.Checked = false;
                            isTopMost = false;
                        }
                        NoteTextBox.Rtf = Data[5];
                    }
                }
                NoteTextBox.Enabled = true;
                //сохранитьToolStripMenuItem.Visible = true;
                добавитьДатуИВремяCtrlF5ToolStripMenuItem.Visible = true;
                шрифтИРазмерТекстаToolStripMenuItem.Visible = true;
                цветТекстаToolStripMenuItem1.Visible = true;
                this.Text = "Справочная система \"Каштан\" (" + Path.GetFileName(fileName) + ")";
                NoteTextBox.SelectionStart = NoteTextBox.Text.Length;
                NoteTextBox.Refresh();
                файлToolStripMenuItem.Visible = true;
                FileAttributes attributes = File.GetAttributes(fileName);
                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    файлToolStripMenuItem.Checked = true;
                    NoteTextBox.ReadOnly = true;
                    NoteTextBox.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
                }
            }
            catch (Exception ex)
            {
                infolabel.Text = "Ошибка открытия файла";
                MessageBox.Show($"Ошибка: {ex.Message}", "Справочная система \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Функционал отключен.
        }
        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (OpenFile.ShowDialog() == DialogResult.Cancel)
                    return;
                OpenTextFile(OpenFile.FileName);
                fileName = OpenFile.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Справочная система \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static string EncryptString(string ishText, string password)
        {
            try
            {
                SHA512 sha512Hash = new SHA512Managed();
                byte[] sourceBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
                string sol = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

                if (string.IsNullOrEmpty(ishText))
                    return "";
                byte[] ishTextB = Encoding.UTF8.GetBytes(ishText);
                byte[] cipherTextBytes = null;
                int iterations = 5192;
                byte[] salt = Encoding.ASCII.GetBytes(sol);
                AesManaged aes = new AesManaged();
                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);
                aes.Mode = CipherMode.CBC;
                ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
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
                aes.Clear();
                return Convert.ToBase64String(cipherTextBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Справочная система \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        public static string DecryptString(string ciphText, string password)
        {
            try
            {
                SHA512 sha512Hash = new SHA512Managed();
                byte[] sourceBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
                string sol = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                if (string.IsNullOrEmpty(ciphText))
                    return "";
                byte[] ishTextB = Encoding.UTF8.GetBytes(ciphText);
                byte[] cipherTextBytes = null;
                int iterations = 5192;
                byte[] salt = Encoding.ASCII.GetBytes(sol);
                AesManaged aes = new AesManaged();
                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);
                aes.Mode = CipherMode.CBC;
                cipherTextBytes = Convert.FromBase64String(ciphText);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int byteCount = 0;
                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
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
                aes.Clear();
                return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Справочная система \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void добавитьДатуИВремяCtrlF5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDateTime();
        }

        private void обАвтореToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Справочная система \"Каштан\" - программа, позволяющая создавать, открывать и сохранять зашифрованные текстовые файлы.\r\n\r\n(c)AlexanderN, 2021\r\n073797@gmail.com", "О программе | Справочная система \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (файлToolStripMenuItem.Checked == true)
            {
                File.SetAttributes(fileName, FileAttributes.ReadOnly);
                NoteTextBox.ReadOnly = true;
           
            }
            else
            {
                File.SetAttributes(fileName, FileAttributes.Normal);
                NoteTextBox.ReadOnly = false;
               
            }
        }

        private void поверхВсехОконToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (поверхВсехОконToolStripMenuItem.Checked == true)
            {
                TopMost = true;
                isTopMost = true;
                infolabel.Text = "Отображение поверх всех окон включено";
            }

            else
            {
                TopMost = false;
                isTopMost = false;
                infolabel.Text = "Отображение поверх всех окон отключено";
            }
        }

        private void шрифтИРазмерТекстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FontSelector.ShowDialog() != DialogResult.Cancel)
            {
                NoteTextBox.SelectionFont = FontSelector.Font;
            }
        }

        private void цветТекстаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ColorSelector.ShowDialog() != DialogResult.Cancel)
            {
                NoteTextBox.SelectionColor = ColorSelector.Color;
            }
        }

        private void HideInterface()
        {
            if (isInterfaceHidden == true)
            {
                MainMenu.Visible = false;
                infolabel.Visible = false;
                NoteTextBox.Width = this.Width - 20;
                NoteTextBox.Height = this.Height - 40;
                NoteTextBox.Location = new Point(0, 0);
                NoteTextBox.Refresh();
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                isInterfaceHidden = false;
            }
            else
            {
                if (isInterfaceHidden == false)
                {
                    if (isFirstTry == false)
                    {
                        isFirstTry = true;
                    }
                    else
                    {
                        NoteTextBox.Width = this.Width;
                        NoteTextBox.Height = this.Height - 27;
                        NoteTextBox.Location = new Point(0, 27);
                        NoteTextBox.Refresh();
                        MainMenu.Visible = true;
                        infolabel.Visible = true;
                        FormBorderStyle = FormBorderStyle.Sizable;
                        isInterfaceHidden = true;
                    }
                }
            }
        }
        private void скрытьИнтерфейсCtrlLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideInterface();
        }
        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ColorSelector.ShowDialog() != DialogResult.Cancel)
                SetBackColor(ColorSelector.Color);
        }
        private void SetBackColor(Color c)
        {
            NoteTextBox.BackColor = c;
            BackColor = c;
            MainMenu.BackColor = c;
        }
        private void SetButtonColor(Color c)
        {
            создатьToolStripMenuItem.ForeColor = c;
            загрузитьToolStripMenuItem.ForeColor = c;
            настройкиToolStripMenuItem.ForeColor = c;
            сохранитьToolStripMenuItem.ForeColor = c;
            выходToolStripMenuItem.ForeColor = c;
            infolabel.ForeColor = c;
        }
        private void цветКнопокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ColorSelector.ShowDialog() != DialogResult.Cancel)
            {
                SetButtonColor(ColorSelector.Color);
            }
        }
        private void сохранитьCtrlSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveTextFile();
        }
        private void сохранитьКакCtrlShiftSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }
        private void MainMenu_MouseDown(object sender, MouseEventArgs e)
        {
            Capture = false;
            Message m = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref m);
        }

    }
}
