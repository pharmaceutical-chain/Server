using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>().HasMany(t => t.SendTransfers)
                .WithOne(transfer => transfer.From)
                .HasForeignKey(transfer => transfer.FromId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Tenant>().HasMany(t => t.ReceiveTransfers)
                .WithOne(transfer => transfer.To)
                .HasForeignKey(transfer => transfer.ToId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicineBatch>().HasMany(b => b.MedicineBatchTransfers)
                .WithOne(t => t.MedicineBatch)
                .HasForeignKey(t => t.MedicineBatchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Medicine>().HasMany(m => m.MedicineBatches)
                .WithOne(b => b.Medicine).
                HasForeignKey(b => b.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineBatch> MedicineBatches { get; set; }
        public DbSet<MedicineBatchTransfer> MedicineBatchTransfers { get; set; }
    }
}
