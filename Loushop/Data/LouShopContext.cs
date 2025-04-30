using Loushop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loushop.Data
{
    public class LouShopContext : DbContext
    {
        public LouShopContext(DbContextOptions<LouShopContext> options) : base(options)
        {

        }

        public DbSet<Category> categories { get; set; }
        public DbSet<CategoryToProduct> CategoryToProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>().HasKey(o => o.DetailId); 

            modelBuilder.Entity<CategoryToProduct>()
                .HasKey(t => new { t.ProductId, t.CategoryId });

            modelBuilder.Entity<Users>().HasKey(u => u.UserId);

         









        #region seed Data Category
        modelBuilder.Entity<Category>().HasData(new Category()
            {
                Id = 1,
                Name = "لباس ورزشی",
                Description = "گروه لباس ورزشی"
            },


            new Category()
            {
                Id = 2,
                Name = "ساعت مچی",
                Description = "گروه ساعت مچی"
            },
            new Category()
            {
                Id = 3,
                Name = "کالای دیجیتال",
                Description = "گروه کالای دیجیتال"
            },

            new Category()
            {
                Id = 4,
                Name = "لوازم منزل",
                Description = "گروه لوازم منزل"
            }
                );


            modelBuilder.Entity<Item>().HasData(
                new Item()
                {
                    Id = 1,
                    Price = 850.0M,
                    QuantityInStocke = 5
                },

                 new Item()
                 {
                     Id = 2,
                     Price = 321.0M,
                     QuantityInStocke = 7
                 },

                  new Item()
                  {
                      Id = 3,
                      Price = 2500,
                      QuantityInStocke = 2
                  });



            modelBuilder.Entity<Product>().HasData(
              new Product()
              {
                  Id = 1,
                  ItemId = 1,
                  Name = "شامپو گلرنگ",
                  Description = "یکی از  محصولات شرکت گلرنگ"
              },
               new Product()
               {
                   Id = 2,
                   ItemId = 2,
                   Name = "گوشی j5",
                   Description = "یکی از محصولات شرکت سامسونگ"
               },
                new Product()
                {
                    Id = 3,
                    ItemId = 3,
                    Name = "دمپایی ابری",
                    Description = "یکی از محصولات شرکت نیکتا"
                });

            modelBuilder.Entity<CategoryToProduct>().HasData(
                new CategoryToProduct() { CategoryId = 1, ProductId = 1 },
                new CategoryToProduct() { CategoryId = 2, ProductId = 1 },
                new CategoryToProduct() { CategoryId = 3, ProductId = 1 },
                new CategoryToProduct() { CategoryId = 4, ProductId = 1 },
                new CategoryToProduct() { CategoryId = 1, ProductId = 2 },
                new CategoryToProduct() { CategoryId = 2, ProductId = 2 },
                new CategoryToProduct() { CategoryId = 3, ProductId = 2 },
                new CategoryToProduct() { CategoryId = 4, ProductId = 2 },
                new CategoryToProduct() { CategoryId = 1, ProductId = 3 },
                new CategoryToProduct() { CategoryId = 2, ProductId = 3 },
                new CategoryToProduct() { CategoryId = 3, ProductId = 3 },
                new CategoryToProduct() { CategoryId = 4, ProductId = 3 }



                );
            #endregion
            base.OnModelCreating(modelBuilder);
        }

        //internal int CategoryToProducts(Func<object, bool> p)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
