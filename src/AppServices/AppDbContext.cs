using Domain.Entities;
using Domain.Framework;
using Microsoft.EntityFrameworkCore;
using System;

namespace AppServices
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable(TableNameConstants.Products);
        }
    }
}
