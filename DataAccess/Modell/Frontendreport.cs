using DataAccessDLL.Interfaces;
using System;
using System.Collections.Generic;

namespace DataAccessDLL.Modell;

public partial class Frontendreport : IFrontendreport
{
	public long Id { get; set; }

	public string Nutzerkennung { get; set; } = null!;

	public DateTime EingereichtAm { get; set; }

	public string Dokumenttyp { get; set; } = null!;

	public string Originalname { get; set; } = null!;

	public string FinalerName { get; set; } = null!;

	public string? Kanalart { get; set; }

	public string? Dokumentenklasse { get; set; }

	public string? Kvnr { get; set; }

	public string? Bpnr { get; set; }

	public string? Btnr { get; set; }

	public string? Boid { get; set; }

	public string? Produktgruppe { get; set; }

	public string Filesize { get; set; } = null!;
}
