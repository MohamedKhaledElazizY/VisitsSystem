using Microsoft.EntityFrameworkCore;
using VisitsSystem.Data;
using VisitsSystem.Dto;
using VisitsSystem.Interfaces;
using VisitsSystem.Models;

namespace VisitsSystem.Repository
{
    public class VisitRepository : IVisitRepository
    {
        private readonly VisitsDbContext _context;

        public VisitRepository(VisitsDbContext context)
        {
            _context = context;
        }
        public bool CreateVisit(Visit visit)
        {
            _context.Add(visit);
            return Save();
        }

        public bool DeleteVisit(Visit visit)
        {
            _context.Remove(visit);
            return Save();
        }

        public Visit GetVisit(int visitId)
        {
            return _context.VISITS.FirstOrDefault(x => x.VisitId == visitId);
        }

        public ICollection<Visit> GetVisits(int officeId)
        {
            return _context.VISITS.Where(x => x.OfficeId== officeId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateVisit(Visit visit)
        {
            _context.Update(visit);
            return Save();
        }

        public bool VisitExists(int visitId)
        {
            return _context.VISITS.Any(x => x.VisitId == visitId);
        }
    }
}
