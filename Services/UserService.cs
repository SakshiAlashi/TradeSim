using TradeSim.Data;
using TradeSim.Models.Domain;
using TradeSim.Models.ViewModels;
using TradeSim.Models.Results;
using Microsoft.AspNetCore.Identity;

namespace TradeSim.Services
{
    public class UserService
    {
        private readonly TradeSimDbContext dbContext;
        private readonly PasswordHasher<User> passwordHasher;

        public UserService(
            TradeSimDbContext dbContext,
            PasswordHasher<User> passwordHasher)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
        }

        public RegistrationResult Register(RegisterViewModel vm)
        {
            if (dbContext.Users.Any(x => x.Email == vm.Email))
            {
                return new RegistrationResult
                {
                    Success = false,
                    Message = "Email already exists."
                };
            }

            if (!string.IsNullOrWhiteSpace(vm.UserName))
            {
                if (dbContext.Users.Any(x => x.UserName == vm.UserName))
                {
                    return new RegistrationResult
                    {
                        Success = false,
                        Message = "Username already exists."
                    };
                }
            }
            string userName = string.IsNullOrWhiteSpace(vm.UserName)
            ? GenerateUniqueUsername()
            : vm.UserName;
            User user = new User
            {
                Id = Guid.NewGuid(),
                FullName = vm.FullName,
                UserName = userName,
                Email = vm.Email,
            };
            user.PasswordHash = passwordHasher.HashPassword(user, vm.Password);
            
            dbContext.Users.Add(user);

            dbContext.SaveChanges();
           
            return new RegistrationResult
            {
                Success = true,
                Message = "Registration successful."
            };
        }

        private string GenerateUniqueUsername()
        {
            string userName;

            do
            {
                userName = Guid.NewGuid().ToString("N").Substring(0, 8);
            }
            while (dbContext.Users.Any(x => x.UserName == userName));

            return userName;
        }

        public LoginResult Login(LoginViewModel vm)
        {
            bool isEmail = vm.Login.Contains("@");
            User? user;
            if (isEmail)
            {
                user = dbContext.Users.SingleOrDefault(x => x.Email == vm.Login);
            }
            else
            {
                 user = dbContext.Users.SingleOrDefault(x => x.UserName == vm.Login);
            }
            if (user == null)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Invalid username/email or password"
                };
            }
            PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(
                                                user,user.PasswordHash,vm.Password);

            if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                return new LoginResult
                {
                    Success = true,
                    Message = "Login Successful!",
                    User = user
                };
            }
            return new LoginResult
            {
                Success = false,
                Message = "Invalid username/email or password."
            };
        }
    }
}