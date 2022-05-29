using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace RGR.Models
{
    public class WhereItem: ReactiveObject
    {
        public string OperatorW
        {
            get;
            set;
        }

        private string valueW;
        public string ValueW
        {
            get => valueW;
            set => this.RaiseAndSetIfChanged(ref valueW, value);
        }
        public string fromTable
        {
            get;
            set;
        }

    }
}
