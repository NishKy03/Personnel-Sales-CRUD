namespace PersonnelSalesManagement.API.Models.Domain
{
    public class Sale
    {
        public int Id { get; set; }
        public DateOnly Report_Date { get; set; }
        public decimal Sales_Amount { get; set; }


        public int Personnel_Id { get; set; } 
        public Personnel? Personnel { get; set; }
    }
}
