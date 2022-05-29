using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR.Models
{
    public class DBContext
    {
        static DBContext? instance = null;
        public static DBContext getInstance()
        {
            if (instance == null) instance = new DBContext();
            return instance;
        }


        private SQLiteConnection sql_con;
        private DataSet tables;

        public DataSet getDataSet()
        {
            return tables.Copy();
        }
        private void getTables()
        {

            SQLiteCommand command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' ORDER BY 1", sql_con);
            DataTable tablesNames = new DataTable();
            tablesNames.Load(command.ExecuteReader());

            foreach (DataRow row in tablesNames.Rows)
            {
                string name = row.ItemArray[0].ToString();
                if (name == "sqlite_sequence") continue;
                SQLiteCommand sqlTab = new SQLiteCommand("SELECT * FROM " + name, sql_con);
                DataTable table = new DataTable();
                table.Load(sqlTab.ExecuteReader());
                tables.Tables.Add(table);
                DataRow t = table.Rows[0];
            }
        }

        public void GetQuery(string query, DataTable table)
        {
            SQLiteCommand command = new SQLiteCommand(query, sql_con);
            table.Load(command.ExecuteReader());
        }

        public void Save(DataSet sTables) {
            try
            {
                foreach (DataTable table in tables.Tables)
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter("select * from " + table.TableName, sql_con);
                    adapter.UpdateCommand = new SQLiteCommandBuilder(adapter).GetUpdateCommand();
                    adapter.InsertCommand = new SQLiteCommandBuilder(adapter).GetInsertCommand();
                    adapter.DeleteCommand = new SQLiteCommandBuilder(adapter).GetDeleteCommand();
                    adapter.Update(sTables, table.TableName);

                }
            }
            catch (Exception ex) { }
        }
        private DBContext() {
            tables = new DataSet();

            sql_con = new SQLiteConnection("Data Source=rugby.db;Mode=ReadWrite");
            sql_con.Open();

            getTables();
        }

        ~DBContext()
        {
            sql_con.Close();
        }
    }
}
