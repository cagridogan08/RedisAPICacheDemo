using APICache.Models;
using Microsoft.EntityFrameworkCore;

namespace APICache.Context;

public class ProductDbContext:DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> contextOptions):base(contextOptions)
    {
    }
    public  DbSet<Product>? Products { get; set; }

}