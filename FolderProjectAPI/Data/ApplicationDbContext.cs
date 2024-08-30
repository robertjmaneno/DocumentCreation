using FolderProjectAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FolderProjectAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Folder> Folders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder>()
                .HasOne(f => f.Parent)
                .WithMany(f => f.Children)
                .HasForeignKey(f => f.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
