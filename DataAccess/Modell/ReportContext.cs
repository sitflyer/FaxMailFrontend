using System;
using System.Collections.Generic;
using DataAccessDLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessDLL.Modell;

public partial class ReportContext : DbContext, IReportContext
{
	private readonly string _connectionstring = "";
	public ReportContext(string connectionstring)
	{
		_connectionstring = connectionstring;
	}

	public ReportContext(DbContextOptions<ReportContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Frontendreport> Frontendreports { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseSqlServer(_connectionstring);

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Frontendreport>(entity =>
		{
			entity.ToTable("Frontendreport");

			entity.Property(e => e.Boid)
				.HasMaxLength(50)
				.IsFixedLength()
				.HasColumnName("BOID");
			entity.Property(e => e.Bpnr)
				.HasMaxLength(50)
				.IsFixedLength()
				.HasColumnName("BPNR");
			entity.Property(e => e.Btnr)
				.HasMaxLength(50)
				.IsFixedLength()
				.HasColumnName("BTNR");
			entity.Property(e => e.Dokumentenklasse)
				.HasMaxLength(50)
				.IsFixedLength();
			entity.Property(e => e.Dokumenttyp)
				.HasMaxLength(4)
				.IsFixedLength();
			entity.Property(e => e.EingereichtAm).HasColumnType("datetime");
			entity.Property(e => e.Filesize)
				.HasMaxLength(10)
				.IsFixedLength();
			entity.Property(e => e.FinalerName)
				.HasMaxLength(50)
				.IsFixedLength();
			entity.Property(e => e.Kanalart)
				.HasMaxLength(10)
				.IsFixedLength();
			entity.Property(e => e.Kvnr)
				.HasMaxLength(50)
				.IsFixedLength()
				.HasColumnName("KVNR");
			entity.Property(e => e.Originalname)
				.HasMaxLength(50)
				.IsFixedLength();
			entity.Property(e => e.Produktgruppe)
				.HasMaxLength(50)
				.IsFixedLength();
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}