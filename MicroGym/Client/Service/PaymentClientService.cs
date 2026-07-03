using MicroGym.Shared.DTOs;
using System.Net.Http.Json;

namespace MicroGym.Client.Service
{
    public class PaymentClientService
    {
        private readonly HttpClient _httpClient;
        public PaymentClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SavePayment(EditMemberDto editmember)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/Payment/SavePayment",
                editmember);

            return response.IsSuccessStatusCode;
        }
    }
}
