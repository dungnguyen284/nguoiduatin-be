using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NDT.BusinessModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDT.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet properties for your entities go here
        public DbSet<News> News { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<FrontPageNews> FrontPageNews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure News entity
            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Link).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.Property(e => e.Source).HasMaxLength(100);

                // Configure relationships
                entity.HasOne(e => e.Author)
                    .WithMany(u => u.News)
                    .HasForeignKey(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Category)
                    .WithMany()
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Tags)
                    .WithMany(e => e.News)
                    .UsingEntity(j 
                        => j.ToTable("NewsTags"
                    ));
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // Configure Advertisement entity
            modelBuilder.Entity<Advertisement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            });

            // Configure Tags entity
            modelBuilder.Entity<Tags>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });
            modelBuilder.Entity<AppUser>(entity =>
                entity.HasOne(e => e.WatchList)
                    .WithOne(w => w.User)
                    .HasForeignKey<WatchList>(w => w.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
            
            );
            modelBuilder.Entity<WatchList>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(wl => wl.Stocks)
                    .WithMany(s => s.WatchLists)
                    .UsingEntity(j => j.ToTable("WatchListStocks"));
            });
            modelBuilder.Entity<FrontPageNews>()
                .HasKey(f => f.SlotNumber);

            modelBuilder.Entity<FrontPageNews>()
                .Property(f => f.SlotNumber)
                .HasConversion<int>()
                .IsRequired();

            modelBuilder.Entity<FrontPageNews>()
                .HasCheckConstraint("CK_FrontpageSlot_SlotNumber", "[SlotNumber] >= 1 AND [SlotNumber] <= 4");
            
            
        }
    }
}
