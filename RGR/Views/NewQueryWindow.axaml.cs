using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Data;

namespace RGR.Views
{
    public partial class NewQueryWindow : Window
    {
        public NewQueryWindow()
        {
            InitializeComponent();
            this.FindControl<Button>("OKbtn").Click += delegate
            {
                var selected = this.FindControl<ListBox>("mList").SelectedItem;
                if(selected!=null)
                {
                    this.Close(selected as DataTable);
                }
            };

            this.FindControl<Button>("Cancelbtn").Click += delegate
            {
                this.Close(null);
            };
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
