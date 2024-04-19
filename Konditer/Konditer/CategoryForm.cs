using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Konditer
{
    public partial class CategoryForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        DataSet ds;
        SqlDataAdapter dataAdapter;
        BindingSource bs1;
        public CategoryForm()
        {
            InitializeComponent();
            LoadDataFromTable();
        }
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommandBuilder CmbSAve = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Update(ds);
                saveToolStripButton.Enabled = false;
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                    MessageBox.Show("Ошибка удаления, есть связанные записи");
                else MessageBox.Show(ex.Message);

                LoadDataFromTable();
            }
        }

        void LoadDataFromTable()
        {
            try
            {
                ds = new DataSet();
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("Select * from tCakeCategory Order by category_name", connection);

                //dt1 = new DataTable();
                ds.Clear();
                dataAdapter.Fill(ds);
                ds.AcceptChanges();
                bs1 = new BindingSource();
                bs1.DataSource = ds.Tables[0];
                bindingNavigator1.BindingSource = bs1;
                dgvTypeTO.DataSource = bs1;

                dgvTypeTO.Columns[0].Visible = false;
                dgvTypeTO.Columns[1].HeaderText = "Вид торта";
                dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void deleteStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                int i = dgvTypeTO.CurrentRow.Index;
                dgvTypeTO.Rows.RemoveAt(i);
                saveToolStripButton.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            LoadDataFromTable();
        }

        private void dgvTypeTO_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            saveToolStripButton.Enabled = true;
            int i = dgvTypeTO.CurrentRow.Index;
        }

        private void dgvTypeTO_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            saveToolStripButton.Enabled = true;
        }

        private void dgvTypeTO_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            saveToolStripButton.Enabled = true;
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            bs1.Filter = "category_name LIKE '%" + toolStripTextBox1.Text + "%'";
        }

        private void dgvTypeTO_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();
            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
    }
}