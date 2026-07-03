using MicroGym.Shared.Model;
using System.Net.Http.Json;

namespace MicroGym.Client.Service
{
    public class RevenueService
    {
        private readonly HttpClient _httpClient;

        public RevenueService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Payment>> GetRevenue(int month, int year)
        {
            var dateParam = new DateOnly(year, month, 1).ToString("yyyy-MM-dd");
            var revenue = await _httpClient.GetFromJsonAsync<List<Payment>>($"api/Revenue/GetRevenue?month={dateParam}");
            return revenue ?? new List<Payment>();
        }

        public async Task<Revenue?> GetYearlyRevenue()
        {
            var totalRevenue = await _httpClient.GetFromJsonAsync<Revenue>("api/Revenue/GetYearlyRevenue");
            return totalRevenue;
        }
    }
}
