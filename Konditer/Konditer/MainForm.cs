using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Konditer
{
    public partial class MainForm : Form
    {
        private CategoryForm categoryForm;
        private DecorForm decorForm;
        private StuffingForm stuffingForm;
        private CakeForm cakeForm;
        private OrdersForm ordersForm;
        public MainForm()
        {
            InitializeComponent();
        }
        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Вы действительно хотите выйти из приложения", "Внимание",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void видыТортовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (categoryForm == null || categoryForm.IsDisposed)
            {
                categoryForm = new CategoryForm();
                categoryForm.MdiParent = this;
                categoryForm.Show();
            }
            else
            {
                categoryForm.Activate();
            }
        }
        private void mnuStuffing_Click(object sender, EventArgs e)
        {
            if (stuffingForm == null || stuffingForm.IsDisposed)
            {
                stuffingForm = new StuffingForm();
                stuffingForm.MdiParent = this;
                stuffingForm.Show();
            }
            else
            {
                stuffingForm.Activate();
            }
        }
        private void mnuDecoration_Click(object sender, EventArgs e)
        {
            if (decorForm == null || decorForm.IsDisposed)
            {
                decorForm = new DecorForm();
                decorForm.MdiParent = this;
                decorForm.Show();
            }
            else
            {
                decorForm.Activate();
            }
        }
        private void mnuCake_Click(object sender, EventArgs e)
        {
            if (cakeForm == null || cakeForm.IsDisposed)
            {
                cakeForm = new CakeForm();
                cakeForm.MdiParent = this;
                cakeForm.Show();
            }
            else
            {
                cakeForm.Activate();
            }
        }
        private void mnuOrders_Click(object sender, EventArgs e)
        {
            if (ordersForm == null || ordersForm.IsDisposed)
            {
                ordersForm = new OrdersForm();
                ordersForm.MdiParent = this;
                ordersForm.Show();
            }
            else
            {
                ordersForm.Activate();
            }
        }
    }
}
