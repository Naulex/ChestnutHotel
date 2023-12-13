using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;


namespace ChestnutHotel
{
    public partial class UserForm : Form //окно формы пользователя
    {
        OleDbDataAdapter dataAdapter;
        DataSet DataSet = new DataSet();

        string DBNamePath = "ChestnutData/ChestnutDatabase.mdb";

        public UserForm()
        {
            InitializeComponent();
            LeavePage.Parent = null; //скрытие или показ вкладок в меню
            Page1Login.Parent = UserFormtabControl;
            Page2Reservation.Parent = UserFormtabControl;
            Page3Request.Parent = null;
            Page3RequestTable.Parent = null;
            Page5Exit.Parent = null;

        }

        private void LoginButton_Click(object sender, EventArgs e) //авторизация
        {
            try
            {
                DataSet LogPassDS;

                LogPassDS = DatabaseWorker.ConnectAndRead(DBNamePath, "select * from Клиент where Фамилия = \"" + LoginAuth.Text + "\" and Пароль = \"" + PasswordAuth.Text + "\"", ref dataAdapter);
                DataSet ReservationInfoDS = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Бронь.ОплаченаЛи FROM Клиент INNER JOIN Бронь ON Клиент.КодБрони = Бронь.КодБрони WHERE ((Клиент.Фамилия) = '" + LoginAuth.Text + "') AND((Клиент.Пароль) = '" + PasswordAuth.Text + "');", ref dataAdapter);

                bool loginSuccessful = ((LogPassDS.Tables.Count > 0) && (LogPassDS.Tables[0].Rows.Count > 0));

                if (loginSuccessful)
                {
                    if (ReservationInfoDS.Tables[0].Rows[0].ItemArray[0].ToString() == "0")
                    {
                        DialogResult result = MessageBox.Show("Ваша бронь не подтверждена; Доступ к услугам ограничен.\r\nБронь будет потдверждена Администратором в момент Вашего заселения.\r\n\r\nЖелаете отозвать бронь и удалить данные?", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.No)
                        {
                            return;
                        }
                        else if (result == DialogResult.Yes)
                        {
                            DialogResult result1 = MessageBox.Show("Вы дейстивтельно хотите удалить бронь?", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (result1 == DialogResult.No)
                            {
                                return;
                            }
                            else if (result1 == DialogResult.Yes)
                            {
                                DeleteClient();
                                MessageBox.Show("Бронь удалена.", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                    Page2Reservation.Parent = null;
                    Page1Login.Parent = null;
                    Page3Request.Parent = UserFormtabControl;
                    Page3RequestTable.Parent = UserFormtabControl;
                    Page5Exit.Parent = UserFormtabControl;
                    LeavePage.Parent = null;

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

        private void Page5Exit_Enter(object sender, EventArgs e) //Кнопка "выход"
        {
            Environment.Exit(0);
        }

        private void RegisterHelpButton_Click(object sender, EventArgs e) //Вызов справки
        {
            try
            {
                NotepadForm notepadForm = new NotepadForm();
                notepadForm.OpenTextFile("ChestnutData/Help/AboutReservations.NoteN");
                notepadForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Page2Reservation_Enter(object sender, EventArgs e) //Загрузка вкладки "Бронирование"
        {
            try
            {
                AdditionalInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet AdditionalInventoryDataset = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодИнвентаря, Наименование, Стоимость FROM Инвентарь;", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(AdditionalInventoryDataset, AdditionalInventory);
                AdditionalInventory.Rows[0].Selected = true;

                ReservationDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Номер.Изображение, Номер.Этаж, Номер.Название, Номер.Описание, КлассНомера.КлассНомера, Номер.БазоваяСтоимость, Номер.КодНомера, Номер.КодКлассаНомера FROM КлассНомера INNER JOIN Номер ON КлассНомера.КодКлассаНомера = Номер.КодКлассаНомера;", ref dataAdapter); //WHERE(((Номер.КодБрони) = 0))
                DatabaseWorker.RefreshDataGridView(DataSet, ReservationDataGridView);


                ReservationDataGridView.Sort(ReservationDataGridView.Columns["БазоваяСтоимость"], ListSortDirection.Ascending);


                //  ReservationDataGridView.Columns[1].HeaderText = "Этаж";
                // ReservationDataGridView.Columns[2].HeaderText = "Название";
                // ReservationDataGridView.Columns[3].HeaderText = "Описание";
                //  ReservationDataGridView.Columns[4].HeaderText = "Класс номера";
                //  ReservationDataGridView.Columns[5].HeaderText = "Стоимость за сутки (руб)";
                // ReservationDataGridView.Columns[6].HeaderText = "Дополнительные удобства";
                //  ReservationDataGridView.Columns["Описание"].Visible = false;
                ReservationDataGridView.Columns["КодНомера"].Visible = false;
                ReservationDataGridView.Columns["КодКлассаНомера"].Visible = false;

                if (ReservationDataGridView.Rows.Count == 0)
                { OUTOFROOMS.Visible = true; }
                else
                { OUTOFROOMS.Visible = false; }
                TypeOfRoom.Items.Clear();
                DataSet RoomClass = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT DISTINCT КлассНомера FROM КлассНомера;", ref dataAdapter);
                List<string> strDetailIDList = new List<string>();
                foreach (DataRow row in RoomClass.Tables[0].Rows)
                {
                    strDetailIDList.Add(row["КлассНомера"].ToString());
                }
                TypeOfRoom.Items.AddRange(strDetailIDList.Distinct().ToArray());
                if (strDetailIDList.Count == 0)
                {
                    TypeOfRoom.Text = "--Класс номера--";
                }
                else
                { TypeOfRoom.SelectedIndex = 0; }
                FloorChooser.Items.Clear();
                DataSet Floors = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT DISTINCT Этаж FROM Номер;", ref dataAdapter);
                List<string> strDetailIDList1 = new List<string>();
                foreach (DataRow row in Floors.Tables[0].Rows)
                {
                    strDetailIDList1.Add(row["Этаж"].ToString());
                }
                FloorChooser.Items.AddRange(strDetailIDList1.Distinct().ToArray());
                if (strDetailIDList1.Count == 0)
                {
                    FloorChooser.Text = "--Этаж--";
                }
                else
                { FloorChooser.SelectedIndex = 0; }
                DopComfort.Items.Clear();
                DataSet UdobstvaNames = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT DISTINCT Удобства.НаименованиеУдобства FROM Удобства;", ref dataAdapter);
                List<string> strDetailIDList2 = new List<string>();
                foreach (DataRow row in UdobstvaNames.Tables[0].Rows)
                {
                    strDetailIDList2.Add(row["НаименованиеУдобства"].ToString());
                }
                DopComfort.Items.AddRange(strDetailIDList2.Distinct().ToArray());

                if (strDetailIDList2.Count == 0)
                {
                    DopComfort.Text = "--Дополнительные услуги--";
                }
                else
                { DopComfort.SelectedIndex = 0; }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ReservationDataGridView_CellEnter(object sender, DataGridViewCellEventArgs e) //Перерасчет стоимости
        {
            try
            {
                monthCalendar1.RemoveAllBoldedDates();
                int ind = ReservationDataGridView.CurrentRow.Index;

                DataSet OccupiedDays = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Бронь.ДатаЗаселения, Бронь.ДатаВыселения FROM Номер INNER JOIN Бронь ON Номер.КодНомера = Бронь.КодНомера WHERE Номер.КодНомера = " + ReservationDataGridView.Rows[ind].Cells["КодНомера"].Value.ToString() + " AND Бронь.ПодтверждениеЗакрытия <> 1", ref dataAdapter);

                if (OccupiedDays.Tables[0].Rows.Count == 0)
                {
                    monthCalendar1.RemoveAllAnnuallyBoldedDates();
                    monthCalendar1.RemoveAllBoldedDates();
                    monthCalendar1.RemoveAllMonthlyBoldedDates();

                    monthCalendar1.UpdateBoldedDates();
                }
                else
                {
                    for (int i = 0; i < OccupiedDays.Tables[0].Rows.Count; i++)
                    {
                        DateTime ReserveStart = Convert.ToDateTime(OccupiedDays.Tables[0].Rows[i].ItemArray[0]).AddHours(12);
                        DateTime ReserveEnd = Convert.ToDateTime(OccupiedDays.Tables[0].Rows[i].ItemArray[1]).AddHours(12);
                        var dateOne = ReserveStart.Date;
                        var dateTwo = ReserveEnd.Date;
                        var datesBetween = Enumerable
                            .Range(1, (int)(dateTwo - dateOne).TotalDays)
                            .Select(diff => dateOne.AddDays(diff))
                            .ToArray();

                        for (int j = 0; j < datesBetween.Length; j++)
                        {
                            monthCalendar1.AddBoldedDate(datesBetween[j]);
                        }
                        monthCalendar1.UpdateBoldedDates();
                    }
                }
                DataSet Udobstva = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Удобства.НаименованиеУдобства FROM(Номер INNER JOIN СвязьУдобств ON Номер.КодНомера = СвязьУдобств.КодНомера) INNER JOIN Удобства ON СвязьУдобств.КодУдобства = Удобства.КодУдобства WHERE(((Номер.КодНомера) = " + ReservationDataGridView.Rows[ind].Cells["КодНомера"].Value.ToString() + "));", ref dataAdapter);
                DescriptionOfRoom.Text = ReservationDataGridView.Rows[ind].Cells[3].Value.ToString();

                DopUdobstva.Clear();
                DopUdobstva.Text = "| ";
                int i1 = 0;
                while (i1 != Udobstva.Tables[0].Rows.Count)
                {
                    DopUdobstva.Text += Udobstva.Tables[0].Rows[i1].ItemArray[0].ToString() + " | ";
                    i1++;
                }

                RecountPrice();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SearchButton_Click(object sender, EventArgs e) //Поиск номеров
        {
            try
            {
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT DISTINCT Номер.Изображение, Номер.Этаж, Номер.Название, Номер.Описание, КлассНомера.КлассНомера, Номер.БазоваяСтоимость, Номер.КодНомера, Номер.КодКлассаНомера FROM((КлассНомера INNER JOIN Номер ON КлассНомера.КодКлассаНомера = Номер.КодКлассаНомера) INNER JOIN СвязьУдобств ON Номер.КодНомера = СвязьУдобств.КодНомера) INNER JOIN Удобства ON СвязьУдобств.КодУдобства = Удобства.КодУдобства WHERE(((Номер.КодБрони) = 0) AND ((Номер.Этаж) = " + Convert.ToString(FloorChooser.SelectedItem) + ") AND ((Удобства.НаименованиеУдобства) = '" + Convert.ToString(DopComfort.SelectedItem) + "') AND ((КлассНомера.КлассНомера) = '" + Convert.ToString(TypeOfRoom.SelectedItem) + "'));", ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(DataSet, ReservationDataGridView);

                if (ReservationDataGridView.Rows.Count == 0)
                { OUTOFROOMS.Visible = true; }
                else
                { OUTOFROOMS.Visible = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void RecountPrice() //Расчет стоимости с учетом выбранных условий
        {
            if (ReservationDataGridView.Rows.Count != 0)
            {

                try
                {
                    int ind = ReservationDataGridView.CurrentRow.Index;

                    Price.Text = CountPrice().ToString("G29") + " рублей.";
                    LongInfo.Text = (monthCalendar1.SelectionRange.End - monthCalendar1.SelectionRange.Start).Add(TimeSpan.FromDays(1)).TotalDays.ToString() + " суток.";
                    RoomInfo.Text = ReservationDataGridView.Rows[ind].Cells[2].Value.ToString() + ".";


                    try
                    {
                        RoomImage.Image = Image.FromFile(@"ChestnutData\Img\" + ReservationDataGridView.Rows[ind].Cells["Изображение"].Value.ToString() + ".jpg");
                    }
                    catch
                    {
                        RoomImage.Image = Image.FromFile(@"ChestnutData\Img\null.jpg");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e) //Перерасчет стоимости
        {
            RecountPrice();

        }
        private decimal CountPrice() //Вычисление стоимости проживания
        {
            try
            {
                var ind = ReservationDataGridView.CurrentRow.Index;

                decimal inventoryPrice = 0;
                DataGridViewSelectedRowCollection DGV = AdditionalInventory.SelectedRows;
                for (int i = 0; i <= DGV.Count - 1; i++)
                {
                    inventoryPrice += Convert.ToDecimal(DGV[i].Cells["Стоимость"].Value);
                }
                var Days = Convert.ToDecimal((monthCalendar1.SelectionRange.End - monthCalendar1.SelectionRange.Start).Add(TimeSpan.FromDays(1)).TotalDays);
                decimal price = (Days * Convert.ToDecimal(ReservationDataGridView.Rows[ind].Cells[5].Value) + inventoryPrice);
                return price;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }


        private void RegisterButton_Click_1(object sender, EventArgs e) //Регистрация брони
        {
            try
            {
                if (ReservationDataGridView.Rows.Count != 0)
                {
                    int ind = ReservationDataGridView.CurrentRow.Index;
                    DateTime start = monthCalendar1.SelectionStart;
                    string formattedStart = start.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                    DateTime end = monthCalendar1.SelectionEnd;
                    string formattedEnd = end.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);


                    DataSet IsRoomOccupied = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Бронь.ДатаЗаселения, Бронь.ДатаВыселения FROM Бронь INNER JOIN Номер ON Бронь.КодНомера = Номер.КодНомера WHERE(((#" + formattedStart + "#) Between [Бронь].[ДатаЗаселения] And [Бронь].[ДатаВыселения] OR (#" + formattedEnd + "#) Between [Бронь].[ДатаЗаселения] And [Бронь].[ДатаВыселения]) AND ((Номер.КодНомера)=" + ReservationDataGridView.Rows[ind].Cells["КодНомера"].Value.ToString() + " AND Бронь.ПодтверждениеЗакрытия <> 1));", ref dataAdapter);

                    if (IsRoomOccupied.Tables[0].Rows.Count != 0)
                    {
                        MessageBox.Show("Номер занят в указанное время!", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    { PrivateInfoFields.Visible = true; }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Page3RequestTable_Enter(object sender, EventArgs e) //Загрузка страницы обслуживания
        {
            try
            {
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter();

                DataSet UslugiDataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Услуги", ref dataAdapter);

                AddictionalUslugi.Items.Clear();
                List<string> strDetailIDList5 = new List<string>();
                foreach (DataRow row in UslugiDataSet.Tables[0].Rows)
                {
                    strDetailIDList5.Add(row["НаименованиеУслуги"].ToString());
                }
                AddictionalUslugi.Items.AddRange(strDetailIDList5.Distinct().ToArray());
                AddictionalUslugi.Text = "--Дополнительные услуги--";

                RequestsdataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet ClientInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Фамилия, Имя, Отчество, Пароль, КодКлиента FROM Клиент WHERE Фамилия LIKE '" + LoginAuth.Text + "' AND Пароль = '" + Convert.ToInt32(PasswordAuth.Text) + "'", ref dataAdapter);


                int UserId = Convert.ToInt32(ClientInfo.Tables[0].Rows[0].ItemArray[4].ToString());
                DataSet ReservationInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодБрони, КодНомера, Стоимость, ДатаЗаселения, ДатаВыселения FROM Бронь WHERE КодКлиента = " + UserId, ref dataAdapter);
                WelcomeLabel.Text = "Добро пожаловать, " + ClientInfo.Tables[0].Rows[0].ItemArray[0].ToString() + " " + ClientInfo.Tables[0].Rows[0].ItemArray[1].ToString() + " " + ClientInfo.Tables[0].Rows[0].ItemArray[2].ToString() + "!";
                MainInformation.Text = "Бронь:\r\nДата заселения: " + ReservationInfo.Tables[0].Rows[0].ItemArray[3].ToString();
                MainInformation.Text += "\r\nДата выселения: " + ReservationInfo.Tables[0].Rows[0].ItemArray[4].ToString();
                MainInformation.Text += "\r\nСтоимость: " + ReservationInfo.Tables[0].Rows[0].ItemArray[2].ToString() + " рублей.";

                int ReservationId = Convert.ToInt32(ReservationInfo.Tables[0].Rows[0].ItemArray[0].ToString());
                DataSet RoomInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодНомера, КодУдобств, КодКлассаНомера, КодБрони, Название, Этаж, Описание, Изображение, БазоваяСтоимость, КодПерсонала FROM Номер WHERE КодБрони = " + ReservationId, ref dataAdapter);

                RoomInfoLabel.Text = "Информация о номере:\r\nНазвание номера:" + RoomInfo.Tables[0].Rows[0].ItemArray[4].ToString();
                RoomInfoLabel.Text += "\r\nЭтаж: " + RoomInfo.Tables[0].Rows[0].ItemArray[5].ToString();
                RoomInfoLabel.Text += "\r\nОписание: " + RoomInfo.Tables[0].Rows[0].ItemArray[6].ToString();
                RoomInfoLabel.Text += "\r\nБазовая стоимость: " + RoomInfo.Tables[0].Rows[0].ItemArray[8].ToString() + " рублей.";
                RoomInfoLabel.Text += "\r\n\r\nНапоминаем, что Вы заказали дополнительные услуги: ";

                DataSet InventoryInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Инвентарь.Наименование, Инвентарь.Стоимость FROM(Бронь INNER JOIN СвязьИнвентаря ON Бронь.КодБрони = СвязьИнвентаря.КодБрони) INNER JOIN Инвентарь ON СвязьИнвентаря.КодИнвентаря = Инвентарь.КодИнвентаря WHERE(((Бронь.КодБрони) = " + ReservationId + "));", ref dataAdapter);
                for (int i = 0; i < InventoryInfo.Tables[0].Rows.Count; i++)
                {
                    RoomInfoLabel.Text += "\r\n" + InventoryInfo.Tables[0].Rows[i].ItemArray[0].ToString() + " стоимостью: " + InventoryInfo.Tables[0].Rows[i].ItemArray[1].ToString() + " рублей.";
                }

                int PersonalId = Convert.ToInt32(RoomInfo.Tables[0].Rows[0].ItemArray[9].ToString());
                DataSet PersonalInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Персонал.Фамилия, Персонал.Имя, Персонал.Отчество, СписокДолжностей.НазваниеДолжности, Персонал.КодПерсонала FROM Персонал INNER JOIN СписокДолжностей ON Персонал.Должность = СписокДолжностей.КодДолжности WHERE(((Персонал.КодПерсонала) = " + PersonalId + "));", ref dataAdapter);

                PersonalInfoLabel.Text = "Вас обслуживает: " + PersonalInfo.Tables[0].Rows[0].ItemArray[3].ToString();
                PersonalInfoLabel.Text += "\r\nФамилия: " + PersonalInfo.Tables[0].Rows[0].ItemArray[0].ToString();
                PersonalInfoLabel.Text += "\r\nИмя: " + PersonalInfo.Tables[0].Rows[0].ItemArray[1].ToString();
                PersonalInfoLabel.Text += "\r\nОтчество: " + PersonalInfo.Tables[0].Rows[0].ItemArray[2].ToString();


                DataSet MessagesInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT ТекстСообщения, КодСообщения, КодАвтора, КодУслуги, КодОтвета, ОтветноеСообщение FROM Сообщения WHERE КодАвтора = " + UserId, ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(MessagesInfo, RequestsdataGridView, true);
                RequestsdataGridView.Columns["КодСообщения"].Visible = false;
                RequestsdataGridView.Columns["КодАвтора"].Visible = false;
                RequestsdataGridView.Columns["КодУслуги"].Visible = false;
                RequestsdataGridView.Columns["КодОтвета"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SendNewMessage_Click(object sender, EventArgs e) //Отправка сообщения персоналу
        {
            try
            {
                if (UserMessageBox.Text.Length == 0)
                {
                    MessageBox.Show("Введите текст сообщения!", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataSet ClientInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Фамилия, Имя, Отчество, Пароль, КодКлиента, КодБрони FROM Клиент WHERE Фамилия LIKE '" + LoginAuth.Text + "' AND Пароль = '" + Convert.ToInt32(PasswordAuth.Text) + "'", ref dataAdapter);
                int UserId = Convert.ToInt32(ClientInfo.Tables[0].Rows[0].ItemArray[4].ToString());


                int UslugaIndexInt = 0;
                if (AddictionalUslugi.Text.ToString() != "--Дополнительные услуги--" && AddictionalUslugi.Text.ToString() != "")
                {
                    DataSet UslugaIndex = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодУслуги FROM Услуги WHERE НаименованиеУслуги LIKE '" + AddictionalUslugi.SelectedItem.ToString() + "';", ref dataAdapter);
                    UslugaIndexInt = Convert.ToInt32(UslugaIndex.Tables[0].Rows[0].ItemArray[0].ToString());
                }

                int ReservationID = Convert.ToInt32(ClientInfo.Tables[0].Rows[0].ItemArray[5].ToString());

                DataSet ReservationCode = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Номер.КодНомера FROM Номер INNER JOIN Бронь ON Номер.КодНомера = Бронь.КодНомера WHERE(((Бронь.КодБрони) = " + ReservationID + "));", ref dataAdapter);

                int RoomCode = Convert.ToInt32(ReservationCode.Tables[0].Rows[0].ItemArray[0].ToString());

                int MessageCode = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Сообщения", ref dataAdapter).Tables[0].Rows.Count;
                MessageCode++;
                DatabaseWorker.WriteInDatabase(DBNamePath, "INSERT INTO Сообщения (КодСообщения, КодАвтора, КодУслуги, ТекстСообщения, КодНомера) VALUES ('" + MessageCode + "', '" + UserId + "', '" + UslugaIndexInt + "', '" + UserMessageBox.Text + "', '" + RoomCode + "')");

                DataSet MessagesInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT ТекстСообщения, КодСообщения, КодАвтора, КодУслуги, КодОтвета, ОтветноеСообщение FROM Сообщения WHERE КодАвтора = " + UserId, ref dataAdapter);
                DatabaseWorker.RefreshDataGridView(MessagesInfo, RequestsdataGridView, true);
                RequestsdataGridView.Columns["КодСообщения"].Visible = false;
                RequestsdataGridView.Columns["КодАвтора"].Visible = false;
                RequestsdataGridView.Columns["КодУслуги"].Visible = false;
                RequestsdataGridView.Columns["КодОтвета"].Visible = false;

                UserMessageBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LeaveHotel_Click(object sender, EventArgs e) //Кнопка выселения
        {
            try
            {
                DialogResult result = MessageBox.Show("Вы действительно хотите выселиться?", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (result == DialogResult.No)
                {
                    return;
                }
                else if (result == DialogResult.Yes)
                {
                    LeavePage.Parent = UserFormtabControl;
                    UserFormtabControl.SelectedTab = LeavePage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateReservation_Click(object sender, EventArgs e) //Создание брони
        {
            UserFormtabControl.SelectedTab = Page2Reservation;
        }

        private void UserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ClearMessage_Click(object sender, EventArgs e)
        {
            AddictionalUslugi.Text = "--Дополнительные услуги--";
            UserMessageBox.Clear();
        }

        private void SaveToFileRTF_Click(object sender, EventArgs e) //Сохранение отчета о выселении в файл
        {
            try
            {
                LeaveRichTextBox.SaveFile("ОтчетОВыселении.rtf", RichTextBoxStreamType.RichText);
                MessageBox.Show("Сохранено в файл \"ОтчетОВыселении.rtf\"", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LeavePage_Enter(object sender, EventArgs e) //Формирование отчета о выселении
        {
            try
            {
                RequestsdataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet ClientInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Фамилия, Имя, Отчество, Пароль, КодКлиента FROM Клиент WHERE Фамилия LIKE '" + LoginAuth.Text + "' AND Пароль = '" + Convert.ToInt32(PasswordAuth.Text) + "'", ref dataAdapter);

                int UserId = Convert.ToInt32(ClientInfo.Tables[0].Rows[0].ItemArray[4].ToString());
                DataSet ReservationInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодБрони, КодНомера, Стоимость, ДатаЗаселения, ДатаВыселения FROM Бронь WHERE КодКлиента = " + UserId, ref dataAdapter);

                LeaveRichTextBox.Text += "Всего доброго, " + ClientInfo.Tables[0].Rows[0].ItemArray[0].ToString() + " " + ClientInfo.Tables[0].Rows[0].ItemArray[1].ToString() + " " + ClientInfo.Tables[0].Rows[0].ItemArray[2].ToString() + "!\r\n";
                LeaveRichTextBox.Text += "\r\n\r\nВаша бронь:\r\nДата заселения: " + ReservationInfo.Tables[0].Rows[0].ItemArray[3].ToString();
                LeaveRichTextBox.Text += "\r\nЗапланированная дата выселения: " + ReservationInfo.Tables[0].Rows[0].ItemArray[4].ToString();
                LeaveRichTextBox.Text += "\r\nФактическая дата выселения: " + DateTime.Now.ToString();
                LeaveRichTextBox.Text += "\r\nСтоимость: " + ReservationInfo.Tables[0].Rows[0].ItemArray[2].ToString() + " рублей.";
                LeaveRichTextBox.Text += "\r\nНЕ ЗАБУДЬТЕ ПРОИЗВЕСТИ ОПЛАТУ БРОНИ!";

                int ReservationId = Convert.ToInt32(ReservationInfo.Tables[0].Rows[0].ItemArray[0].ToString());
                DataSet RoomInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодНомера, КодУдобств, КодКлассаНомера, КодБрони, Название, Этаж, Описание, Изображение, БазоваяСтоимость, КодПерсонала FROM Номер WHERE КодБрони = " + ReservationId, ref dataAdapter);

                if (DateTime.Now.ToShortDateString() != ReservationInfo.Tables[0].Rows[0].ItemArray[4].ToString())
                {
                    DateTime d1 = Convert.ToDateTime(ReservationInfo.Tables[0].Rows[0].ItemArray[4]);
                    DateTime d2 = Convert.ToDateTime(DateTime.Now);
                    TimeSpan time = d1 - d2;

                    decimal AccuratePrice = Convert.ToDecimal(ReservationInfo.Tables[0].Rows[0].ItemArray[2]) - (Convert.ToDecimal(RoomInfo.Tables[0].Rows[0].ItemArray[8]) * Convert.ToDecimal(time.Days));
                    LeaveRichTextBox.Text += "\r\nСтоимость с учетом перерасчета: " + AccuratePrice + " рублей.";
                }

                LeaveRichTextBox.Text += "\r\n\r\nИнформация о номере:\r\nНазвание номера: " + RoomInfo.Tables[0].Rows[0].ItemArray[4].ToString();
                LeaveRichTextBox.Text += "\r\nЭтаж: " + RoomInfo.Tables[0].Rows[0].ItemArray[5].ToString();
                LeaveRichTextBox.Text += "\r\nОписание: " + RoomInfo.Tables[0].Rows[0].ItemArray[6].ToString();
                LeaveRichTextBox.Text += "\r\nБазовая стоимость: " + RoomInfo.Tables[0].Rows[0].ItemArray[8].ToString();

                int PersonalId = Convert.ToInt32(RoomInfo.Tables[0].Rows[0].ItemArray[9].ToString());
                DataSet PersonalInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Персонал.Фамилия, Персонал.Имя, Персонал.Отчество, СписокДолжностей.НазваниеДолжности, Персонал.КодПерсонала FROM Персонал INNER JOIN СписокДолжностей ON Персонал.Должность = СписокДолжностей.КодДолжности WHERE(((Персонал.КодПерсонала) = " + PersonalId + "));", ref dataAdapter);

                LeaveRichTextBox.Text += "\r\n\r\nВас обслуживали: " + PersonalInfo.Tables[0].Rows[0].ItemArray[3].ToString();
                LeaveRichTextBox.Text += "\r\nФамилия: " + PersonalInfo.Tables[0].Rows[0].ItemArray[0].ToString();
                LeaveRichTextBox.Text += "\r\nИмя: " + PersonalInfo.Tables[0].Rows[0].ItemArray[1].ToString();
                LeaveRichTextBox.Text += "\r\nОтчество: " + PersonalInfo.Tables[0].Rows[0].ItemArray[2].ToString();

                LeaveRichTextBox.Font = new Font("Century Gothic", 15, FontStyle.Bold);
                LeaveRichTextBox.Text += "\r\nСпасибо, что выбрали Каштан!";

                DeleteClient();

                Page3Request.Parent = null;
                Page3RequestTable.Parent = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteClient() //Удаление клиента из БД
        {
            try
            {
                DataSet ClientInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодКлиента, КодБрони FROM Клиент WHERE Фамилия LIKE '" + LoginAuth.Text + "' AND Пароль = '" + Convert.ToInt32(PasswordAuth.Text) + "'", ref dataAdapter);
                int UserId = Convert.ToInt32(ClientInfo.Tables[0].Rows[0].ItemArray[0].ToString());
                int ReservationID = Convert.ToInt32(ClientInfo.Tables[0].Rows[0].ItemArray[1].ToString());

                DataSet ReservationCode = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Номер.КодНомера FROM Номер INNER JOIN Бронь ON Номер.КодНомера = Бронь.КодНомера WHERE(((Бронь.КодБрони) = " + ReservationID + "));", ref dataAdapter);

                int RoomCode = Convert.ToInt32(ReservationCode.Tables[0].Rows[0].ItemArray[0].ToString());

                Random newpassrnd = new Random();
                int newpassword = newpassrnd.Next(1000000, 9999999);
                DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Клиент SET Пароль =" + newpassword + ", ВыселенЛи = 1 WHERE КодКлиента=" + UserId);
                DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Номер SET КодБрони = 0 WHERE КодНомера=" + RoomCode);
                DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Бронь SET КодБрони = 0, ФактическаяДата = '" + DateTime.Now.ToShortDateString() + "' WHERE КодНомера=" + RoomCode + " AND КодКлиента= " + UserId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        bool IsValidEmail(string email) //Проверка валидности почты
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CreateRegister_Click(object sender, EventArgs e) //Регистрация брони с данными клиента
        {
            try
            {
                if (IsValidEmail(EmailField.Text) == false)
                {
                    MessageBox.Show("Некорректный адрес электронной почты!", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if ((Family.Text.Length == 0) || (UserNameReg.Text.Length == 0) || (FatherName.Text.Length == 0) || (EmailField.Text.Length == 0))
                {
                    MessageBox.Show("Заполнены не все поля!", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                int ind = ReservationDataGridView.CurrentRow.Index;
                int ClientCode = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь", ref dataAdapter).Tables[0].Rows.Count;
                ClientCode++;

                int ReservationCode = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь", ref dataAdapter).Tables[0].Rows.Count;
                ReservationCode++;

                int RoomCode = Convert.ToInt32(ReservationDataGridView.Rows[ind].Cells["КодНомера"].Value);
                decimal Price = CountPrice();

                DateTime DateIn = monthCalendar1.SelectionRange.Start.AddHours(12);
                DateTime DateOut = monthCalendar1.SelectionRange.End.AddHours(12);

                Random rnd = new Random();
                int password = rnd.Next(1000, 9999);


                int RowsCount = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Бронь", ref dataAdapter).Tables[0].Rows.Count;
                DatabaseWorker.WriteInDatabase(DBNamePath, "INSERT INTO Бронь (КодБрони, КодКлиента, КодНомера, Стоимость, ДатаЗаселения, ДатаВыселения) VALUES ('" + ReservationCode + "', '" + ClientCode + "', '" + RoomCode + "', '" + Price + "', '" + DateIn + "', '" + DateOut + "')");


                int ClientCodeInClientTable = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Клиент", ref dataAdapter).Tables[0].Rows.Count;
                ClientCodeInClientTable++;
                DatabaseWorker.WriteInDatabase(DBNamePath, "INSERT INTO Клиент (КодКлиента, КодБрони, Фамилия, Имя, Отчество, Пароль, Почта) VALUES ('" + ClientCodeInClientTable + "', '" + ReservationCode + "', '" + Family.Text + "', '" + UserNameReg.Text + "', '" + FatherName.Text + "', '" + password + "', '" + EmailField.Text + "')");
                DatabaseWorker.WriteInDatabase(DBNamePath, "UPDATE Номер SET КодБрони=" + ReservationCode + " WHERE КодНомера=" + RoomCode);


                for (int i = 0; i < AdditionalInventory.SelectedRows.Count; i++)
                {
                    DatabaseWorker.WriteInDatabase(DBNamePath, "INSERT INTO СвязьИнвентаря (КодИнвентаря, КодБрони) VALUES (" + AdditionalInventory.SelectedRows[i].Cells["КодИнвентаря"].Value.ToString() + ", " + ReservationCode + ")");
                }

                MessageBox.Show("Регистрация брони завершена.\r\n\r\nЗапомните и используйте следующие данные для входа:\r\n\r\nЛогин: " + Family.Text + "\r\nПароль: " + password, "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Information);

                PrivateInfoFields.Visible = false;
                UserFormtabControl.SelectedIndex = 0;
                Page2Reservation.Parent = null;
                Page2Reservation.Parent = UserFormtabControl;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClosePrivateForm_Click(object sender, EventArgs e)
        {
            PrivateInfoFields.Visible = false;
        }

        private void ClearRegister_Click(object sender, EventArgs e)
        {
            Family.Clear();
            UserNameReg.Clear();
            FatherName.Clear();
            EmailField.Clear();
        }

        private void ResetSearch_Click(object sender, EventArgs e) //Сброс поиска
        {
            try
            {
                ReservationDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT Номер.Изображение, Номер.Этаж, Номер.Название, Номер.Описание, КлассНомера.КлассНомера, Номер.БазоваяСтоимость, Номер.КодНомера, Номер.КодКлассаНомера FROM КлассНомера INNER JOIN Номер ON КлассНомера.КодКлассаНомера = Номер.КодКлассаНомера;", ref dataAdapter); //WHERE(((Номер.КодБрони) = 0))
                DatabaseWorker.RefreshDataGridView(DataSet, ReservationDataGridView);


                //  ReservationDataGridView.Columns[1].HeaderText = "Этаж";
                // ReservationDataGridView.Columns[2].HeaderText = "Название";
                // ReservationDataGridView.Columns[3].HeaderText = "Описание";
                //  ReservationDataGridView.Columns[4].HeaderText = "Класс номера";
                //  ReservationDataGridView.Columns[5].HeaderText = "Стоимость за сутки (руб)";
                // ReservationDataGridView.Columns[6].HeaderText = "Дополнительные удобства";
                //  ReservationDataGridView.Columns["Описание"].Visible = false;
                ReservationDataGridView.Columns["КодНомера"].Visible = false;
                ReservationDataGridView.Columns["КодКлассаНомера"].Visible = false;

                if (ReservationDataGridView.Columns.Contains("БазоваяСтоимость") == true && ReservationDataGridView.Rows.Count != 0)
                {
                    ReservationDataGridView.Sort(ReservationDataGridView.Columns["БазоваяСтоимость"], ListSortDirection.Ascending);
                }

                if (ReservationDataGridView.Rows.Count == 0)
                { OUTOFROOMS.Visible = true; }
                else
                { OUTOFROOMS.Visible = false; }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowComplaintWindow_Click(object sender, EventArgs e) //Отображение окна жалоб
        {
            try
            {
                DataSet ClientInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодКлиента, КодБрони FROM Клиент WHERE Фамилия LIKE '" + LoginAuth.Text + "' AND Пароль = '" + Convert.ToInt32(PasswordAuth.Text) + "'", ref dataAdapter);
                int UserId = Convert.ToInt32(ClientInfo.Tables[0].Rows[0].ItemArray[0].ToString());
                ComplaintWindow.Visible = true;

                DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодСообщения, ТекстЖалобы, ТекстОтвета FROM Жалобы WHERE КодАвтора = " + UserId, ref dataAdapter);
                DatabaseWorker.UpdateDatabase(DataSet, ComplaintHistory, ref dataAdapter);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void CloseComplaint_Click(object sender, EventArgs e)
        {
            ComplaintWindow.Visible = false;
        }

        private void SendComplaint_Click(object sender, EventArgs e) //Отправка жалобы
        {
            try
            {
                if (ComplaintText.Text != "")
                {
                    DataSet ClientInfo = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодКлиента, КодБрони FROM Клиент WHERE Фамилия LIKE '" + LoginAuth.Text + "' AND Пароль = '" + Convert.ToInt32(PasswordAuth.Text) + "'", ref dataAdapter);
                    int UserId = Convert.ToInt32(ClientInfo.Tables[0].Rows[0].ItemArray[0].ToString());


                    int ComplainCode = Convert.ToInt32(DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT * FROM Жалобы", ref dataAdapter).Tables[0].Rows.Count);
                    ComplainCode++;

                    DatabaseWorker.WriteInDatabase(DBNamePath, "INSERT INTO Жалобы (КодСообщения, КодАвтора, ТекстЖалобы) VALUES (" + ComplainCode + ", " + UserId + ", '" + ComplaintText.Text + "')");


                    DataSet = DatabaseWorker.ConnectAndRead(DBNamePath, "SELECT КодСообщения, ТекстЖалобы, ТекстОтвета FROM Жалобы WHERE КодАвтора = " + UserId, ref dataAdapter);
                    DatabaseWorker.UpdateDatabase(DataSet, ComplaintHistory, ref dataAdapter);
                    ComplaintText.Clear();
                }
                else
                {
                    MessageBox.Show("Не введен текст жалобы", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdditionalInventory_SelectionChanged_1(object sender, EventArgs e)
        {
            RecountPrice();
        }

        private void GoToRequests_Click(object sender, EventArgs e)
        {
            UserFormtabControl.SelectedTab = Page3RequestTable;
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void ClientHelp_Click(object sender, EventArgs e)
        {
            try
            {
                NotepadForm notepadForm = new NotepadForm();
                notepadForm.OpenTextFile("ChestnutData/Help/AboutClient.NoteN");
                notepadForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Система управления гостиницей \"Каштан\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
