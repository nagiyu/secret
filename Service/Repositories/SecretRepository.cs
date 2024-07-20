using Microsoft.EntityFrameworkCore;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repositories
{
    public class SecretRepository : ISecretRepository
    {
        private readonly SecretDbContext _context;

        public SecretRepository(SecretDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Secret>> GetAllAsync()
        {
            return await _context.Secrets.ToListAsync();
        }

        public async Task<Secret?> GetSecret(string key)
        {
            return await _context.Secrets.SingleOrDefaultAsync(s => s.Key == key);
        }

        public async Task<Secret?> GetByIdAsync(int id)
        {
            return await _context.Secrets.SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Secret secret)
        {
            _context.Secrets.Add(secret);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Secret secret)
        {
            _context.Entry(secret).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var secret = await _context.Secrets.FindAsync(id);
            if (secret != null)
            {
                _context.Secrets.Remove(secret);
                await _context.SaveChangesAsync();
            }
        }
    }
}
