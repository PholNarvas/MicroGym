using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Shared
{
    public partial class AddMemberModal
    {
        private void OnMembershipTypeChanged(ChangeEventArgs e)
        {
            if (!int.TryParse(e.Value?.ToString(), out var typeId)) return;

            model.MemberShipTypeID = typeId;
            RecalculatePrice();
        }

        private void OnAnnualToggled(ChangeEventArgs e)
        {
            var isOn = e.Value is true;
            model.TierID = isOn ? annualTier?.TierID : null;
            RecalculatePrice();
        }

        private void RecalculatePrice()
        {
            if (model.MemberShipTypeID <= 0) return;

            var plan = membershipTypes.FirstOrDefault(mt => mt.MembershipTypeID == model.MemberShipTypeID);
            if (plan is null) return;

            if (isAnnualSelected && annualTier is not null && annualTier.DiscountPct > 0)
                model.PaymentAmount = Math.Round(plan.Price * (1 - annualTier.DiscountPct / 100), 2);
            else
                model.PaymentAmount = plan.Price;
        }
    }
}
