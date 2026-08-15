using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assessment2526
{
    public class Budget_structure
    {
        private string title;
        private double total;
        private double added;
        public Budget_structure(string TITLE, double TOTAL, double ADDED)
        {
            this.title = TITLE;
            this.total =TOTAL;
            this.added = ADDED;
        }
        public string Title { 
            get { return title; }
            set { title = value; }  
        }
        public double Total {
            get { return total; }
            set { total = value; } 
        }
        public double Added {
            get { return added; }
            set { added = value; }
        }

    }
}
