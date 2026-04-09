using Microsoft.EntityFrameworkCore;
using PersonnelSalesManagement.API.Models.Domain;

namespace PersonnelSalesManagement.API.Data
{
    public class PersonnelSalesManagementDBContext : DbContext
    {
        public PersonnelSalesManagementDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<Sale> Sales { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Personnel)
                .WithMany(p => p.Sales)
                .HasForeignKey(s => s.Personnel_Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
