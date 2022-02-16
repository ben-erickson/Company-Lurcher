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
    public partial class CompanyEditor : Form
    {
        private Company _workingCompany;
        private SuccessToken _workingToken;

        public CompanyEditor()
        {
            InitializeComponent();

            this._workingCompany = new Company();
            this._workingToken = new SuccessToken();

            this.Load += CompanyEditor_Load;
        }

        public CompanyEditor(Company editingCompany, SuccessToken editingToken)
        {
            InitializeComponent();

            this._workingCompany = editingCompany;
            this._workingToken = editingToken;

            this.Load += CompanyEditor_Load;
        }

        private void CompanyEditor_Load(object sender, EventArgs e)
        {
            // Subscribe to events
            this.btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Halt method if the user hasn't entered a name or any keywords
            if (this.txtCompanyName.Text == string.Empty)
            {
                MessageBox.Show("Please enter a name for the company.");
                return;
            }
            else if (this.grdKeywords.Rows.Count == 0)
            {
                MessageBox.Show("Please enter some keywords for the company.");
                return;
            }

            // Assign the data from the controls to the workingCompany
            this._workingCompany.Name = this.txtCompanyName.Text;
            this._workingCompany.Keywords.Clear();

            foreach (DataGridViewRow row in this.grdKeywords.Rows)
            {
                this._workingCompany.Keywords.Add(row.Cells[0].Value.ToString());
            }

            // Process the success of the form to the token
            this._workingToken.ProcessSuccess();

            this.Close();
        }
    }
}
