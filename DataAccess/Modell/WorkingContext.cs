using System;
using System.Collections.Generic;
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

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
