namespace XMLBox
{
	public abstract class Dokument
	{
		public string DokumentName { get; set; } = string.Empty;
		public List<string> DokumentZeilen = new() { };
		/// <summary>
		/// Konstruktor setzt den Dokumentenname und Defaultstatus
		/// </summary>
		/// <param name="dokumentName"></param>
		public Dokument(string dokumentName)
		{
			DokumentName = dokumentName;
		}
		/// <summary>
		/// Einlesen eines Dokumentes mittels Dateinamen
		/// </summary>
		/// <returns></returns>
		public virtual bool DokumentEinlesen()
		{
			if (File.Exists(DokumentName))
			{
				try
				{
					DokumentZeilen = File.ReadLines(DokumentName).ToList();
				}
				catch (Exception ex)
				{
					throw new Exception($"Fehler beim Lesen der Datei {DokumentName}. {ex.Message}");
				}
			}
			else
			{
				throw new Exception($"Die Datei {DokumentName} existiert nicht.");

			}
			return true;
		}
		/// <summary>
		/// Schreiben eine Dokumentes
		/// </summary>
		/// <param name="overWrite">Überschreibt eine existierende Datei wenn True</param>
		/// <returns></returns>
		public virtual bool DokumentSchreiben(bool overWrite)
		{
			//Wenn File nicht da ist oder overWrite = true
			//Datei auf Platte schreiben
			if (!File.Exists(DokumentName) || overWrite)
			{
				try
				{
					File.WriteAllLines(DokumentName, DokumentZeilen);
				}
				catch (Exception ex)
				{
					throw new Exception($"Fehler beim Schreiben der Datei {DokumentName}. {ex.Message}");
				}
				return true;
			}
			//Sonst nicht schreiben
			else
			{
				throw new Exception($"Die Datei {DokumentName} existiert bereits und wurde nicht überschrieben.");
			}
		}
		/// <summary>
		/// Erstellt eine Dokumentkopie. Diese ist abhängig vom Typ und muss im vererbter Klasse implementiert werden
		/// </summary>
		/// <param name="neuerDokumentenName">Neuer Dateiname</param>
		/// <returns></returns>
		public abstract Dokument DokumentKopie(string neuerDokumentenName);
	}
}
