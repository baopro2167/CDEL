using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.StaffRepo
{
    public class StaffRepository : IStaffRepository
    {
        private readonly BloodlineDbContext _context;

        public StaffRepository(BloodlineDbContext Context)
        {
            _context = Context;
        }

        public async Task AddAsync(Staff staff)
        {
            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();
        }

        public async Task<Staff?> GetByIdAsync(int id)
        {
            return await _context.Set<Staff>().FindAsync(id);
        }

        public async Task<IEnumerable<Staff>> GetAllAsync()
        {
            return await _context.Staffs
                .AsNoTracking()
                .ToListAsync();
        }

   
        public async Task DeleteAsync(int id)
        {
            var kit = await _context.Set<Staff>().FindAsync(id);
            if (kit != null)
            {
                _context.Set<Staff>().Remove(kit);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Staff> UpdateAsync(Staff staff)
        {
            _context.Entry(staff).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return staff;
        }
    }
}

