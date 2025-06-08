using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;
namespace Repositories.ServiceRepo
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly BloodlineDbContext _context;
        public ServiceRepository(BloodlineDbContext context)
        {
            _context = context;
        }
        public IQueryable<Service> GetAll()
        {
            return _context.ServiceBs.AsQueryable();
        }
        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _context.Set<Service>().ToListAsync();
        }
        public async Task<Service> GetByIdAsync(int id)
        {
            return await _context.Set<Service>().FindAsync(id);
        }
        public async Task<Service> AddAsync(Service service)
        {
            _context.Set<Service>().Add(service);
            await _context.SaveChangesAsync();
            return service;
        }
        public async Task<Service> UpdateAsync(Service service)
        {
            _context.Entry(service).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return service;
        }
        public async Task DeleteAsync(int id)
        {
            var service = await _context.Set<Service>().FindAsync(id);
            if (service != null)
            {
                _context.Set<Service>().Remove(service);
                await _context.SaveChangesAsync();
            }
        }
    }
}
