using VisitsSystem.Dto;
using VisitsSystem.Models;

namespace VisitsSystem.Interfaces
{
    public interface IVisitorsRepository
    {
        ICollection<Visitor> GetVisitors();
        Visitor GetVisitor(int ID);
        ICollection<Visitor> VisitorsSearch(int? id,String? Name, string? Rank,String? JobTitle);

        bool CreateVisitor(Visitor Visitor);
        bool VisitorExists(int ID);
        bool UpdateVisitor(Visitor Visitor);
        bool DeleteVisitor(Visitor Visitor);
      //   int GetEnumIndex(Enum value);
        bool Save();
    }
}
