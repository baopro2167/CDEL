
using Microsoft.EntityFrameworkCore;



namespace Model
{
    public class BloodlineDbContext : DbContext
    {
        public BloodlineDbContext(DbContextOptions<BloodlineDbContext> options) : base(options)
        { }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(**)
        //    // ...
        //}


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Kit> Kits { get; set; }
        public DbSet<KitDelivery> KitDeliveries { get; set; }
        public DbSet<ExaminationRequest> ExaminationRequests { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<KitDelivery> KitDeliverys { get; set; }
        public DbSet<ExaminationResult> ExaminationResults { get; set; }
        
        public DbSet<Service> ServiceBs { get; set; }

        public DbSet<SampleMethod> SampleMethods { get; set; }
        public DbSet<ServiceSampleMethod> ServiceSampleMethods { get; set; }
        public DbSet<Staff> Staffs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Role>().HasData(
         new Role
         {
             Id = 1,
             Name = "Admin",
             CreatedAt = new DateTime(2025, 1, 1),
             UpdatedAt = new DateTime(2025, 1, 1)
         },
        new Role
        {
            Id = 2,
            Name = "User",
            CreatedAt = new DateTime(2025, 1, 1),
            UpdatedAt = new DateTime(2025, 1, 1)
        },
         new Role
         {
             Id = 3,
             Name = "Staff",
             CreatedAt = new DateTime(2025, 1, 1),
             UpdatedAt = new DateTime(2025, 1, 1)
         },
          new Role
          {
              Id = 4,
              Name = "Manager",
              CreatedAt = new DateTime(2025, 1, 1),
              UpdatedAt = new DateTime(2025, 1, 1)
          }

    );

            modelBuilder.Entity<User>().HasData(
    new User
    {
        Id = 1,
        Name = "System Admin",
        Email = "admin@bloodline.com",
        Password = "AQAAAAIAAYagAAAAEMNhg1Bx5QAKCmZZy+N6T/C05Tc6+0vwWQ17vibbv68jKjtuzfp+bMA2z15j1ySUfw==", // INSECURE - PLAIN TEXT FOR TESTING ONLY
        Phone = "1234567890",
        Address = "123 Admin Street",
        RoleId = 1, // Admin role ID
        CreatedAt = new DateTime(2023, 1, 1),
        UpdatedAt = new DateTime(2023, 1, 1)
    },
    new User
    {
        Id = 2,
        Name = "John Doe",
        Email = "john.doe@example.com",
        Password = "AQAAAAIAAYagAAAAEGdT4Bxik+YTE6kNF/L7RQcysjRUIwSdcojiAWNvJtK7iBKIVUCYuxvXhNUqCNvGiQ==", // INSECURE - PLAIN TEXT FOR TESTING ONLY
        Phone = "1987654321",
        Address = "456 User Avenue",
        RoleId = 2, // User role ID
        CreatedAt = new DateTime(2023, 1, 1),
        UpdatedAt = new DateTime(2023, 1, 1)
    }
);

            // Cấu hình độ chính xác và tỷ lệ cho Price
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasPrecision(18, 2); // 18 chữ số, 2 chữ số thập phân


            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<ExaminationRequest>()
                .HasOne(er => er.User)
                .WithMany(u => u.ExaminationRequests)
                .HasForeignKey(er => er.UserId);

          

            modelBuilder.Entity<ExaminationRequest>()
                .HasOne(er => er.Service)
                .WithMany()
                .HasForeignKey(er => er.ServiceId);

            modelBuilder.Entity<ExaminationRequest>()
                .HasOne(er => er.SampleMethod)
                .WithMany()
                .HasForeignKey(er => er.SampleMethodId);

            modelBuilder.Entity<KitDelivery>()
.HasOne(kd => kd.Kit)
.WithMany(k => k.KitDeliveries)
.HasForeignKey(kd => kd.KitId);

            modelBuilder.Entity<KitDelivery>()
                .HasOne(kd => kd.Request)
                .WithMany(er => er.KitDeliveries)
                .HasForeignKey(kd => kd.RequestId);

            modelBuilder.Entity<ExaminationResult>()
                    .HasOne(er => er.Request)
                    .WithMany(e => e.ExaminationResults)
                    .HasForeignKey(er => er.RequestId);



            modelBuilder.Entity<KitDelivery>()
                .HasOne(kd => kd.Kit)
                .WithMany(k => k.KitDeliveries)
                .HasForeignKey(kd => kd.KitId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ServiceSampleMethod>()
                .HasKey(ssm => new { ssm.ServiceId, ssm.SampleMethodId });

            modelBuilder.Entity<ServiceSampleMethod>()
                .HasOne(ssm => ssm.Service)
                .WithMany(s => s.ServiceSampleMethods)
                .HasForeignKey(ssm => ssm.ServiceId);

            modelBuilder.Entity<ServiceSampleMethod>()
                .HasOne(ssm => ssm.SampleMethod)
                .WithMany(sm => sm.ServiceSampleMethods)
                .HasForeignKey(ssm => ssm.SampleMethodId);
        }
    }

}
