using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonnelSalesManagement.API.Data;
using PersonnelSalesManagement.API.Models.Domain;
using PersonnelSalesManagement.API.Models.DTO;

namespace PersonnelSalesManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly PersonnelSalesManagementDBContext dbContext;

        public SalesController(PersonnelSalesManagementDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetAllSales")]
        public async Task<IActionResult> GetAllSales()
        {
            var salesDomain = await dbContext.Sales.ToListAsync();

            var salesDto = new List<SaleDto>();
            foreach (var saleDomain in salesDomain)
            {
                salesDto.Add(new SaleDto()
                {
                    Id = saleDomain.Id,
                    Report_Date = saleDomain.Report_Date,
                    Sales_Amount = saleDomain.Sales_Amount,
                    Personnel_Id = saleDomain.Personnel_Id,
                });
            }

            return Ok(salesDto);
        }

        [HttpGet]
        [Route("GetByIdSales{id:int}")]
        public async Task<IActionResult> GetByIdSale([FromRoute] int id)
        {
            var saleDomainModel = await dbContext.Sales.FirstOrDefaultAsync(x => x.Id == id);

            if (saleDomainModel == null)
            {
                return NotFound();
            }
            else
            {
                var salesDto = new SaleDto
                {
                    Id = saleDomainModel.Id,
                    Report_Date = saleDomainModel.Report_Date,
                    Sales_Amount = saleDomainModel.Sales_Amount,
                    Personnel_Id = saleDomainModel.Personnel_Id,
                };

                return Ok(salesDto);
            }
        }

        [HttpGet]
        [Route("GetSalesByPersonnel/{id:int}")]
        public async Task<IActionResult> GetSalesByPersonnel([FromRoute] int id)
        {
            var saleDomainModel = await dbContext.Sales.Where(x => x.Personnel_Id == id).ToListAsync();
          
            var salesDto = saleDomainModel.Select(salesDomainModel => new SaleDto
            {
                Id = salesDomainModel.Id,
                Report_Date = salesDomainModel.Report_Date,
                Sales_Amount = salesDomainModel.Sales_Amount,
                Personnel_Id = salesDomainModel.Personnel_Id,
            }).ToList();

            return Ok(salesDto);
            
        }

        [HttpPost]
        [Route("CreateSales")]
        public async Task<IActionResult> CreateSale(AddSaleRequestDto addSaleRequestDto)
        {
            var isPersonnelExists = await dbContext.Personnels.AnyAsync(p => p.Id == addSaleRequestDto.Personnel_Id);

            if (!isPersonnelExists)
            {
                return BadRequest("Personnel ID does not exist.");
            }
            
            var salesDomain = new Sale
            {
                Report_Date = DateOnly.FromDateTime(DateTime.Today),
                Sales_Amount = addSaleRequestDto.Sales_Amount,
                Personnel_Id = addSaleRequestDto.Personnel_Id,
            };

            await dbContext.Sales.AddAsync(salesDomain);
            await dbContext.SaveChangesAsync();

            var saleDto = new SaleDto
            {
                Id = salesDomain.Id,
                Report_Date = salesDomain.Report_Date,
                Sales_Amount = salesDomain.Sales_Amount,
                Personnel_Id=salesDomain.Personnel_Id,
            };

            return CreatedAtAction(nameof(GetByIdSale), new { id = saleDto.Id }, saleDto);
        }

        [HttpDelete]
        [Route("DeleteSales/{id:int}")]
        public async Task<IActionResult> DeleteSales(int id)
        {
            var sale = await dbContext.Sales.FindAsync(id);

            if(sale == null)
            {
                return NotFound();
            }
            else
            {
                dbContext.Sales.Remove(sale);
                await dbContext.SaveChangesAsync();

                return Ok();
            }
        }
    }
}
