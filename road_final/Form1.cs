using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace road_final
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form2 f = new form2();  // form2에 form1 전달
            f.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.Show();
        }

    }
}
