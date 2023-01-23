using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TodoApp.Api.DB;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(args[0]);
        
        return new AppDbContext(optionsBuilder.Options);
    }
}
//"Server=localhost;Database=ToDoAppDB;User Id=sa;Password=Anabec3451.;Encrypt=False"