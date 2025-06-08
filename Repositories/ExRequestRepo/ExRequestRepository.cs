using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ExRequestRepo
{
    public class ExRequestRepository : IExRequestRepository
    {
        private readonly BloodlineDbContext _context;

        public ExRequestRepository(BloodlineDbContext context)
        {
            _context = context;
        }

      
       
        public IQueryable<ExaminationRequest> GetAll()
        {
            return _context.ExaminationRequests.AsQueryable();
        }

        public async Task<ExaminationRequest> GetByIdAsync(int id)
        {
            return await _context.Set<ExaminationRequest>().FindAsync(id);
        }
        public IQueryable<ExaminationRequest> GetByAccountId(int id)
        {
            return _context.ExaminationRequests.Where(o => o.UserId == id);
        }
        public async Task<IEnumerable<ExaminationRequest>> GetAsync()
        {
            return await _context.Set<ExaminationRequest>().ToListAsync();
        }
        public async Task<ExaminationRequest> AddAsync(ExaminationRequest examinationRequest)
        {
            _context.Set<ExaminationRequest>().Add(examinationRequest);
            await _context.SaveChangesAsync();
            return examinationRequest;
        }


        public async Task<ExaminationRequest> UpdateAsync(ExaminationRequest examinationRequest)
        {
            _context.Entry(examinationRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return examinationRequest;
        }

        public async Task DeleteAsync(int id)
        {
            var examination = await _context.Set<ExaminationRequest>().FindAsync(id);
            if (examination != null)
            {
                _context.Set<ExaminationRequest>().Remove(examination);
                await _context.SaveChangesAsync();
            }
        }
    }
}
