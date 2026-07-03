using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages.BaseApplicationMethods
{
    public partial class BaseApplication
    {
        public async Task<Revenue?> GetYearlyRevenue()
        {
            return await this.RevenueService.GetYearlyRevenue();
        }


    }
}
