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

	public virtual DbSet<GposListe> GposListes { get; set; }

	public virtual DbSet<LeistungserbringerLanr> LeistungserbringerLanrs { get; set; }

	public virtual DbSet<StammdatenFirmenkunde> StammdatenFirmenkundes { get; set; }

	public virtual DbSet<StammdatenVersicherte> StammdatenVersichertes { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseSqlServer(_connectionstring);

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<GposListe>(entity =>
		{
			entity
				.HasNoKey()
				.ToTable("GPOS_LISTE");

			entity.Property(e => e.BezeichnungGebührenposition)
				.HasMaxLength(100)
				.HasColumnName("BEZEICHNUNG_GEBÜHRENPOSITION");
			entity.Property(e => e.Id)
				.ValueGeneratedOnAdd()
				.HasColumnName("ID");
			entity.Property(e => e.Produktgruppe)
				.HasMaxLength(60)
				.HasColumnName("PRODUKTGRUPPE");
		});

		modelBuilder.Entity<LeistungserbringerLanr>(entity =>
		{
			entity
				.HasNoKey()
				.ToTable("LEISTUNGSERBRINGER_LANR");

			entity.Property(e => e.Arztnummer)
				.HasMaxLength(12)
				.HasColumnName("ARZTNUMMER");
			entity.Property(e => e.Bezeichnung)
				.HasMaxLength(30)
				.HasColumnName("BEZEICHNUNG");
			entity.Property(e => e.Bpnr)
				.HasMaxLength(10)
				.HasColumnName("BPNR");
			entity.Property(e => e.Hausnummer)
				.HasMaxLength(10)
				.HasColumnName("HAUSNUMMER");
			entity.Property(e => e.Nachname)
				.HasMaxLength(60)
				.HasColumnName("NACHNAME");
			entity.Property(e => e.Name1)
				.HasMaxLength(60)
				.HasColumnName("NAME1");
			entity.Property(e => e.Name2)
				.HasMaxLength(60)
				.HasColumnName("NAME2");
			entity.Property(e => e.Name3)
				.HasMaxLength(60)
				.HasColumnName("NAME3");
			entity.Property(e => e.Name4)
				.HasMaxLength(60)
				.HasColumnName("NAME4");
			entity.Property(e => e.Ort)
				.HasMaxLength(60)
				.HasColumnName("ORT");
			entity.Property(e => e.Plz)
				.HasMaxLength(10)
				.HasColumnName("PLZ");
			entity.Property(e => e.Record)
				.ValueGeneratedOnAdd()
				.HasColumnName("RECORD");
			entity.Property(e => e.Rolle)
				.HasMaxLength(6)
				.HasColumnName("ROLLE");
			entity.Property(e => e.Strasse)
				.HasMaxLength(60)
				.HasColumnName("STRASSE");
			entity.Property(e => e.Type)
				.HasMaxLength(6)
				.HasColumnName("TYPE");
			entity.Property(e => e.Verarbdatum)
				.HasMaxLength(20)
				.HasColumnName("VERARBDATUM");
			entity.Property(e => e.Vorname)
				.HasMaxLength(60)
				.HasColumnName("VORNAME");
		});

		modelBuilder.Entity<StammdatenFirmenkunde>(entity =>
		{
			entity
				.HasNoKey()
				.ToTable("STAMMDATEN_FIRMENKUNDE");

			entity.Property(e => e.Btnr)
				.HasMaxLength(20)
				.HasColumnName("BTNR");
			entity.Property(e => e.Firmenname)
				.HasMaxLength(180)
				.HasColumnName("FIRMENNAME");
			entity.Property(e => e.Hausnummer)
				.HasMaxLength(10)
				.HasColumnName("HAUSNUMMER");
			entity.Property(e => e.Identart)
				.HasMaxLength(4)
				.HasColumnName("IDENTART");
			entity.Property(e => e.Ort)
				.HasMaxLength(60)
				.HasColumnName("ORT");
			entity.Property(e => e.Plz)
				.HasMaxLength(10)
				.HasColumnName("PLZ");
			entity.Property(e => e.Strasse)
				.HasMaxLength(60)
				.HasColumnName("STRASSE");
			entity.Property(e => e.UniqueId)
				.ValueGeneratedOnAdd()
				.HasColumnName("UNIQUE_ID");
			entity.Property(e => e.Verarbdatum)
				.HasMaxLength(20)
				.HasColumnName("VERARBDATUM");
		});

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
