using Microsoft.EntityFrameworkCore;
using System.Xml;
using WebForms.Models;

namespace WebForms.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Template> Templates { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Form> Forms { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Template>()
            .HasMany(t => t.Tags)
            .WithMany(t => t.Templates)
            .UsingEntity<Dictionary<string, object>>(
                "TemplateTag",
                j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                j => j.HasOne<Template>().WithMany().HasForeignKey("TemplateId")
            );

            modelBuilder.Entity<Template>()
            .HasOne<User>()
            .WithMany(u => u.CreatedTemplates)
            .HasForeignKey(t => t.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Form>()
            .HasOne<User>()
            .WithMany(u => u.AssignedForms)
            .HasForeignKey(f => f.AssignedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}