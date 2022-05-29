using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.ComponentModel;

namespace RGR.Models
{
    public class MyQuery: DataTable, IReactiveObject
    {

        private string queryString;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        public string QueryString
        {
            get => queryString;
            private set => queryString=value;
        }

        private void GenerateQuery()
        {
            
            string res = "SELECT ";
            for (int j = 0; j < Items.Count; j++)
            {
                var sel = Items[j].getSelected();
                for (int i = 0; i < sel.Count; i++)
                {
                    res += Items[j].TableName + ".'" + sel[i]+"', ";
                }
            }
            res = res.Substring(0, res.Length - 2);
            res += " FROM " + Items[0].getString();
            for(int i=1; i<Items.Count; i++)
            {
                var t1 = Joins[i - 1].firstTable.TableName + "." + Joins[i - 1].firstColumn;
                var t2 = Joins[i - 1].secondTable.TableName + "." + Joins[i - 1].secondColumn;
                var tbn = Items[i].TableName;
                if(Joins[i - 1].secondTable as MyQuery != null)
                {
                    (Joins[i - 1].secondTable as MyQuery).GenerateQuery();
                    tbn = "(" + (Joins[i - 1].secondTable as MyQuery).QueryString + ") as "+tbn;
                }
                res += " JOIN " + tbn + " ON " + t1 + "=" + t2;
            }
            if (WhereItems.Count > 0) res += " WHERE ";
            for(int i=0; i<WhereItems.Count; i++)
            {
                var items = WhereItems[i];
                res += "(";
                for(int j=0; j<items.Count; j++)
                {
                    res += items[j].fromTable +" "+ items[j].OperatorW +" "+ items[j].ValueW;
                    if (j != items.Count - 1) res += " OR ";
                }
                res += ")";
                if (i != WhereItems.Count - 1) res += " AND ";
            }
            foreach(var str in GroupItems)
            {
                res += " GROUP BY '" + str+"'";
            }
            res += ";";
            queryString = res;
        }
        public void Run()
        {
            GenerateQuery();
            Clear();
            Columns.Clear();
            DBContext.getInstance().GetQuery(QueryString, this);
        }

        
        public string QueryName
        {
            get => TableName;
            set
            {
                if(TableName != value)
                {
                    TableName = value;
                    this.RaisePropertyChanged(new PropertyChangedEventArgs("QueryName"));
                } else
                {
                    TableName = value;
                }
            }
        }
        
        public List<List<WhereItem>> WhereItems
        {
            get;
            set;
        }

        public void RaisePropertyChanging(PropertyChangingEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        public List<string> GroupItems
        {
            get;
            set;
        }
        public List<JoinResult> Joins
        {
            get;
            set;
        }

        public ObservableCollection<string> Modifiers
        {
            get;
            set;
        }

        public void AddMod(string modName)
        {
            Modifiers.Add(modName);
            this.RaisePropertyChanged(nameof(Modifiers));
        }
        public ObservableCollection<MyQueryItem> Items
        {
            get;
            set;
        }
        public MyQuery(string name) :base()
        {
            TableName = name;
            Items = new ObservableCollection<MyQueryItem>();
            Joins = new List<JoinResult>();
            GroupItems = new List<string>();
            WhereItems = new List<List<WhereItem>>();
            Modifiers = new ObservableCollection<string>();

        }
    }
}
