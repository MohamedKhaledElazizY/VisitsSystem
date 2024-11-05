using VisitsSystem.Data;
using VisitsSystem.Interfaces;
using VisitsSystem.Models;

namespace VisitsSystem.Repository
{
    public class OfficeRepository:IOfficeRepository
    {
        private readonly VisitsDbContext _context;

        public OfficeRepository(VisitsDbContext context)
        {
            _context = context;

        }
        public bool Add(Office office)
        {
            if (_context.OFFICES.Where(x => x.OfficeName == office.OfficeName).Any())
            {
                return false;
            }
            _context.OFFICES.Add(office);
            return Save();
        }
        public bool Delete(Office office)
        {
            _context.OFFICES.Remove(office);
            return Save();
        }
        public bool Exist(String name)
        {
           Office office= _context.OFFICES.FirstOrDefault(x => x.OfficeName == name);
            if(office == null)
            {
                return false;
            }
            return true;
        }
        public bool Exist(int id)
        {
            bool office = _context.OFFICES.Any(x => x.OfficeId == id);
            
            return office;
        }
        public Office GetOffice(int id)
        {
            Office office = _context.OFFICES.FirstOrDefault(x => x.OfficeId == id);

            return office;
        }
        public List<Office> GetAllOffices()
        {
            List<Office> offices = _context.OFFICES.ToList();

            return offices;
        }
        public bool EditOffice(Office office)
        {
            _context.OFFICES.Update(office);

            return Save();
        }
        public List<User> GetOfficeusers(int id)
        {
            List<User> users = _context.USERS.Where(x => x.OfficeId == id).ToList();

            return users;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        
    }
}
