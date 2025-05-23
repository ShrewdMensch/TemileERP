using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Personnel>(b =>
            {
                b.HasKey(p => p.Id);
                b.Property(p => p.Id).ValueGeneratedNever();
            });

            builder.Entity<Arrear>()
            .HasOne(a => a.CorrectivePayroll)
            .WithMany(p => p.Arrears)
            .HasForeignKey(a => a.CorrectivePayrollId)
            .OnDelete(DeleteBehavior.Restrict);

        }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Deduction> Deductions { get; set; }
        public DbSet<Vessel> Vessels { get; set; }
        public DbSet<Allowance> Allowances { get; set; }
        public DbSet<SpecificDeduction> SpecificDeductions { get; set; }
        public DbSet<DeductionDetail> DeductionDetails { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<Arrear> Arrears { get; set; }
        public DbSet<EmailSentToBankLog> EmailSentToBankLogs { get; set; }
        public DbSet<CompanyInformation> CompanyInformation { get; set; }
    }
}
