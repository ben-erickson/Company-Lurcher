using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompanySearcherUserInterface
{
    public partial class Form1 : Form
    {
        private string APIKey;
        private string SearchEngineKey;
        private List<Company> Companies;
        public Form1()
        {
            InitializeComponent();

            this.APIKey = string.Empty;
            this.SearchEngineKey = string.Empty;
            this.Companies = new List<Company>();

            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Subscribe to events
            this.btnAdd.Click += BtnAdd_Click;
            this.btnEdit.Click += BtnEdit_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Company outgoingCompany = new Company();
            SuccessToken outgoingToken = new SuccessToken();

            CompanyEditor newEditor = new CompanyEditor(outgoingCompany, outgoingToken);
            newEditor.ShowDialog();

            if (outgoingToken.CheckSuccess())
            {
                this.Companies.Add(outgoingCompany);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
