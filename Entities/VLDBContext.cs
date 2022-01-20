using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Entities
{
    /// <summary>
    /// The class that Manage teh DB
    /// </summary>
    public class VLDBContext : DbContext
    {
        public VLDBContext()
        {

        }
        public VLDBContext(DbContextOptions<VLDBContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }



        /// <summary>
        /// In this action seeding the DB
        /// </summary>
        /// <param name="modelBuilder">The Model Builder</param>

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Ernest Hemingway", Nationality = "Estadounidense", DateOfBirth = new DateTime(1899, 6, 21) },
                new Author { Id = 2, Name = "Edgar Allan Poe", Nationality = "Estadounidense", DateOfBirth = new DateTime(1809, 1, 19) }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = Guid.NewGuid(), Name = "User1", Email = "user1@gmail.com", CreatedAt = DateTime.UtcNow, ImageURL = "http://user1Image.com/image.jpg" },
                new User { Id = Guid.NewGuid(), Name = "User2", Email = "user2@gmail.com", CreatedAt = DateTime.UtcNow },
                new User { Id = Guid.NewGuid(), Name = "User3", Email = "user3@gmail.com", CreatedAt = DateTime.UtcNow, ImageURL = "http://user3Image.com/image.jpg" },
                new User { Id = Guid.NewGuid(), Name = "User4", Email = "user4@gmail.com", CreatedAt = DateTime.UtcNow, ImageURL = "http://user4Image.com/image.jpg" },
                new User { Id = Guid.NewGuid(), Name = "User5", Email = "user5@gmail.com", CreatedAt = DateTime.UtcNow }
            );

        }

    }
}
