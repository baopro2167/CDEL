using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;
namespace Repositories.ExResultRepo
{
    public class ExResultRepository : IExResultRepository
    {
        private readonly BloodlineDbContext _context;

        public ExResultRepository(BloodlineDbContext context)
        {
            _context = context;
        }

        public async Task<ExaminationResult> GetByIdAsync(int id)
        {
            return await _context.Set<ExaminationResult>().FindAsync(id);
        }
        public IQueryable<ExaminationResult> GetAll()
        {
            return _context.ExaminationResults.AsQueryable();
        }
        public async Task<IEnumerable<ExaminationResult>> GetAsync()
        {
            return await _context.Set<ExaminationResult>().ToListAsync();
        }


        public async Task<ExaminationResult> AddAsync(ExaminationResult result)
        {
            _context.Set<ExaminationResult>().Add(result);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<ExaminationResult> UpdateAsync(ExaminationResult result)
        {
            _context.Entry(result).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            var result = await _context.Set<ExaminationResult>().FindAsync(id);
            if (result != null)
            {
                _context.Set<ExaminationResult>().Remove(result);
                await _context.SaveChangesAsync();
            }
        }
    }
}
