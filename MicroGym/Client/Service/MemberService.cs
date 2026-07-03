using MicroGym.Shared.DTOs;
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

        public async Task<List<User>> GetMembersAsync()
        {
            var members = await httpClient.GetFromJsonAsync<List<User>>("api/Member/GetMembers");
            return members ?? new List<User>();
        }

        public async Task<bool> AddMemberAsync(RegisterRequestDto request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/register", request);
            return response.IsSuccessStatusCode;
        }
        public async Task<User?> GetMemberByIdAsync(int memberID)
        {
            var response = await httpClient.GetAsync($"api/Member/GetMemberById?memberID={memberID}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task<bool> SaveMemberInfo(EditMemberDto memberDto)
        {
            var response = await httpClient.PostAsJsonAsync("api/Member/SaveMember", memberDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMemberAsync(int userID)
        {
            var response = await httpClient.DeleteAsync($"api/Member/DeleteMember/{userID}");
            return response.IsSuccessStatusCode;
        }
    }
}
