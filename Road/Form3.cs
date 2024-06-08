using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml;

namespace Road
{
    public partial class Form3 : Form
    {
        private readonly Dictionary<string, string> subwayId = new Dictionary<string, string>()
    {
        { "1001", "1호선" },
        { "1002", "2호선" },
        { "1003", "3호선" },
        { "1004", "4호선" },
        { "1005", "5호선" },
        { "1006", "6호선" },
        { "1007", "7호선" },
        { "1008", "8호선" },
        { "1009", "9호선" },
        { "1063", "경의중앙선" },
        { "1065", "공항철도" },
        { "1067", "경춘선" },
        { "1075", "수인분당선" },
        { "1077", "신분당선" },
        { "1092", "우이신설선" },
        { "1032", "GTX-A" }
    };
        private readonly Dictionary<string, string> arvlCd = new Dictionary<string, string>()
        {
            {"0","진입" },
            {"1","도착" },
            {"2","출발" },
            {"3","전역출발" },
            {"4","전역진입" },
            {"5","전역도착" },
            {"99","운행중" }
        };
        public Form3()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            string your_key = textBox1.Text;

            string query = "http://swopenAPI.seoul.go.kr/api/subway/5563794b4e63757439337270534577/xml/realtimeStationArrival/0/100/" + textBox2.Text;

            WebRequest wr = WebRequest.Create(query);
            wr.Method = "GET";

            WebResponse wrs = wr.GetResponse();
            Stream s = wrs.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            string response = sr.ReadToEnd();

            //richTextBox1.Text = response;

            XmlDocument xd = new XmlDocument();
            xd.LoadXml(response);

            XmlNode xn = xd["realtimeStationArrival"];

            listView3.Items.Clear();

            for (int i = 1; i < xn.ChildNodes.Count; i++)
            {
                XmlNode node = xn.ChildNodes[i];
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems.Add(xn.ChildNodes[i]["recptnDt"].InnerText); //생성시간
                lvi.Text = subwayId[xn.ChildNodes[i]["subwayId"].InnerText]; // 지하철 호선

                lvi.SubItems.Add(xn.ChildNodes[i]["updnLine"].InnerText); // 상하행선

                lvi.SubItems.Add(arvlCd[node["arvlCd"].InnerText]); /// 도착코드

                lvi.SubItems.Add(xn.ChildNodes[i]["trainLineNm"].InnerText); // 도착지방면

                int barvlDt = int.Parse(node["barvlDt"].InnerText);
                int barvlDtMinutes = barvlDt / 60; // 초를 분으로 변환
                lvi.SubItems.Add(barvlDtMinutes.ToString() + " 분"); // 열차도착예상시간 (분 단위)

                lvi.SubItems.Add(xn.ChildNodes[i]["bstatnNm"].InnerText); // 종착 지하철명 



                lvi.Text = subwayId[xn.ChildNodes[i]["subwayId"].InnerText]; // 지하철 호선
                lvi.SubItems.Add(xn.ChildNodes[i]["updnLine"].InnerText); // 상하행선
                lvi.SubItems.Add(arvlCd[node["arvlCd"].InnerText]); /// 도착코드
                lvi.SubItems.Add(xn.ChildNodes[i]["trainLineNm"].InnerText); // 도착지방면
                lvi.SubItems.Add(xn.ChildNodes[i]["bstatnNm"].InnerText); // 종착 지하철명
                lvi.SubItems.Add(xn.ChildNodes[i]["arvlMsg3"].InnerText); // 2번
                lvi.SubItems.Add(xn.ChildNodes[i]["recptnDt"].InnerText); // 생성코드 
            }
        }
    }
}