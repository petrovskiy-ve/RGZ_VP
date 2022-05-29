using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using RGR.Models;
using RGR.ViewModels;
using System.Collections.Generic;

namespace RGR.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dGrid.CellEditEnding += OnCellEditEnd;
        }

        private void showQueryWindow(object sender, RoutedEventArgs args)
        {
            var context = this.DataContext as MainWindowViewModel;
            QueryWindowViewModel qcx = new QueryWindowViewModel(context);
            var queryWindow = new QueryWindow() { DataContext=qcx};
            queryWindow.Show();
        }
        private void deleteQuer(object sender, RoutedEventArgs args)
        {
            var item = (sender as Button).CommandParameter as MyQuery;
            var tabItems =  myTabs.Items as ObservableCollection<DataTable>;
            var sel = myTabs.SelectedItem;
            if (sel == item) sel = tabItems[0];
            tabItems.Remove(item);
            myTabs.SelectedItem = sel;

        }
        private void OnCellEditEnd(object sender, DataGridCellEditEndingEventArgs args)
        {
            if (args.EditAction != DataGridEditAction.Commit) return;
            if (args.EditingElement as TextBox == null) return;
            DataGrid dataGrid = sender as DataGrid;
            string index = args.Column.Header as string;
            string a = (args.EditingElement as TextBox).Text;

            DataRowCollection rows = dataGrid.Items as DataRowCollection;

            rows[dataGrid.SelectedIndex].BeginEdit();
            rows[dataGrid.SelectedIndex][index] = a;
            rows[dataGrid.SelectedIndex].EndEdit();
        }
        public void OnSelect(object sender, SelectionChangedEventArgs args)
        {
            if (args.AddedItems.Count == 0) return;
            DataTable table = args.AddedItems[0] as DataTable;
            if (table != null)
            {
                int i = 0;
                dGrid.Columns.Clear();
                //dGrid.Items = table.Rows;
                foreach(DataColumn col in table.Columns)
                {
                    dGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = col.ColumnName,
                        Binding = new Binding($"ItemArray[{i}]"),
                        IsReadOnly = false
                    });
                    i++;
                }
            }
        }
    }
}
