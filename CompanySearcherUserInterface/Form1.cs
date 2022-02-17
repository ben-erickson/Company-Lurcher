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

                // Add the company to the datagridview
                DataGridViewRow companyRow = new DataGridViewRow();
                DataGridViewTextBoxCell companyCell = new DataGridViewTextBoxCell();

                companyCell.Value = outgoingCompany.Name;
                companyRow.Cells.Add(companyCell);

                this.grdCompanies.Rows.Add(companyRow);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            // If the user does not have a single company selected, don't edit one
            if (this.grdCompanies.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please select a company to edit.");
                return;
            }
            else if (this.grdCompanies.SelectedCells.Count != 1) {
                MessageBox.Show("Please select a single row company to edit.");
                return;
            }

            Company originalCompany = this.Companies[this.grdCompanies.SelectedCells[0].RowIndex];
            Company outgoingCompany = new Company();
            SuccessToken outgoingToken = new SuccessToken();

            // Copy the data from the original company to the new one without assigning a reference to the original
            outgoingCompany.CopyData(originalCompany);

            CompanyEditor editingEditor = new CompanyEditor(outgoingCompany, outgoingToken);
            editingEditor.ShowDialog();

            if (outgoingToken.CheckSuccess())
            {
                originalCompany.CopyData(outgoingCompany);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // If they don't have a row selected, stop processing
            if (this.grdCompanies.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            // Give the user a warning before they delete anything
            DialogResult confirmationResult = DialogResult.No;
            if (this.grdCompanies.SelectedCells.Count == 1)
            {
                confirmationResult = MessageBox.Show("Would you like to delete the selected company?", "Deletion Confirmation", MessageBoxButtons.YesNo);
            }
            else if (this.grdCompanies.SelectedCells.Count > 1)
            {
                confirmationResult = MessageBox.Show("You have selected multiple companies. Would you like to delete all of them?", "Deletion Confirmation", MessageBoxButtons.YesNo);
            }

            if (confirmationResult == DialogResult.Yes)
            {
                for (int i = 0; i < this.grdCompanies.SelectedCells.Count; i++)
                {
                    this.Companies.RemoveAt(this.grdCompanies.SelectedCells[i].RowIndex);
                    this.grdCompanies.Rows.RemoveAt(i);
                }
            }
        }
    }
}
