using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Data;
using RGR.Models;

namespace RGR.ViewModels
{
    public class QueryWindowViewModel: ViewModelBase
    {
        private MyQuery targetMainQuery;
        public MyQuery TargetQuery
        {
            get => targetMainQuery;
            set => this.RaiseAndSetIfChanged(ref targetMainQuery, value);
        }

        public void setTarget(MyQuery target)
        {
            if (!queryList.Contains(target)) queryList.Add(target);
            TargetQuery = target;
        }


        private ObservableCollection<MyQuery> queryList;
        public ObservableCollection<MyQuery> QueryItems
        {
            get => queryList;
            set => this.RaiseAndSetIfChanged(ref queryList, value);
        }

        private ObservableCollection<DataTable> tables;
        private ObservableCollection<DataTable> allItems;
        public ObservableCollection<DataTable> AllItems
        {
            get => allItems;
            private set => allItems = value;
        }
        public void RunQuery()
        {
            try
            {
                targetMainQuery.Run();
                context.AddTable(TargetQuery);
            }
            catch (Exception ex) { }
        }

        private MainWindowViewModel context;
        public QueryWindowViewModel(MainWindowViewModel context): this()
        {
            this.context = context;
            tables = context.Tables;
            foreach(DataTable table in tables)
            {
                allItems.Add(table);
            }
            foreach(DataTable table in tables)
            {
                if(table as MyQuery != null) {
                    queryList.Add(table as MyQuery);
                }
            }
        }

        private ObservableCollection<WhereItem> whereItem;
        public ObservableCollection<WhereItem> WhereItems
        {
            get => whereItem;
            set => this.RaiseAndSetIfChanged(ref whereItem, value);
        }

        private WhereItem targetWhere;
        public WhereItem TargetWhere
        {
            get => targetWhere;
            set => this.RaiseAndSetIfChanged(ref targetWhere, value);
        }
        public QueryWindowViewModel()
        {
            queryList = new ObservableCollection<MyQuery>();
            allItems = new ObservableCollection<DataTable>();
        }
        public void CreateQuery()
        {
            
        }

    }
}
