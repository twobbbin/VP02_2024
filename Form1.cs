using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace _02_test
{
    public partial class Form1 : Form
    {
        ComboBox[] grade;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string result = "";
            double Name = double.Parse(txtname.Text);
            result += Name;


            if(radioButton1.Checked ) 
            {
                result += "의료IT공학과\n";
            }
            else if(radioButton2.Checked )
            {
                result += "의공학과\n";
            }
            else if (radioButton3.Checked)
            {
                result += "의료신소재학과\n";
            }
            else if (radioButton4.Checked)
            {
                result += "제약생명공학과\n";
            }
            else
            {
                result += "의료공간디자인학과\n";
            }

            MessageBox.Show(result, "학생정보");

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add(1);
            comboBox1.Items.Add(2);
            comboBox1.Items.Add(3);
            comboBox1.Items.Add(4);

            MessageBox.Show(result, "학생정보");
        }
    }
} 
//못푸는데 어떻게 블로그를 써요........... 이거 키보드좀 바꿔주세뇨 제발 
