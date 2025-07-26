using Model;
using Repositories.Pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PaymentRepo
{
    public interface IPaymentRepository
    {
        Task UpdateAsync(Payment payment);
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment> GetByIdAsync(int id);

        Task<PaginatedList<Payment>> GetByUserIdAsync(int userId, int pageNumber, int pageSize);

       
    }
}
