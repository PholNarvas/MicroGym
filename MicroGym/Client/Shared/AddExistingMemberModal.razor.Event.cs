using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Shared
{
    public partial class AddExistingMemberModal
    {
        private void OnMembershipTypeChanged(ChangeEventArgs e)
        {
            if (!int.TryParse(e.Value?.ToString(), out var typeId)) return;

            model.MemberShipTypeID = typeId;
            RecalculatePrice();
            ComputeExpiryDate();
        }

        private void OnAnnualToggled(ChangeEventArgs e)
        {
            var isOn = e.Value is true;
            model.TierID         = isOn ? annualTier?.TierID : null;
            model.TierExpiryDate = null;
            if (isOn) tierStartDate = DateTime.Today;  // default to today; owner can adjust
            ComputeTierExpiryDate();
            RecalculatePrice();
        }

        private void OnTierStartDateChanged(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out var date))
            {
                tierStartDate = date;
                ComputeTierExpiryDate();
            }
        }

        private void OnDateJoinedChanged(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out var date))
            {
                model.DateJoined = date;
                ComputeExpiryDate();
                // TierExpiryDate is NOT re-computed here — it depends on tierStartDate, not DateJoined
            }
        }

        // Computes TierExpiryDate from tierStartDate (when they availed annual) + tier's DurationMonths.
        // tierStartDate is independent from DateJoined — members can upgrade anytime.
        // No-op if annual membership is not selected.
        private void ComputeTierExpiryDate()
        {
            if (!isAnnualSelected || annualTier is null) return;
            model.TierExpiryDate = tierStartDate.AddMonths(annualTier.DurationMonths).AddDays(-1);
        }

        private void OnExpiryDateChanged(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out var date))
                model.ExpiryDate = date;
        }

        private void OnTierExpiryChanged(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out var date))
                model.TierExpiryDate = date;
            else
                model.TierExpiryDate = null;
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

        // Auto-computes ExpiryDate from DateJoined + plan's DurationInMonths.
        // Matches the same formula used in pr_RegisterUser and pr_RenewMember.
        //   Walk-in  → same day (expires today)
        //   Regular  → AddMonths - 1 day, so July 1 + 1 month = July 31 (last valid day)
        private void ComputeExpiryDate()
        {
            if (model.MemberShipTypeID <= 0) return;

            var plan = membershipTypes.FirstOrDefault(mt => mt.MembershipTypeID == model.MemberShipTypeID);
            if (plan is null) return;

            if (plan.IsWalkIn)
            {
                model.ExpiryDate = model.DateJoined;
                return;
            }

            if (plan.DurationInMonths is null) return;
            model.ExpiryDate = model.DateJoined.AddMonths(plan.DurationInMonths.Value).AddDays(-1);
        }
    }
}
