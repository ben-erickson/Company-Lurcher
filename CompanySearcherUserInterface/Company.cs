using System.Collections.Generic;

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

        public void CopyData(Company original)
        {
            this.Keywords.Clear();

            this.Name = original.Name;
            foreach (string keyword in original.Keywords)
            {
                this.Keywords.Add(keyword);
            }
        }
    }
}
