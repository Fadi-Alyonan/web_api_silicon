using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<SubscribEntity> Subscribes { get; set; }
}
    

