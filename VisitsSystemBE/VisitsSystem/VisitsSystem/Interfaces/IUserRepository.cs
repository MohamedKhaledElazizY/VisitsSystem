using Microsoft.EntityFrameworkCore;
using VisitsSystem.Models;

namespace VisitsSystem.Interfaces
{
    public interface IUserRepository
    {
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);


        User GetUser(int id);
        List<User> GetAllUsers();

        bool UserExists(int id);
        bool UsernameExists(string name);
        bool UserLogIn(string username, string password);

        int GetUserID(string username, string password);
        bool Save();
        string LogIn(int ID, string hashid);
    }
}
