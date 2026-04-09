namespace PersonnelSalesManagement.API.Models.Domain
{
    public class Personnel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Phone { get; set; } = string.Empty;

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
            
    }
}
