using FolderProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FolderProjectAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Folder> Folders { get; set; }
        public DbSet<FileItem> FileItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Folder - Subfolder relationship
            modelBuilder.Entity<Folder>()
                .HasOne(f => f.Parent)
                .WithMany(f => f.Subfolders)
                .HasForeignKey(f => f.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure FileItem - Folder relationship
            modelBuilder.Entity<FileItem>()
                .HasOne(f => f.Folder)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.FolderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: Configure unique index on Folder.Path
            modelBuilder.Entity<Folder>()
                .HasIndex(f => f.Path)
                .IsUnique();


        }
    }
}
