using InventorySystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Services
{
    public interface IUserService
    {
        User LoginAuthentication(string username, string password);
        Task<List<User>> GetAllUser();
        Task<int> AddUser(User user);
        Task<int> DeleteUser(int? UserId);
        Task UpdateUser(User user);
    }
}
