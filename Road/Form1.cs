using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Road
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string yourkey = "D3E%2FT87O%2FjdSgFajaPAsY0jVTMlcpYTwq13hpx%2FBejXAuQQKOIrnLsVSkIMMV1cSU1J1%2F6EZTQYll2BudXJMOQ%3D%3D";
            string query = "http://apis.data.go.kr/1613000/ArvlInfoInqireService/getSttnAcctoArvlPrearngeInfoList?serviceKey=" + yourkey + "&cityCode=" + textBox1.Text + "&nodeId=" + textBox2.Text;
            WebRequest wr = WebRequest.Create(query);
            wr.Method = "GET";

            WebResponse wrs = wr.GetResponse();
            Stream s = wrs.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            string response = sr.ReadToEnd();


            XmlDocument xd = new XmlDocument();
            xd.LoadXml(response);

            XmlNode xn = xd["response"]["body"]["items"];

            listView2.Items.Clear();
            listView1.Items.Clear();

            for (int i = 0; i < xn.ChildNodes.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = xn.ChildNodes[i]["nodenm"].InnerText;
                lvi.SubItems.Add(xn.ChildNodes[i]["routeno"].InnerText);
                lvi.SubItems.Add(xn.ChildNodes[i]["arrprevstationcnt"].InnerText);
                lvi.SubItems.Add(xn.ChildNodes[i]["arrtime"].InnerText);
                lvi.SubItems.Add(xn.ChildNodes[i]["routetp"].InnerText);
                lvi.SubItems.Add(xn.ChildNodes[i]["vehicletp"].InnerText);

                int arrtime = int.Parse(xn.ChildNodes[i]["arrtime"].InnerText);
                if (arrtime < 300)
                {
                    listView2.Items.Add(lvi);
                }
                else
                {
                    listView1.Items.Add(lvi);
                } //GGB233001653 31240
            }
        }
    }
}
