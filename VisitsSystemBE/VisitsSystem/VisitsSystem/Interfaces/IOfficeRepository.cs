using VisitsSystem.Models;

namespace VisitsSystem.Interfaces
{
    public interface IOfficeRepository
    {
        bool Add(Office office);
        bool Delete(Office office);
        Office GetOffice(int id);
        bool Exist(string name);
        bool Exist(int id);
        bool EditOffice(Office office);
        List<User> GetOfficeusers(int id);
        List<Office> GetAllOffices();
        bool Save();
    }
}
