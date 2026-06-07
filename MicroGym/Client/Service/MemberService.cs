using MicroGym.Shared.Model;
using System.Net.Http.Json;

namespace MicroGym.Client.Service
{
    public class MemberService
    {
        private readonly HttpClient httpClient;

        public MemberService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<List<Members>> GetMembersAsync()
        {
            var members = await httpClient.GetFromJsonAsync<List<Members>>("api/Member/GetMembers");
            return members ?? new List<Members>();
        }
    }
}
