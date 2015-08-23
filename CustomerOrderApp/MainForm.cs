using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.Diagnostics;

namespace CustomerOrderApp
{
    public partial class MainForm : Form
    {
        CustomerContext _context;
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _context = new CustomerContext();

            _context.Customer.Load();
            _context.Order.Load();
            this.customerBindingSource.DataSource = _context.Customer.Local.ToBindingList();
            this.orderBindingSource.DataSource = _context.Order.Local.ToBindingList();
        }

        private void customerBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            _context.SaveChanges();
        }

        private void customerBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            int customerId = ((Customer)customerBindingSource.Current).CustomerId;
            // Display filtered data in order grid
            if (customerId != null)
                this.orderBindingSource.DataSource = _context.Order.Local.Where(x => x.CustomerId == customerId);
        }

        //private void customerDataGridView_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        //{
        //    // For any other operation except, StateChanged, do nothing
        //    if (e.StateChanged != DataGridViewElementStates.Selected) return;

        //    //int customerId = ((Customer)customerBindingSource.Current).CustomerId;
        //    int customerId = 0;
        //    if (e.Row.Cells["CustomerId"].Value != null &&
        //        e.Row.Cells["CustomerId"].Value != DBNull.Value)
        //        customerId = (int)e.Row.Cells["CustomerId"].Value;
        //    else
        //        return;

        //    // Display filtered data in order grid
        //    if (customerId != null)
        //        this.orderBindingSource.DataSource = _context.Order.Local.Where(x => x.CustomerId == customerId);
        //}

        private void customerBindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            Customer currentCustomer = (Customer)this.customerBindingSource.Current;
            if (currentCustomer.Order.Count > 0)
            {
                // warning with cascade deleted order amount
                string message = "In case of deleting '" + currentCustomer.Name.ToString() +
                                 "' (id=" + currentCustomer.CustomerId.ToString() + "), " +
                                 currentCustomer.Order.Count.ToString() + " order(s) will be deleted." +
                                 "Do you want to continue?";
                DialogResult dialogResult = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    this.customerBindingSource.RemoveCurrent();
                }
            }
            else
                this.customerBindingSource.RemoveCurrent();
            // show all (unfiltered) orders after deleting one
            this.orderBindingSource.DataSource = _context.Order.Local.ToBindingList(); 
        }

        private void orderBindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            this.orderBindingSource.RemoveCurrent();
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
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
                }
            }
        }

        private void customerDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Customer currentCustomer = (Customer)customerBindingSource.Current;
            CustomerDetailsForm editCustomerDialog = new CustomerDetailsForm(currentCustomer);

            editCustomerDialog.Owner = this;
            if (editCustomerDialog.ShowDialog() == DialogResult.OK)
            {
                Customer editedCustomer = editCustomerDialog.Customer;
                _context.Entry(editedCustomer).State = EntityState.Modified;
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("***************" + ex);
                }
            }
        }

        
    }
}
