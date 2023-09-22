using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<bool> AuthenticateAsync(string email, string password, CancellationToken  token = default);
        Task<bool> SignUpAsync(User user, string password, CancellationToken token = default);
        Task<User> GetByEmailAsync(string requestEmail, CancellationToken token = default);
    }
}
