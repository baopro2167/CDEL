using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PaymentSS
{
    public interface IPaymentService
    {
       
        Task<string> CreatePaymentUrl(int userId, int requestId, decimal amount, string orderInfo);
        Task<string> UpdatePaymentUrlAsync(int paymentId);

        bool ValidateVNPaySignature(IQueryCollection query);
    }
}
