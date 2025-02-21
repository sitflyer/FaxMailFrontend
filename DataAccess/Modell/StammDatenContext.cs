using System;
using System.Collections.Generic;
using DataAccessDLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessDLL.Modell;

public partial class StammDatenContext : DbContext, IStammDatenContext
{
	private string _connectionstring = "";
	public StammDatenContext(string connectionstring)
	{
		_connectionstring = connectionstring;
	}

	public StammDatenContext(DbContextOptions<StammDatenContext> options)
		: base(options)
	{
	}

	public virtual DbSet<StammdatenVersicherte> StammdatenVersichertes { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseSqlServer(_connectionstring);

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<StammdatenVersicherte>(entity =>
		{
			entity
				.HasNoKey()
				.ToTable("STAMMDATEN_VERSICHERTE");

			entity.Property(e => e.Blocklist)
				.HasMaxLength(1)
				.HasColumnName("BLOCKLIST");
			entity.Property(e => e.Bpnr)
				.HasMaxLength(10)
				.HasColumnName("BPNR");
			entity.Property(e => e.Geburtsdatum)
				.HasMaxLength(10)
				.HasColumnName("GEBURTSDATUM");
			entity.Property(e => e.Geschlecht)
				.HasMaxLength(20)
				.HasColumnName("GESCHLECHT");
			entity.Property(e => e.Kvnr10)
				.HasMaxLength(30)
				.HasColumnName("KVNR10");
			entity.Property(e => e.Kvnr9)
				.HasMaxLength(30)
				.HasColumnName("KVNR9");
			entity.Property(e => e.KzMitarbeiter)
				.HasMaxLength(1)
				.HasColumnName("KZ_MITARBEITER");
			entity.Property(e => e.Name)
				.HasMaxLength(40)
				.HasColumnName("NAME");
			entity.Property(e => e.Ort)
				.HasMaxLength(60)
				.HasColumnName("ORT");
			entity.Property(e => e.Plz)
				.HasMaxLength(10)
				.HasColumnName("PLZ");
			entity.Property(e => e.Rvnr)
				.HasMaxLength(12)
				.HasColumnName("RVNR");
			entity.Property(e => e.Strasse)
				.HasMaxLength(60)
				.HasColumnName("STRASSE");
			entity.Property(e => e.Verarbdatum)
				.HasMaxLength(20)
				.HasColumnName("VERARBDATUM");
			entity.Property(e => e.Vorname)
				.HasMaxLength(40)
				.HasColumnName("VORNAME");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
