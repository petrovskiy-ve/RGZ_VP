using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Data;

namespace RGR.Views
{
    public partial class GroupWindow : Window
    {
        public GroupWindow()
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
            var tree = this.FindControl<TreeView>("TreeView");
            var selected = tree.SelectedItem as DataColumn;
            if (selected != null)
            {
                var res = selected.Table.TableName + "." + selected.ColumnName;
                this.Close(res);
            }
        }

        private void CancelClick(object? sender, RoutedEventArgs args)
        {
            this.Close();
        }

    }
}
