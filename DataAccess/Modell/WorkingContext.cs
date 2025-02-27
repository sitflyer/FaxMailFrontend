using DataAccessDLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessDLL.Modell;

public partial class WorkingContext : DbContext, IWorkingContext
{
	private string _connectionstring = "";
	public WorkingContext(string connectionstring)
	{
		_connectionstring = connectionstring;
	}

	public WorkingContext(DbContextOptions<WorkingContext> options)
		: base(options)
	{
	}

	public virtual DbSet<DokumentenProcessor> DokumentenProcessors { get; set; }

	public virtual DbSet<Favoriten> Favoritens { get; set; }

	public virtual DbSet<Nutzer> Nutzers { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseSqlServer(_connectionstring);

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DokumentenProcessor>(entity =>
		{
			entity.HasKey(e => e.Dokumentklasse);

			entity.ToTable("DokumentenProcessor");

			entity.Property(e => e.Dokumentklasse).HasMaxLength(50);
			entity.Property(e => e.Dokumentart).HasMaxLength(50);
			entity.Property(e => e.Dokumenttyp).HasMaxLength(50);
			entity.Property(e => e.Kategorie).HasMaxLength(50);
			entity.Property(e => e.Ob).HasColumnName("OB");
			entity.Property(e => e.OtherOb).HasColumnName("OtherOB");
			entity.Property(e => e.Unterkategorie).HasMaxLength(50);
		});

		modelBuilder.Entity<Favoriten>(entity =>
		{
			entity.ToTable("Favoriten");

			entity.Property(e => e.Id).HasColumnName("ID");
			entity.Property(e => e.Dokumentenklasse).HasMaxLength(50);
			entity.Property(e => e.NutzerId).HasColumnName("NutzerID");

			entity.HasOne(d => d.DokumentenklasseNavigation).WithMany(p => p.Favoritens)
				.HasForeignKey(d => d.Dokumentenklasse)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Favoriten_DokumentenProcessor");

			entity.HasOne(d => d.Nutzer).WithMany(p => p.Favoritens)
				.HasForeignKey(d => d.NutzerId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Favoriten_Nutzer");
		});

		modelBuilder.Entity<Nutzer>(entity =>
		{
			entity.ToTable("Nutzer");

			entity.Property(e => e.Id).HasColumnName("ID");
			entity.Property(e => e.Email)
				.HasMaxLength(150)
				.HasColumnName("EMail");
			entity.Property(e => e.Nachname).HasMaxLength(50);
			entity.Property(e => e.Telefonnummer).HasMaxLength(50);
			entity.Property(e => e.Vorname).HasMaxLength(50);
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
