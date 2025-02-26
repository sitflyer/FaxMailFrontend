using DataAccessDLL.Interfaces;
using System;
using System.Collections.Generic;

namespace DataAccessDLL.Modell;

public partial class LeistungserbringerLanr : ILeistungserbringerLanr
{
	public string? Bpnr { get; set; }

	public string? Type { get; set; }

	public string? Rolle { get; set; }

	public string? Bezeichnung { get; set; }

	public string? Arztnummer { get; set; }

	public string? Name1 { get; set; }

	public string? Name2 { get; set; }

	public string? Name3 { get; set; }

	public string? Name4 { get; set; }

	public string? Vorname { get; set; }

	public string? Nachname { get; set; }

	public string? Plz { get; set; }

	public string? Ort { get; set; }

	public string? Strasse { get; set; }

	public string? Hausnummer { get; set; }

	public string? Verarbdatum { get; set; }

	public int Record { get; set; }
}
