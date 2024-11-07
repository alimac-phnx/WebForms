using WebForms.Models;
using WebForms.ViewModels;

namespace WebForms.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> TryRegisterAsync(RegistrationDataViewModel registrationData, CancellationToken cancellationToken = default);

        User StartNewUser(RegistrationDataViewModel registrationData);

        Task RegisterAsync(RegistrationDataViewModel registrationData, CancellationToken cancellationToken = default);

        Task<User> GetUserAsync(string email, CancellationToken cancellationToken = default);

        Task<bool> LoginAsync(string email, string password, CancellationToken cancellationToken = default);

        Task<bool> IsUserVerifiedAsync(User user, string password, CancellationToken cancellationToken = default);

        Task StartUserSessionAsync(User user, CancellationToken cancellationToken = default);

        Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);

        Task<List<User>> FindSelectedUsers(List<int> selectedUsers, CancellationToken cancellationToken = default);

        Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);

        Task<StringContent> PrepareSfAccount(int userId, CancellationToken cancellationToken = default);

        Task<StringContent> PrepareSfAccountContact(string result, StringContent accountContent);
    }
}