using Microsoft.EntityFrameworkCore;

namespace Demo.Functions.EntityFrameworkCore
{
    public partial class RentalArrearsContext : DbContext
    {
        public RentalArrearsContext()
        {
        }

        public RentalArrearsContext(DbContextOptions<RentalArrearsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<RentArrangement> RentArrangements { get; set; }
        public virtual DbSet<RentPayment> RentPayments { get; set; }
        public virtual DbSet<RentalProperty> RentalProperties { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WeeklyCalendar> WeeklyCalendar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable(nameof(Location));

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Address2).HasMaxLength(100);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Postcode)
                    .IsRequired()
                    .HasMaxLength(4);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<RentArrangement>(entity =>
            {
                entity.ToTable(nameof(RentArrangement));

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ReferenceId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.WeeklyRent).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.ClientId);
                entity.Property(e => e.ManagerId);
                entity.Property(e => e.PropertyId);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.RentArrangementClients)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RentArrangement_ClientId_User");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.RentArrangementManagers)
                    .HasForeignKey(d => d.ManagerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RentArrangement_ManagerId_User");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.RentArrangements)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RentArrangement_RentalProperty");


            });

            modelBuilder.Entity<RentPayment>(entity =>
            {
                entity.ToTable("RentPayment");

                entity.Property(e => e.DueAmount).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.Paid).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.PaidDate).HasColumnType("datetime");

                entity.Property(e => e.ReferenceId);
            });

            modelBuilder.Entity<RentalProperty>(entity =>
            {
                entity.ToTable(nameof(RentalProperty));

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.RentalProperty)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RentalProperty_Location");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable(nameof(Role));

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasMany(x => x.Users)
                    .WithOne(y => y.Role);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(nameof(User));

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<WeeklyCalendar>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
