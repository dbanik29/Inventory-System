using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventorySystem.Model;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Services
{
    public class UserService : IUserService
    {
        InventoryDbContext _con;        //object create for InventoryDbContext class
        public UserService(InventoryDbContext _dbcon)
        {
            _con = _dbcon;
        }
        public async Task<int> AddUser(User user)
        {
            if (_con != null)
            {
                await _con.UserInformations.AddAsync(user);
                await _con.SaveChangesAsync();
                return user.UserId;
            }
            return 0;
        }

        public async Task<int> DeleteUser(int? UserId)
        {
            int result = 0;
            if (_con != null)
            {
                var deluser = await _con.UserInformations.FirstOrDefaultAsync(x => x.UserId == UserId);
                if (deluser != null)
                {
                    _con.UserInformations.Remove(deluser);
                    result = await _con.SaveChangesAsync();
                }
                return result;
            }
            return result;
        }

        public async Task<List<User>> GetAllUser()
        {
            if (_con != null)
            {
                return await _con.UserInformations.ToListAsync();
            }
            return null;
        }

        public async Task UpdateUser(User user)
        {
            if (_con != null)
            {
                _con.UserInformations.Update(user);
                await _con.SaveChangesAsync();
            }
        }
    }
}
