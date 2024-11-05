using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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


    public class VisitorsController : Controller
    {
        private readonly IVisitorsRepository _visitorRepository;
        private readonly IMapper _mapper;
        public VisitorsController(IVisitorsRepository visitorRepository, IMapper mapper)
        {
            _visitorRepository = visitorRepository;
            _mapper = mapper;
          
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Visitor>))]
        public IActionResult GetVisitors()
        {
            var visitors = _visitorRepository.GetVisitors();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(visitors);

        }

        [HttpGet("{id}")]
        public IActionResult GetVisitor(int id)
        {
            if (!_visitorRepository.VisitorExists(id))
                return NotFound();
            var Visitor = _visitorRepository.GetVisitor(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(Visitor);
        }

        [HttpPost]
        public IActionResult CreateVisitor([FromBody] VisitorsTblDto Visitor)
        {
            if (Visitor == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
          var  visitor = _mapper.Map<Visitor>(Visitor);
         //   string s =
          //  visitor.Rank = (int)Enum.Parse(typeof(Rank),Visitor.Rank);

//            visitor.Rank = _visitorRepository.GetEnumIndex(Visitor.Rank);
            if (!_visitorRepository.CreateVisitor(visitor))
            {
                ModelState.AddModelError("", "حدث خطأ في إضافة الزائر ");
                return StatusCode(500, ModelState);
            }

            return Ok(visitor);
        }

        [HttpPut("{VisitorID}")]
        public IActionResult UpdateUser(int VisitorID, [FromBody] VisitorsTblDto Visitor)
        {
            if (Visitor == null)
            {
                return BadRequest(ModelState);
            }
            var visitor = _mapper.Map<Visitor>(Visitor);

          visitor.VisitorId= VisitorID;
            //if (id != visitor.VisitorId)
            //    return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_visitorRepository.VisitorExists(VisitorID))
                return NotFound();

            if (!_visitorRepository.UpdateVisitor(visitor))
            {
                ModelState.AddModelError("", "حدث خطأ في تحديث بيانات الزائر ");
                return StatusCode(500, ModelState);
            }
            return Ok(visitor);
        }

        [HttpDelete]
        public IActionResult DeleteVisitor(int VisitorID)
        {
            if (VisitorID == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var Visitor = _visitorRepository.GetVisitor(VisitorID);
            var visitor = _mapper.Map<Visitor>(Visitor);

            if (!_visitorRepository.DeleteVisitor(visitor))
            {
                ModelState.AddModelError("", "حدث خطأ في حذف المستخدم ");
                return StatusCode(500, ModelState);
            }
            return Ok(visitor);
        }

        [HttpGet("VisitorsSearch")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Visitor>))]
        public IActionResult VisitorsSearch([FromQuery] int? Id, [FromQuery] String? Name, [FromQuery] string? Rank, [FromQuery] String? JobTitle)
        {
        
            
            var Visitors = _mapper.Map<List<Visitor>>(_visitorRepository.VisitorsSearch(Id, Name, Rank, JobTitle));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Visitors);
        }
    }
}
