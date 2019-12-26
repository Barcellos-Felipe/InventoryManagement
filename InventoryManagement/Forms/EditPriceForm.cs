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
    public partial class EditPriceForm : Form
    {
        MainForm _mainForm;

        public EditPriceForm(MainForm mainForm)
        {
            _mainForm = mainForm;

            InitializeComponent();

            txtPrice.SelectAll();
            txtPrice.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                _mainForm.txtPrice.Text = this.txtPrice.Text;

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
