using System;
using System.Collections.Generic;
using System.Text;

namespace PackageFoodManagementSystem.Repository
{
    public interface IUserRepository
    {
        Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
