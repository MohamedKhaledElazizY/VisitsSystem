using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using VisitsSystem.Data;
using VisitsSystem.Dto;
using VisitsSystem.Interfaces;
using VisitsSystem.Models;
using VisitsSystem.Repository;

namespace VisitsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitsController : Controller
    {
        private readonly IVisitRepository _visitRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPostponedVisitRepository _postponedVisit;
        private readonly IVisitorsRepository _visitorsRepository;

        public VisitsController(IVisitRepository visitRepository, IUserRepository userRepository,
            IMapper mapper, IPostponedVisitRepository postponedVisit, IVisitorsRepository visitorsRepository)
        {
            _visitRepository = visitRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _postponedVisit = postponedVisit;
            _visitorsRepository = visitorsRepository;
        }

        [HttpGet("{officeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Visit>))]
        public IActionResult GetVisits(int officeId) 
        {
            if(officeId == null)
                return BadRequest(ModelState);

            var visits = _visitRepository.GetVisits(officeId);
            foreach (Visit visit in visits)
            {
                visit.Visitor = _visitorsRepository.GetVisitor(visit.VisitId);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(visits);
        }

        [HttpGet("finished/{officeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FinishedVisitsDto>))]
        public IActionResult GetFinishedVisits(int officeId)
        {
            if (officeId == null) 
                return BadRequest(ModelState);

            var visits = _mapper.Map<List<FinishedVisitsDto>>(_visitRepository.GetVisits(officeId).Where(x => x.LeavingDate != null));
            foreach (var visit in visits)
            {
                visit.Visitor = _visitorsRepository.GetVisitor(visit.VisitorId);
                visit.StateName = ((State)visit.State).ToString();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(visits);
        }

        [HttpGet("current/{officeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Visit>))]
        public IActionResult GetCurrentVisits(int officeId)
        {
            if (officeId == null)
                return BadRequest(ModelState);

            var visits = _visitRepository.GetVisits(officeId).Where(x => x.LeavingDate == null);
            foreach(Visit visit in visits)
            {
                visit.Visitor = _visitorsRepository.GetVisitor(visit.VisitorId);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(visits);
        }

        [HttpGet("postponed/{officeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PostponedVisit>))]
        public IActionResult GetPostponedVisits(int officeId)
        {
            if (officeId == null )
                return BadRequest(ModelState);

            var postponedVisits = _postponedVisit.GetPostponedVisits(officeId).Where(x => x.PostponedDate.Date == DateTime.Today).ToList();
            foreach(var postponedvisit in postponedVisits)
            {
                postponedvisit.Visit = _visitRepository.GetVisit(postponedvisit.VisitId);
                postponedvisit.Visit.Visitor = _visitorsRepository.GetVisitor(postponedvisit.Visit.VisitorId);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(postponedVisits);
        }

        [HttpPost("{officeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatVisit(int officeId, [FromBody] VisitDto visitCreate) 
        {
            if (visitCreate == null)
                return BadRequest(ModelState);
            visitCreate.ArrivalDate = DateTime.Now;
            var visit = _mapper.Map<Visit>(visitCreate);
            visit.OfficeId = officeId;
            if (!_visitRepository.CreateVisit(visit)) 
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
        [HttpPut("{visitId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateVisit(int visitId,[FromBody]StateDto state) 
        {
            if(state == null)
                return BadRequest(ModelState);

            if (!_visitRepository.VisitExists(visitId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();
            var updateVisit = _visitRepository.GetVisit(visitId);
            updateVisit.State = state.StateId;
            if(state.StateId == 1)
                updateVisit.EntryDate= DateTime.Now;
            else if(state.StateId == 4 && state.date != null) 
            {
               if (_postponedVisit.PostponedVisitExists(visitId)) 
                {
                    var postponedVisit = _postponedVisit.GetPostponedVisit(visitId);
                    postponedVisit.PostponedDate = (DateTime)state.date;
                    _postponedVisit.UpdatePostponedVisit(postponedVisit);

                }
                else 
                {
                    var postponedVisit = new PostponedVisit();
                    postponedVisit.VisitId = visitId;
                    postponedVisit.PostponedDate = (DateTime)state.date;
                    _postponedVisit.CreatePostponedVisit(postponedVisit);
                }

                updateVisit.Notes = $"تأجيل التاريخ ل {(DateTime)state.date}";
            }

            if (!_visitRepository.UpdateVisit(updateVisit))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }


        [HttpPut("end/{visitId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult EndVisit(int visitId)
        {

            if (!_visitRepository.VisitExists(visitId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var endVisit = _visitRepository.GetVisit(visitId);
            
            endVisit.LeavingDate= DateTime.Now;

            if (!_visitRepository.UpdateVisit(endVisit))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }
    }
}
