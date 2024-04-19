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
    public partial class DecorForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        DataTable dtDecor;
        SqlDataAdapter dataDecorAdapter;
        BindingSource bsDecor;
        bool add_items;
        public DecorForm()
        {
            InitializeComponent();
            LoadDataFromTable();

            add_items = false;
        }
        void LoadDataFromTable()
        {
            try
            {
                dtDecor = new DataTable();
                dataDecorAdapter = new SqlDataAdapter();
                dataDecorAdapter.SelectCommand = new SqlCommand("SELECT * " +
                                                          "FROM tDecor", connection);
                dataDecorAdapter.Fill(dtDecor);
                bsDecor = new BindingSource();
                bsDecor.DataSource = dtDecor;
                bindingNavigator1.BindingSource = bsDecor;
                dgvTypeTO.DataSource = bsDecor;
                dgvTypeTO.Columns[0].Visible = false;
                dgvTypeTO.Columns[1].HeaderText = "Название";
                dgvTypeTO.Columns[2].HeaderText = "Описание";
                dgvTypeTO.Columns[3].HeaderText = "Цена за кг.";
                dgvTypeTO.Columns[4].HeaderText = "Фото";
                dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                ((DataGridViewImageColumn)dgvTypeTO.Columns[4]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                tbName.DataBindings.Clear();
                tbName.DataBindings.Add(new Binding("Text", bsDecor, "decor_name"));
                tbPrice.DataBindings.Clear();
                tbPrice.DataBindings.Add(new Binding("Text", bsDecor, "price"));
                tbInfo.DataBindings.Clear();
                tbInfo.DataBindings.Add(new Binding("Text", bsDecor, "info"));
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
            if (bsDecor.Count > 0)
            {
                int i = bsDecor.Position;
                if ((((DataRowView)this.bsDecor.Current).Row["photo"] != System.DBNull.Value))
                {
                    byte[] byteArrayIn = (byte[])(((DataRowView)this.bsDecor.Current).Row["photo"]);
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
            if ((tbName.Text == "") || (tbPrice.Text == "") || (tbInfo.Text == ""))
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
            bsDecor.Filter = "decor_name LIKE '%" + toolStripTextBox1.Text + "%'";
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
            if (bsDecor.Count > 0)
            {
                int i = bsDecor.Position;
                int ID_SS = Convert.ToInt32(((DataRowView)this.bsDecor.Current).Row["ID_decor"]);
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
                        SqlCommand commandDelete = new SqlCommand("Delete From tDecor where ID_Decor = @ID", connection);
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
                    SqlCommand commandInsert = new SqlCommand("INSERT INTO [tDecor] VALUES" +
                        " (@Name,@Info, @Price, NULL)", connection);
                    commandInsert.Parameters.AddWithValue("@Name", tbName.Text);
                    commandInsert.Parameters.AddWithValue("@Info", tbInfo.Text);
                    commandInsert.Parameters.AddWithValue("@Price", tbPrice.Text);
                    commandInsert.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand commandInsert = new SqlCommand("INSERT INTO [tDecor] VALUES" +
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
            if (bsDecor.Count > 0)
            {
                int i = bsDecor.Position;
                int ii = bsDecor.Position;
                try
                {
                    connection.Close();
                    connection.Open();
                    int ID_SS = Convert.ToInt32(((DataRowView)this.bsDecor.Current).Row["Id_decor"]);
                    if (pictureBox1.Image == null)
                    {
                        SqlCommand commandUpdate = new SqlCommand("UPDATE tDecor SET" +
                        " decor_name=@name, " +
                        " info=@info, price=@price," +
                        " WHERE Id_decor = @IDSS", connection);
                        commandUpdate.Parameters.AddWithValue("@Name", tbName.Text);
                        commandUpdate.Parameters.AddWithValue("@Info", tbInfo.Text);
                        commandUpdate.Parameters.AddWithValue("@Price", tbPrice.Text);
                        commandUpdate.Parameters.AddWithValue("@IDSS", ID_SS);
                        commandUpdate.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand commandUpdate = new SqlCommand("UPDATE tDecor SET" +
                        " decor_name=@name, " +
                        " info=@info, price=@price, photo =@photo" +
                        " WHERE Id_decor = @IDSS", connection);
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
                    bsDecor.Position = ii;
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
            if ((bsDecor.Count > 0))
            {
                pictureBox1.Image = LoadImage();
            }
        }
    }
}

