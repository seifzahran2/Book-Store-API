using BookAPIProject.Controllres.Dtos;
using BookAPIProject.Models;
using BookAPIProject.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookAPIProject.Controllres
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : Controller
    {
        private readonly ICountryRepository _countryRepositry;
        public CountriesController(ICountryRepository countryRepositry)
        {
            _countryRepositry = countryRepositry;
        }
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        public IActionResult GetCountries()
        {
            var countries = _countryRepositry.GetCountries().ToList();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countriesDto = new List<CountryDto>();
            foreach (var country in countries)
            {
                countriesDto.Add(new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }
            return Ok(countriesDto);
        }

        [HttpGet("{countryId}",Name = "GetCountry")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountry(int countryId)
        {
            if(!_countryRepositry.CountryExist(countryId))
            {
                return NotFound();
            }
            var country = _countryRepositry.GetCountry(countryId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };
            
            return Ok(countryDto);
        }



        [HttpGet("Author/{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountryOfAuthor(int authorId)
        {
            
            var country = _countryRepositry.GetCountryOfAuthor(authorId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };

            return Ok(countryDto);
        }

        [HttpGet("{countryId}/Author")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        public IActionResult GetAuthorsFromCountry (int countryId)
        {
            if (!_countryRepositry.CountryExist(countryId))
            {
                return NotFound();
            }
            var authors = _countryRepositry.GetAuthorsFromContry(countryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var authorDto = new List<AuthorDto>();
            foreach (var author in authors)
            {
                authorDto.Add(new AuthorDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }
            return Ok(authorDto);
        }

        [HttpPost]
            public IActionResult CreateCountry([FromBody]Country countrycreate)
        {
            if(countrycreate == null)
                return BadRequest(ModelState);
            var country = _countryRepositry.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countrycreate.Name.Trim().ToUpper())
                .FirstOrDefault();
            if(country != null)
            {
                ModelState.AddModelError("",$"Country {countrycreate.Name} already exist");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!_countryRepositry.Add(countrycreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {countrycreate.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCountry",new {countryId = countrycreate.Id},countrycreate);
        }
        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateCountry(int CountryId,[FromBody] Country countryUpdate)
        {
             if(countryUpdate == null)
                return BadRequest(ModelState);

             if(CountryId !=countryUpdate.Id)
                return BadRequest(ModelState);

             if(!_countryRepositry.CountryExist(CountryId))
                return NotFound();
            if (_countryRepositry.IDuplicateCountryName(CountryId, countryUpdate.Name))
            {
                ModelState.AddModelError("", $"Country {countryUpdate.Name} already exist");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_countryRepositry.Update(countryUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating {countryUpdate.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult DeleteCountry(int CountryId)
        {
            if (!_countryRepositry.CountryExist(CountryId))
                return NotFound();
            var coutryToDelete = _countryRepositry.GetCountry(CountryId);
            if(_countryRepositry.GetAuthorsFromContry(CountryId).Count > 0)
            {
                ModelState.AddModelError("", $"Country {coutryToDelete.Name}"+"can't be deleted because it is used by at least one auther");
                return StatusCode(409,ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepositry.Delete(coutryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {coutryToDelete.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
