using MicroGym.Shared.Model;
using System.Net.Http.Json;

namespace MicroGym.Client.Service
{
    public class AttendanceClientService
    {
        private readonly HttpClient _httpClient;

        public AttendanceClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AttendanceModel>> GetTodayAttendanceAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<AttendanceModel>>("api/Attendance/GetTodayAttendance");
            return result ?? new List<AttendanceModel>();
        }

        public async Task<(bool Success, string Message)> CheckInMemberAsync(int userId)
        {
            var response = await _httpClient.PostAsync($"api/Attendance/CheckIn/{userId}", null);

            var content = await response.Content.ReadFromJsonAsync<CheckInResponse>();
            return (response.IsSuccessStatusCode, content?.Message ?? "Unknown error.");
        }

        private class CheckInResponse
        {
            public string Message { get; set; } = string.Empty;
        }
    }
}
