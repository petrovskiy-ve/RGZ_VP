using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using RGR.ViewModels;
using System.Data;

namespace RGR.Views
{
    public partial class WhereWindow : Window
    {
        public WhereWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private bool AddItem()
        {
            var tree = this.FindControl<TreeView>("TreeView1");
            var sel = tree.SelectedItem as DataColumn;
            if (sel == null) return false;
            var context = DataContext as QueryWindowViewModel;
            context.TargetWhere.OperatorW = (this.FindControl<ComboBox>("ComboBox").SelectedItem as ComboBoxItem).Content as string;
            context.TargetWhere.fromTable = sel.Table.TableName + ".'" + sel.ColumnName+"'";
            context.TargetWhere.ValueW = "'"+context.TargetWhere.ValueW+"'";
            context.WhereItems.Add(context.TargetWhere);
            context.TargetWhere = new Models.WhereItem();
            return true;
        }

        private void ORClick(object? sender, RoutedEventArgs args)
        {
            AddItem();
        }

        private void OKClick(object? sender, RoutedEventArgs args)
        {
            if(AddItem())
            {
                this.Close();
            }
        }

        private void CancelClick(object? sender, RoutedEventArgs args)
        {
            var context = DataContext as QueryWindowViewModel;
            context.WhereItems.Clear();
            this.Close();
        }

        private void TreeSelected(object sender, SelectionChangedEventArgs args)
        {
            if(args.AddedItems.Count > 0)
            {
                var context = DataContext as QueryWindowViewModel;
                var sel = args.AddedItems[0] as DataColumn;
                if (sel == null) return;
                context.TargetWhere.ValueW = sel.Table.TableName + ".'" + sel.ColumnName+"'";
                (sender as TreeView).SelectedItem = null;
            }
        }
    }
}
