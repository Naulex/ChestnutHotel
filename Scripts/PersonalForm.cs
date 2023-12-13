using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;

namespace ChestnutHotel
{
    public partial class PersonalForm : Form
    {
        OleDbDataAdapter dataAdapter;
        DataSet DataSet = new DataSet();
        public static int PersonalCode;

        string DBNamePath = "ChestnutData/ChestnutDatabase.mdb";
        public PersonalForm() //установка видимости вкладок
        {
            InitializeComponent();

            Page1Authorization.Parent = PersonalUserTabControl;
            Page2Welcome.Parent = null;
            Page3Tasks.Parent = null;
            Page4Exit.Parent = null;
        }

        private void Page4Exit_Enter(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void LoginAuth_Click(object sender, EventArgs e) //авторизция
        {
            try
            {
                DataSet LogPassDS;

                LogPassDS = DatabaseWorker.ConnectAndRead(DBNamePath, "select * from Персонал where Фамилия = \"" + FamilyAuth.Text + "\" and Пароль = \"" + PassAuth.Text + "\"", ref dataAdapter);
                bool loginSuccessful = ((LogPassDS.Tables.Count > 0) && (LogPassDS.Tables[0].Rows.Count > 0));
                if (loginSuccessful)
                {

                    Page2Welcome.Parent = PersonalUserTabControl;
                    Page3Tasks.Parent = PersonalUserTabControl;
                    Page4Exit.Parent = PersonalUserTabControl;
                    Page1Authorization.Parent = null;
                    int PersonalCode = Convert.ToInt32(LogPassDS.Tables[0].Rows[0].ItemArray[1]);


                }
                else
                {
                    MessageBox.Show("Неправильная фамилия или пароль!", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LogPassDS.Clear();
                LogPassDS.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Page2Welcome_Enter(object sender, EventArgs e) //Загрузка приветственной формы
        {
            try
            {
                DataSet LogPassDS = DatabaseWorker.ConnectAndRead(DBNamePath, "select * from Персонал where Фамилия = \"" + FamilyAuth.Text + "\" and Пароль = \"" + PassAuth.Text + "\"", ref dataAdapter);

                PersonalCode = Convert.ToInt32(LogPassDS.Tables[0].Rows[0].ItemArray[1]);


                DataSet PersonalInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Персонал WHERE КодПерсонала = " + PersonalCode, ref dataAdapter);
                WelcomeLabel.Text = "Добро пожаловать, " + PersonalInfo.Tables[0].Rows[0].ItemArray[2].ToString() + " " + PersonalInfo.Tables[0].Rows[0].ItemArray[3].ToString() + " " + PersonalInfo.Tables[0].Rows[0].ItemArray[4].ToString() + "!";

                DataSet CallsInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Сообщения.КодСообщения, Сообщения.КодАвтора, Сообщения.КодОтвета, Персонал.КодПерсонала FROM Сообщения INNER JOIN(Номер INNER JOIN Персонал ON Номер.КодПерсонала = Персонал.КодПерсонала) ON Сообщения.КодНомера = Номер.КодНомера WHERE(((Сообщения.КодОтвета) = 0));", ref dataAdapter);

                if (CallsInfo.Tables[0].Rows.Count > 0)
                {
                    CountOfCalls.ForeColor = Color.Red;
                    CountOfCalls.Text = "Количество новых вызовов: " + CallsInfo.Tables[0].Rows.Count.ToString();
                }
                else
                {
                    CountOfCalls.Text = "Количество новых вызовов: " + CallsInfo.Tables[0].Rows.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Page3Tasks_Enter(object sender, EventArgs e) //Загрузка формы с вызовами
        {
            try
            {
                CallsWOAnswerDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Сообщения.КодНомера, Клиент.Фамилия, Клиент.Имя, Сообщения.ТекстСообщения, Услуги.НаименованиеУслуги, Сообщения.КодСообщения, Сообщения.КодУслуги, Клиент.КодБрони FROM Услуги INNER JOIN(Клиент INNER JOIN(Сообщения INNER JOIN Номер ON Сообщения.КодНомера = Номер.КодНомера) ON Клиент.КодБрони = Номер.КодБрони) ON Услуги.КодУслуги = Сообщения.КодУслуги WHERE(((Сообщения.КодОтвета) = 0));", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, CallsWOAnswerDataGridView, true);

                CallsANDAnswerDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Клиент.Фамилия, Клиент.Имя, Сообщения.ТекстСообщения, Услуги.НаименованиеУслуги, Сообщения.ОтветноеСообщение FROM Услуги INNER JOIN(Клиент INNER JOIN(Сообщения INNER JOIN Номер ON Сообщения.КодНомера = Номер.КодНомера) ON Клиент.КодБрони = Номер.КодБрони) ON Услуги.КодУслуги = Сообщения.КодУслуги WHERE(((Сообщения.КодОтвета) <> 0));", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, CallsANDAnswerDataGridView, true);
                StatusLabel.Text = "SQL-запрос выполнен. Таблица загружена успешно.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void SendMessage_Click(object sender, EventArgs e) //Отправка сообщения
        {
            try
            {
                if (MessageToSendTextbox.Text.Length == 0)
                {
                    MessageBox.Show("Отсутствует текст сообщения!", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (CallsWOAnswerDataGridView.Columns.Contains("КодНомера") == true)
                {
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Сообщения.КодНомера, Клиент.Фамилия, Клиент.Имя, Сообщения.ТекстСообщения, Услуги.НаименованиеУслуги, Сообщения.КодСообщения, Сообщения.КодУслуги, Клиент.КодБрони FROM Услуги INNER JOIN(Клиент INNER JOIN(Сообщения INNER JOIN Номер ON Сообщения.КодНомера = Номер.КодНомера) ON Клиент.КодБрони = Номер.КодБрони) ON Услуги.КодУслуги = Сообщения.КодУслуги WHERE(((Сообщения.КодОтвета) = 0));", ref dataAdapter);
                    DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Сообщения SET КодОтвета = 1, ОтветноеСообщение = '" + MessageToSendTextbox.Text + "' WHERE КодСообщения=" + CallsWOAnswerDataGridView.CurrentRow.Cells[5].Value);

                    DataSet PriceInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Стоимость FROM Услуги WHERE КодУслуги = " + CallsWOAnswerDataGridView.CurrentRow.Cells[6].Value, ref dataAdapter);

                    if (PriceInfo.Tables[0].Rows[0].ItemArray[0].ToString() == "0,0000")
                    {

                    }
                    else
                    {
                        DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Бронь SET Стоимость = Стоимость + " + PriceInfo.Tables[0].Rows[0].ItemArray[0].ToString() + " WHERE КодБрони=" + CallsWOAnswerDataGridView.CurrentRow.Cells[7].Value);
                    }

                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Сообщения.КодНомера, Клиент.Фамилия, Клиент.Имя, Сообщения.ТекстСообщения, Услуги.НаименованиеУслуги, Сообщения.КодСообщения, Сообщения.КодУслуги, Клиент.КодБрони FROM Услуги INNER JOIN(Клиент INNER JOIN(Сообщения INNER JOIN Номер ON Сообщения.КодНомера = Номер.КодНомера) ON Клиент.КодБрони = Номер.КодБрони) ON Услуги.КодУслуги = Сообщения.КодУслуги WHERE(((Сообщения.КодОтвета) = 0));", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, CallsWOAnswerDataGridView, true);

                    CallsANDAnswerDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Клиент.Фамилия, Клиент.Имя, Сообщения.ТекстСообщения, Услуги.НаименованиеУслуги, Сообщения.ОтветноеСообщение FROM Услуги INNER JOIN(Клиент INNER JOIN(Сообщения INNER JOIN Номер ON Сообщения.КодНомера = Номер.КодНомера) ON Клиент.КодБрони = Номер.КодБрони) ON Услуги.КодУслуги = Сообщения.КодУслуги WHERE(((Сообщения.КодОтвета) <> 0));", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, CallsANDAnswerDataGridView, true);

                    MessageToSendTextbox.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PersonalForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void GoToRequests_Click(object sender, EventArgs e)
        {
            PersonalUserTabControl.SelectedTab = Page3Tasks;
        }


        private void OpenENChat_Click_1(object sender, EventArgs e) //Открытие системы связи
        {
            try
            {
                DataSet PersonalInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Персонал WHERE КодПерсонала = " + PersonalCode, ref dataAdapter);
                ChatForm chatForm = new ChatForm();
                chatForm.userName = PersonalInfo.Tables[0].Rows[0].ItemArray[2].ToString();
                chatForm.Show();
                chatForm.Login();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenChat2_Click(object sender, EventArgs e) //Открытие системы связи 2
        {
            try
            {
                DataSet PersonalInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Персонал WHERE КодПерсонала = " + PersonalCode, ref dataAdapter);
                ChatForm chatForm = new ChatForm();
                chatForm.userName = PersonalInfo.Tables[0].Rows[0].ItemArray[2].ToString();
                chatForm.Show();
                chatForm.Login();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PersonalHelp_Click(object sender, EventArgs e)
        {
            try
            {
                NotepadForm notepadForm = new NotepadForm();
                notepadForm.OpenTextFile("ChestnutData/Help/AboutPersonal.NoteN");
                notepadForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
