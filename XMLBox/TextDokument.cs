namespace XMLBox
{
	/// <summary>
	/// Abbildung eines einzelne Textdokumentes mit allgemeinen unstrukturiertem Text (eine List<string>)
	/// </summary>
	public class TextDokument : Dokument
	{
		/// <summary>
		/// Liest das Sokument ein
		/// </summary>
		/// <param name="dokumentenName">Dateiname des einzulesenden Elementes</param>
		public TextDokument(string dokumentenName) : base(dokumentenName)
		{
			//if (!DokumentEinlesen())
			//{
			//    throw new System.Exception($"Konnte Datei {dokumentenName} nciht einlesen.");
			//}
		}
		/// <summary>
		/// Erstellt ein Sokument auf Basis dr Zeilenliste
		/// </summary>
		/// <param name="dokumentenName">Neuer Dateiname</param>
		/// <param name="zeilen">Liste mit Dokumentzeilen</param>
		public TextDokument(string dokumentenName, List<string> zeilen) : base(dokumentenName)
		{
			DokumentZeilen = zeilen;
		}
		/// <summary>
		/// Erstellt eine Kopie einer Datei. Wichtig das Call by Reference wenn Klasse kopiert wurde
		/// </summary>
		/// <param name="neuerDokumentenName">Neuer Dateiname</param>
		/// <returns></returns>
		public override TextDokument DokumentKopie(string neuerDokumentenName)
		{
			TextDokument doc = new(neuerDokumentenName, this.DokumentZeilen);
			return doc;
		}
		/// <summary>
		/// Löscht alle leeren Zeilen in diesem Dokument
		/// </summary>
		public void RemoveEmptyLines()
		{
			List<string> helper = new() { };
			foreach (string line in DokumentZeilen)
			{
				if (line != "")
				{
					if (line.Replace(" ", string.Empty).Length != 0)
					{
						helper.Add(line);
					}
				}
			}
			DokumentZeilen = helper;
		}
	}
}
