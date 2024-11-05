using VisitsSystem.Dto;
using VisitsSystem.Models;

namespace VisitsSystem.Interfaces
{
    public interface IVisitRepository
    {
        ICollection<Visit> GetVisits(int officeId);
        Visit GetVisit(int visitId);
        bool VisitExists(int visitId);
        bool CreateVisit(Visit visit);
        bool DeleteVisit(Visit visit);
        bool UpdateVisit(Visit visit);
        bool Save();

    }
}
