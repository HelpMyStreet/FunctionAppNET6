using FunctionAppNET6.Repo.EntityFramework.Entities;
using FunctionAppNET6.Repo.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FunctionAppNET6.Repo
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            if (Database.IsSqlServer())
            {
                SqlConnection conn = (SqlConnection)Database.GetDbConnection();
                conn.AddAzureToken();
            }
        }

        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<Request> Request { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("Job", "Request");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Details).IsUnicode(false);

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.NotBeforeDate).HasColumnType("datetime");

                entity.Property(e => e.SupportActivityId).HasColumnName("SupportActivityID");

                entity.Property(e => e.VolunteerUserId).HasColumnName("VolunteerUserID");

                entity.Property(e => e.JobStatusId).HasColumnName("JobStatusID");

                entity.Property(e => e.DueDateTypeId).ValueGeneratedNever();

                entity.HasOne(d => d.NewRequest)
                   .WithMany(p => p.Job)
                   .HasForeignKey(d => d.RequestId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_NewRequest_NewRequestID");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("Request", "Request");

                entity.HasIndex(e => e.Guid)
                    .HasDatabaseName("UC_Guid")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.DateRequested)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.OtherDetails).IsUnicode(false);
                entity.Property(e => e.OrganisationName).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.PersonIdRecipient).HasColumnName("PersonID_Recipient");

                entity.Property(e => e.PersonIdRequester).HasColumnName("PersonID_Requester");

                entity.Property(e => e.PostCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SpecialCommunicationNeeds).IsUnicode(false);

                entity.Property(e => e.Language)
                    .HasMaxLength(20);

            });
        }
    }
}