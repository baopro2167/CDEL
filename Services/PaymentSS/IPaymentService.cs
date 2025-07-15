using Microsoft.AspNetCore.Http;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PaymentSS
{
    public interface IPaymentService
    {
       
        Task<string> CreatePaymentUrl(CreatePaymentRequest request);
        Task<string> UpdatePaymentUrlAsync(int paymentId);

        bool ValidateVNPaySignature(IQueryCollection query);
    }
}
