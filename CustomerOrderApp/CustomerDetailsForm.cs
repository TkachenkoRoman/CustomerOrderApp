using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomerOrderApp
{
    public partial class CustomerDetailsForm : Form
    {
        public Customer Customer { get { return _customer; } }
        private Customer _customer;
        public CustomerDetailsForm(Customer customer = null)
        {
            _customer = new Customer();
            InitializeComponent();
            if (customer != null)
            {
                this._customer = customer;
                this.textBoxCustomerId.Text = _customer.CustomerId.ToString();
                this.textBoxCustomerId.ReadOnly = true;
                this.textBoxAddress.Text = _customer.Address;
                this.textBoxName.Text = _customer.Name;
                this.textBoxPhoneNumber.Text = _customer.PhoneNum;
            }
        }

        private void buttonCustomerSave_Click(object sender, EventArgs e)
        {
            _customer.CustomerId = Convert.ToInt32(this.textBoxCustomerId.Text);
            _customer.Address = this.textBoxAddress.Text;
            _customer.Name = this.textBoxName.Text;
            _customer.PhoneNum = this.textBoxPhoneNumber.Text;
        }
    }
}
