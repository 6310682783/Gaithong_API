using GaiThong_API.Models;
using System.Net;
using System.Text;

namespace GaiThong_API.Services
{
    public interface ILineNotifyService
    {
     Task<bool> SentNotify(string line);

    }
    public class LineNotifyService : ILineNotifyService
    {
        private IConfiguration configuration;

        private string PROXY_URL;
        private string PROXY_USERNAME;
        private string PROXY_PASSWORD;
        private string LINE_TOKEN;
        private string LINE_API_URL;

        public LineNotifyService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.PROXY_URL = configuration.GetSection("Proxysetting:Url").Value;
            this.PROXY_USERNAME = configuration.GetSection("Proxysetting:ProxyName").Value;
            this.PROXY_PASSWORD = configuration.GetSection("Proxysetting:ProxyPass").Value;
            this.LINE_TOKEN = configuration.GetSection("LineAPI:Token").Value;
            this.LINE_API_URL = configuration.GetSection("LineAPI:HostURL").Value;

        }

        public async Task<bool> SentNotify(string message)
        {
            bool isSendSuscess = false;
            try
            {
               
                string strMessage = System.Web.HttpUtility.UrlEncode(message, Encoding.UTF8);
                var request = (HttpWebRequest)WebRequest.Create(LINE_API_URL);

                //setting proxy
                WebProxy webProxy = new WebProxy(PROXY_URL, true)
                {
                    Credentials = new NetworkCredential(PROXY_USERNAME, PROXY_PASSWORD),
                    UseDefaultCredentials = true
                };
                request.Proxy = webProxy;

                var postData = string.Format("message={0}", strMessage);
                var data = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + LINE_TOKEN);
                var stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                var response = await request.GetResponseAsync() as HttpWebResponse;
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                    isSendSuscess = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return isSendSuscess;
        }
    }
}
