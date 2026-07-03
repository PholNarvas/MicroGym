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

        public async Task<List<User>> GetExpiringMemberAsync()
        {
            var expiring = await httpClient.GetFromJsonAsync<List<User>>("api/Member/GetExpiringMembers");
            return expiring ?? new List<User>();
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

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(error)
                    ? "Failed to update member. Please try again."
                    : error.Trim('"'));
            }

            return true;
        }

        public async Task<bool> RenewMemberAsync(RenewMemberDto dto)
        {
            var response = await httpClient.PostAsJsonAsync("api/Member/RenewMember", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(error)
                    ? "Failed to renew membership. Please try again."
                    : error.Trim('"'));
            }

            return true;
        }

        public async Task<bool> PurchaseAnnualMembershipAsync(PurchaseAnnualMembershipDto dto)
        {
            var response = await httpClient.PostAsJsonAsync("api/Member/PurchaseAnnualMembership", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(error)
                    ? "Failed to activate annual membership. Please try again."
                    : error.Trim('"')); // strip JSON string quotes if response is a plain string
            }

            return true;
        }

        public async Task<bool> DeleteMemberAsync(int userID)
        {
            var response = await httpClient.DeleteAsync($"api/Member/DeleteMember/{userID}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<MemberPaymentRecord>> GetMemberPaymentHistoryAsync(int memberID)
        {
            var records = await httpClient.GetFromJsonAsync<List<MemberPaymentRecord>>(
                $"api/Member/GetMemberPaymentHistory?memberID={memberID}");
            return records ?? new List<MemberPaymentRecord>();
        }

        public async Task<bool> UpdateProfilePictureAsync(UpdateProfilePictureDto dto)
        {
            var response = await httpClient.PostAsJsonAsync("api/Member/UpdateProfilePicture", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<MembershipTier>> GetMembershipTiersAsync()
        {
            var tiers = await httpClient.GetFromJsonAsync<List<MembershipTier>>("api/Member/GetMembershipTiers");
            return tiers ?? new List<MembershipTier>();
        }

        // Returns true = success, false = server error, null = duplicate email
        public async Task<bool?> AddExistingMemberAsync(AddExistingMemberDto dto)
        {
            var response = await httpClient.PostAsJsonAsync("api/Member/AddExistingMember", dto);

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                return null;

            return response.IsSuccessStatusCode;
        }
    }
}
