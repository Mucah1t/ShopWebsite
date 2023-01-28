﻿using EntityLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EfCore
{
    public class ShopContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=localhost\\SQLEXPRESS;database=DbShopApp;integrated security=true");
		}
        protected override void OnModelCreating(ModelBuilder modelBuilder) //many to many relation
        {
            modelBuilder.Entity<ProductCategory>().HasKey(c=> new {c.CategoryId,c.ProductId});
        }
    }
}