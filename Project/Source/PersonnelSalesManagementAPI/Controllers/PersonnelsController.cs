using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonnelSalesManagement.API.Data;
using PersonnelSalesManagement.API.Models.Domain;
using PersonnelSalesManagement.API.Models.DTO;

namespace PersonnelSalesManagement.API.Controllers
{
    // https://localhost:7122/api/personnels
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnelsController : ControllerBase
    {
        private readonly PersonnelSalesManagementDBContext dbContext;

        public PersonnelsController(PersonnelSalesManagementDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersonnels()
        {
            //GET data from the database - Domain Models
            var personnelsDomain = await dbContext.Personnels.ToListAsync();

            //Map Domain Models to DTOs
            var personnelsDto = new List<PersonnelDto>();
            foreach (var personnelDomain in personnelsDomain)
            {
                personnelsDto.Add(new PersonnelDto()
                {
                    Id = personnelDomain.Id,
                    Name = personnelDomain.Name,
                    Age = personnelDomain.Age,
                    Phone = personnelDomain.Phone
                });
            }
            //return DTOs to Clients
            return Ok(personnelsDto);
        }

        [HttpGet]
        [Route("GetPersonnelsBy{id:int}")]
        public async Task<IActionResult> GetByIdPersonnel([FromRoute] int id)
        {
            //var personnel = dbContext.Personnels.Find(id);

            //Get Personnel Domain Model from Database
            var personnelDomain = await dbContext.Personnels.FirstOrDefaultAsync(x => x.Id == id);

            if (personnelDomain == null)
            {
                return NotFound();
            }
            else
            {
                //Map Personnel Domain Model to Personnel DTO
                var personnelDto = new PersonnelDto
                {
                    Id = personnelDomain.Id,
                    Name = personnelDomain.Name,
                    Age = personnelDomain.Age,
                    Phone = personnelDomain.Phone
                };

                //Return Personnel DTO to Clients
                return Ok(personnelDto);
            }
        }

        [HttpPost]
        [Route("CreatePersonnels")]
        public async Task<IActionResult> CreatePersonnel([FromBody] AddPersonnelRequestDto addPersonnelRequestDto)
        {
            //Map DTO to Domain Model
            var personnelDomainModel = new Personnel
            {
                Name = addPersonnelRequestDto.Name,
                Age = addPersonnelRequestDto.Age,
                Phone = addPersonnelRequestDto.Phone
            };

            //Use Domain Model to Create Personnel
            await dbContext.Personnels.AddAsync(personnelDomainModel);
            await dbContext.SaveChangesAsync();

            //Map Domain Model back to DTO
            var personnelDto = new PersonnelDto
            {
                Id = personnelDomainModel.Id,
                Name = personnelDomainModel.Name,
                Age = personnelDomainModel.Age,
                Phone = personnelDomainModel.Phone
            };
            return CreatedAtAction(nameof(GetByIdPersonnel), new { id = personnelDto.Id }, personnelDto);
        }

        [HttpPut]
        [Route("UpdatePersonnels/{id:int}")]
        public async Task<IActionResult> UpdatePersonnel(int id, UpdatePersonnelDto updatePersonnelDto)
        {
            var personnelDomain = await dbContext.Personnels.FindAsync(id);

            if (personnelDomain == null)
            {
                return NotFound();
            }
            
            personnelDomain.Name = updatePersonnelDto.Name;
            personnelDomain.Age = updatePersonnelDto.Age;
            personnelDomain.Phone = updatePersonnelDto.Phone;

            await dbContext.SaveChangesAsync();

            var personnelDto = new PersonnelDto
            {
                Id = personnelDomain.Id,
                Name = personnelDomain.Name,
                Age = personnelDomain.Age,
                Phone = personnelDomain.Phone
            };

            return Ok(personnelDto);
            
        }

        [HttpDelete]
        [Route("DeletePersonnels/{id:int}")]
        public async Task<IActionResult> DeletePersonnel(int id)
        {
            var personnel = await dbContext.Personnels.FindAsync(id);

            if (personnel == null)
            {
                return NotFound();
            }
            else
            {
                dbContext.Personnels.Remove(personnel);
                await dbContext.SaveChangesAsync();

                return Ok();
            }
        }
    }
}
