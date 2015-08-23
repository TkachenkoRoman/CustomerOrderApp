using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace CustomerOrderApp
{
    public partial class OrderDetailsForm : Form
    {
        private CustomerContext _context;
        public OrderDetailsForm(Customer selectedCustomer, CustomerContext context)
        {
            _context = context;
            InitializeComponent();
            this.comboBoxClient.DataSource = _context.Customer.Local.ToBindingList();
            this.comboBoxClient.SelectedValue = selectedCustomer.CustomerId;
        }

        private void buttonAddNewClient_Click(object sender, EventArgs e)
        {
            addNewClientDialog();
        }

        private void addNewClientDialog()
        {
            CustomerDetailsForm addNewCustomerDialog = new CustomerDetailsForm();

            addNewCustomerDialog.Owner = this;
            if (addNewCustomerDialog.ShowDialog() == DialogResult.OK)
            {
                Customer newCustomer = addNewCustomerDialog.Customer;
                _context.Customer.Add(newCustomer);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("***************" + ex);
                    return;
                }
                this.comboBoxClient.SelectedValue = newCustomer.CustomerId;
            }
        }
    }
}
