using System.Globalization;
using Demo.Functions.EntityFrameworkCore;
using Demo.Functions.Extensions;

namespace Demo.Functions.Models
{
    public static class Converter
    {
        public static PaymentInArrears ConvertToPaymentInArrears(RentPayment rentPayment)
        {
            var rent = rentPayment?.RentArrangement;
            if (rent == null) return null;

            var propertyLocation = rent?.Property?.Location;
            var client = rent?.Client;
            return new PaymentInArrears
            {
                PaymentId = rentPayment.ReferenceId,
                Property = $"{propertyLocation?.Address1}{(string.IsNullOrEmpty(propertyLocation?.Address2) ? "" : " " + propertyLocation?.Address2)} {propertyLocation?.City}, {propertyLocation?.State} {propertyLocation?.Postcode}",
                Amount = rentPayment.DueAmount.ToString("C", DateTimeFormatInfo.InvariantInfo),
                DueDate = rentPayment.DueDate.ToAEST().ToString("D", DateTimeFormatInfo.InvariantInfo),
                Client = client?.Name,
                ClientEmail = client?.Email,
                ClientMobile = client?.Mobile
            };
        }
    }
}
