using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using RGR.ViewModels;
using RGR.Models;
using System.Data;
using System.Collections.Generic;

namespace RGR.Views
{
    public partial class QueryWindow : Window
    {
        public QueryWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }



        private async void CreateQuery(object? sender, RoutedEventArgs args)
        {
            var context = this.DataContext as QueryWindowViewModel;
            NewQueryWindow dialog = new NewQueryWindow() { DataContext = context };
            var rTable = await dialog.ShowDialog<DataTable?>(this.VisualRoot as Window);
            if(rTable != null)
            {
                var q = new Models.MyQuery("New Query");
                q.Items.Add(new Models.MyQueryItem(rTable));
                context.QueryItems.Add(q);
                context.setTarget(q);
            }
        }
        private async void JoinQuery(object? sender, RoutedEventArgs args)
        {
            var context = this.DataContext as QueryWindowViewModel;
            JoinWindow dialog = new JoinWindow() { DataContext = context };
            var res = await dialog.ShowDialog<JoinResult?>(this.VisualRoot as Window);
            if(res != null)
            {
                context.TargetQuery.Items.Add(new MyQueryItem(res.secondTable));
                context.TargetQuery.Joins.Add(res);
                context.TargetQuery.AddMod("JOIN");
            }
        }

        private async void GroupQuery(object? sender, RoutedEventArgs args)
        {
            var context = this.DataContext as QueryWindowViewModel;
            GroupWindow dialog = new GroupWindow() { DataContext = context };
            var res = await dialog.ShowDialog<string?>(this.VisualRoot as Window);
            if(res != null)
            {
                context.TargetQuery.GroupItems.Add(res);
                context.TargetQuery.AddMod("GROUP");
            }
        }

        private async void WhereQuery(object? sender, RoutedEventArgs args)
        {
            var context = this.DataContext as QueryWindowViewModel;
            context.WhereItems = new System.Collections.ObjectModel.ObservableCollection<WhereItem>();
            context.TargetWhere = new WhereItem();
            WhereWindow dialog = new WhereWindow() { DataContext = context };
            await dialog.ShowDialog(this.VisualRoot as Window);
            if(context.WhereItems.Count>0)
            {
                List<WhereItem> items = new List<WhereItem>();
                foreach(WhereItem item in context.WhereItems)
                {
                    items.Add(item);
                }
                context.TargetQuery.WhereItems.Add(items);
                context.TargetQuery.AddMod("WHERE");
            }
            
        }
        

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
