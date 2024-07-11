using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repositories
{
    public interface ISecretRepository
    {
        Task<IEnumerable<Secret>> GetAllAsync();
        Task<Secret> GetByIdAsync(int id);
        Task AddAsync(Secret secret);
        Task UpdateAsync(Secret secret);
        Task DeleteAsync(int id);
    }
}
