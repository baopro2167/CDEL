using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.KitRepo
{
    public class KitRepository : IKitRepository
    {
        private readonly BloodlineDbContext _context;

        public KitRepository (BloodlineDbContext context)
        {
            _context = context;
        }

        public async Task<Kit> AddAsync(Kit kit)
        {
            _context.Set<Kit>().Add(kit);
            await _context.SaveChangesAsync();
            return kit;
        }

        public async Task DeleteAsync(int id)
        {
            var kit = await _context.Set<Kit>().FindAsync(id);
            if (kit != null)
            {
                _context.Set<Kit>().Remove(kit);
                await _context.SaveChangesAsync();
            }
        }

    

        public async Task<Kit> GetByIdAsync(int id)
        {
            return await _context.Set<Kit>().FindAsync(id);
        }

        public async Task<IEnumerable<Kit>> GetkitAsync()
        {
            return await _context.Set<Kit>().ToListAsync();
        }

        public async Task<Kit> UpdateAsync(Kit kit)
        {
            _context.Entry(kit).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return kit;
        }
        public IQueryable<Kit> GetAll()
        {
            return _context.Kits.AsQueryable();
        }
    }
}
