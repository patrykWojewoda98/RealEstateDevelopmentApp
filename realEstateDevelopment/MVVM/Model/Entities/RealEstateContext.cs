using Microsoft.EntityFrameworkCore;

namespace realEstateDevelopment.MVVM.Model.Entities
{
    public class RealEstateContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=Patryk\\SQLEXPRESS;Database=RealEstateDeveloperDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
