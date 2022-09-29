using ConfigurationLibrary.Classes;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Models;

namespace PizzaShop.Data
{
    public partial class PizzaContext : DbContext
    {
        private readonly bool _create;

        public PizzaContext()
        {
        }

        /// <summary>
        /// This overloaded constructor is used to setup for creating the database
        /// </summary>
        /// <param name="create"></param>
        public PizzaContext(bool create)
        {
            _create = create;
        }

        public PizzaContext(DbContextOptions<PizzaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!_create) return;
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer(ConfigurationHelper.ConnectionString());
            //}

            // If there are sporadic timeouts use the following
            //optionsBuilder.UseSqlServer(ConfigurationHelper.ConnectionString(), options =>
            //{
            //    options.EnableRetryOnFailure(
            //        maxRetryCount: 3,
            //        maxRetryDelay: TimeSpan.FromSeconds(10),
            //        errorNumbersToAdd: new List<int> { 4060 }); //additional error codes to treat as transient
            //});


            optionsBuilder.UseSqlServer(ConfigurationHelper.ConnectionString(), builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
