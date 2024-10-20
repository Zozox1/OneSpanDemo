namespace oneSpanDemo.Services
{
    using Models;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public class OneSpanEdocument : Iedocument
    {
        private IConfiguration configuration;
        private IoneSpanAuthorize iOneSpanAuthorize;
        private readonly string sandBoxUrl;
        private const int minDocumentsBytes = 1000;

        public OneSpanEdocument(IConfiguration _iconfig,IoneSpanAuthorize _iOneSpanAuthorize)
        {
            configuration = _iconfig;
            iOneSpanAuthorize = _iOneSpanAuthorize;
            sandBoxUrl = configuration.GetValue<string>("OneSpanSandboxUrl");
        }
        public async Task<DocumentsResult> GetEdocument(string id)
        {
            DocumentsResult documentsResult = new DocumentsResult();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string token = await iOneSpanAuthorize.GetAccessToken();

                    if (string.IsNullOrEmpty(token))
                    {
                        documentsResult.status = DocumentsResultStatus.UnAuthorized;
                        return documentsResult;
                    }

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
                    Stream filesToReturn = await httpClient.GetStreamAsync(sandBoxUrl + "api/packages/" + id + "/documents/zip");

                    if (filesToReturn == null || filesToReturn.Length < minDocumentsBytes)
                        documentsResult.status = DocumentsResultStatus.NoFile;
                    else
                    {
                        documentsResult.status = DocumentsResultStatus.OK;
                        documentsResult.stream = filesToReturn;
                    }

                }
            }
            catch (Exception ex) 
            {
                documentsResult.status = DocumentsResultStatus.UnAbleToLoad;
                //TODO Logging 
            }
            return documentsResult;
        }
    }
}
