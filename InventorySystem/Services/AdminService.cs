using InventorySystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventorySystem.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventorySystem.Services
{
    public interface IAdminService
    {    
        Admin Authenticate(string username, string password);
        IEnumerable<Admin> GetAll();
        int AddAdmin(Admin entity);
        int UpdateAdmin(Admin entity);
        int DeleteAdmin(int? id);
    }
    public class AdminService : IAdminService
    {
        private readonly AppSettings _appSettings;
        private readonly InventoryDbContext _context;     //object create for InventoryDbContext class
        public AdminService(IOptions<AppSettings> appSettings, InventoryDbContext _dbcontext)
        {
            _context = _dbcontext;
        }

        public Admin Authenticate(string username, string password)
        {
            var useradmin = _context.AdminDetailss.SingleOrDefault(x => x.UserName == username && x.Password == password);
            if (useradmin == null)
                return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
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

        public IEnumerable<Admin> GetAll()
        {
            return _context.AdminDetailss.ToList();
        }
        public int AddAdmin(Admin entity)
        {
            if (entity != null)
            {
                _context.AdminDetailss.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }
            return 0;
        }

        public int UpdateAdmin(Admin entity)
        {
            if (entity != null)
            {
                _context.AdminDetailss.Update(entity);
                _context.SaveChangesAsync();
                return 1;
            }
            return 0;
        }

        public int DeleteAdmin(int? id)
        {
            if (id != null)
            {
                var adminuser = _context.AdminDetailss.FirstOrDefault(X => X.Id == id);
                _context.Remove(adminuser);
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }
    }
}
