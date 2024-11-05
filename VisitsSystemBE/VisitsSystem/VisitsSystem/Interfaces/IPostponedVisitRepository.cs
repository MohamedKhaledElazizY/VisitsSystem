using VisitsSystem.Models;

namespace VisitsSystem.Interfaces
{
    public interface IPostponedVisitRepository
    {
        ICollection<PostponedVisit> GetPostponedVisits(int officeId);
        PostponedVisit GetPostponedVisit(int IvisitId);
        bool PostponedVisitExists(int visitId);
        bool CreatePostponedVisit(PostponedVisit postponedVisit);
        bool DeletePostponedVisit(PostponedVisit postponedVisit);
        bool UpdatePostponedVisit(PostponedVisit postponedVisit);
        bool Save();
    }
}
