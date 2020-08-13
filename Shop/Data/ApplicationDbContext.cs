using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Data{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) {}
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartLine> CartLines { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            const string ROLE_ID = ADMIN_ID;
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ROLE_ID,
                Name = "Admin",
                NormalizedName = "Admin"
            });

            var hasher = new PasswordHasher<User>();
            builder.Entity<User>().HasData(new User
            {
                Id = ADMIN_ID,
                UserName = "Admin",
                NormalizedUserName = "Admin",
                Email = "some-admin-email@nonce.fake",
                NormalizedEmail = "some-admin-email@nonce.fake",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@1"),
                SecurityStamp = string.Empty
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });

            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasMany(cl => cl.CartLines)
                .WithOne(p => p.Product);



            builder.Entity<Category>()
                .HasIndex(i => i.Name)
                .IsUnique();
            
            builder.Entity<CartLine>()
                .HasOne(o => o.Order)
                .WithMany(cl => cl.CartLines)
                .IsRequired();

            builder.Entity<Order>()
                  .Property(o => o.StatusId)
                  .HasDefaultValue(1)
                  .IsRequired();


            builder.Entity<Order>().HasData(
                new Order {  Id = 1 , Name = "name1" , StatusId = 1 }
            );

            builder.Entity<OrderStatus>().HasData(
                new OrderStatus { Id = 1, Name = "InProgress" },
                new OrderStatus { Id = 2, Name = "Completed" },
                new OrderStatus { Id = 3, Name = "NotPaid" },
                new OrderStatus { Id = 4, Name = "Reservation" },
                new OrderStatus { Id = 5, Name = "Complaint" },
                new OrderStatus { Id = 6, Name = "Canceled" }
            );


            builder.Entity<CartLine>().HasData(
                new CartLine { 
                    Id = 1,
                    ProductId = 1,
                    OrderId = 1
                }
             );


            builder.Entity<Category>().HasData(
                   new Category { Id = 1, Name = "Laptops" },
                   new Category { Id = 2, Name = "Televisions" },
                   new Category { Id = 3, Name = "Desktops" },
                   new Category { Id = 4, Name = "Automation" },
                   new Category { Id = 5, Name = "Digital Cameras" },
                   new Category { Id = 6, Name = "Cell Phones" }
           );

            builder.Entity<Product>().HasData(

                  new Product { Id = 1,
                      Name = "Apple MacBook Air 13.3", 
                      CategoryId =1, 
                      Description= "The 13-inch MacBook Air features 8GB of memory, " +
                                   "a fifth-generation Intel Core processor, Thunderbolt 2, great built-in apps and all-day battery life.* It’s thin, light " +
                                   "and durable enough to take everywhere you go -- and powerful enough to do everything once you get there.",
                      Price = 1.199M 
                  },

                  new Product
                  {
                      Id = 2,
                      Name = "Lenovo 15.6\"",
                      CategoryId = 1,
                      Description = "The Lenovo 15.6 laptop offers portable computing packaged in a compact size." +
                      " It features a 2133 MHz DDR4 RAM and Intel Pentium Gold 5405Uprocessor which consume less power, reducing heat and extending battery life. " +
                      "This laptop offers great picture quality while viewing photos and videos along with excellent sound performance.",
                      Price = 549.99M
                  },
                 new Product
                 {
                     Id = 3,
                     Name = "Acer Nitro Gaming PC",
                     CategoryId = 3,
                     Description = "Powerful, lag-free gaming is easily experienced with the Acer Nitro gaming PC." +
                     " This compact design boasts a 2.9GHz Intel Core i5-9400F processor with 8GB RAM, and large 1TB HDD for storing all your digital files. " +
                     "The NVIDIA GeForce GTX 1050 graphics card with 2GB of dedicated memory offers smooth gaming and graphic-intensive tasking.",
                     Price = 999.99M
                 },
                 new Product
                 {
                     Id = 4,
                     Name = "Samsung Galaxy Note10+ 256GB ",
                     CategoryId = 6,
                     Description = "EAD SELLER'S STORE DESCRIPTION FOR MORE INFO. Open Box: Unused, " +
                     "10/10 condition product with full warranty still valid, only difference with Factory Fresh is open packaging.",
                     Price = 899.99M
                 }
            );

        }
    }
}
