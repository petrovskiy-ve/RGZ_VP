using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR.Models
{

    public class MyQueryItem
    {
        public class Item
        {
            public bool IsChecked
            {
                get;
                set;
            }
            public string Name
            {
                get;
                set;
            }
        }
        public ObservableCollection<Item> Items
        {
            get;
            set;
        }
        public string TableName
        {
            get => table.TableName;
            private set => table.TableName = value;
        }

        public string getString()
        {
            if (table as MyQuery != null) return (table as MyQuery).QueryString;
            return TableName;
        }
        public List<string> getSelected()
        {
            List<string> res = new List<string>();
            foreach(Item item in Items)
            {
                if(item.IsChecked) res.Add(item.Name);
            }
            return res;
        }

        DataTable table;
        public DataTable Table
        {
            get => table;
            set => table = value;
        }
        public MyQueryItem(DataTable table)
        {
            this.table = table;
            Items = new ObservableCollection<Item>();
            foreach(DataColumn row in table.Columns)
            {
                Items.Add(new Item() {IsChecked=false, Name=row.ColumnName});
            }
        }
    }
}
