using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using iTravelerServer.DAL.Interfaces;
using iTravelerServer.DAL.Repositories;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Enum;
using iTravelerServer.Domain.Helpers;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.AccountVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace iTravelerServer.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IBaseRepository<Account> _accountRepository;

        //private readonly AccountRepository _accountRep;
        private readonly IConfiguration _configuration;

        public AccountService(IBaseRepository<Account> accountRepository,
            IConfiguration configuration)
        {
            _accountRepository = accountRepository;

            _configuration = configuration;
        }

        public async Task<BaseResponse<IEnumerable<Account>>> GetAccounts()
        {
            var baseResponse = new BaseResponse<IEnumerable<Account>>();
            try
            {
                var flights = await _accountRepository.GetAll().ToListAsync();


                baseResponse.Data = flights;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Account>>()
                {
                    Description = $"[GetAccounts] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }


        // api/account/login
        //[HttpPost("login")]
        public async Task<BaseResponse<Account>> Login(LoginRequestVM loginData)
        {
            var baseResponse = new BaseResponse<Account>();
            try
            {
                var acc = new Account()
                {
                    Email = loginData.Email
                };
                var user = await _accountRepository.Get(acc);
                if (user == null || user.Password == null)
                    return null;

                if (!HashPasswordHelper.MatchPasswordHash(user.Password, loginData.Password))
                    return null;

                return new BaseResponse<Account>()
                {
                    Data = user,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Account>()
                {
                    Description = $"[Login] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }


        public async Task<BaseResponse<Account>> Register(RegisterVM registerData)
        {
            var baseResponse = new BaseResponse<Account>();
            try
            {
                var acc = new Account()
                {
                    Email = registerData.Email
                };
                var user = await _accountRepository.Get(acc);
                if (user != null)
                {
                    return new BaseResponse<Account>()
                    {
                        Description = "User already exists",
                        StatusCode = StatusCode.NotFound
                    };
                }

                user = new Account()
                {
                    Email = registerData.Email,
                    Password = HashPasswordHelper.HashPassword(registerData.Password),
                    Username = registerData.userName,
                    Role = Role.User
                };
                await _accountRepository.Create(user);


                acc.Email = user.Email;
                user = await _accountRepository.Get(acc);
                if (user == null)
                {
                    return new BaseResponse<Account>()
                    {
                        Description = "cant find the user",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<Account>()
                {
                    //Data = user,
                    Description = "User registered",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Account>()
                {
                    Description = $"[Register] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Account>> ChangePassword(ChangePasswordVM userData)
        {
            var baseResponse = new BaseResponse<Account>();
            try
            {
                var acc = new Account()
                {
                    Email = userData.Email
                };
                var existedUser = await _accountRepository.Get(acc);

                if (HashPasswordHelper.HashPassword(userData.NewPassword) == existedUser.Password)
                {
                    existedUser.Password = HashPasswordHelper.HashPassword(userData.NewPassword);
                    await _accountRepository.Update(existedUser);

                    existedUser = await _accountRepository.Get(existedUser);
                    if (existedUser.Password == HashPasswordHelper.HashPassword(userData.NewPassword))
                    {
                        return new BaseResponse<Account>()
                        {
                            Description = "Password changed",
                            StatusCode = StatusCode.OK
                        };
                    }
                }
                return new BaseResponse<Account>()
                {
                    //Data = user,
                    Description = "Passwords not match",
                    StatusCode = StatusCode.NotFound
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Account>()
                {
                    Description = $"[Register] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public string CreateJWT(Account user)
        {
            var secretKey = _configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(secretKey));

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("role", user.Role.ToString())
            };

            var signingCredentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}