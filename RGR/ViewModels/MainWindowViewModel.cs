using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using ReactiveUI;
using RGR.Models;
using System.Collections.ObjectModel;

namespace RGR.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DataSet tables;
        private DBContext context;

        private ObservableCollection<DataTable> tablesList;
        public ObservableCollection<DataTable> Tables
        {
            get => tablesList;
            private set => this.RaiseAndSetIfChanged(ref tablesList, value);
        }

        private DataTable selectedTable;
        public DataTable SelectedTable
        {
            get => selectedTable;
            set => this.RaiseAndSetIfChanged(ref selectedTable, value);
        }

        
        public MainWindowViewModel()
        {
            context = DBContext.getInstance();
            tables = context.getDataSet();
            Tables = new ObservableCollection<DataTable>();
            foreach(DataTable t in tables.Tables)
            {
                Tables.Add(t);
            }
        }

        public void AddRow()
        {
            if (SelectedTable as MyQuery != null) return;
            try { DataRow row = SelectedTable.NewRow(); 
            row.BeginEdit();
            for(int i=0; i<row.ItemArray.Length; i++)
            {
                row[row.Table.Columns[i].ColumnName] = "0";
            }
            row.EndEdit();
            SelectedTable.Rows.Add(row);
            }
            catch (Exception ex) { }
        }

        public void DeleteRows()
        {
            try
            {
                SelectedTable.Rows.RemoveAt(SelectedTable.Rows.Count - 1);
                this.RaisePropertyChanged(nameof(SelectedTable.Rows));
            }
            catch(Exception ex) { }
        }
        public void AddTable(DataTable table)
        {
            if(!Tables.Contains(table))
                Tables.Add(table);
        }
        
        public void OnClick()
        {
            context.Save(tables);
        }
        public void deleteQuery(MyQuery quer)
        {
            Tables.Remove(quer);
            this.RaisePropertyChanged("Tables");
        }
    }
}
