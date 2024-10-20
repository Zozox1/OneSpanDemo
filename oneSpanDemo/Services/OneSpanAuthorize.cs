using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Transactions;

namespace oneSpanDemo.Services
{
    public class OneSpanAuthorize :IoneSpanAuthorize
    {
        private IConfiguration configuration;
        private readonly string sandBoxUrl;
        private readonly string oneSpanClientId;
        private readonly string oneSpanClientSecret;

        public OneSpanAuthorize(IConfiguration _iconfig)
        {
            configuration = _iconfig;
            sandBoxUrl = configuration.GetValue<string>("OneSpanSandboxUrl");
            oneSpanClientId = configuration.GetValue<string>("OneSpanClientId");
            oneSpanClientSecret = configuration.GetValue<string>("OneSpanSecret");
        }
        public async Task<string> GetAccessToken()
        {
            string token = string.Empty;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, sandBoxUrl + "apitoken/clientApp/accessToken");
                    requestMessage.Content = new StringContent("{\"clientId\":\"" + oneSpanClientId + "\",\"secret\":\"" + oneSpanClientSecret + "\",\"type\": \"OWNER\"}", Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

                    string httpResult = await response.Content.ReadAsStringAsync();

                    JObject json = JObject.Parse(httpResult);
                    if (json.HasValues)
                    {
                        JToken? stringAccessToken = json.GetValue("accessToken");
                        if (stringAccessToken != null)
                        {
                            token = stringAccessToken.ToString();
                        }
                    }
                }
                catch (Exception ex) { 
                // TODO- Logging
                }
            }
            return token;
        }
    }
}
