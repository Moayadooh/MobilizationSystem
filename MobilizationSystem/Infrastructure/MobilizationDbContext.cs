using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MobilizationSystem.Domain;
using System.Collections.Generic;

namespace MobilizationSystem.Infrastructure
{
    public class MobilizationDbContext : DbContext // IdentityDbContext<IdentityUser> // if using Identity
    {
        public MobilizationDbContext(DbContextOptions<MobilizationDbContext> options) : base(options) { }

        public DbSet<Person> Persons => Set<Person>();
        public DbSet<Resource> Resources => Set<Resource>();
        public DbSet<MobilizationRequest> MobilizationRequests => Set<MobilizationRequest>();
        public DbSet<MobilizationRequestPerson> MobilizationRequestPersons => Set<MobilizationRequestPerson>();
        public DbSet<MobilizationRequestResource> MobilizationRequestResources => Set<MobilizationRequestResource>();
        public DbSet<Approval> Approvals => Set<Approval>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MobilizationRequestPerson>()
                .HasOne(mrp => mrp.MobilizationRequest)
                .WithMany(m => m.MobilizationRequestPersons)
                .HasForeignKey(mrp => mrp.MobilizationRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MobilizationRequestPerson>()
                .HasOne(mrp => mrp.Person)
                .WithMany(p => p.MobilizationRequestPersons)
                .HasForeignKey(mrp => mrp.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MobilizationRequestResource>()
                .HasOne(mrr => mrr.MobilizationRequest)
                .WithMany(m => m.MobilizationRequestResources)
                .HasForeignKey(mrr => mrr.MobilizationRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MobilizationRequestResource>()
                .HasOne(mrr => mrr.Resource)
                .WithMany(r => r.MobilizationRequestResources)
                .HasForeignKey(mrr => mrr.ResourceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Optional: configure Approval relationships
            modelBuilder.Entity<Approval>()
                .HasOne(a => a.MobilizationRequest)
                .WithMany(m => m.Approvals)
                .HasForeignKey(a => a.MobilizationRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: indexes, constraints...
        }
    }
}
