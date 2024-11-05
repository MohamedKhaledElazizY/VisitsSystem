using Microsoft.EntityFrameworkCore;
using VisitsSystem.Data;
using VisitsSystem.Interfaces;
using VisitsSystem.Models;

namespace VisitsSystem.Repository
{
    public class PostponedVisitRepository : IPostponedVisitRepository
    {
        private readonly VisitsDbContext _context;

        public PostponedVisitRepository(VisitsDbContext context)
        {
            _context = context;
        }
        public bool CreatePostponedVisit(PostponedVisit postponedVisit)
        {
            _context.Add(postponedVisit);
            return Save();
        }

        public bool DeletePostponedVisit(PostponedVisit postponedVisit)
        {
            _context.Remove(postponedVisit);
            return Save();
        }

        public PostponedVisit GetPostponedVisit(int IvisitId)
        {
            return _context.POSTPONED_VISITS.FirstOrDefault(x => x.VisitId == IvisitId);
        }

        public ICollection<PostponedVisit> GetPostponedVisits(int officeId)
        {
            return _context.POSTPONED_VISITS.Where(x => x.Visit.OfficeId == officeId).ToList();
        }

        public bool PostponedVisitExists(int visitId)
        {
            return _context.POSTPONED_VISITS.Any(x => x.VisitId == visitId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePostponedVisit(PostponedVisit postponedVisit)
        {
            _context.Update(postponedVisit);
            return Save();
        }
    }
}
