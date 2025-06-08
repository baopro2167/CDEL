using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.KitDeliveryRepo
{
    public class KitDeliveryRepository : IKitDeliveryRepository
    {
        private readonly BloodlineDbContext _context;

        public KitDeliveryRepository(BloodlineDbContext context)
        {
            _context = context;
        }

        public async Task<KitDelivery> AddAsync(KitDelivery kitDelivery)
        {
            _context.Set<KitDelivery>().Add(kitDelivery);
            await _context.SaveChangesAsync();
            return kitDelivery;
        }

        public async Task DeleteAsync(int id)
        {
            var kitDelivery = await _context.Set<KitDelivery>().FindAsync(id);
            if (kitDelivery != null)
            {
                _context.Set<KitDelivery>().Remove(kitDelivery);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<KitDelivery> GetAll()
        {
            return _context.KitDeliverys.AsQueryable();
        }

        public async Task<KitDelivery> GetByIdAsync(int id)
        {
            return await _context.Set<KitDelivery>().FindAsync(id);
        }

        public async Task<IEnumerable<KitDelivery>> GetkitAsync()
        {
            return await _context.Set<KitDelivery>().ToListAsync();
        }

        public async Task<KitDelivery> UpdateAsync(KitDelivery kitDelivery)
        {
            _context.Entry(kitDelivery).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return kitDelivery;
        }
    }
}
