using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace RGR.Models
{
    public class JoinResult
    {
        public DataTable firstTable;
        public string firstColumn;

        public DataTable secondTable;
        public string secondColumn;
    }
}
