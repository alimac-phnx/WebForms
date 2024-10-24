﻿using WebForms.Models;

namespace WebForms.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<IEnumerable<User>> GetUsersWithTemplatesAsync(CancellationToken cancellationToken = default);
    }
}