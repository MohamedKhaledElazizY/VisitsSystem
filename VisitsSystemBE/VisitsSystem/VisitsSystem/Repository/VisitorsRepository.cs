using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using VisitsSystem.Data;
using VisitsSystem.Interfaces;
using VisitsSystem.Models;

namespace VisitsSystem.Repository
{
    public class VisitorsRepository : IVisitorsRepository
    {
        private readonly VisitsDbContext _context;
        private readonly IMapper _mapper;

        public VisitorsRepository(VisitsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public ICollection<Visitor> GetVisitors()
        {

            return _context.VISITORS.ToList();

        }
        public Visitor GetVisitor(int id)
        {
            return _context.VISITORS.Where(x => x.VisitorId == id).FirstOrDefault();
        }
        public bool CreateVisitor(Visitor visitor)
        {
            _context.Add(visitor);
            return Save();
        }
        public bool UpdateVisitor(Visitor visitor)
        {

            _context.Update(visitor);
            return Save();
        }
        public bool VisitorExists(int id)
        {
            return _context.VISITORS.Any(x => x.VisitorId == id);
        }
        public bool DeleteVisitor(Visitor visitor)
        {
            _context.Remove(visitor);
            return Save();
        }
        public ICollection<Visitor> VisitorsSearch(int? id,String? Name, string? Rank, String? JobTitle)
        {
            List<Visitor> Visitors = _context.VISITORS.ToList();

            if (id != null)
                Visitors = Visitors.Where(x => x.VisitorId == id).ToList();
            if (Name != null)
                Visitors = Visitors.Where(x => x.VisitorName.Contains(Name)).ToList();
            if (Rank != null)
                Visitors = Visitors.Where(x => x.Rank == Rank).ToList();
          
            if (JobTitle != null)
                Visitors = Visitors.Where(x => (x.JobTitle + "").Trim() == JobTitle).ToList();
            return Visitors;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    
    }
}
