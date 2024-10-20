namespace oneSpanDemo.Services
{
    public interface IoneSpanAuthorize
    {
        public Task<string> GetAccessToken();
    }
}
