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
using System.IO;
using System.Drawing.Imaging;
using Konditer.models;
using Excel = Microsoft.Office.Interop.Excel;

namespace Konditer
{
    public partial class NewOrderForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlDataAdapter  dataCategoryAdapter;
        BindingSource  bsCategory;
        double DecorPrice = 0;
        List<Tort> tortMainList, tortSecondList;
        List<Stuffing> stuffMainList, stuffSecondList;
        List<Decor> decorMainList, decorSecondList;
        List<int> cakesCategoryList;
        private ListViewItem lastItemChecked, lastItemChecked1, lastItemChecked2;
        int ID_order = -1;
        int ID_cake = -1;
        int ID_stuffing = -1;
        bool add_items;
        Order order;

        public NewOrderForm()
        {
            InitializeComponent();
            LoadTort();
            LoadStuffing();
            LoadDecor();
            loadCheckboxlist();
            label1.Text = "";
            add_items = false;
            ID_order = -1;
            dtpStopDate.Enabled = false;
        }
        public NewOrderForm(int order)
        {
            InitializeComponent();
            LoadTort();
            LoadStuffing();
            LoadDecor();
            loadCheckboxlist();
            label1.Text = "";
            add_items = true;
            ID_order = order;
        }

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // if we have the lastItem set as checked, and it is different
            // item than the one that fired the event, uncheck it
            if (lastItemChecked != null && lastItemChecked.Checked
                && lastItemChecked != listViewTort.Items[e.Index])
            {
                // uncheck the last item and store the new one
                lastItemChecked.Checked = false;
            }
            // store current item
            ID_cake = Convert.ToInt32(listViewTort.Items[e.Index].Tag);
           // MessageBox.Show(ID_cake.ToString());
            lastItemChecked = listViewTort.Items[e.Index];
        }
        void loadCheckboxlist()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
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
        }
        void FilterData()
        {
            if ((chbTort.Checked == false))
            {
                FullTortImageList(tortMainList);
                return;
            }
            List<Tort> z = new List<Tort>();
            tortSecondList = new List<Tort>();
            tortSecondList = tortMainList;
            List<int> y = new List<int>();
            foreach (DataRowView L in clbType.CheckedItems)
            {
                y.Add(Convert.ToInt32(L[0]));
            }
            foreach (Tort s in tortSecondList)
            {
                if (s.cake_category.Intersect(y).Any())
                {
                    z.Add(s);
                }
            }
            tortSecondList = z;
            FullTortImageList(tortSecondList);
        }
        private void listView2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // if we have the lastItem set as checked, and it is different
            // item than the one that fired the event, uncheck it
            if (lastItemChecked1 != null && lastItemChecked1.Checked
                && lastItemChecked1 != listViewStuffing.Items[e.Index])
            {
                // uncheck the last item and store the new one
                lastItemChecked1.Checked = false;
            }

            // store current item
            ID_stuffing = Convert.ToInt32(listViewStuffing.Items[e.Index].Tag);
            //MessageBox.Show(ID_stuffing.ToString());
            lastItemChecked1 = listViewStuffing.Items[e.Index];
        }
        private void listView2_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked)
            {
                tbCostStuffing.Text = e.Item.SubItems[2].Text.ToString();
            }
            else tbCostStuffing.Text = "0";
            PriceForWeight();
        }

        private void chbTort_CheckedChanged(object sender, EventArgs e)
        {
            FilterData();
        }
        private void NewOrderForm_Load(object sender, EventArgs e)
        {
            LoadDataFromTable();
        }
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if ((tbCustomerName.Text == "")
               || (tbCustomerPhone.Text == "")
               || (tbCustomerEmail.Text == "")
               || (tbCostDecor.Text == "0")
               || (tbCostStuffing.Text == "")
               )
            {
                MessageBox.Show("Ключевые поля пустые");
                return;
            }
            if (add_items == true)
            {
                UpdateData();
            }
            else
            {
                SaveData();
            }
        }

        private void listView3_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            PriceForWeight();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            PriceForWeight();
        }

        private void chbStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (chbStatus.Checked)
            {
                dtpStopDate.Enabled = true;
                tsbAktDone.Enabled = true;
            }

            else
            {
                dtpStopDate.Enabled = false;
                tsbAktDone.Enabled = false;
            }
        }

        private void tsbAktPriema_Click(object sender, EventArgs e)
        {
            PrintAktPriema();
        }

        private void PrintAktPriema()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\" + "AktPriema" + ".xltx";
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
                xlSheet.Name = "Акт Приема";

                string tort_name = "";
                foreach (ListViewItem x in listViewTort.CheckedItems)
                {
                    tort_name = x.Text;
                }
                string stuffing_name = "";
                foreach (ListViewItem x in listViewStuffing.CheckedItems)
                {
                    stuffing_name = x.Text;
                }
                string decor_name = "";
                foreach (ListViewItem x in listViewDecor.CheckedItems)
                {
                    decor_name = decor_name +", " + x.Text;
                }
                decor_name = decor_name.Substring(2);

                xlSheet.Cells[2, 2] = order.ID_order.ToString();
                xlSheet.Cells[4, 2] = order.customer_name;
                xlSheet.Cells[5, 2] = order.customer_phone;
                xlSheet.Cells[6, 2] = order.customer_email;
                xlSheet.Cells[8, 2] = order.date_start.ToShortDateString();
                xlSheet.Cells[9, 2] = order.weight.ToString();
                xlSheet.Cells[10, 2] = tort_name;
                xlSheet.Cells[11, 2] = stuffing_name;
                xlSheet.Cells[12, 2] = decor_name;
                xlSheet.Cells[13, 2] = order.comment;
                xlSheet.Cells[14, 2] = order.price.ToString();
                xlSheet.Cells[17, 2] = "/" + getFam(order.customer_name) + "/";
                //выбираем всю область данных*/
                xlSheetRange = xlSheet.UsedRange;

                //выравниваем строки и колонки по их содержимому
                //xlSheetRange.Columns.AutoFit();
                //xlSheetRange.Rows.AutoFit();
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
        string getFam(string s)
        {
            int k = s.Length;
            string Fam = "", imya = "", otch = "";
            Fam = s.Substring(0, s.IndexOf(' '));
            int l = s.IndexOf(' ');
            if (l > 0 && s.Length > 0)
            {
                s = s.Remove(0, s.IndexOf(' ') + 1);
                imya = s.Substring(0, 1) + ".";
            }
            else s = "";

            l = s.IndexOf(' ');
            if (l > 0 && s.Length > 0)
            {
                s = s.Remove(0, s.IndexOf(' ') + 1);
                otch = s.Substring(0, 1) + ". ";
            }
            return imya + otch + " " + Fam;
        }


        private void PrintAktDone()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\" + "AktDone" + ".xltx";
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
                xlSheet.Name = "Акт выполненных работ";
                string tort_name = "";
                foreach (ListViewItem x in listViewTort.CheckedItems)
                {
                    tort_name = x.Text;
                }
                string stuffing_name = "";
                foreach (ListViewItem x in listViewStuffing.CheckedItems)
                {
                    stuffing_name = x.Text;
                }
                string decor_name = "";
                foreach (ListViewItem x in listViewDecor.CheckedItems)
                {
                    decor_name = decor_name + ", " + x.Text;
                }
                decor_name = decor_name.Substring(2);

                xlSheet.Cells[2, 2] = order.ID_order.ToString();
                xlSheet.Cells[4, 2] = order.customer_name;
                xlSheet.Cells[5, 2] = order.customer_phone;
                xlSheet.Cells[6, 2] = order.customer_email;
                xlSheet.Cells[8, 2] = order.date_start.ToShortDateString();
                xlSheet.Cells[9, 2] = order.date_end.ToShortDateString();
                xlSheet.Cells[10, 2] = order.weight.ToString();
                xlSheet.Cells[11, 2] = tort_name;
                xlSheet.Cells[12, 2] = stuffing_name;
                xlSheet.Cells[13, 2] = decor_name;
                xlSheet.Cells[14, 2] = order.comment;
                xlSheet.Cells[15, 2] = order.price.ToString();
                xlSheet.Cells[18, 2] = "/"+getFam(order.customer_name)+"/";
                //выбираем всю область данных*/
                xlSheetRange = xlSheet.UsedRange;
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

        private void tsbAktDone_Click(object sender, EventArgs e)
        {
            PrintAktDone();
        }

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

        void LoadDataFromTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    SqlCommand SelectCommand = new SqlCommand(" SELECT " +
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
                      " tOrder.status, " +
                      " tOrder.weight " +
                    "FROM tOrder WHERE ID_order = " + ID_order, connection);
                    SqlDataReader reader = SelectCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        order = new Order();
                        order.ID_order = Convert.ToInt32(reader.GetValue(0));
                        order.price = Convert.ToDouble(reader.GetValue(1));
                        order.date_start = Convert.ToDateTime(reader.GetValue(2));
                        if (reader.GetValue(3) != DBNull.Value)
                        {
                            order.date_end = Convert.ToDateTime(reader.GetValue(3));
                            dtpStopDate.Value = order.date_end;
                        }
                        order.comment = Convert.ToString(reader.GetValue(4));
                        order.customer_name = Convert.ToString(reader.GetValue(5));
                        order.customer_phone = Convert.ToString(reader.GetValue(6));
                        order.customer_email = Convert.ToString(reader.GetValue(7));
                        order.ID_cake = Convert.ToInt32(reader.GetValue(8));
                        order.ID_stuffing = Convert.ToInt32(reader.GetValue(9));
                        order.status = Convert.ToBoolean(reader.GetValue(10));
                        order.weight = Convert.ToDouble(reader.GetValue(11));
                        label1.Text = order.ID_order.ToString();
                        dtpStartDate.Value = order.date_start;
                        tbComment.Text = order.comment;
                        tbITOGO.Text = order.price.ToString();
                        if (order.status)
                        {
                            dtpStopDate.Enabled = true;
                            tsbAktDone.Enabled = true;
                        }

                        else
                        {
                            dtpStopDate.Enabled = false;
                            tsbAktDone.Enabled = false;
                        }
                        chbStatus.Checked = order.status;
                        tbCustomerName.Text = order.customer_name;
                        tbCustomerPhone.Text = order.customer_phone;
                        tbCustomerEmail.Text = order.customer_email;
                        numericUpDown2.Value = Convert.ToDecimal(order.weight);
                        foreach (ListViewItem x in listViewTort.Items)
                        {
                            if (x.Tag.ToString() == order.ID_cake.ToString())
                                x.Checked = true;
                        }
                        foreach (ListViewItem x in listViewStuffing.Items)
                        {
                            if (x.Tag.ToString() == order.ID_stuffing.ToString())
                                x.Checked = true;
                        }
                        LoadDecorOfOrder(order);
                        ID_order = order.ID_order;
                    }
            }
                catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        }

        void LoadDecorOfOrder(Order new_order)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    for (int i = 0; i < clbType.Items.Count; i++)
                    {
                        clbType.SetItemChecked(i, false);
                    }
                    SqlCommand SelectCommand = new SqlCommand("select ID_decor FROM tDecorCake WHERE ID_order =" + new_order.ID_order.ToString(), connection);
                    SqlDataReader dataReader = SelectCommand.ExecuteReader();
                    if (dataReader.HasRows) // если есть данные
                    {
                        List<int> x = new List<int>();
                        while (dataReader.Read()) // построчно считываем данные
                        {
                            x.Add(Convert.ToInt32(dataReader.GetValue(0)));
                        }
                        new_order.iddecor = x;
                        foreach (ListViewItem item in listViewDecor.Items)
                        {
                            if (x.Contains(Convert.ToInt32(item.Tag)))
                                item.Checked = true;
                        }
                    }
                }
                catch
                    (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        void SaveData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int ID_SS = 0;
                try
                {
                  SqlCommand commandInsert = new SqlCommand("INSERT INTO [tOrder] VALUES(" +
                                                   
                                                    "@price," +
                                                    "@date_start," +
                                                    "@date_end," +
                                                    "@comment," +
                                                    "@customer_name," +
                                                    "@customer_phone," +
                                                    "@customer_email," +
                                                    "@ID_cake," +
                                                    "@ID_stuffing," +
                                                     "@status," +
                                                     "@weight" +
                                                    ") ; SELECT SCOPE_IDENTITY()", connection);

                    commandInsert.Parameters.AddWithValue("@price", Convert.ToDouble(tbITOGO.Text));
                    commandInsert.Parameters.AddWithValue("@date_start", dtpStartDate.Value);
                    if (chbStatus.Checked)
                        commandInsert.Parameters.AddWithValue("@date_end", dtpStopDate.Value);
                    else
                        commandInsert.Parameters.AddWithValue("@date_end", DBNull.Value);
                    commandInsert.Parameters.AddWithValue("@comment",tbComment.Text);
                    commandInsert.Parameters.AddWithValue("@customer_name", tbCustomerName.Text);
                    commandInsert.Parameters.AddWithValue("@customer_phone", tbCustomerPhone.Text);
                    commandInsert.Parameters.AddWithValue("@customer_email", tbCustomerEmail.Text);
                    commandInsert.Parameters.AddWithValue("@ID_cake", ID_cake);
                    commandInsert.Parameters.AddWithValue("@ID_stuffing", ID_stuffing);
                    commandInsert.Parameters.AddWithValue("@status", Convert.ToBoolean(chbStatus.Checked));
                    commandInsert.Parameters.AddWithValue("@weight", Convert.ToDouble(numericUpDown2.Value));
                    ID_order = Convert.ToInt32(commandInsert.ExecuteScalar());
                    foreach (ListViewItem x in listViewDecor.CheckedItems)
                    {
                       commandInsert = new SqlCommand("INSERT INTO [tDecorCake] VALUES(" +
                                                    "@ID_order," +
                                                     "@ID_decor)" , connection);
                        commandInsert.Parameters.AddWithValue("@ID_order", ID_order);
                        commandInsert.Parameters.AddWithValue("@ID_decor", Convert.ToInt32(x.Tag));
                        commandInsert.ExecuteNonQuery();
                    }

                    MessageBox.Show("Запись добавлена");
                    add_items = true;
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {

                    LoadDataFromTable();
                }
            }
        }

        void UpdateData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                 SqlCommand commandUpdate = new SqlCommand("UPDATE tOrder SET" +
                                     " price=@price," +
                                    "date_start=@date_start," +
                                    "date_end=@date_end," +
                                    "comment=@comment," +
                                    "customer_name=@customer_name," +
                                    "customer_phone=@customer_phone," +
                                    "customer_email=@customer_email," +
                                    "ID_cake=@ID_cake," +
                                    "ID_stuffing=@ID_stuffing," +
                                    "status=@status, weight=@weight" +
                                    "  WHERE ID_order= @IDSS", connection);
                    commandUpdate.Parameters.AddWithValue("@price", Convert.ToDouble(tbITOGO.Text));
                    commandUpdate.Parameters.AddWithValue("@date_start", dtpStartDate.Value);
                    if (chbStatus.Checked)
                        commandUpdate.Parameters.AddWithValue("@date_end", dtpStopDate.Value);
                    else
                    commandUpdate.Parameters.AddWithValue("@date_end", DBNull.Value);
                    commandUpdate.Parameters.AddWithValue("@comment", tbComment.Text);
                    commandUpdate.Parameters.AddWithValue("@customer_name", tbCustomerName.Text);
                    commandUpdate.Parameters.AddWithValue("@customer_phone", tbCustomerPhone.Text);
                    commandUpdate.Parameters.AddWithValue("@customer_email", tbCustomerEmail.Text);
                    commandUpdate.Parameters.AddWithValue("@ID_cake", ID_cake);
                    commandUpdate.Parameters.AddWithValue("@ID_stuffing", ID_stuffing);
                    commandUpdate.Parameters.AddWithValue("@status", Convert.ToBoolean(chbStatus.Checked));
                    commandUpdate.Parameters.AddWithValue("@weight", Convert.ToDouble(numericUpDown2.Value));
                    commandUpdate.Parameters.AddWithValue("@IDSS", order.ID_order);
                    commandUpdate.ExecuteNonQuery();
                    List<int> s = new List<int>();
                    foreach (ListViewItem x in listViewDecor.CheckedItems)
                    {
                        if (!(order.iddecor.Contains(Convert.ToInt32(x.Tag))))
                          {
                            SqlCommand commandInsert = new SqlCommand("INSERT INTO [tDecorCake] VALUES(" +
                                                         "@ID_order," +
                                                          "@ID_decor)", connection);
                            commandInsert.Parameters.AddWithValue("@ID_order", ID_order);
                            commandInsert.Parameters.AddWithValue("@ID_decor", Convert.ToInt32(x.Tag));
                            commandInsert.ExecuteNonQuery();
                        }
                        s.Add(Convert.ToInt32(x.Tag));
                    }

                    foreach (int x in order.iddecor)
                    {
                        if (!s.Contains(x))
                        {
                            SqlCommand delete = new SqlCommand("DELETE FROM [tDecorCake] WHERE "+
                                "(ID_order= @ID_order) and (ID_decor = @ID_decor)", connection);
                            delete.Parameters.AddWithValue("@ID_order", ID_order);
                            delete.Parameters.AddWithValue("@ID_decor", Convert.ToInt32(x));
                            delete.ExecuteNonQuery();

                        }
                    }

                    MessageBox.Show("Запись обновлена");
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    LoadDataFromTable();
                }
            }
        }

        void LoadTort()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter daTort = new SqlDataAdapter("SELECT * FROm tCake", connection);
                DataTable dt2 = new DataTable();
                daTort.Fill(dt2);

                tortMainList = new List<Tort>();
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    DataRow dr = dt2.Rows[i];
                    Tort X = new Tort
                    {
                        ID_cake = Convert.ToInt32(dr[0]),
                        cake_name = dr[1].ToString(),
                        photo = (byte[])(dr[2])
                    };
                    LoadCakeCategory(X);
                    tortMainList.Add(X);
                }
                FullTortImageList(tortMainList);
            }
            
        }

        void LoadCakeCategory(Tort cake)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    for (int i = 0; i < clbType.Items.Count; i++)
                    {
                        clbType.SetItemChecked(i, false);
                    }
                    SqlCommand SelectCommand =
                        new SqlCommand("select ID_cake_category FROM dbo.tCakeAndCategory WHERE ID_cake =" + cake.ID_cake.ToString(),
                            connection);
                    SqlDataReader dataReader = SelectCommand.ExecuteReader();
                    if (dataReader.HasRows) // если есть данные
                    {
                        List<int> x = new List<int>();
                        while (dataReader.Read()) // построчно считываем данные
                        {
                            x.Add(Convert.ToInt32(dataReader.GetValue(0)));
                        }
                        cake.cake_category = x;
                    }
                }
                catch
                    (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        void FullTortImageList(List<Tort> X)
        {
            imageList1.Images.Clear();
            listViewTort.Items.Clear();
            listViewTort.LargeImageList = imageList1;
            int ImageIndex = 0;
            foreach (Tort s in X)
            {
                ListViewItem listitem = new ListViewItem();
                listitem.Text = s.cake_name;

                listitem.Tag = s.ID_cake.ToString();
                listitem.ForeColor = Color.Black;
                listitem.Font = new Font("Arial", 10, FontStyle.Bold);
                listitem.UseItemStyleForSubItems = false;
                byte[] byteArrayIn = s.photo;
                Image im;
                using (var ms = new MemoryStream(byteArrayIn))
                {
                    im = Image.FromStream(ms);
                }
                imageList1.Images.Add(im);
                listitem.ImageIndex = ImageIndex;
                ImageIndex++;
                listViewTort.Items.Add(listitem);
            }
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            dtpStopDate.Value = dtpStartDate.Value;
        }

        void LoadStuffing()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlDataAdapter daStuff = new SqlDataAdapter("SELECT * FROm tStuffing", connection);
                DataTable dt2 = new DataTable();
                daStuff.Fill(dt2);
                stuffMainList = new List<Stuffing>();
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    DataRow dr = dt2.Rows[i];
                    Stuffing X = new Stuffing
                    {
                        ID_stuffing = Convert.ToInt32(dr[0]),
                        stuffing_name = dr[1].ToString(),
                        price = Convert.ToDouble(dr[3]),
                        photo = (byte[])(dr[4])
                    };
                    stuffMainList.Add(X);
                }
                FullStuffingImageList(stuffMainList);
            }
        }

        void FullStuffingImageList(List<Stuffing> X)
        {
            imageList2.Images.Clear();
            listViewStuffing.Items.Clear();
            listViewStuffing.LargeImageList = imageList2;
            int ImageIndex = 0;
            foreach (Stuffing s in X)
            {
                ListViewItem listitem = new ListViewItem();
                listitem.Text = s.stuffing_name;
                listitem.Tag = s.ID_stuffing.ToString();
                listitem.ForeColor = Color.Black;
                listitem.Font = new Font("Arial", 10, FontStyle.Bold);
                listitem.UseItemStyleForSubItems = false;
                byte[] byteArrayIn = s.photo;
                Image im;
                using (var ms = new MemoryStream(byteArrayIn))
                {
                    im = Image.FromStream(ms);
                }
                imageList2.Images.Add(im);
                listitem.ImageIndex = ImageIndex;
                ListViewItem.ListViewSubItem subsItem = listitem.SubItems.Add("цена =" + s.price.ToString() + "р. за кг.");
                subsItem.ForeColor = Color.DarkBlue;
                subsItem.Font = new Font("Arial", 8, FontStyle.Bold);
                listitem.SubItems.Add(s.price.ToString());
                subsItem.ForeColor = Color.DarkBlue;
                subsItem.Font = new Font("Arial", 8, FontStyle.Bold);
                ImageIndex++;
                listViewStuffing.Items.Add(listitem);
            }
        }

        void LoadDecor()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlDataAdapter daDecor = new SqlDataAdapter("SELECT * FROm tDecor", connection);
                DataTable dt2 = new DataTable();
                daDecor.Fill(dt2);
                decorMainList = new List<Decor>();
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    DataRow dr = dt2.Rows[i];
                    Decor X = new Decor
                    {
                        ID_decor = Convert.ToInt32(dr[0]),
                        decor_name = dr[1].ToString(),
                        price = Convert.ToDouble(dr[3]),
                        photo = (byte[])(dr[4])
                    };
                    decorMainList.Add(X);
                }
                FullDecorImageList(decorMainList);
            }
        }

        void FullDecorImageList(List<Decor> X)
        {
            imageList3.Images.Clear();
            listViewDecor.Items.Clear();
            listViewDecor.LargeImageList = imageList3;
            int ImageIndex = 0;
            foreach (Decor s in X)
            {
                ListViewItem listitem = new ListViewItem();
                listitem.Text = s.decor_name;
                listitem.Tag = s.ID_decor.ToString();
                listitem.ForeColor = Color.Black;
                listitem.Font = new Font("Arial", 10, FontStyle.Bold);
                listitem.UseItemStyleForSubItems = false;
                byte[] byteArrayIn = s.photo;
                Image im;
                using (var ms = new MemoryStream(byteArrayIn))
                {
                    im = Image.FromStream(ms);
                }
                imageList3.Images.Add(im);
                listitem.ImageIndex = ImageIndex;
                ListViewItem.ListViewSubItem subsItem = listitem.SubItems.Add("цена =" + s.price.ToString() + "р. за кг.");
                subsItem.ForeColor = Color.DarkBlue;
                subsItem.Font = new Font("Arial", 8, FontStyle.Bold);
                listitem.SubItems.Add(s.price.ToString());
                subsItem.ForeColor = Color.DarkBlue;
                subsItem.Font = new Font("Arial", 8, FontStyle.Bold);
                ImageIndex++;
                listViewDecor.Items.Add(listitem);
            }
        }

        void PriceForWeight()
        {
            double s = 0;
            foreach (ListViewItem item in listViewDecor.Items)
            {
                if (item.Checked)
                {
                    s += Convert.ToDouble(item.SubItems[2].Text);
                }
            }
            DecorPrice = s;
            tbCostDecor.Text = DecorPrice.ToString();

            double x = Convert.ToDouble(tbCostStuffing.Text);
            double y = Convert.ToDouble(numericUpDown2.Value);
            tbPrice.Text = Convert.ToString(x*y);

            tbITOGO.Text = (DecorPrice + x * y).ToString();
            
        }
    }
}
