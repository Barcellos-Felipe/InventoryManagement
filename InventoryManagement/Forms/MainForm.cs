using System;
using System.Windows.Forms;
using InventoryManagement.Forms;
using System.Data;
using System.IO;


namespace InventoryManagement
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // ToolTip for buttons
            ToolTip toolTip = new ToolTip();

            toolTip.SetToolTip(this.btnExit, "Close");
            toolTip.SetToolTip(this.btnMinimize, "Minimize");
            toolTip.SetToolTip(this.btnSearch, "Searches an item");
            toolTip.SetToolTip(this.btnAddNew, "Adds a new Item");
            toolTip.SetToolTip(this.btnSaveChanges, "Updates the item");
            toolTip.SetToolTip(this.btnDelete, "Deletes the item");
            toolTip.SetToolTip(this.btnAdd, "Adds quantity to the item");
            toolTip.SetToolTip(this.btnSubtract, "Subtracts quantity from the item");
            toolTip.SetToolTip(this.btnEdit, "Edits the item's price");
            toolTip.SetToolTip(this.btnReport, "Generates a report of the inventory");
            toolTip.SetToolTip(this.btnClear, "Clear all fields");

            // Event Handlers
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnAddNew.Click += new EventHandler(btnAddNew_Click);
            btnSaveChanges.Click += new EventHandler(btnSaveChanges_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnSubtract.Click += new EventHandler(btnSubtract_Click);
            btnEdit.Click += new EventHandler(btnEdit_Click);
            btnReport.Click += new EventHandler(btnReport_Click);
            btnClear.Click += new EventHandler(btnClear_Click);
            btnExit.Click += new EventHandler(btnExit_Click);
            btnMinimize.Click += new EventHandler(btnMinimize_Click);
        }
        
        private void ClearFields()
        {
            txtCode.Text = "";
            txtDescription.Text = "";
            txtQuantity.Text = "0";
            txtPrice.Text = "0.00";
        }

        // Calls 'Create' method
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == "" || txtDescription.Text == "")
            {
                MessageBox.Show("Fill Code and Description Boxes");
            }
            else
            {
                Product product = new Product
                {
                    Code = txtCode.Text,
                    Description = txtDescription.Text,
                    Quantity = Convert.ToInt32(txtQuantity.Text),
                    Price = Convert.ToDouble(txtPrice.Text)
                };

                try
                {
                    DatabaseHandler db = new DatabaseHandler();

                    db.Create(product);

                    MessageBox.Show($"Item {product.Code} added succesfully");
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, $"Error: {ex.Source}");
                }
            }   
        }

        // Calls 'GetProductByCode' method
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == "")
            {
                MessageBox.Show("No Input Provided");
            }
            else
            {
                try
                {
                    DatabaseHandler db = new DatabaseHandler();
                    Product product = db.GetProductByCode(txtCode.Text);

                    txtDescription.Text = product.Description;
                    txtQuantity.Text = Convert.ToString(product.Quantity);
                    txtPrice.Text = Convert.ToString(product.Price);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, $"Error: {ex.Source}");
                }
            }
        }

        // Calls 'Update' method
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            DialogResult userConfirmation;
            userConfirmation = MessageBox.Show(
                $"Do you really want to update item {txtCode.Text}? This action cannot be undone",
                "Update Confirmation",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (userConfirmation == DialogResult.OK)
            {
                try
                {
                    DatabaseHandler db = new DatabaseHandler();
                    Product product = db.GetProductByCode(txtCode.Text);

                    db.Update(
                        product,
                        txtDescription.Text,
                        Convert.ToInt32(txtQuantity.Text),
                        Convert.ToDouble(txtPrice.Text)
                    );

                    MessageBox.Show($"Item {txtCode.Text} updated succesfully");
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, $"Error: {ex.Source}");
                }
            }
        }

        // Calls 'Delete' method
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult userConfirmation;
            userConfirmation = MessageBox.Show(
                $"Do you really want to delete item {txtCode.Text}? This action cannot be undone",
                "Delete Confirmation",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (userConfirmation == DialogResult.OK)
            {
                try
                {
                    DatabaseHandler db = new DatabaseHandler();
                    Product product = db.GetProductByCode(txtCode.Text);

                    db.Delete(product);

                    MessageBox.Show($"Item {txtCode.Text} deleted succesfully");
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, $"Error: {ex.Source}");
                }
            }
        }

        // Calls 'GetProducts' method
        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseHandler db = new DatabaseHandler();

                using (DataTable table = db.GetProducts())
                {
                    if (!Directory.Exists(@"reports"))
                    {
                        Directory.CreateDirectory(@"reports");
                    }

                    // Erase all text from the file before writing again
                    System.IO.File.WriteAllText(@"reports\report.txt", string.Empty);

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"reports\report.txt", true))
                    {
                        Report report = new Report(table);
                        file.WriteLine(report.GenerateReport(table));
                    }
                }

                MessageBox.Show(@"Report generated at 'reports\report.txt'");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, $"Error: {ex.Source}");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddQuantityForm addForm = new AddQuantityForm(this);
            addForm.ShowDialog();
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            SubtractQuantityForm subForm = new SubtractQuantityForm(this);
            subForm.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditPriceForm subForm = new EditPriceForm(this);
            subForm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
