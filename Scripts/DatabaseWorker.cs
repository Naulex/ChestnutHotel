using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;

namespace ChestnutHotel
{
    public class DatabaseWorker
    {
        /// <summary>
        /// Подключается у указанной БД, выполняет запрос SELECT и возвращает DataTable с данными.
        /// </summary>
        /// <param name="path">Путь и имя файла БД</param>
        /// <param name="command">SQL-запрос</param>
        /// <param name="dataAdapter">dataAdapter, устанавливающий соединение. Должен быть объявлен заранее и передан при помощи ref</param>
        public static DataSet ConnectAndRead(string path, string command, ref OleDbDataAdapter dataAdapter)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable table = new DataTable();
                string connectionString = "provider=Microsoft.Jet.OLEDB.4.0;" + "data source=" + path;
                OleDbConnection connection;
                connection = new OleDbConnection(connectionString);
                connection.Open();
                dataAdapter = new OleDbDataAdapter(command, connection);
                dataAdapter.Fill(ds);
                connection.Close();

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка: {e.Message}");
            }
        }
        /// <summary>
        /// Анализирует DataSet и обновляет указанную БД. Не забудьте вызвать Refresh для DataGridView.
        /// </summary>
        /// <param name="dataSet">dataSet с новыми данными</param>
        /// <param name="dataAdapter">dataAdapter, устанавливающий соединение. Должен быть объявлен заранее и передан при помощи ref</param>
        /// <param name="dataGridView">Информацию из какого dataGridView брать?</param>
        /// <param name="isShowId">Показывать ли первый столбец датасета?</param>
        public static void UpdateDatabase(DataSet dataSet, DataGridView dataGridView, ref OleDbDataAdapter dataAdapter, bool isShowId = false)
        {
            OleDbCommandBuilder cb;
            DataSet changes;
            try
            {
                cb = new OleDbCommandBuilder(dataAdapter);
                changes = dataSet.GetChanges();
                if (changes != null)
                {
                    dataAdapter.Update(changes);
                }
                RefreshDataGridView(dataSet, dataGridView, isShowId);
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка: {e.Message}");
            }
        }
        /// <summary>
        /// Перезагружает указанный DataGridView при помощи DataSet и включает отображение первого столбца (по умолчанию отключено)
        /// </summary>
        /// <param name="dataSet">dataSet с новыми данными</param>
        /// <param name="dataGridView">Информацию из какого dataGridView брать?</param>
        /// <param name="isShowId">Показывать ли первый столбец датасета?</param>
        public static void RefreshDataGridView(DataSet dataSet, DataGridView dataGridView, bool isShowId = false)
        {
            dataGridView.DataSource = dataSet.Tables[0];
            if (isShowId)
                dataGridView.Columns[0].Visible = true;
            else
                dataGridView.Columns[0].Visible = false;
        }

        /// <summary>
        /// Подключается у указанной БД и сбрасывает в ней счетчик индекса.
        /// </summary>
        /// <param name="path">Путь и имя файла БД</param>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="newIdent">Позиция нового счетчика</param>
        public static void ResetTable(string path, string tableName, int newIdent = 1)
        {
            try
            {
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
                string command = "DBCC CHECKIDENT('" + tableName +"', RESEED, "+ newIdent +")";
                DataSet ds = new DataSet();
                DataTable table = new DataTable();
                string connectionString = "provider=Microsoft.Jet.OLEDB.4.0;" + "data source=" + path;
                OleDbConnection connection;
                connection = new OleDbConnection(connectionString);
                connection.Open();
                dataAdapter = new OleDbDataAdapter(command, connection);
                connection.Close();
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка: {e.Message}");
            }
        }
        /// <summary>
        /// Подключается у указанной БД и выполняет команду SQL (допускаются команды записи, обновления, удаления и т.д.)
        /// </summary>
        /// <param name="path">Путь и имя файла БД</param>
        /// <param name="command">Команда</param>
        public static void WriteInDatabase(string path, string command)
        {
            try
            {
                OleDbConnection connection;
                string connectionString = "provider=Microsoft.Jet.OLEDB.4.0;" + "data source=" + path;
                connection = new OleDbConnection(connectionString);
                connection.Open();
                OleDbCommand command1 = new OleDbCommand();
                command1.Connection = connection;
                command1.CommandText = command;
                command1.ExecuteNonQuery();
                connection.Close();              
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка: {e.Message}");
            }
        }

    }
}
