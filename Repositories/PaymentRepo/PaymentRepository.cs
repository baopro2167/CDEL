using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PaymentRepo
{
   public  class PaymentRepository : IPaymentRepository
    {
        private readonly BloodlineDbContext _context;

        public PaymentRepository(BloodlineDbContext context)
        {
            _context = context;
        }
        public async Task<Payment> GetByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Request)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            payment.CreatedAt = DateTime.UtcNow;
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task UpdateAsync(Payment payment)
        {
            payment.UpdatedAt = DateTime.UtcNow;
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }


    }
}
