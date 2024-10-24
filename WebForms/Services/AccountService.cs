using System.Threading;
using WebForms.Data;
using WebForms.Interfaces;
using WebForms.Models;

namespace WebForms.Services
{
    public class AccountService
    {
        private readonly IUserRepository _userRepository;

        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> TryRegisterAsync(RegistrationData registrationData)
        {
            try
            {
                await RegisterAsync(registrationData);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public User StartNewUser(RegistrationData registrationData)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registrationData.Password);

            var user = new User
            {
                Username = registrationData.Username,
                PasswordHash = passwordHash,
                Email = registrationData.Email,
                IsAdmin = false,
                Status = "Active",
                RegistrationDate = DateTime.Now
            };

            return user;
        }

        public async Task RegisterAsync(RegistrationData registrationData, CancellationToken cancellationToken = default)
        {
            var user = StartNewUser(registrationData);
            await _userRepository.AddAsync(user, cancellationToken);
        }

        public async Task<User> GetUserAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetByEmailAsync(email, cancellationToken);
        }

        public async Task<bool> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await GetUserAsync(email, cancellationToken);

            return await IsUserVerifiedAsync(user, password);
        }

        public async Task<bool> IsUserVerifiedAsync(User user, string password)
        {
            string message = "Invalid e-mail or password";

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                if (user.Status == "Active")
                {
                    await StartUserSessionAsync(user);
                    
                    return true;
                }
                else message = "Sorry, your account was blocked.";
            }

            return false;
        }

        public async Task StartUserSessionAsync(User user)
        {
            user.LastLoginDate = DateTime.Now;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<User> GetCurrentUserAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }
    }
}
