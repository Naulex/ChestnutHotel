using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace ChestnutHotel
{
    public partial class AdministratorForm : Form
    {
        OleDbDataAdapter dataAdapter;
        DataSet DataSet = new DataSet();

        string DBNamePath = "ChestnutData/ChestnutDatabase.mdb";

        public AdministratorForm() //установка видимости вкладок
        {
            InitializeComponent();

            PageResidents2.Parent = null;
            PageMessages9.Parent = null;
            PageReservation3.Parent = null;
            PageStaff4.Parent = null;
            PageRooms5.Parent = null;
            PageServices6.Parent = null;
            PageExtra7.Parent = null;
            PageExit8.Parent = null;
            PageAdministrators9.Parent = null;
            StatusLabel.Visible = false;
            RemoveRecordButton.Visible = false;
            SaveTableButton.Visible = false;
            RefreshTableButton.Visible = false;
        }

        private void SaveTableButton_Click(object sender, EventArgs e) //Кнопка "Сохранить таблицу"
        {
            try
            {
                if (AdministratorTabControl.SelectedTab.Text == "Администраторы")
                {
                    DatabaseWorker.UpdateDatabase(DataSet, AdministratorsDataGridView, ref dataAdapter);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Администраторы", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Администраторы", ref dataAdapter), AdministratorsDataGridView);
                    StatusLabel.Text = "Таблица сохранена в БД.";
                    DataSet.Clear();
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Постояльцы")
                {
                    if (ResidentsDataGridView.ContainsFocus)
                    {
                        DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Клиент WHERE ВыселенЛи = 0", ref dataAdapter);
                        DatabaseWorker.UpdateDatabase(DataSet, ResidentsDataGridView, ref dataAdapter);
                        DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Клиент WHERE ВыселенЛи = 0", ref dataAdapter), ResidentsDataGridView);
                        DataSet.Clear();
                    }

                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Клиент WHERE ВыселенЛи = 1", ref dataAdapter);
                    DatabaseWorker.UpdateDatabase(DataSet, LeavedResidentsDataGridView, ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Клиент WHERE ВыселенЛи = 1", ref dataAdapter), LeavedResidentsDataGridView);

                    StatusLabel.Text = "Таблица сохранена в БД.";
                    DataSet.Clear();
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Сообщения")
                {
                    DatabaseWorker.UpdateDatabase(DataSet, MessagesDataGridView, ref dataAdapter);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Сообщения", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Сообщения", ref dataAdapter), MessagesDataGridView);
                    StatusLabel.Text = "Таблица сохранена в БД.";
                    DataSet.Clear();
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Брони")
                {

                    DatabaseWorker.UpdateDatabase(DataSet, ReservationDataGridView, ref dataAdapter);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь", ref dataAdapter), ReservationDataGridView);
                    DataSet.Clear();
                    DatabaseWorker.UpdateDatabase(DataSet, UnconfirmedReservations, ref dataAdapter);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь", ref dataAdapter), UnconfirmedReservations);

                    StatusLabel.Text = "Таблица сохранена в БД.";
                    DataSet.Clear();
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Персонал")
                {
                    DatabaseWorker.UpdateDatabase(DataSet, StaffDataGridView, ref dataAdapter);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Персонал", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Персонал", ref dataAdapter), StaffDataGridView);
                    StatusLabel.Text = "Таблица сохранена в БД.";
                    DataSet.Clear();
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Номера")
                {
                    DatabaseWorker.UpdateDatabase(DataSet, RoomsDataGridView, ref dataAdapter);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Номер", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Номер", ref dataAdapter), RoomsDataGridView);
                    StatusLabel.Text = "Таблица сохранена в БД.";
                    DataSet.Clear();
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Услуги")
                {
                    DatabaseWorker.UpdateDatabase(DataSet, ServicesDataGridView, ref dataAdapter);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Услуги", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Услуги", ref dataAdapter), ServicesDataGridView);
                    StatusLabel.Text = "Таблица сохранена в БД.";
                    DataSet.Clear();
                }
                else if (AdministratorTabControl.SelectedTab.Name == "PageExtra7")
                {

                    DatabaseWorker.UpdateDatabase(DataSet, ComplaintsWOAnswerDataGridView, ref dataAdapter);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Клиент.КодКлиента, Клиент.Фамилия, Клиент.Имя, Клиент.Отчество, Жалобы.КодСообщения, Жалобы.ТекстЖалобы, Номер.КодНомера, Жалобы.КодОтвета FROM Номер INNER JOIN(Жалобы INNER JOIN Клиент ON Жалобы.КодАвтора = Клиент.КодКлиента) ON Номер.КодБрони = Клиент.КодБрони WHERE(((Жалобы.КодОтвета) = 0));", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, ComplaintsWOAnswerDataGridView, true);
                    DataSet.Clear();

                    DatabaseWorker.UpdateDatabase(DataSet, ComplaintsANDAnswerDataGridView, ref dataAdapter);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Клиент.КодКлиента, Клиент.Фамилия, Клиент.Имя, Клиент.Отчество, Жалобы.КодСообщения, Жалобы.ТекстЖалобы, Номер.КодНомера, Жалобы.КодОтвета FROM Номер INNER JOIN(Жалобы INNER JOIN Клиент ON Жалобы.КодАвтора = Клиент.КодКлиента) ON Номер.КодБрони = Клиент.КодБрони WHERE(((Жалобы.КодОтвета) <> 0));", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, ComplaintsANDAnswerDataGridView, true);

                    StatusLabel.Text = "Таблица сохранена в БД.";
                    DataSet.Clear();
                }
                else
                {
                    MessageBox.Show("Не выбрана активная таблица для сохранения либо сохранение этой таблицы не поддерживатеся ", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                StatusLabel.Text = "Ошибка сохранения БД. Подробности в окне информации.";
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveRecordButton_Click(object sender, EventArgs e) //Кнопка "Удалить запись"
        {
            try
            {
                StatusLabel.Text = "Запись удалена. Не забудьте сохранить изменения.";
                if (AdministratorTabControl.SelectedTab.Text == "Администраторы")
                {
                    AdministratorsDataGridView.Rows.Remove(AdministratorsDataGridView.SelectedRows[0]);
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Постояльцы")
                {
                    if (ResidentsDataGridView.Focused)
                        ResidentsDataGridView.Rows.Remove(ResidentsDataGridView.SelectedRows[0]);
                    if (LeavedResidentsDataGridView.Focused)
                        LeavedResidentsDataGridView.Rows.Remove(LeavedResidentsDataGridView.SelectedRows[0]);
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Сообщения")
                {
                    MessagesDataGridView.Rows.Remove(MessagesDataGridView.SelectedRows[0]);
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Брони")
                {
                    if (ReservationDataGridView.Focused)
                        ReservationDataGridView.Rows.Remove(ReservationDataGridView.SelectedRows[0]);
                    if (UnconfirmedReservations.Focused)
                        UnconfirmedReservations.Rows.Remove(UnconfirmedReservations.SelectedRows[0]);
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Персонал")
                {
                    StaffDataGridView.Rows.Remove(StaffDataGridView.SelectedRows[0]);
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Номера")
                {
                    RoomsDataGridView.Rows.Remove(RoomsDataGridView.SelectedRows[0]);
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Услуги")
                {
                    ServicesDataGridView.Rows.Remove(ServicesDataGridView.SelectedRows[0]);
                }
                else if (AdministratorTabControl.SelectedTab.Name == "PageExtra7")
                {
                    if (ComplaintsWOAnswerDataGridView.Focused)
                        ComplaintsWOAnswerDataGridView.Rows.Remove(ComplaintsWOAnswerDataGridView.SelectedRows[0]);
                    if (ComplaintsANDAnswerDataGridView.Focused)
                        ComplaintsANDAnswerDataGridView.Rows.Remove(ComplaintsANDAnswerDataGridView.SelectedRows[0]);
                }

                else
                {
                    MessageBox.Show("Не выбрана активная таблица для сохранения либо удаление записей в этой таблице не поддерживатеся ", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                StatusLabel.Text = "Ошибка удаления записи из БД. Подробности в окне информации.";
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshTableButton_Click(object sender, EventArgs e) //Кнопка "Обновить таблицу"
        {
            try
            {
                if (AdministratorTabControl.SelectedTab.Text == "Администраторы")
                {
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Администраторы", ref dataAdapter), AdministratorsDataGridView);
                    StatusLabel.Text = "SQL-запрос выполнен, таблица обновлена";
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Постояльцы")
                {
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Клиент WHERE ВыселенЛи = 0", ref dataAdapter), ResidentsDataGridView);
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Клиент WHERE ВыселенЛи = 1", ref dataAdapter), LeavedResidentsDataGridView);
                    StatusLabel.Text = "SQL-запрос выполнен, таблица обновлена";
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Сообщения")
                {
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Сообщения", ref dataAdapter), MessagesDataGridView);
                    StatusLabel.Text = "SQL-запрос выполнен, таблица обновлена";
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Брони")
                {
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь where ОплаченаЛи = 0", ref dataAdapter), UnconfirmedReservations);
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь where ОплаченаЛи = 1 AND ПодтверждениеЗакрытия = 0", ref dataAdapter), ReservationDataGridView);
                    StatusLabel.Text = "SQL-запрос выполнен, таблица обновлена";
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Персонал")
                {
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Персонал", ref dataAdapter), StaffDataGridView);
                    StatusLabel.Text = "SQL-запрос выполнен, таблица обновлена";
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Номера")
                {
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Номер", ref dataAdapter), RoomsDataGridView);
                    StatusLabel.Text = "SQL-запрос выполнен, таблица обновлена";
                }
                else if (AdministratorTabControl.SelectedTab.Text == "Услуги")
                {
                    DatabaseWorker.RefreshDataGridView(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Услуги", ref dataAdapter), ServicesDataGridView);
                    StatusLabel.Text = "SQL-запрос выполнен, таблица обновлена";
                }
                else if (AdministratorTabControl.SelectedTab.Name == "PageExtra7")
                {
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Клиент.КодКлиента, Клиент.Фамилия, Клиент.Имя, Клиент.Отчество, Жалобы.КодСообщения, Жалобы.ТекстЖалобы, Номер.КодНомера, Жалобы.КодОтвета FROM Номер INNER JOIN(Жалобы INNER JOIN Клиент ON Жалобы.КодАвтора = Клиент.КодКлиента) ON Номер.КодБрони = Клиент.КодБрони WHERE(((Жалобы.КодОтвета) = 0));", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, ComplaintsWOAnswerDataGridView, true);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Клиент.КодКлиента, Клиент.Фамилия, Клиент.Имя, Клиент.Отчество, Жалобы.КодСообщения, Жалобы.ТекстЖалобы, Номер.КодНомера, Жалобы.КодОтвета FROM Номер INNER JOIN(Жалобы INNER JOIN Клиент ON Жалобы.КодАвтора = Клиент.КодКлиента) ON Номер.КодБрони = Клиент.КодБрони WHERE(((Жалобы.КодОтвета) <> 0));", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, ComplaintsANDAnswerDataGridView, true);
                    StatusLabel.Text = "SQL-запрос выполнен, таблица обновлена";
                }
                else
                {
                    MessageBox.Show("Не выбрана активная таблица для обновления либо обновление в этой таблице не поддерживатеся", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                StatusLabel.Text = "Ошибка обновления записи в БД. Подробности в окне информации.";
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResidentsPage_Enter(object sender, EventArgs e) //Загрузка таблицы с клиентами
        {
            try
            {
                ResidentsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Клиент WHERE ВыселенЛи = 0", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, ResidentsDataGridView);

                LeavedResidentsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Клиент WHERE ВыселенЛи = 1", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, LeavedResidentsDataGridView);


                StatusLabel.Text = "SQL-запрос выполнен. Таблица загружена успешно.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExitPage_Enter(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void AuthorizationLoginButton_Click(object sender, EventArgs e) //Окно авторизации
        {
            try
            {
                DataSet LogPassDS;

                LogPassDS = DatabaseWorker.ConnectAndRead(DBNamePath, "select * from Администраторы where Фамилия = \"" + AuthorizationLogin.Text + "\" and Пароль = \"" + AuthorizationPassword.Text + "\"", ref dataAdapter);
                bool loginSuccessful = ((LogPassDS.Tables.Count > 0) && (LogPassDS.Tables[0].Rows.Count > 0));
                if (loginSuccessful)
                {
                    PageAdministrators9.Parent = AdministratorTabControl;
                    PageResidents2.Parent = AdministratorTabControl;
                    PageMessages9.Parent = AdministratorTabControl;
                    PageReservation3.Parent = AdministratorTabControl;
                    PageStaff4.Parent = AdministratorTabControl;
                    PageRooms5.Parent = AdministratorTabControl;
                    PageServices6.Parent = AdministratorTabControl;
                    PageExtra7.Parent = AdministratorTabControl;
                    PageExit8.Parent = AdministratorTabControl;
                    PageAuthorization1.Parent = null;

                    StatusLabel.Visible = true;
                    ManualMode.Visible = true;
                    RemoveRecordButton.Visible = true;
                    SaveTableButton.Visible = true;
                    RefreshTableButton.Visible = true;
                    AboutAdmin.Visible = true;
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

        private void PageAdministrators9_Enter(object sender, EventArgs e) //Вкладка с администраторами
        {
            try
            {
                AdministratorsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Администраторы", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, AdministratorsDataGridView);
                StatusLabel.Text = "SQL-запрос выполнен. Таблица загружена успешно.";


                DataSet Newcomplaint = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Жалобы WHERE КодОтвета = 0", ref dataAdapter);
                if (Newcomplaint.Tables[0].Rows.Count != 1 || Newcomplaint.Tables[0].Rows.Count != 0)
                {
                    PageExtra7.Text = "Жалобы (" + Newcomplaint.Tables[0].Rows.Count + ")";
                }

                DataSet OutdatedReservations = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь WHERE(((Бронь.ДатаВыселения) < Now()) AND((Бронь.ПодтверждениеЗакрытия) = 0));", ref dataAdapter);
                if (OutdatedReservations.Tables[0].Rows.Count != 0)
                {
                    DialogResult result = MessageBox.Show("Система обнаружила " + OutdatedReservations.Tables[0].Rows.Count + " истёкших, но не аннулированных броней. Проверьте их состояние на вкладке \"Брони\", либо нажмите \"Да\", чтобы аннулировать их автоматически.", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Бронь SET ПодтверждениеЗакрытия = 1 WHERE(((Бронь.ДатаВыселения) < Now()) AND((Бронь.ПодтверждениеЗакрытия) = 0))");
                        MessageBox.Show("Указанное число броней аннулировано успешно. Если некоторые из них не были подтверждены, формальную процедуру подтверждения нужно провести вручную. См. вкладку \"Брони\".", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PageMessages9_Enter(object sender, EventArgs e) //Вкладка с сообщениями
        {
            try
            {
                MessagesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Сообщения", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, MessagesDataGridView);
                StatusLabel.Text = "SQL-запрос выполнен. Таблица загружена успешно.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PageReservation3_Enter(object sender, EventArgs e) //Вкладка с бронью
        {
            try
            {
                UnconfirmedReservations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь where ОплаченаЛи = 0", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, UnconfirmedReservations);

                ReservationDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь where ОплаченаЛи = 1 AND ПодтверждениеЗакрытия = 0", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, ReservationDataGridView);
                StatusLabel.Text = "SQL-запрос выполнен. Таблица загружена успешно.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PageRooms5_Enter(object sender, EventArgs e) //Вкладка с номерами
        {
            try
            {
                RoomsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Номер", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, RoomsDataGridView);
                AllStaff.Items.Clear();
                DataSet AllStaffDS = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT DISTINCT Персонал.КодПерсонала, Персонал.Фамилия, Персонал.Имя, Персонал.Отчество, СписокДолжностей.НазваниеДолжности FROM Персонал INNER JOIN СписокДолжностей ON Персонал.Должность = СписокДолжностей.КодДолжности WHERE(((Персонал.УволенЛи) <> 1));", ref dataAdapter);

                List<string> strDetailIDList = new List<string>();
                foreach (DataRow row in AllStaffDS.Tables[0].Rows)
                {
                    strDetailIDList.Add(row["Фамилия"].ToString() + " " + row["Имя"].ToString() + " " + row["Отчество"].ToString() + " (" + row["НазваниеДолжности"].ToString() + ")");
                }
                AllStaff.Items.AddRange(strDetailIDList.Distinct().ToArray());
                if (strDetailIDList.Count == 0)
                {
                    AllStaff.Text = "--Персонал--";
                }
                else
                { AllStaff.SelectedIndex = 0; }

                AllRoomsWOStaff.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодНомера, Название, Этаж FROM Номер WHERE КодПерсонала = 0", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, AllRoomsWOStaff);

                UdobstvaRoom.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet AdditionalInventoryDataset = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодУдобства, НаименованиеУдобства, Стоимость FROM Удобства;", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(AdditionalInventoryDataset, UdobstvaRoom);
                UdobstvaRoom.Rows[0].Selected = true;

                DataSet AllRoomClasses = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT DISTINCT КодКлассаНомера, КлассНомера FROM КлассНомера", ref dataAdapter);

                List<string> strDetailIDList1 = new List<string>();
                foreach (DataRow row in AllRoomClasses.Tables[0].Rows)
                {
                    strDetailIDList1.Add(row["КлассНомера"].ToString());
                }
                ClassOfRoomsComboBox.Items.Clear();
                ClassOfRoomsComboBox.Items.AddRange(strDetailIDList1.Distinct().ToArray());
                if (strDetailIDList1.Count == 0)
                {
                    ClassOfRoomsComboBox.Text = "--Класс номера--";
                }
                else
                { ClassOfRoomsComboBox.SelectedIndex = 0; }


                StatusLabel.Text = "SQL-запрос выполнен. Таблица загружена успешно.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddRoom_Click(object sender, EventArgs e) //Добавление номера
        {
            try
            {
                int RoomCode = Convert.ToInt32(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT MAX(КодНомера) FROM Номер", ref dataAdapter).Tables[0].Rows[0].ItemArray[0]);
                RoomCode++;

                int RoomClassCode = Convert.ToInt32(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT DISTINCT КодКлассаНомера FROM КлассНомера WHERE КлассНомера = '" + ClassOfRoomsComboBox.SelectedItem.ToString() + "';", ref dataAdapter).Tables[0].Rows[0].ItemArray[0]);

                DatabaseWorker.WriteInDatabase(DBNamePath, "INSERT INTO Номер (КодНомера, КодКлассаНомера, Название, Этаж, Описание, Изображение, БазоваяСтоимость) VALUES (" + RoomCode + ", " + RoomClassCode + ", '" + RoomName.Text + "', " + RoomFloor.Value + ", '" + RoomDesc.Text + "', '" + RoomPic.Text + "', " + RoomPrice.Value + ")");

                for (int i = 0; i < UdobstvaRoom.SelectedRows.Count; i++)
                {
                    DatabaseWorker.WriteInDatabase(DBNamePath, "INSERT INTO СвязьУдобств (КодНомера, КодУдобства) VALUES (" + RoomCode + ", " + UdobstvaRoom.SelectedRows[i].Cells["КодУдобства"].Value.ToString() + ")");
                }


                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Номер", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, RoomsDataGridView);

                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодНомера, Название, Этаж FROM Номер WHERE КодПерсонала = 0", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, AllRoomsWOStaff);

                MessageBox.Show("Номер добавлен успешно!", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void PageStaff4_Enter(object sender, EventArgs e) //Вкладка с персоналом
        {
            try
            {
                StaffDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Персонал", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, StaffDataGridView);

                PersonalFreeRooms.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet Dataset2 = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодНомера, Название, Этаж FROM Номер WHERE КодПерсонала = 0", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(Dataset2, PersonalFreeRooms);

                DataSet AllStaffWorks = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT DISTINCT НазваниеДолжности FROM СписокДолжностей", ref dataAdapter);
                PersonalPosition.Items.Clear();
                List<string> strDetailIDList = new List<string>();
                foreach (DataRow row in AllStaffWorks.Tables[0].Rows)
                {
                    strDetailIDList.Add(row["НазваниеДолжности"].ToString());
                }
                PersonalPosition.Items.AddRange(strDetailIDList.Distinct().ToArray());
                if (strDetailIDList.Count == 0)
                {
                    PersonalPosition.Text = "--Должность--";
                }
                else
                { PersonalPosition.SelectedIndex = 0; }


                StatusLabel.Text = "SQL-запрос выполнен. Таблица загружена успешно.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PageServices6_Enter(object sender, EventArgs e) //Вкладка с услугами
        {
            try
            {
                ServicesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Услуги", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, ServicesDataGridView);
                StatusLabel.Text = "SQL-запрос выполнен. Таблица загружена успешно.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ShowAllBroines_Click(object sender, EventArgs e) //Все брони постояльца
        {
            try
            {
                if (ResidentsDataGridView.Columns.Contains("КодКлиента") == true && ResidentsDataGridView.Rows.Count != 1)
                {
                    int ID = Convert.ToInt32(ResidentsDataGridView.Rows[ResidentsDataGridView.CurrentRow.Index].Cells[1].Value);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь WHERE КодКлиента = " + ID, ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, ResidentsDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowAllMessages_Click(object sender, EventArgs e) //Все сообщения постояльца
        {
            try
            {
                if (MessagesDataGridView.Columns.Contains("КодАвтора") == true && MessagesDataGridView.Rows.Count != 1)
                {
                    int ID = Convert.ToInt32(MessagesDataGridView.Rows[MessagesDataGridView.CurrentRow.Index].Cells[2].Value);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Сообщения WHERE КодАвтора = " + ID, ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, MessagesDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FullAuthorName_Click(object sender, EventArgs e) //Имя посетителя по номеру брони
        {
            try
            {
                if (MessagesDataGridView.Columns.Contains("КодАвтора") == true && MessagesDataGridView.Rows.Count != 1)
                {

                    int ID = Convert.ToInt32(MessagesDataGridView.Rows[MessagesDataGridView.CurrentRow.Index].Cells[2].Value);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодКлиента, Фамилия, Имя, Отчество FROM Клиент WHERE КодКлиента = " + ID, ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, MessagesDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void WhoIsBroni_Click(object sender, EventArgs e) //Чья бронь?
        {
            try
            {
                if (ReservationDataGridView.Columns.Contains("КодБрони") == true && ReservationDataGridView.Rows.Count != 1)
                {
                    int ID = Convert.ToInt32(ReservationDataGridView.Rows[ReservationDataGridView.CurrentRow.Index].Cells[2].Value);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодКлиента, Фамилия, Имя, Отчество FROM Клиент WHERE КодКлиента = " + ID, ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, ReservationDataGridView);
                }
                if (UnconfirmedReservations.Columns.Contains("КодБрони") == true && UnconfirmedReservations.Rows.Count != 1)
                {
                    int ID = Convert.ToInt32(UnconfirmedReservations.Rows[UnconfirmedReservations.CurrentRow.Index].Cells["КодБрони"].Value);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодКлиента, Фамилия, Имя, Отчество FROM Клиент WHERE КодКлиента = " + ID, ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, UnconfirmedReservations);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RoomInfo_Click(object sender, EventArgs e) //Информация о брони
        {
            try
            {
                if (ReservationDataGridView.Columns.Contains("КодНомера") == true && ReservationDataGridView.Rows.Count != 1)
                {
                    int ID = Convert.ToInt32(ReservationDataGridView.Rows[ReservationDataGridView.CurrentRow.Index].Cells[3].Value);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Номер WHERE КодНомера = " + ID, ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, ReservationDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RoomServiceWho_Click(object sender, EventArgs e) //Кто обслуживает номер?
        {
            try
            {
                if (StaffDataGridView.Columns.Contains("КодПерсонала") == true && StaffDataGridView.Rows.Count != 1)
                {
                    int ID = Convert.ToInt32(StaffDataGridView.Rows[StaffDataGridView.CurrentRow.Index].Cells[1].Value);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Номер WHERE КодПерсонала = " + ID, ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, StaffDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WhoLiveInRoom_Click(object sender, EventArgs e) //Кто проживает в номере?
        {
            try
            {
                if (RoomsDataGridView.Columns.Contains("КодНомера") == true && RoomsDataGridView.Rows.Count != 1)
                {
                    int ID = Convert.ToInt32(RoomsDataGridView.Rows[RoomsDataGridView.CurrentRow.Index].Cells[5].Value);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Клиент.Код, Клиент.Фамилия, Клиент.Имя, Клиент.Отчество, Клиент.КодБрони FROM Клиент INNER JOIN Номер ON Клиент.КодБрони = Номер.КодБрони WHERE(((Клиент.КодБрони) =" + ID + "));", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, RoomsDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WhoServiceRoom_Click(object sender, EventArgs e) //Кто обслуживает номер?
        {
            try
            {
                if (RoomsDataGridView.Columns.Contains("Название") == true && RoomsDataGridView.Rows.Count != 1)
                {
                    int ID = Convert.ToInt32(RoomsDataGridView.Rows[RoomsDataGridView.CurrentRow.Index].Cells[4].Value);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Персонал WHERE КодПерсонала = " + ID, ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, RoomsDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BroniHistory_Click(object sender, EventArgs e) //История брони
        {
            try
            {
                if (RoomsDataGridView.Columns.Contains("КодНомера") == true && RoomsDataGridView.Rows.Count != 1)
                {
                    int ID = Convert.ToInt32(RoomsDataGridView.Rows[RoomsDataGridView.CurrentRow.Index].Cells[1].Value);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь WHERE КодНомера = " + ID, ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, RoomsDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdministratorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ConfirmReservation_Click(object sender, EventArgs e) //Подтверждение брони
        {
            try
            {
                if (UnconfirmedReservations.Columns.Contains("ОплаченаЛи") == true && UnconfirmedReservations.Rows.Count != 1)
                {
                    if (UserPassportData.Text != "")
                    {
                        int ID = Convert.ToInt32(UnconfirmedReservations.Rows[UnconfirmedReservations.CurrentRow.Index].Cells["КодБрони"].Value);
                        DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Бронь SET ОплаченаЛи = 1 WHERE КодБрони = " + ID);
                        DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Клиент SET ПаспортныеДанные = '" + UserPassportData.Text + "' WHERE КодБрони = " + ID);

                        UnconfirmedReservations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь where ОплаченаЛи = 0", ref dataAdapter);
                        DatabaseWorker.RefreshDataGridView(DataSet, UnconfirmedReservations);

                        ReservationDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь where ОплаченаЛи = 1 AND ПодтверждениеЗакрытия = 0", ref dataAdapter);
                        DatabaseWorker.RefreshDataGridView(DataSet, ReservationDataGridView);
                        UserPassportData.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: не внесены паспортные данные!", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MakeZeroToReservation_Click(object sender, EventArgs e) //Обнуление брони
        {
            try
            {
                if (ReservationDataGridView.Columns.Contains("ПодтверждениеЗакрытия") == true && ReservationDataGridView.Rows.Count != 1)
                {
                    int ID = Convert.ToInt32(ReservationDataGridView.Rows[ReservationDataGridView.CurrentRow.Index].Cells["КодБрони"].Value);
                    DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Бронь SET ПодтверждениеЗакрытия = 1 WHERE КодБрони = " + ID);

                    UnconfirmedReservations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь where ОплаченаЛи = 0", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, UnconfirmedReservations);

                    ReservationDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь where ОплаченаЛи = 1 AND ПодтверждениеЗакрытия = 0", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, ReservationDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PersonalAddBtn_Click(object sender, EventArgs e) //Добавление персонала
        {
            try
            {
                if (PersonalFamily.Text != "" && PersonalName.Text != "" && PersonalFatherName.Text != "" && PersonalPosition.Text != "")
                {
                    string password = PersonalPassword.Text;
                    if (PersonalPassword.Text == "")
                    {
                        Random x = new Random();
                        password = x.Next(1000, 9999).ToString();
                    }
                    int PersonalCode = Convert.ToInt32(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT MIN(КодПерсонала) FROM Персонал", ref dataAdapter).Tables[0].Rows[0].ItemArray[0]);
                    PersonalCode++;

                    int PositionCode = Convert.ToInt32(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT DISTINCT КодДолжности FROM СписокДолжностей WHERE НазваниеДолжности = '" + PersonalPosition.SelectedItem.ToString() + "';", ref dataAdapter).Tables[0].Rows[0].ItemArray[0]);


                    DatabaseWorker.WriteInDatabase(DBNamePath, "INSERT INTO Персонал (КодПерсонала, Фамилия, Имя, Отчество, Должность, Пароль) VALUES (" + PersonalCode + ", '" + PersonalFamily.Text + "', '" + PersonalName.Text + "', '" + PersonalFatherName.Text + "', '" + PositionCode + "', '" + password + "')");

                    for (int i = 0; i < PersonalFreeRooms.SelectedRows.Count; i++)
                    {
                        DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Номер SET КодПерсонала = " + PersonalCode + " WHERE КодНомера = " + PersonalFreeRooms.SelectedRows[i].Cells["КодНомера"].Value.ToString());
                    }

                    MessageBox.Show("Работник добавлен успешно. Логин - фамилия работника, пароль: " + password, "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    StaffDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Персонал", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, StaffDataGridView);

                    PersonalFreeRooms.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    DataSet Dataset2 = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодНомера, Название, Этаж FROM Номер WHERE КодПерсонала = 0", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(Dataset2, PersonalFreeRooms);
                }
                else
                {
                    MessageBox.Show("Ошибка: не заполнено одно или несколько полей!", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeletePersonal_Click(object sender, EventArgs e) //Удаление персонала
        {
            try
            {
                DialogResult result = MessageBox.Show("Вы действительно хотите удалить этих сотрудников?", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
                else if (result == DialogResult.Yes)
                {
                    Random newpassrnd = new Random();
                    for (int i = 0; i < StaffDataGridView.SelectedRows.Count; i++)
                    {
                        int newpassword = newpassrnd.Next(1000000, 9999999);
                        DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Персонал SET УволенЛи = 1, Пароль = '" + newpassword + "' WHERE КодПерсонала = " + StaffDataGridView.SelectedRows[i].Cells["КодПерсонала"].Value.ToString());

                        DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Номер SET КодПерсонала = 0 WHERE КодПерсонала = " + StaffDataGridView.SelectedRows[i].Cells["КодПерсонала"].Value.ToString());
                    }
                    MessageBox.Show("Указанный персонал уволен! За некоторыми номерами теперь не закреплен ни один человек! Проверьте состояние на вкладке \"Номера\"!", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    StaffDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Персонал", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, StaffDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void ShowOnlyDeleted_Click(object sender, EventArgs e) //Показать уволенных
        {
            try
            {
                if (StaffDataGridView.Columns.Contains("УволенЛи") == true)
                {
                    StaffDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Персонал WHERE УволенЛи = 1", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, StaffDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddStaffToRoom_Click(object sender, EventArgs e) //Закрепить человека за номером
        {
            try
            {
                DataSet AllStaffDS = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT DISTINCT КодПерсонала, Фамилия, Имя, Отчество, Должность FROM Персонал WHERE УволенЛи <> 1;", ref dataAdapter);
                for (int i = 0; i < AllRoomsWOStaff.SelectedRows.Count; i++)
                {
                    DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Номер SET КодПерсонала = " + AllStaffDS.Tables[0].Rows[AllStaff.SelectedIndex].ItemArray[0] + " WHERE КодНомера = " + AllRoomsWOStaff.SelectedRows[i].Cells["КодНомера"].Value.ToString());
                }

                AllRoomsWOStaff.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодНомера, Название, Этаж FROM Номер WHERE КодПерсонала = 0", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, AllRoomsWOStaff);

                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Номер", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, RoomsDataGridView);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveStaffFromRoom_Click(object sender, EventArgs e) //Открепить человека от номера
        {
            try
            {
                for (int i = 0; i < RoomsDataGridView.SelectedRows.Count; i++)
                {
                    DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Номер SET КодПерсонала = 0 WHERE КодНомера = " + RoomsDataGridView.SelectedRows[i].Cells["КодНомера"].Value.ToString());

                }
                RoomsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Номер", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, RoomsDataGridView);

                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодНомера, Название, Этаж FROM Номер WHERE КодПерсонала = 0", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, AllRoomsWOStaff);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MessagesWoAnswer_Click(object sender, EventArgs e) //Сообщения без ответа
        {
            try
            {
                if (MessagesDataGridView.Columns.Contains("КодОтвета") == true)
                {
                    MessagesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Сообщения WHERE КодОтвета = 0", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, MessagesDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MEssagesWAnswer_Click(object sender, EventArgs e) //Сообщения с ответом
        {
            try
            {
                if (MessagesDataGridView.Columns.Contains("КодОтвета") == true)
                {
                    MessagesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Сообщения WHERE КодОтвета <> 0", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, MessagesDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AllClosedReservations_Click(object sender, EventArgs e) //Все закрытые брони
        {
            try
            {
                if (ReservationDataGridView.Columns.Contains("ПодтверждениеЗакрытия") == true)
                {
                    ReservationDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь where ОплаченаЛи = 1 AND ПодтверждениеЗакрытия = 1", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, ReservationDataGridView);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PageExtra7_Enter(object sender, EventArgs e) //Вкладка жалоб
        {
            try
            {
                ComplaintsWOAnswerDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Клиент.КодКлиента, Клиент.Фамилия, Клиент.Имя, Клиент.Отчество, Жалобы.КодСообщения, Жалобы.ТекстЖалобы, Номер.КодНомера, Жалобы.КодОтвета FROM Номер INNER JOIN(Жалобы INNER JOIN Клиент ON Жалобы.КодАвтора = Клиент.КодКлиента) ON Номер.КодБрони = Клиент.КодБрони WHERE(((Жалобы.КодОтвета) = 0));", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, ComplaintsWOAnswerDataGridView, true);

                ComplaintsANDAnswerDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Клиент.КодКлиента, Клиент.Фамилия, Клиент.Имя, Клиент.Отчество, Жалобы.КодСообщения, Жалобы.ТекстЖалобы, Номер.КодНомера, Жалобы.КодОтвета FROM Номер INNER JOIN(Жалобы INNER JOIN Клиент ON Жалобы.КодАвтора = Клиент.КодКлиента) ON Номер.КодБрони = Клиент.КодБрони WHERE(((Жалобы.КодОтвета) <> 0));", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, ComplaintsANDAnswerDataGridView, true);
                StatusLabel.Text = "SQL-запрос выполнен. Таблица загружена успешно.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendMessage_Click(object sender, EventArgs e) //Отправить ответ на жалобу
        {
            try
            {
                if (MessageToSendTextbox.Text.Length == 0)
                {
                    MessageBox.Show("Отсутствует текст сообщения!", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (ComplaintsWOAnswerDataGridView.Columns.Contains("КодСообщения") == true && ComplaintsWOAnswerDataGridView.Rows.Count != 1)
                {
                    DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Жалобы SET КодОтвета = 1, ТекстОтвета = '" + MessageToSendTextbox.Text + "' WHERE КодСообщения=" + ComplaintsWOAnswerDataGridView.CurrentRow.Cells["КодСообщения"].Value);
                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Клиент.КодКлиента, Клиент.Фамилия, Клиент.Имя, Клиент.Отчество, Жалобы.КодСообщения, Жалобы.ТекстЖалобы, Номер.КодНомера, Жалобы.КодОтвета FROM Номер INNER JOIN(Жалобы INNER JOIN Клиент ON Жалобы.КодАвтора = Клиент.КодКлиента) ON Номер.КодБрони = Клиент.КодБрони WHERE(((Жалобы.КодОтвета) = 0));", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, ComplaintsWOAnswerDataGridView, true);

                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Клиент.КодКлиента, Клиент.Фамилия, Клиент.Имя, Клиент.Отчество, Жалобы.КодСообщения, Жалобы.ТекстЖалобы, Номер.КодНомера, Жалобы.КодОтвета FROM Номер INNER JOIN(Жалобы INNER JOIN Клиент ON Жалобы.КодАвтора = Клиент.КодКлиента) ON Номер.КодБрони = Клиент.КодБрони WHERE(((Жалобы.КодОтвета) <> 0));", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, ComplaintsANDAnswerDataGridView, true);
                    MessageToSendTextbox.Clear();

                    DataSet Newcomplaint = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Жалобы WHERE КодОтвета = 0", ref dataAdapter);
                    if (Newcomplaint.Tables[0].Rows.Count != 1)
                    {
                        PageExtra7.Text = "Жалобы (" + Newcomplaint.Tables[0].Rows.Count + ")";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EvictClient_Click(object sender, EventArgs e) //выселить клиента
        {
            try
            {
                if (ResidentsDataGridView.Columns.Contains("КодКлиента") == true && ResidentsDataGridView.Rows.Count != 1)
                {
                    DialogResult result = MessageBox.Show("Вы действительно хотите выселить этого постояльца? Бронь нужно будет аннулировать отдельно.", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        DataSet ClientInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Фамилия, Имя, Отчество, Пароль, КодКлиента FROM Клиент WHERE Фамилия LIKE '" + ResidentsDataGridView.Rows[ResidentsDataGridView.CurrentRow.Index].Cells["Фамилия"].Value.ToString() + "' AND Пароль = '" + ResidentsDataGridView.Rows[ResidentsDataGridView.CurrentRow.Index].Cells["Пароль"].Value.ToString() + "'", ref dataAdapter);

                        int UserId = Convert.ToInt32(ClientInfo.Tables[0].Rows[0].ItemArray[4].ToString());
                        DataSet ReservationInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодБрони, КодНомера, Стоимость, ДатаЗаселения, ДатаВыселения FROM Бронь WHERE КодКлиента = " + UserId, ref dataAdapter);

                        string LeaveStringInfo = "";
                        LeaveStringInfo += "Всего доброго, " + ClientInfo.Tables[0].Rows[0].ItemArray[0].ToString() + " " + ClientInfo.Tables[0].Rows[0].ItemArray[1].ToString() + " " + ClientInfo.Tables[0].Rows[0].ItemArray[2].ToString() + "!\r\n";
                        LeaveStringInfo += "\r\n\r\nВаша бронь:\r\nДата заселения: " + ReservationInfo.Tables[0].Rows[0].ItemArray[3].ToString();
                        LeaveStringInfo += "\r\nЗапланированная дата выселения: " + ReservationInfo.Tables[0].Rows[0].ItemArray[4].ToString();
                        LeaveStringInfo += "\r\nФактическая дата выселения: " + DateTime.Now.ToString();
                        LeaveStringInfo += "\r\nСтоимость: " + ReservationInfo.Tables[0].Rows[0].ItemArray[2].ToString() + " рублей.";

                        int ReservationId = Convert.ToInt32(ReservationInfo.Tables[0].Rows[0].ItemArray[0].ToString());
                        DataSet RoomInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодНомера, КодУдобств, КодКлассаНомера, КодБрони, Название, Этаж, Описание, Изображение, БазоваяСтоимость, КодПерсонала FROM Номер WHERE КодБрони = " + ReservationId, ref dataAdapter);

                        if (DateTime.Now.ToShortDateString() != ReservationInfo.Tables[0].Rows[0].ItemArray[4].ToString())
                        {
                            DateTime d1 = Convert.ToDateTime(ReservationInfo.Tables[0].Rows[0].ItemArray[4]);
                            DateTime d2 = Convert.ToDateTime(DateTime.Now);
                            TimeSpan time = d1 - d2;

                            decimal AccuratePrice = Convert.ToDecimal(ReservationInfo.Tables[0].Rows[0].ItemArray[2]) - (Convert.ToDecimal(RoomInfo.Tables[0].Rows[0].ItemArray[8]) * Convert.ToDecimal(time.Days));
                            LeaveStringInfo += "\r\nСтоимость с учетом перерасчета: " + AccuratePrice + " рублей.";
                        }

                        LeaveStringInfo += "\r\n\r\nИнформация о номере:\r\nНазвание номера: " + RoomInfo.Tables[0].Rows[0].ItemArray[4].ToString();
                        LeaveStringInfo += "\r\nЭтаж: " + RoomInfo.Tables[0].Rows[0].ItemArray[5].ToString();
                        LeaveStringInfo += "\r\nОписание: " + RoomInfo.Tables[0].Rows[0].ItemArray[6].ToString();
                        LeaveStringInfo += "\r\nБазовая стоимость: " + RoomInfo.Tables[0].Rows[0].ItemArray[8].ToString();

                        int PersonalId = Convert.ToInt32(RoomInfo.Tables[0].Rows[0].ItemArray[9].ToString());
                        DataSet PersonalInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Персонал.Фамилия, Персонал.Имя, Персонал.Отчество, СписокДолжностей.НазваниеДолжности, Персонал.КодПерсонала FROM Персонал INNER JOIN СписокДолжностей ON Персонал.Должность = СписокДолжностей.КодДолжности WHERE(((Персонал.КодПерсонала) = " + PersonalId + "));", ref dataAdapter);

                        LeaveStringInfo += "\r\n\r\nВас обслуживали: " + PersonalInfo.Tables[0].Rows[0].ItemArray[3].ToString();
                        LeaveStringInfo += "\r\nФамилия: " + PersonalInfo.Tables[0].Rows[0].ItemArray[0].ToString();
                        LeaveStringInfo += "\r\nИмя: " + PersonalInfo.Tables[0].Rows[0].ItemArray[1].ToString();
                        LeaveStringInfo += "\r\nОтчество: " + PersonalInfo.Tables[0].Rows[0].ItemArray[2].ToString();
                        LeaveStringInfo += "\r\nСпасибо, что выбрали Каштан!";





                        DialogResult result1 = MessageBox.Show("Отчет о выселении:\r\n" + LeaveStringInfo + "\r\n\r\n\r\nСохранить отчет в файл?", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result1 == DialogResult.No)
                        {
                            try
                            {
                                DeleteClient();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (result1 == DialogResult.Yes)
                        {
                            try
                            {
                                StreamWriter SW = new StreamWriter(new FileStream("ОтчетОВыселении.rtf", FileMode.Create, FileAccess.Write));
                                SW.Write(LeaveStringInfo);
                                SW.Close();


                                DeleteClient();
                                MessageBox.Show("Сохранено в файл \"ОтчетОВыселении.rtf\"", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteClient() //Удалить клиента из БД
        {
            try
            {
                DataSet ClientInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодКлиента, КодБрони FROM Клиент WHERE Фамилия LIKE '" + ResidentsDataGridView.Rows[ResidentsDataGridView.CurrentRow.Index].Cells["Фамилия"].Value.ToString() + "' AND Пароль = '" + ResidentsDataGridView.Rows[ResidentsDataGridView.CurrentRow.Index].Cells["Пароль"].Value.ToString() + "'", ref dataAdapter);
                int UserId = Convert.ToInt32(ClientInfo.Tables[0].Rows[0].ItemArray[0].ToString());
                int ReservationID = Convert.ToInt32(ClientInfo.Tables[0].Rows[0].ItemArray[1].ToString());

                DataSet ReservationCode = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Номер.КодНомера FROM Номер INNER JOIN Бронь ON Номер.КодНомера = Бронь.КодНомера WHERE(((Бронь.КодБрони) = " + ReservationID + "));", ref dataAdapter);

                int RoomCode = Convert.ToInt32(ReservationCode.Tables[0].Rows[0].ItemArray[0].ToString());

                Random newpassrnd = new Random();
                int newpassword = newpassrnd.Next(1000000, 9999999);
                DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Клиент SET Пароль =" + newpassword + ", ВыселенЛи = 1 WHERE КодКлиента=" + UserId);
                DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Номер SET КодБрони = 0 WHERE КодНомера=" + RoomCode);
                DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Бронь SET КодБрони = 0, ФактическаяДата = '" + DateTime.Now.ToShortDateString() + "' WHERE КодНомера=" + RoomCode + " AND КодКлиента= " + UserId);


                ResidentsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Клиент WHERE ВыселенЛи = 0", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, ResidentsDataGridView);

                LeavedResidentsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Клиент WHERE ВыселенЛи = 1", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, LeavedResidentsDataGridView);

            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteRoom_Click(object sender, EventArgs e) //Удалить номер из БД
        {
            if (RoomsDataGridView.Columns.Contains("КодНомера") == true && RoomsDataGridView.Rows.Count != 1)
            {
                DialogResult result = MessageBox.Show("Вы действительно хотите удалить информацию об этом номере? Если этот номер был забронирован, брони нужно будет аннулировать отдельно.", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.No)
                {
                    return;
                }
                else if (result == DialogResult.Yes)
                {

                    int RoomCode = Convert.ToInt32(RoomsDataGridView.Rows[RoomsDataGridView.CurrentRow.Index].Cells["КодНомера"].Value);


                    DatabaseWorker.WriteInDatabase(DBNamePath, "DELETE FROM Номер WHERE КодНомера = " + RoomCode);

                    DatabaseWorker.WriteInDatabase(DBNamePath, "DELETE FROM СвязьУдобств WHERE КодНомера = " + RoomCode);


                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Номер", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, RoomsDataGridView);

                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодНомера, Название, Этаж FROM Номер WHERE КодПерсонала = 0", ref dataAdapter);
                    DatabaseWorker.RefreshDataGridView(DataSet, AllRoomsWOStaff);

                    MessageBox.Show("Номер удален успешно!", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private void AboutAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                NotepadForm notepadForm = new NotepadForm();
                notepadForm.OpenTextFile("ChestnutData/Help/AboutAdmin.NoteN");
                notepadForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}

