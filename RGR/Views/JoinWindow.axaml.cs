using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using RGR.Models;
using System.Data;

namespace RGR.Views
{
    public partial class JoinWindow : Window
    {
        public JoinWindow()
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
        private void OKClick(object? sender, RoutedEventArgs args)
        {
            var tree1 = this.FindControl<TreeView>("TreeView1");
            var tree2 = this.FindControl<TreeView>("TreeView2");
            var item1 = tree1.SelectedItem as DataColumn;
            var item2 = tree2.SelectedItem as DataColumn;
            if(item1 != null && item2 != null)
            {
                JoinResult res = new JoinResult();
                res.firstTable = item1.Table;
                res.secondTable = item2.Table;
                res.firstColumn = item1.ColumnName;
                res.secondColumn = item2.ColumnName;
                this.Close(res);
            }
        }
        private void CancelClick(object? sender, RoutedEventArgs args)
        {
            this.Close(null);
        }
    }
}
