namespace DataAccessDLL.Interfaces
{
	public interface IStammdatenFirmenkunde
	{
		string? Btnr { get; set; }
		string? Firmenname { get; set; }
		string? Hausnummer { get; set; }
		string? Identart { get; set; }
		string? Ort { get; set; }
		string? Plz { get; set; }
		string? Strasse { get; set; }
		int UniqueId { get; set; }
		string? Verarbdatum { get; set; }
	}
}