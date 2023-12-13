using System;
using System.Windows.Forms;
using System.IO;

namespace ChestnutHotel
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
            try
            {
                if (!Directory.Exists("ChestnutData"))
                {
                    DialogResult result = MessageBox.Show("Похоже, эта система запускается в первый раз.\r\n\r\nПри нажатии \"Да\" приложение создаст папку \"ChestnutData\" и извлечёт туда необходимые для работы файлы, при нажатии \"Нет\" приложение будет закрыто.", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.No)
                    {
                        Environment.Exit(0);
                    }
                    else if (result == DialogResult.Yes)
                    {
                        ExtractBaseData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ExtractBaseData()
        {
            try
            {
                Directory.CreateDirectory("ChestnutData");
                Directory.CreateDirectory("ChestnutData\\Help");
                Directory.CreateDirectory("ChestnutData\\Img");


                File.WriteAllBytes(@"ChestnutData\ChestnutDatabase.mdb", Properties.Resources.ChestnutDatabase);

                File.WriteAllBytes(@"КлиентЗапуск.bat", Properties.Resources.КлиентЗапуск);
                File.WriteAllBytes(@"АдминистраторЗапуск.bat", Properties.Resources.АдминистраторЗапуск);
                File.WriteAllBytes(@"ПерсоналЗапуск.bat", Properties.Resources.ПерсоналЗапуск);

                File.WriteAllBytes(@"ChestnutData\Help\AboutAdmin.NoteN", Properties.Resources.AboutAdmin);
                File.WriteAllBytes(@"ChestnutData\Help\AboutClient.NoteN", Properties.Resources.AboutClient);
                File.WriteAllBytes(@"ChestnutData\Help\AboutPersonal.NoteN", Properties.Resources.AboutPersonal);
                File.WriteAllBytes(@"ChestnutData\Help\AboutReservations.NoteN", Properties.Resources.AboutReservations);
                File.WriteAllBytes(@"ChestnutData\Help\AboutSystem.NoteN", Properties.Resources.AboutSystem);

                Properties.Resources._1.Save(@"ChestnutData\Img\1.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._2.Save(@"ChestnutData\Img\2.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._3.Save(@"ChestnutData\Img\3.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._4.Save(@"ChestnutData\Img\4.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._5.Save(@"ChestnutData\Img\5.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._6.Save(@"ChestnutData\Img\6.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._7.Save(@"ChestnutData\Img\7.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._8.Save(@"ChestnutData\Img\8.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._9.Save(@"ChestnutData\Img\9.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._10.Save(@"ChestnutData\Img\10.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._11.Save(@"ChestnutData\Img\11.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._12.Save(@"ChestnutData\Img\12.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._13.Save(@"ChestnutData\Img\13.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._14.Save(@"ChestnutData\Img\14.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._15.Save(@"ChestnutData\Img\15.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._16.Save(@"ChestnutData\Img\16.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._17.Save(@"ChestnutData\Img\17.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._18.Save(@"ChestnutData\Img\18.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._19.Save(@"ChestnutData\Img\19.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._20.Save(@"ChestnutData\Img\20.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._21.Save(@"ChestnutData\Img\21.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._22.Save(@"ChestnutData\Img\22.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._23.Save(@"ChestnutData\Img\23.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._24.Save(@"ChestnutData\Img\24.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._25.Save(@"ChestnutData\Img\25.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._26.Save(@"ChestnutData\Img\26.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._27.Save(@"ChestnutData\Img\27.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._28.Save(@"ChestnutData\Img\28.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._29.Save(@"ChestnutData\Img\29.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Properties.Resources._30.Save(@"ChestnutData\Img\30.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                Properties.Resources._null.Save(@"ChestnutData\Img\null.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainMenu_Shown(object sender, EventArgs e)
        {
            string[] arguments = Environment.GetCommandLineArgs();
            try
            {
                if (Directory.Exists("ChestnutData"))
                {
                    if (arguments[1] == "--Administrator")
                    {
                        SetVisibleCore(false);
                        AdministratorForm AdministratorForm = new AdministratorForm();
                        AdministratorForm.Show();
                    }

                    if (arguments[1] == "--Personal")
                    {

                        SetVisibleCore(false);
                        PersonalForm PersonalForm = new PersonalForm();
                        PersonalForm.Show();
                    }

                    if (arguments[1] == "--User")
                    {
                        SetVisibleCore(false);
                        UserForm UserForm = new UserForm();
                        UserForm.Show();
                    }
                }

            }
            catch
            {
                this.Show();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Visible = false;
                UserForm UserForm = new UserForm();
                UserForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                this.Visible = false;
                PersonalForm PersonalForm = new PersonalForm();
                PersonalForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Visible = false;
                AdministratorForm AdministratorForm = new AdministratorForm();
                AdministratorForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExitApplicationButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void AboutSystem_Click(object sender, EventArgs e)
        {
            try
            {
                NotepadForm notepadForm = new NotepadForm();
                notepadForm.OpenTextFile("ChestnutData/Help/AboutSystem.NoteN");
                notepadForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
