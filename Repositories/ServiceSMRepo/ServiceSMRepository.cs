using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Repositories.ServiceSMRepo
{
    public class ServiceSMRepository : IServiceSMRepository
    {
        private readonly BloodlineDbContext _context;
        public ServiceSMRepository(BloodlineDbContext context)
        {
            _context = context;
        }
        public IQueryable<ServiceSampleMethod> GetAll()
        {
            return _context.ServiceSampleMethods.AsQueryable();
        }
        public async Task<IEnumerable<ServiceSampleMethod>> GetAllAsync()
        {
            return await _context.Set<ServiceSampleMethod>().ToListAsync();
        }
        public async Task<ServiceSampleMethod> GetByIdAsync(int id)
        {
            return await _context.Set<ServiceSampleMethod>().FindAsync(id);
        }
        public async Task<ServiceSampleMethod> AddAsync(ServiceSampleMethod serviceSampleMethod)
        {
            _context.Set<ServiceSampleMethod>().Add(serviceSampleMethod);
            await _context.SaveChangesAsync();
            return serviceSampleMethod;
        }
        public async Task<ServiceSampleMethod> UpdateAsync(ServiceSampleMethod serviceSM)
        {
            _context.Entry(serviceSM).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return serviceSM;
        }
        public async Task DeleteAsync(int id)
        {
            var serviceSampleMethods = _context.ServiceSampleMethods
                                       .Where(ssm => ssm.ServiceId == id);

            _context.ServiceSampleMethods.RemoveRange(serviceSampleMethods);  // Xóa tất cả các bản ghi liên quan
            await _context.SaveChangesAsync();

        }
    }
}
