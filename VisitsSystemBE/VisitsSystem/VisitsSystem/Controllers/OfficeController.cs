using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VisitsSystem.Dto;
using VisitsSystem.Interfaces;
using VisitsSystem.Models;
using VisitsSystem.Repository;

namespace VisitsSystem.Controllers
{
    [Route("api/[controller]")]
    public class OfficeController : Controller
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public OfficeController(IOfficeRepository officeRepository,
            IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
            _officeRepository = officeRepository;
            _configuration = configuration;
        }
        [HttpGet("{id}")]
        public IActionResult GetOffice(int id)
        {
            Office office = _officeRepository.GetOffice(id);
            if (office==null)
                return NotFound();

            return Ok(_mapper.Map<OfficeDto>(office));
        }
        [HttpGet("[action]")]
        public IActionResult GetAllOffices()
        {
            List<Office> offices = _officeRepository.GetAllOffices();
            if (offices == null)
                return NotFound();

            return Ok(_mapper.Map<List<OfficeDto>>(offices));
        }
        [HttpGet("GetOfficeUsers/{id}")]
        public IActionResult GetOfficeUsers(int id)
        {
            List<User> users = _officeRepository.GetOfficeusers(id);
            if (users == null)
                return NotFound();

            return Ok(_mapper.Map<List<UserDto>>(users));
        }
        [HttpPut("UpdateOffice/{id}")]
        public IActionResult UpdateOffice(int id,OfficeDto office)
        {
            if (_officeRepository.Exist(id) == null)
            {
                return BadRequest("invalid id");
            }
            office.OfficeId = id;
            bool edit = _officeRepository.EditOffice(_mapper.Map<Office>(office));
            if (!edit)
                return BadRequest("update faild");

            return Ok(office);
        }
        [HttpPost]
        public IActionResult AddOffice([FromBody] OfficeDto office)
        {
            if (_officeRepository.Exist(office.OfficeName))
            {
                return BadRequest(ModelState);
            }
            
            if (!_officeRepository.Add(_mapper.Map < Office >( office)))
            {
                ModelState.AddModelError("", "حدث خطأ في إضافة المكتب ");
                return StatusCode(500, ModelState);
            }
            return Ok(office);
        }

        [HttpDelete]
        public IActionResult DeleteOffice(int id)
        {
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            
            var office = _officeRepository.GetOffice(id);

            if (!_officeRepository.Delete(office))
            {
                ModelState.AddModelError("", "حدث خطأ في حذف المكتب ");
                return StatusCode(500, ModelState);
            }
            return Ok(office);
        }
    }
}
