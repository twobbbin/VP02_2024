using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Syndication;
using System.Collections.Generic;
using System.Xml;
using Windows.UI.Xaml.Media;
using Ical.Net.CalendarComponents;
using Ical.Net;

namespace _01_smartmirror
{
    public sealed partial class MainPage : Page
    {
        private const string API_KEY = "41b71a7f7430e07f43d532edf96cf8e7";
        private const string CITY_NAME = "Daejeon"; // 원하는 도시 이름으로 변경
        //미세먼지
        private const string ApiUrl = "https://apis.data.go.kr/B552584/ArpltnInforInqireSvc/getMsrstnAcctoRltmMesureDnsty";
        private const string ServiceKey = "D3E%2FT87O%2FjdSgFajaPAsY0jVTMlcpYTwq13hpx%2FBejXAuQQKOIrnLsVSkIMMV1cSU1J1%2F6EZTQYll2BudXJMOQ%3D%3D";
        private const string ReturnType = "xml";
        private const string NumOfRows = "1";
        private const string PageNo = "1";
        private const string StationName = "정림동";
        private const string DataTerm = "DAILY";
        private const string Ver = "1.0";

        public MainPage()
        {
            this.InitializeComponent();
            LoadWeatherData(); // 날씨를 가져오는 비동기 메서드 호출
            LoadNewsAsync(); // 뉴스를 가져오는 비동기 메서드 호출
            GetDustInfo(); // 미세먼지를 가져오는 비동기 메서드 호출
            LoadEvents(); // 일정을 가져오는 비동기 메서드 호출

            // 타이머 설정
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // 1초마다 업데이트
            timer.Tick += Timer_Tick;
            timer.Start();


        }

        // 시간, 날짜
        private void Timer_Tick(object sender, object e)
        {
            // 현재 시간 가져오기
            DateTime currentTime = DateTime.Now;

            // 시간 및 날짜 텍스트 업데이트
            digitalClock.Text = currentTime.ToString("HH:mm:ss");
            dateTextBlock.Text = currentTime.ToString("yyyy-MM-dd");

            string dayOfWeek = currentTime.ToString("dddd");
            dayOfWeekTextBlock.Text = dayOfWeek;
        }

        //날씨
        private async void LoadWeatherData()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 현재 날씨 정보 가져오기
                    string currentWeatherUrl
                        = $"http://api.openweathermap.org/data/2.5/weather?q={CITY_NAME}&appid={API_KEY}&units=metric";
                    HttpResponseMessage currentWeatherResponse = await client.GetAsync(currentWeatherUrl);
                    currentWeatherResponse.EnsureSuccessStatusCode();
                    string currentWeatherResponseBody = await currentWeatherResponse.Content.ReadAsStringAsync();
                    dynamic currentWeatherData = JsonConvert.DeserializeObject(currentWeatherResponseBody);

                    string cityName = currentWeatherData.name;
                    double currentTemperature = currentWeatherData.main.temp;
                    string description = currentWeatherData.weather[0].description;
                    string iconCode = currentWeatherData.weather[0].icon;

                    // 온도를 정수로 변환
                    int temperatureInteger = (int)Math.Round(currentTemperature);

                    // UI 업데이트
                    WeatherText.Text = $"{temperatureInteger}°C";
                    DescriptionText.Text = $"{description}";
                    CityText.Text = $"{cityName}";

