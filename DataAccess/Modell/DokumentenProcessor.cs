using DataAccessDLL.Interfaces;
using System;
using System.Collections.Generic;

namespace DataAccessDLL.Modell;

public partial class DokumentenProcessor : IDokumentenProcessor
{
	public string Kategorie { get; set; } = null!;

	public string Unterkategorie { get; set; } = null!;

	public string Dokumenttyp { get; set; } = null!;

	public string Dokumentart { get; set; } = null!;

	public string Dokumentklasse { get; set; } = null!;

	public byte Kanalart { get; set; }

	public byte Ob { get; set; }

	public byte OtherOb { get; set; }

	public byte Mailbody { get; set; }

	public virtual ICollection<Favoriten> Favoritens { get; set; } = new List<Favoriten>();
}
