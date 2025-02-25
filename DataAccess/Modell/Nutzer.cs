using DataAccessDLL.Interfaces;
using System;
using System.Collections.Generic;

namespace DataAccessDLL.Modell;

public partial class Nutzer : INutzer
{
	public long Id { get; set; }

	public string Vorname { get; set; } = null!;

	public string Nachname { get; set; } = null!;

	public string Telefonnummer { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string? Kennung { get; set; }

	public virtual ICollection<Favoriten> Favoritens { get; set; } = new List<Favoriten>();
}