                    string iconUrl = $"http://openweathermap.org/img/w/{iconCode}.png";
                    WeatherIcon.Source = new BitmapImage(new Uri(iconUrl));
                }
                catch (HttpRequestException e)
                {
                    WeatherText.Text = $"날씨 정보를 불러오는 중 오류가 발생했습니다: {e.Message}";
                }
            }
        }

        //뉴스 기사
        private async Task LoadNewsAsync()
        {
            try
            {
                SyndicationClient client = new SyndicationClient();
                // YTN의 RSS 피드에서 뉴스 가져오기
                SyndicationFeed feed = await client.RetrieveFeedAsync
                    (new Uri("https://rss.ohmynews.com/rss/top.xml"));

                List<NewsItem> newsItems = new List<NewsItem>();
                int count = 0; // 뉴스 항목의 개수를 세는 변수 추가

                foreach (var item in feed.Items)
                {
                    // 뉴스 항목을 NewsItem 객체에 추가
                    newsItems.Add(new NewsItem
                    {
                        Title = item.Title.Text,
                        Summary = item.Summary.Text,
                        PublishDate = item.PublishedDate.DateTime
                    });

                    count++; // 뉴스 항목의 개수 증가

                    if (count >= 5) // 만약 뉴스 항목의 개수가 5개 이상이면 반복 종료
                        break;
                }

                // 뉴스 항목을 UI에 바인딩 (예: ListView)
                newsListView.ItemsSource = newsItems;
            }
            catch (Exception ex)
            {
                // 예외 처리
            }
        }

        public class NewsItem
        {
            public string Title { get; set; }
            public string Summary { get; set; }
            public DateTime PublishDate { get; set; }
        }

        //미세먼지 농도
        private async void GetDustInfo()
        {
            string url = $"{ApiUrl}?serviceKey={ServiceKey}&returnType={ReturnType}" +
                $"&numOfRows={NumOfRows}&pageNo={PageNo}&stationName={StationName}&dataTerm={DataTerm}&ver={Ver}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // XML 문자열을 XmlDocument로 파싱
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(responseBody);

                    // 필요한 정보 추출
                    XmlNodeList itemNodes = xmlDoc.SelectNodes("//item");
                    foreach (XmlNode itemNode in itemNodes)
                    {
                        string dataTime = itemNode.SelectSingleNode("dataTime").InnerText;
                        string pm10Value = itemNode.SelectSingleNode("pm10Value").InnerText;
                        string pm25Value = itemNode.SelectSingleNode("pm25Value").InnerText;

                        // 미세먼지 상태 평가
                        string dustStatus = EvaluateDustStatus(int.Parse(pm10Value), int.Parse(pm25Value));

                        // 추출한 정보를 TextBlock에 출력
                        //DustInfoTextBlock.Text += $"측정일시: {dataTime}\n";
                        DustInfoTextBlock.Text += $"미세먼지 {dustStatus}\n\n";
                        DustInfoTextBlock.Text += $"미세먼지  {pm10Value} µg/m³   ";
                        DustInfoTextBlock.Text += $"초미세먼지  {pm25Value} µg/m³";

                    }
                }
                catch (HttpRequestException ex)
                {
                    DustInfoTextBlock.Text = $"API 요청 실패: {ex.Message}";
                }
            }
        }
        //미세먼지 상태
        private string EvaluateDustStatus(int pm10Value, int pm25Value)
        {
            if (pm10Value <= 30 && pm25Value <= 15)
            {
                return "좋음";
            }
            else if ((pm10Value > 30 && pm10Value <= 80) || (pm25Value > 15 && pm25Value <= 35))
            {
                return "보통";
            }
            else if ((pm10Value > 80 && pm10Value <= 150) || (pm25Value > 35 && pm25Value <= 75))
            {
                return "나쁨";
            }
            else
            {
                return "매우 나쁨";
            }
        }

        //일정
        private async Task<string> GetICalDataAsync(string icalUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(icalUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        private List<CalendarEvent> ParseICalData(string icalData)
        {
            var calendar = Calendar.Load(icalData);
            var events = new List<CalendarEvent>();

            // 오늘 날짜부터 10일 후까지의 이벤트만 가져오기
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(10);

            foreach (var calendarEvent in calendar.Events)
            {
                // 이벤트의 시작일이 오늘부터 10일 후 사이에 있는 경우에만 리스트에 추가
                if (calendarEvent.Start.Date >= startDate && calendarEvent.Start.Date <= endDate)
                {
                    events.Add(calendarEvent);
                }
            }

            return events;
        }

        private async void LoadEvents()
        {
            string icalUrl = "https://calendar.google.com/calendar/ical/ricesoyeggs%40gmail.com/private-6d10458702da0da5eb23295a46e4b705/basic.ics";
            string icalData = await GetICalDataAsync(icalUrl);
            var events = ParseICalData(icalData);

            EventsListView.ItemsSource = events;
        }
    }
}
