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
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms.VisualStyles;
using Excel = Microsoft.Office.Interop.Excel;

namespace Konditer
{
    public partial class OrdersForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        DataTable dtOrders;
        SqlDataAdapter dataAdapterOrder;
        BindingSource bsOrders;

        private void dgvTypeTO_DoubleClick(object sender, EventArgs e)
        {
            ShowOrderToChange();
        }

        private void tsbChange_Click(object sender, EventArgs e)
        {
            ShowOrderToChange();
        }

        public OrdersForm()
        {
            InitializeComponent();
            LoadDataFromTable();
            dtpBegin.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today;
        }

        private void chbStatus_CheckedChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void chbDate_CheckedChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void tsbExcel_Click(object sender, EventArgs e)
        {
            PrintExcel();
        }

        void FilterData()
        {
            bsOrders.RemoveFilter();
            var queries = new List<string>();
            if (chbStatus.Checked)
            {
                queries.Add(string.Format("[status]={0}", true));

            }
            if (chbDate.Checked)
            {
                queries.Add(string.Format("[date_start] >=#{0}# AND [date_start]<=#{1}#",
                    dtpBegin.Value.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat),
                    dtpEnd.Value.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat)));
            }
            if (toolStripTextBox1.Text != "")
            {
                queries.Add(string.Format("[ID_order] = {0}", toolStripTextBox1.Text));
            }
            if (queries.Count >= 1)
            {
                var queryFilter = String.Join(" AND ", queries);
                bsOrders.Filter = queryFilter;
            }
        }

        void ShowOrderToChange()
        {
            if ((bsOrders.Count > 0) && (dgvTypeTO.SelectedRows.Count > 0))
            {
                int ID_SS = Convert.ToInt32(((DataRowView)this.bsOrders.Current).Row["ID_order"]);
                NewOrderForm flat_form = new NewOrderForm(ID_SS);
                flat_form.ShowDialog();
                LoadDataFromTable();
                bsOrders.Position = bsOrders.Find("ID_order", (ID_SS));
            }
        }
        private void tsbAdd_Click(object sender, EventArgs e)
        {
            NewOrderForm x = new NewOrderForm();
            x.ShowDialog();
            LoadDataFromTable();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (bsOrders.Count > 0)
            {
                int i = bsOrders.Position;
                int ID_SS = Convert.ToInt32(((DataRowView)this.bsOrders.Current).Row["ID_Order"]);
                try
                {
                    DialogResult result = MessageBox.Show("Вы действительно хотите удалить запись", "Внимание", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        LoadDataFromTable();
                        return;
                    }
                    if (result == DialogResult.Yes)
                    {
                        connection.Close();
                        connection.Open();
                        SqlCommand commandDelete = new SqlCommand("Delete From tOrder where ID_order = @ID_order", connection);
                        commandDelete.Parameters.AddWithValue("@ID_order", ID_SS);
                        commandDelete.ExecuteNonQuery();
                    }
                }
                catch (SqlException exception)
                {
                    if (exception.HResult == -2146232060)
                        MessageBox.Show("Ошибка удаления, есть связанные записи");
                    else MessageBox.Show(exception.Message);
                }
                finally
                {
                    connection.Close();
                    LoadDataFromTable();
                }
            }
        }

        void LoadDataFromTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    dtOrders = new DataTable();
                    dataAdapterOrder = new SqlDataAdapter();
                    dataAdapterOrder.SelectCommand = new SqlCommand(" SELECT " +
                        " tOrder.ID_order," +
                        " tOrder.price," +
                       " tOrder.date_start," +
                      " tOrder.date_end," +
                      " tOrder.comment," +
                      " tOrder.customer_name," +
                      " tOrder.customer_phone," +
                      " tOrder.customer_email," +
                      " tOrder.ID_cake," +
                      " tOrder.ID_stuffing," +
                      " tOrder.status, " + " tOrder.weight " +
                    "FROM tOrder ", connection);
                    dataAdapterOrder.Fill(dtOrders);
                    bsOrders = new BindingSource();
                    bsOrders.DataSource = dtOrders;
                    bindingNavigator1.BindingSource = bsOrders;
                    dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvTypeTO.DataSource = bsOrders;
                    dgvTypeTO.Columns[0].HeaderText = "Номер заказа";
                    dgvTypeTO.Columns[1].HeaderText = "Стоимость";
                    dgvTypeTO.Columns[3].HeaderText = "Дата подачи";
                    dgvTypeTO.Columns[2].HeaderText = "Дата выдачи";
                    dgvTypeTO.Columns[4].HeaderText = "Информация";
                    dgvTypeTO.Columns[5].HeaderText = "Клиент";
                    dgvTypeTO.Columns[6].HeaderText = "Телефон клиента";
                    dgvTypeTO.Columns[7].HeaderText = "e-mail клиента";
                    dgvTypeTO.Columns[8].Visible = false;
                    dgvTypeTO.Columns[9].Visible = false;
                    dgvTypeTO.Columns[10].HeaderText = "выполнен";
                    dgvTypeTO.Columns[11].HeaderText = "вес";
                    if (bsOrders.Count <= 0) tsbDelete.Enabled = false;
                    else tsbDelete.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
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

        private void PrintExcel()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\" + "Orders" + ".xltx";
            Excel.Application xlApp = new Excel.Application();
            Excel.Worksheet xlSheet = new Excel.Worksheet();
            try
            {
                //добавляем книгу
                xlApp.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                          Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                          Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                          Type.Missing, Type.Missing);
                //делаем временно неактивным документ
                xlApp.Interactive = false;
                xlApp.EnableEvents = false;
                Excel.Range xlSheetRange;
                //выбираем лист на котором будем работать (Лист 1)
                xlSheet = (Excel.Worksheet)xlApp.Sheets[1];
                //Название листа
                xlSheet.Name = "Список";
                int row = 2;
                int i = 0;
                if (dgvTypeTO.RowCount > 0)
                {
                    for (i = 0; i < dgvTypeTO.RowCount; i++)
                    {
                        xlSheet.Cells[row, 1] = dgvTypeTO.Rows[i].Cells[0].Value.ToString();
                        string x = "";
                        xlSheet.Cells[row, 2] = dgvTypeTO.Rows[i].Cells[1].Value.ToString();
                        if (dgvTypeTO.Rows[i].Cells[2].Value != DBNull.Value)
                            x = Convert.ToDateTime(dgvTypeTO.Rows[i].Cells[2].Value).ToShortDateString();
                        xlSheet.Cells[row, 3] = x;
                        x = "";
                        if (dgvTypeTO.Rows[i].Cells[3].Value != DBNull.Value)
                            x = Convert.ToDateTime(dgvTypeTO.Rows[i].Cells[3].Value).ToShortDateString();
                        xlSheet.Cells[row, 4] = x;
                        xlSheet.Cells[row, 5] = dgvTypeTO.Rows[i].Cells[4].Value.ToString();
                        xlSheet.Cells[row, 6] = dgvTypeTO.Rows[i].Cells[5].Value.ToString();
                        xlSheet.Cells[row, 7] = dgvTypeTO.Rows[i].Cells[6].Value.ToString();
                        xlSheet.Cells[row, 8] = dgvTypeTO.Rows[i].Cells[7].Value.ToString();
                        xlSheet.Cells[row, 9] = dgvTypeTO.Rows[i].Cells[11].Value.ToString();
                        x = "не выполнен";
                        if (Convert.ToBoolean(dgvTypeTO.Rows[i].Cells[10].Value) == true)
                            x = "выполнен";
                        xlSheet.Cells[row, 10] = x;
                        row++;
                        Excel.Range r = xlSheet.get_Range("A" + row.ToString(), "J" + row.ToString());
                        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    }
                }
                row--;
                xlSheetRange = xlSheet.get_Range("A2:J" + row.ToString(), Type.Missing);
                xlSheetRange.Borders.LineStyle = true;
                row++;
                //выбираем всю область данных*/
                xlSheetRange = xlSheet.UsedRange;
                //выравниваем строки и колонки по их содержимому
                xlSheetRange.Columns.AutoFit();
                xlSheetRange.Rows.AutoFit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //Показываем ексель
                xlApp.Visible = true;
                xlApp.Interactive = true;
                xlApp.ScreenUpdating = true;
                xlApp.UserControl = true;
            }
        }
    }
}
