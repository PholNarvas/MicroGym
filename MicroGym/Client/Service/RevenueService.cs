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

        public async Task<List<RevenuePaymentDetail>> GetRevenue(int month, int year)
        {
            var dateParam = new DateOnly(year, month, 1).ToString("yyyy-MM-dd");
            var result = await _httpClient.GetFromJsonAsync<List<RevenuePaymentDetail>>($"api/Revenue/GetRevenue?month={dateParam}");
            return result ?? new List<RevenuePaymentDetail>();
        }

        public async Task<Revenue?> GetYearlyRevenue()
        {
            var totalRevenue = await _httpClient.GetFromJsonAsync<Revenue>("api/Revenue/GetYearlyRevenue");
            return totalRevenue;
        }

        public async Task<List<RevenueChartMonth>> GetRevenueChartByYearAsync(int year)
        {
            var result = await _httpClient.GetFromJsonAsync<List<RevenueChartMonth>>($"api/Revenue/GetRevenueChart?year={year}");
            return result ?? new List<RevenueChartMonth>();
        }
    }
}
