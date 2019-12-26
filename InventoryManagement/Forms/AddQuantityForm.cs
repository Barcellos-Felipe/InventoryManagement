using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement.Forms
{
    public partial class AddQuantityForm : Form
    {
        public MainForm _mainForm;

        public AddQuantityForm(MainForm mainForm)
        {
            _mainForm = mainForm;

            InitializeComponent();

            txtQuantity.SelectAll();
            txtQuantity.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                int resultQuantity;

                resultQuantity = Convert.ToInt32(_mainForm.txtQuantity.Text) +
                    Convert.ToInt32(this.txtQuantity.Text);

                _mainForm.txtQuantity.Text = Convert.ToString(resultQuantity);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
