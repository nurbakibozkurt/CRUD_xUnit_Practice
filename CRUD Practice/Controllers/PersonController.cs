using AutoMapper;
using CRUD_Practice.Data;
using CRUD_Practice.Models;
using CRUD_Practice.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUD_Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IEntityRepository<Person> _entityRepository;
        private readonly IMapper _mapper;
    
        public PersonController(IEntityRepository<Person> entityRepository, IMapper mapper)
        {
            _entityRepository = entityRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllPersons()
        {
            try
            {
                return Ok(_entityRepository.GetAll().ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}")]
        public IActionResult GetPerson(int id)
        {
            try
            {
                var person = _entityRepository.GetById(id);
                if (person == null) { return NotFound("Person not found."); }
                return Ok(person);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult PostPerson(PersonDto personDto)
        {
            try
            {
                var person = _mapper.Map<Person>(personDto);
                _entityRepository.Add(person);
                return Ok(person);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id}")]
        public IActionResult PutPerson(int id, PersonDto personDto)
        {
            try
            {
                var personToUpdate = _entityRepository.GetById(id);
                if (personToUpdate == null)
                {
                    return NotFound("There is no such person to update.");
                }

                personToUpdate.Name = personDto.Name;
                personToUpdate.Surname = personDto.Surname;
                personToUpdate.Age = personDto.Age;

                _entityRepository.Update(personToUpdate);
                return Ok(personToUpdate);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePerson(int id)
        {
            try
            {
                var personToDelete = _entityRepository.GetById(id);
                if (personToDelete == null)
                {
                    return NotFound("There is no such person to delete");
                }

                _entityRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
