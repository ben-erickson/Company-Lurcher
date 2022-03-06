using System;
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
            // Place the values from the working objects into the controls
            this.txtCompanyName.Text = this._workingCompany.Name;
            foreach (string keyword in this._workingCompany.Keywords)
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();

                cell.Value = keyword;
                row.Cells.Add(cell);

                grdKeywords.Rows.Add(row);
            }

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

            for (int i = 0; i < this.grdKeywords.Rows.Count - 1; i++)
            {
                if (this.grdKeywords.Rows[i].Cells[0].Value != null)
                {
                    this._workingCompany.Keywords.Add(this.grdKeywords.Rows[i].Cells[0].Value.ToString());
                }
            }

            // Process the success of the form to the token
            this._workingToken.ProcessSuccess();

            this.Close();
        }
    }
}
