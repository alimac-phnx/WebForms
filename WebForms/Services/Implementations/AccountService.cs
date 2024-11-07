using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WebForms.Models;
using WebForms.Repositories.Interfaces;
using WebForms.Services.Interfaces;
using WebForms.ViewModels;

namespace WebForms.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;

        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> TryRegisterAsync(RegistrationDataViewModel registrationData, CancellationToken cancellationToken = default)
        {
            try
            {
                await RegisterAsync(registrationData, cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public User StartNewUser(RegistrationDataViewModel registrationData)
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

        public async Task RegisterAsync(RegistrationDataViewModel registrationData, CancellationToken cancellationToken = default)
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

            return await IsUserVerifiedAsync(user, password, cancellationToken);
        }

        public async Task<bool> IsUserVerifiedAsync(User user, string password, CancellationToken cancellationToken = default)
        {
            string message = "Invalid e-mail or password";

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                if (user.Status == "Active")
                {
                    await StartUserSessionAsync(user, cancellationToken);

                    return true;
                }
                else message = "Sorry, your account was blocked.";
            }

            return false;
        }

        public async Task StartUserSessionAsync(User user, CancellationToken cancellationToken = default)
        {
            user.LastLoginDate = DateTime.Now;
            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        public async Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetByIdAsync(userId, cancellationToken);
        }

        public async Task<List<User>> FindSelectedUsers(List<int> selectedUsers, CancellationToken cancellationToken = default)
        {
            return await _userRepository.FindAsync(u => selectedUsers.Contains(u.Id), cancellationToken);
        }

        public async Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetAllAsync(cancellationToken);
        }

        public async Task<StringContent> PrepareSfAccount(int userId, CancellationToken cancellationToken = default)
        {
            var user = await GetUserByIdAsync(userId, cancellationToken);

            var accountData = new
            {
                Name = user.Username,
                Email__c = user.Email,
            };

            return new StringContent(JsonConvert.SerializeObject(accountData), Encoding.UTF8, "application/json");
        }

        public async Task<StringContent> PrepareSfAccountContact(string result, StringContent accountContent)
        {
            var jsonResult = JObject.Parse(result);
            string accountId = jsonResult["id"].ToString();

            string jsonString = await accountContent.ReadAsStringAsync();
            var jsonAccount = JObject.Parse(jsonString);

            var contactData = new
            {
                LastName = jsonAccount["Name"].ToString(),
                Email = jsonAccount["Email__c"].ToString(),
                AccountId = accountId
            };

            return new StringContent(JsonConvert.SerializeObject(contactData), Encoding.UTF8, "application/json");
        }
    }
}