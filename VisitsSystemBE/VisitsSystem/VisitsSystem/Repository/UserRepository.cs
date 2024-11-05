using AutoMapper;
using System.Security.Cryptography;
using VisitsSystem.Data;
using VisitsSystem.Interfaces;
using VisitsSystem.Models;

namespace VisitsSystem.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly VisitsDbContext _context;

        public UserRepository(VisitsDbContext context)
        {
            _context = context;

        }
        public bool CreateUser(User user)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(user.HashedPassword);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            user.HashedPassword = Convert.ToHexString(hashBytes);
            _context.Add(user);
            return Save();
        }
        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }
        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }


        public User GetUser(int id)
        {
            return _context.USERS.Where(x => x.UserId == id).FirstOrDefault();
        }
        public List<User> GetAllUsers()
        {
            return _context.USERS.ToList();
        }
        public bool UserExists(int id)
        {
            return _context.USERS.Any(x => x.UserId == id);
        }
        public bool UsernameExists(string name)
        {
            return _context.USERS.Any(x => x.UserName == name);
        }
        public bool UserLogIn(string username, string password)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            password = Convert.ToHexString(hashBytes);
            return _context.USERS.Any(x => x.UserName == username && x.HashedPassword == password);
        }

        public int GetUserID(string username, string password)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            password = Convert.ToHexString(hashBytes);
            int userid = _context.USERS.FirstOrDefault(x => x.UserName == username && x.HashedPassword == password).UserId;
            return userid;

        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public string LogIn(int ID, string hashid)
        {

            //try
            //{
            //    JwtSecurityToken j = (new JwtSecurityTokenHandler().ReadJwtToken(Token));
            //}
            //catch(Exception ex) {
            //    return ("invalid token");
            //}
            if (!UserExists(ID))
            {
                return ("id not exist");
            }
            
            var user = GetUser(ID);
            if (user.Hash == null)
            {
                return ("not logged in");
            }
            var hash = new HMACSHA512(System.Text.Encoding.UTF8.GetBytes(user.Hash));
            string IDuserid = Convert.ToBase64String(hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.UserId.ToString())));
            if (hashid != IDuserid)
            {
                return ("invalid hased id");
            }
            return ("");
        }
    }
}
