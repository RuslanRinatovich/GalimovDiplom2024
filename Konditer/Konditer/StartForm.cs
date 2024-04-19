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
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
            progressBar1.Value = 0;
            timer1.Start();


        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (progressBar1.Value == 100)
            {
                MainForm fMain = new MainForm();
                fMain.Show();
                this.Hide();
                timer1.Stop();
            }
            else progressBar1.Value += 10;
        }
    }
}