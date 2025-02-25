using System;
using System.Collections.Generic;

namespace DataAccessDLL.Modell;

public partial class Favoriten : IFavoriten
{
	public long Id { get; set; }

	public long NutzerId { get; set; }

	public string Dokumentenklasse { get; set; } = null!;

	public virtual DokumentenProcessor DokumentenklasseNavigation { get; set; } = null!;

	public virtual Nutzer Nutzer { get; set; } = null!;
}
