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
    public partial class CakeForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        private DataTable dtCake;
        SqlDataAdapter dataCakeAdapter,dataCategoryAdapter;
        BindingSource bsCake, bsCategory;
        private List<int> cakesCategoryList;
        bool add_items;
        public CakeForm()
        {
            InitializeComponent();
            LoadDataFromTable();
            loadCheckboxlist();
            add_items = false;
        }
        void LoadDataFromTable()
        {
            try
            {
                dtCake = new DataTable();
                dataCakeAdapter = new SqlDataAdapter();
                dataCakeAdapter.SelectCommand = new SqlCommand("SELECT * " +
                                                          "FROM tCake", connection);
                dataCakeAdapter.Fill(dtCake);
                bsCake = new BindingSource();
                bsCake.DataSource = dtCake;
                bindingNavigator1.BindingSource = bsCake;
                dgvTypeTO.DataSource = bsCake;
                dgvTypeTO.Columns[0].Visible = false;
                dgvTypeTO.Columns[1].HeaderText = "Название";
                dgvTypeTO.Columns[2].HeaderText = "Фото";
                ((DataGridViewImageColumn)dgvTypeTO.Columns[2]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                tbName.DataBindings.Clear();
                tbName.DataBindings.Add(new Binding("Text", bsCake, "cake_name"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        void loadCheckboxlist()
        {
            connection.Close();
            connection.Open();
            dataCategoryAdapter = new SqlDataAdapter();
            dataCategoryAdapter.SelectCommand = new SqlCommand("SELECT * FROM tCakeCategory Order by category_name", connection);
            DataTable dt = new DataTable();
            dataCategoryAdapter.Fill(dt);
            bsCategory = new BindingSource();
            bsCategory.DataSource = dt;
            clbType.DataSource = bsCategory;
            clbType.ValueMember = "ID_cake_category";
            clbType.DisplayMember = "category_name";
        }

        void LoadCakeCategory(int ID_cake)
        {
            connection.Close();
            connection.Open();
            try
            {
                for (int i = 0; i < clbType.Items.Count; i++)
                {
                    clbType.SetItemChecked(i, false);
                }
                cakesCategoryList = new List<int>();
                SqlCommand SelectCommand =
                    new SqlCommand("select ID_cake_category FROM dbo.tCakeAndCategory WHERE ID_cake =" + ID_cake.ToString(),
                        connection);
                SqlDataReader dataReader = SelectCommand.ExecuteReader();
                if (dataReader.HasRows) // если есть данные
                {
                    List<int> x = new List<int>();
                    while (dataReader.Read()) // построчно считываем данные
                    {
                        x.Add(Convert.ToInt32(dataReader.GetValue(0)));
                   }
                    int id;

                    for (int i = 0; i < clbType.Items.Count; i++)
                    {
                        var row = (clbType.Items[i] as DataRowView).Row;
                        id = Convert.ToInt32(row["ID_cake_category"]);
                        if (x.Contains(id))
                            clbType.SetItemChecked(i, true);
                        else clbType.SetItemChecked(i, false);
                    }
                    cakesCategoryList = x;
                }
            }
            catch
                (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void SaveCategoryList()
        {
            try
            {
                List<int> y = new List<int>();
                foreach (DataRowView L in clbType.CheckedItems)
                {
                    y.Add(Convert.ToInt32(L[0]));
                }
                foreach (var x in cakesCategoryList)
                {
                    if (!(y.Contains(x)))
                    {
                        DeleteItem(x);
                    }
                }
                foreach (var x in y)
                {
                    if (!(cakesCategoryList.Contains(x)))
                    {
                        AddItem(x);
                    }
                }
            }
            catch
               (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        void DeleteItem(int ID_cat)
        {
            int ID_SS = Convert.ToInt32(((DataRowView)this.bsCake.Current).Row["ID_cake"]);
            connection.Close();
            connection.Open();
            SqlCommand commandDelete = new SqlCommand("Delete From tCakeAndCategory where ID_cake = @ID1 and ID_cake_category =@ID2", connection);
            commandDelete.Parameters.AddWithValue("@ID1", ID_SS);
            commandDelete.Parameters.AddWithValue("@ID2", ID_cat);
            commandDelete.ExecuteNonQuery();
        }
        void AddItem(int ID_cat)
        {
            int ID_SS = Convert.ToInt32(((DataRowView)this.bsCake.Current).Row["ID_cake"]);
            connection.Close();
            connection.Open();
            SqlCommand commandInsert = new SqlCommand("INSERT INTO [tCakeAndCategory] VALUES" +
                       " (@ID1, @ID2)", connection);
            commandInsert.Parameters.AddWithValue("@ID1", ID_SS);
            commandInsert.Parameters.AddWithValue("@ID2", ID_cat);
            commandInsert.ExecuteNonQuery();
        }
        Image LoadImage()
        {
            if (bsCake.Count > 0)
            {
                int i = bsCake.Position;
                if ((((DataRowView)this.bsCake.Current).Row["photo"] != System.DBNull.Value))
                {
                    byte[] byteArrayIn = (byte[])(((DataRowView)this.bsCake.Current).Row["photo"]);
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
            if ((tbName.Text == "") )
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
            bsCake.Filter = "cake_name LIKE '%" + toolStripTextBox1.Text + "%'";
        }
        void ClearItems()
        {
            tbName.Text = "";
            
            pictureBox1.Image = pictureBox1.InitialImage;
            for (int i = 0; i < clbType.Items.Count; i++)
            {
                clbType.SetItemChecked(i, false);
            }
        }
        /// <summary>
        /// DeleteData()
        /// </summary>
        void DeleteData()
        {
            if (bsCake.Count > 0)
            {
                int i = bsCake.Position;
                int ID_SS = Convert.ToInt32(((DataRowView)this.bsCake.Current).Row["ID_cake"]);
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
                        SqlCommand commandDelete = new SqlCommand("Delete From tCake where ID_cake = @ID", connection);
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
                    SqlCommand commandInsert = new SqlCommand("INSERT INTO [tCake] VALUES" +
                        " (@Name, NULL)", connection);
                    commandInsert.Parameters.AddWithValue("@Name", tbName.Text);

                    commandInsert.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand commandInsert = new SqlCommand("INSERT INTO [tCake] VALUES" +
                                           " (@Name, @photo)", connection);
                    commandInsert.Parameters.AddWithValue("@Name", tbName.Text);

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
            if (bsCake.Count > 0)
            {
                int i = bsCake.Position;
                int ii = bsCake.Position;
                try
                {
                    connection.Close();
                    connection.Open();
                    int ID_SS = Convert.ToInt32(((DataRowView)this.bsCake.Current).Row["Id_cake"]);
                    if (pictureBox1.Image == null)
                    {
                        SqlCommand commandUpdate = new SqlCommand("UPDATE tCake SET" +
                        " cake_name=@name " +
                      
                        " WHERE Id_cake = @IDSS", connection);
                        commandUpdate.Parameters.AddWithValue("@Name", tbName.Text);

                        commandUpdate.Parameters.AddWithValue("@IDSS", ID_SS);
                        commandUpdate.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand commandUpdate = new SqlCommand("UPDATE tCake SET" +
                        " cake_name=@name, " +
                        " photo =@photo" +
                        " WHERE Id_cake = @IDSS", connection);
                        commandUpdate.Parameters.AddWithValue("@Name", tbName.Text);

                        commandUpdate.Parameters.AddWithValue("@Photo", ConvertInBytes(pictureBox1.Image));
                        commandUpdate.Parameters.AddWithValue("@IDSS", ID_SS);
                        commandUpdate.ExecuteNonQuery();
                    }
                    SaveCategoryList();
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
                    bsCake.Position = ii;
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
            if ((bsCake.Count > 0))
            {
                pictureBox1.Image = LoadImage();
                LoadCakeCategory(Convert.ToInt32(((DataRowView) this.bsCake.Current).Row["Id_cake"]));
            }
        }
    }
}
