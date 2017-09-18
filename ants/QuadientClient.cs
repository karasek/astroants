using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ants
{
    public class QuadientClient
    {
        const string Url = "http://tasks-rad.quadient.com:8080/task";
        readonly HttpClient _client = new HttpClient();

        public async Task<string> GetAssignmentRaw()
        {
            return await _client.GetStringAsync(Url);
        }

        public async Task<string> CheckResponse(string id, string path)
        {
            var content = new StringContent($"{{\"path\": \"{path}\"}}", Encoding.UTF8, "application/json");
            var resp = await _client.PutAsync($"{Url}/{id}", content);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }
    }
}