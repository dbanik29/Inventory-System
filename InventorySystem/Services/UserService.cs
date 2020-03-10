using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventorySystem.Model;
using Microsoft.EntityFrameworkCore;
using InventorySystem.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace InventorySystem.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _Configuration;
        private readonly InventoryDbContext _context;        //object create for InventoryDbContext class
        public UserService(IConfiguration Configuration, InventoryDbContext _dbcontext)
        {
            _context = _dbcontext;
            _Configuration = Configuration;
        }
        public User LoginAuthentication(string username, string password)
        {
            var useradmin = _context.UserInformations.SingleOrDefault(x => x.UserName == username && x.Password == password);
            if (useradmin == null)
                return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var appSettingsSection = _Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role,useradmin.Role),
                    new Claim(ClaimTypes.Name,useradmin.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            useradmin.Token = tokenHandler.WriteToken(token);
            // remove password before returning
            useradmin.Password = null;
            return useradmin;
        }

        public async Task<int> AddUser(User user)
        {
            if (_context != null)
            {
                await _context.UserInformations.AddAsync(user);
                await _context.SaveChangesAsync();
                return user.UserId;
            }
            return 0;
        }

        public async Task<int> DeleteUser(int? UserId)
        {
            int result = 0;
            if (_context != null)
            {
                var deluser = await _context.UserInformations.FirstOrDefaultAsync(x => x.UserId == UserId);
                if (deluser != null)
                {
                    _context.UserInformations.Remove(deluser);
                    result = await _context.SaveChangesAsync();
                }
                return result;
            }
            return result;
        }

        public async Task<List<User>> GetAllUser()
        {
            if (_context != null)
            {
                return await _context.UserInformations.ToListAsync();
            }
            return null;
        }

        //public async Task<List<User>> GetAllUser()
        //{
        //    var odds = _context.UserInformations.Where(x => x.UserId % 2 == 0);
        //    if (_context != null)
        //    {
        //        return await _context.FindAsync(odds);
        //        //return await _context.if (_context != null).ToListAsync(odds);
        //    }
        //    return null;
        //}

        public async Task UpdateUser(User user)
        {
            if (_context != null)
            {
                _context.UserInformations.Update(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
