using MicroGym.Shared.Model;
using System.Net.Http.Json;

namespace MicroGym.Client.Service
{
    public class MembershipTypeClientService
    {
        private readonly HttpClient httpClient;

        public MembershipTypeClientService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<List<MembershipType>> GetAllAsync()
        {
            var result = await httpClient.GetFromJsonAsync<List<MembershipType>>("api/MembershipType/GetAll");
            return result ?? new List<MembershipType>();
        }
    }
}
