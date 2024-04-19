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

namespace Konditer
{
    public partial class StuffingForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        DataTable dtStuff;
        SqlDataAdapter dataStuffAdapter;
        BindingSource bsStuff;
        bool add_items;
        public StuffingForm()
        {
            InitializeComponent();
            LoadDataFromTable();
           
            add_items = false;
        }
        void LoadDataFromTable()
        {
            try
            {
                dtStuff = new DataTable();
                dataStuffAdapter = new SqlDataAdapter();
                dataStuffAdapter.SelectCommand = new SqlCommand("SELECT * " +
                                                          "FROM tStuffing", connection);

                dataStuffAdapter.Fill(dtStuff);
                bsStuff = new BindingSource();
                bsStuff.DataSource = dtStuff;
                bindingNavigator1.BindingSource = bsStuff;
                dgvTypeTO.DataSource = bsStuff;
                dgvTypeTO.Columns[0].Visible = false;
                dgvTypeTO.Columns[1].HeaderText = "Название";
                dgvTypeTO.Columns[2].HeaderText = "Описание";
                dgvTypeTO.Columns[3].HeaderText = "Цена за кг.";
                dgvTypeTO.Columns[4].HeaderText = "Фото";
                ((DataGridViewImageColumn)dgvTypeTO.Columns[4]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                tbName.DataBindings.Clear();
                tbName.DataBindings.Add(new Binding("Text", bsStuff, "stuffing_name"));
                tbPrice.DataBindings.Clear();
                tbPrice.DataBindings.Add(new Binding("Text", bsStuff, "price"));
                tbInfo.DataBindings.Clear();
                tbInfo.DataBindings.Add(new Binding("Text", bsStuff, "info"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
               (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        Image LoadImage()
        {
            if (bsStuff.Count > 0)
            {
                int i = bsStuff.Position;
                if ((((DataRowView)this.bsStuff.Current).Row["photo"] != System.DBNull.Value))
                {
                    byte[] byteArrayIn = (byte[])(((DataRowView)this.bsStuff.Current).Row["photo"]);
                    using (var ms = new MemoryStream(byteArrayIn))
                    {
                        return Image.FromStream(ms);
                    }
                }
                return null;
            }
            else return null;
        }
        private void deleteStripButton2_Click(object sender, EventArgs e)
        {
            if (add_items)
            {
                LoadDataFromTable();
                add_items = false;
                return;
            }
            DeleteData();
        }
        /// <summary>
        /// преобразует картинку в биты
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        byte[] ConvertInBytes(Image img)
        {
            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                bytes = ms.ToArray();
            }
            return bytes;
        }
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if ((tbName.Text == "") || (tbPrice.Text == "") || (tbInfo.Text == "") )
                return;
            if (add_items)
            {
                SaveData();
            }
            else
            {
                UpdateData();
            }
        }
        private void dgvTypeTO_SelectionChanged(object sender, EventArgs e)
        {
            LoadCombo();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            try
            {
                string filename = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(filename);// читаем файл в строку
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки файла", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearItems();
            add_items = true;
        }
        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            //          bsGoods.Filter = "ID_good LIKE '%" + toolStripTextBox1.Text + "%'";
                bsStuff.Filter = "stuffing_name LIKE '%" + toolStripTextBox1.Text + "%'";
        }
       
        void ClearItems()
        {
            tbName.Text = "";
            tbPrice.Text = "";
            tbInfo.Text = "";
            pictureBox1.Image = pictureBox1.InitialImage;

        }
        /// <summary>
        /// DeleteData()
        /// </summary>
        void DeleteData()
        {
            if (bsStuff.Count > 0)
            {
                int i = bsStuff.Position;
                int ID_SS = Convert.ToInt32(((DataRowView)this.bsStuff.Current).Row["ID_stuffing"]);
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
                        SqlCommand commandDelete = new SqlCommand("Delete From tStuffing where ID_stuffing = @ID", connection);
                        commandDelete.Parameters.AddWithValue("@ID", ID_SS);
                        commandDelete.ExecuteNonQuery();
                        ClearItems();
                    }
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    connection.Close();
                    LoadDataFromTable();
                }
            }
        }
        void SaveData()
        {
            int ID_SS = 0;
            try
            {
                connection.Close();
                connection.Open();
                if (pictureBox1.Image == null)
                {
                    SqlCommand commandInsert = new SqlCommand("INSERT INTO [tStuffing] VALUES" +
                        " (@Name,@Info, @Price, NULL)", connection);
                    commandInsert.Parameters.AddWithValue("@Name", tbName.Text);
                    commandInsert.Parameters.AddWithValue("@Info", tbInfo.Text);
                    commandInsert.Parameters.AddWithValue("@Price", tbPrice.Text);
                    commandInsert.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand commandInsert = new SqlCommand("INSERT INTO [tStuffing] VALUES" +
                                           " (@Name,@Info, @Price, @photo)", connection);
                    commandInsert.Parameters.AddWithValue("@Name", tbName.Text);
                    commandInsert.Parameters.AddWithValue("@Info", tbInfo.Text);
                    commandInsert.Parameters.AddWithValue("@Price", tbPrice.Text);
                    commandInsert.Parameters.AddWithValue("@photo", ConvertInBytes(pictureBox1.Image));
                    commandInsert.ExecuteNonQuery();
                }
                MessageBox.Show("Запись добавлена");
                add_items = false;
            }
            catch (SqlException exception)
            {
                MessageBox.Show(exception.ToString());
            }
            finally
            {
                connection.Close();
                LoadDataFromTable();
            }
        }

        void UpdateData()
        {
            if (bsStuff.Count > 0)
            {
                int i = bsStuff.Position;
                int ii = bsStuff.Position;
                try
                {
                    connection.Close();
                    connection.Open();
                    int ID_SS = Convert.ToInt32(((DataRowView)this.bsStuff.Current).Row["Id_stuffing"]);
                    if (pictureBox1.Image == null)
                    {
                        SqlCommand commandUpdate = new SqlCommand("UPDATE tStuffing SET" +
                        " stuffing_name=@name, " +
                        " info=@info, price=@price," +
                        " WHERE Id_stuffing = @IDSS", connection);
                        commandUpdate.Parameters.AddWithValue("@Name", tbName.Text);
                        commandUpdate.Parameters.AddWithValue("@Info", tbInfo.Text);
                        commandUpdate.Parameters.AddWithValue("@Price", tbPrice.Text);
                        commandUpdate.Parameters.AddWithValue("@IDSS", ID_SS);
                        commandUpdate.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand commandUpdate = new SqlCommand("UPDATE tStuffing SET" +
                        " stuffing_name=@name, " +
                        " info=@info, price=@price, photo =@photo" +
                        " WHERE Id_stuffing = @IDSS", connection);
                        commandUpdate.Parameters.AddWithValue("@Name", tbName.Text);
                        commandUpdate.Parameters.AddWithValue("@Info", tbInfo.Text);
                        commandUpdate.Parameters.AddWithValue("@Price", tbPrice.Text);
                        commandUpdate.Parameters.AddWithValue("@Photo", ConvertInBytes(pictureBox1.Image));
                        commandUpdate.Parameters.AddWithValue("@IDSS", ID_SS);
                        commandUpdate.ExecuteNonQuery();
                    }
                    MessageBox.Show("Запись обновлена");
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    connection.Close();
                    LoadDataFromTable();
                    bsStuff.Position = ii;
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
        void LoadCombo()
        {
            if ((bsStuff.Count > 0))
            {
               pictureBox1.Image = LoadImage();
            }
        }
    }
}

