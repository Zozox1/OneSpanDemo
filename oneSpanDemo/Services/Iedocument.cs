using oneSpanDemo.Models;

namespace oneSpanDemo.Services
{
    public interface Iedocument
    {
        public Task<DocumentsResult> GetEdocument(string id);
    }
}
