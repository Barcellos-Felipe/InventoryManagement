using System;
using System.Windows.Forms;
using System.Data.SQLite;
using InventoryManagement.Forms;
using System.Data;
using System.Collections.Generic;

namespace InventoryManagement
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ClearFields()
        {
            txtCode.Text = "";
            txtDescription.Text = "";
            txtQuantity.Text = "0";
            txtPrice.Text = "0.00";
        }

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
            else
            {
                // pass
            }
        }

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
            else
            {
                // pass
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            DatabaseHandler db = new DatabaseHandler();

            using (DataTable table = db.GetProducts())
            {
                System.IO.File.WriteAllText(@"F:\felip\Desktop\test.txt", string.Empty);

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"F:\felip\Desktop\test.txt", true))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            file.WriteLine(item);
                        }
                    }
                }
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
    }
}
