using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly BloodlineDbContext _context;
        public UserRepository(BloodlineDbContext context)
        {
            _context = context;
        }

        public async Task<User> Register(User user)
        {
            _context.Set<User>().Add(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public IQueryable<User> GetAll()
        {
            return _context.Users.AsQueryable();
        }
       

        public IQueryable<User> GetUsersByRoleId(int roleId)
        {
            return _context.Users.Where(u => u.RoleId == roleId).AsNoTracking();
        }
    }
    }

