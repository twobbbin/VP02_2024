using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Road
{
    public partial class Form2 : Form
    {
        private Form1 form1;
        private Form3 form3;

        public Form2(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form1.Show();
            this.Hide(); // Form2를 숨깁니다.
        }

        private void button2_Click(object sender, EventArgs e)
        {
            form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            form1.Show(); // Form1을 다시 표시합니다.
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            form1.Show(); // Form1을 다시 표시합니다.
        }
    }
}
