using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanySearcherUserInterface
{
    public class Company
    {
        public string Name { get; set; }
        public List<string> Keywords { get; set; }

        public Company()
        {
            this.Name = string.Empty;
            this.Keywords = new List<string>();
        }

        public Company(string name)
        {
            this.Name = name;
            this.Keywords = new List<string>();
        }

        public Company(string name, List<string> keywords)
        {
            this.Name = name;
            this.Keywords = keywords;
        }
    }
}
