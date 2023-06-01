using AntDesign;
using ArtLib;
using Newtonsoft.Json;
using NuGet.Configuration;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace WebArt.Models
{
    public static class Global
    {
        public static string Api { get; set; } = "http://localhost/7065/api";
        private static HttpClient _httpClient;
        public static HttpClient ClientNet
        {
            get
            {
                if(_httpClient == null)
                {
                    _httpClient = new HttpClient();
                    Global.ClientNet.BaseAddress = new Uri(HTTPService._basePath + "/");
                    Global.ClientNet.DefaultRequestHeaders.Accept.Clear();
                    Global.ClientNet.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HTTPService._httpClient = ClientNet;
                }
                return _httpClient;
            }
        }
        //public static Frame MainFrame { get; set; }
       // public static MainWindow MainWin { get; set; }
        //public static Dialog DialogWindow { get; set; }
        //public static int Kod_Tov = 0;
        //public static Product SelectedTovar;
        //public static List<Product> SelectedTovarList = new List<Product>();
       // public static WebArt.User CurrentUser { get; set; }

        //public static NotificationManager notificationManager = new NotificationManager();
        public static void InitializeClient()
        {
            //Api = Settings.Default.URL;
            /*Global.ClientNet = new HttpClient();
            Global.ClientNet.BaseAddress = new Uri(HTTPService._basePath + "/");
            Global.ClientNet.DefaultRequestHeaders.Accept.Clear();
            Global.ClientNet.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           // Global.ClientNet.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Default.Token);
            HTTPService._httpClient = ClientNet;*/
        }
        public static void ShowNotif(string title, string Message, NotificationType type)
        {
            //notificationManager.Show(title, Message, type/*, onClick: () => SomeAction()*/);
        }

        public static void ShowWarning(string message, string title = "Внимание")
        {
            Global.ShowNotif(title, message, NotificationType.Warning);
        }

        public static void ErrorLog(string message, string title = "Ошибка")
        {
            if (message == "Невозможно соединиться с удаленным сервером") ShowNotif(message, "Повторите операцию", NotificationType.Error);
            else if (message.Contains("Unauthorized"))
            {
                Global.ShowNotif("Внимание", "Требуется авторизация" + message.ToString(), NotificationType.Warning);
                
            }
            else ShowNotif(title, message, NotificationType.Error);
        }


    }

    public class HttpRequests<T>
    {
        public static async Task<T> GetRequestAsync(string url, T responce)
        {
            try
            {
                HttpResponseMessage response = await Global.ClientNet.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    Global.ErrorLog(response.StatusCode.ToString() + " " + await response.Content.ReadAsStringAsync());
                }
            }
            catch (HttpRequestException ex)
            {
                Global.ErrorLog(ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                Global.ErrorLog(ex.Message);
            }
            return responce;
        }
        public static T GetRequest(string url, T responce)
        {
            try
            {
                HttpResponseMessage response = Global.ClientNet.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    responce = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Global.ErrorLog(response.StatusCode.ToString());
                }
            }
            catch (HttpRequestException ex)
            {
                Global.ErrorLog(ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                Global.ErrorLog(ex.Message);
            }
            return responce;
        }
        public static async Task DeleteRequest(string url)
        {
            HttpResponseMessage response = await Global.ClientNet.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                string excep = response.Content.ReadAsStringAsync().Result;
                Global.ErrorLog(response.StatusCode.ToString() + " " + excep);
            }
        }

        public static async Task PutRequest(string url, T responce)
        {
            HttpResponseMessage response = await Global.ClientNet.PutAsJsonAsync(url, responce);
            if (response.IsSuccessStatusCode)
            {
            }
            else
            {
                string excep = response.Content.ReadAsStringAsync().Result;
                Global.ErrorLog(response.StatusCode.ToString() + " " + excep);
            }
        }

        public static async Task<T> PostRequest(string url, T responce)
        {
            HttpResponseMessage response = await Global.ClientNet.PostAsJsonAsync(url, responce);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                string excep = response.Content.ReadAsStringAsync().Result;
                Global.ErrorLog(response.StatusCode.ToString() + " " + excep);
                return responce;
            }
        }
    }

    public class HttpPostRequests<T, K>
    {
        public static async Task<T> PostRequest(string url, T responce, K query)
        {
            try
            {
                HttpResponseMessage response = await Global.ClientNet.PostAsJsonAsync(url, query);
                string content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(content);
                }
                else
                {
                    string excep = response.Content.ReadAsStringAsync().Result;
                    Global.ErrorLog(response.StatusCode.ToString() + " " + excep);
                    return responce;
                }
            }
            catch (Exception ex)
            {
                Global.ErrorLog(ex.InnerException.Message);
                return responce;
            }
        }
    }

   

}

    

