using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;
namespace Repositories.SampleMethodRepo
{
    public class SampleMethodRepository : ISampleMethodRepository
    {
        private readonly BloodlineDbContext _context;
        public SampleMethodRepository(BloodlineDbContext context)
        {
            _context = context;
        }

      

        public IQueryable<SampleMethod> GetAll()
        {
            return _context.SampleMethods.AsQueryable();
        }

        public async Task<SampleMethod> GetByIdAsync(int id)
        {
            return await _context.Set<SampleMethod>().FindAsync(id);
        }
        public async Task<IEnumerable<SampleMethod>> GetByServiceIdAsync(int serviceId)
        {
            return await _context.SampleMethods
                                 .Where(sm => sm.Id == serviceId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<SampleMethod>> GetAsync()
        {
            return await _context.Set<SampleMethod>().ToListAsync();
        }
        public async Task<SampleMethod> AddAsync(SampleMethod method)
        {
            _context.Set<SampleMethod>().Add(method);
            await _context.SaveChangesAsync();
            return method;

        }


        public async Task<SampleMethod> UpdateAsync(SampleMethod method)
        {
            _context.Entry(method).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return method;

        }


        public async Task DeleteAsync(int id)
        {
            var method = await _context.Set<SampleMethod>().FindAsync(id);
            if (method != null)
            {
                _context.Set<SampleMethod>().Remove(method);
                await _context.SaveChangesAsync();
            }
        }
    }
}
