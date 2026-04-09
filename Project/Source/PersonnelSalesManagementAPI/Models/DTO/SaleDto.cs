using PersonnelSalesManagement.API.Models.Domain;

namespace PersonnelSalesManagement.API.Models.DTO
{
    public class SaleDto
    {
        public int Id { get; set; }
        public DateOnly Report_Date { get; set; }
        public decimal Sales_Amount { get; set; }
        public int Personnel_Id { get; set; }
    }
}
