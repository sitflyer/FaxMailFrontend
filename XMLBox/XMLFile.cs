using System.Globalization;
using System.Xml;

namespace XMLBox
{
	public class XMLFile
	{
		public readonly XmlDocument _XMLFile;
		public readonly XMLKollector XMLCollectedData;

		public string DokumentName { get; set; } = string.Empty;
		/// <summary>
		/// Erzeugt ein XML File zur Analyse des Inhalts mittels Parser. Es erfolgt kein Aufbau einer Datenstruktur
		/// </summary>
		/// <param name="filename">Das Ursprungsfile (muss valides XML beinhalten)</param>
		public XMLFile(string filename)
		{
			DokumentName = filename;
			try
			{
				_XMLFile = new()
				{
					PreserveWhitespace = true
				};
				_XMLFile.Load(filename);
				XMLCollectedData = new(this);
			}
			catch (Exception ex)
			{
				throw new Exception($"Datei {DokumentName} konnte nicht gelesen werden. {ex.Message}");
			}
		}
		public string? GetKnotenValue(string knotenname)
		{
			foreach (XmlNode node in _XMLFile.SelectNodes("//*")!)
			{
				if (node.Name == knotenname)
				{
					return node.InnerText;
				}
			}
			return null;
		}

		public static string DateToString(DateTime date, string digits)
		{
			return date.ToString(digits);
		}
		public static string DateToString(DateTime date)
		{
			return DateToString(date, "yyyy-MM-dd");
		}
		public static bool StringToDate(out DateTime date, string dateString, string digits)
		{
			try
			{
				date = DateTime.ParseExact(dateString, digits, CultureInfo.InvariantCulture);
				return true;
			}
			catch (Exception ex)
			{
				throw new Exception($"Der String {dateString} konnte nicht in ein Datum konvertirert werden werden. {ex.Message}");
			}
		}
		public static bool StringToDate(out DateTime date, string dateString)
		{
			try
			{
				date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
				return true;
			}
			catch (Exception ex)
			{
				throw new Exception($"Der String {dateString} konnte nicht in ein Datum konvertirert werden. {ex.Message}");

			}
		}
		/// <summary>
		/// Sichert das Dokument auf den Filenamen aus DokumentName. Ggfls. den DokumentName vorher setzen. (Property der Klasse.)
		/// </summary>
		/// <param name="overwrite"></param>
		/// <returns></returns>
		public bool DokumentSchreiben(bool overwrite)
		{
			try
			{
				if (File.Exists(DokumentName))
				{
					if (!overwrite)
					{
						throw new Exception($"Datei {DokumentName} existiert und durfte nicht überschrieben werden.");
					}
					File.Delete(DokumentName);
				}
				_XMLFile.PreserveWhitespace = true;
				_XMLFile.Save(DokumentName);
				TextDokument x = new(DokumentName);
				x.RemoveEmptyLines();
				x.DokumentSchreiben(true);
				return true;
			}
			catch (Exception ex)
			{
				throw new Exception($"Datei {DokumentName} konnte nicht geschrieben werden. {ex.Message}");
			}
		}
		public bool RemoveSingleKnoten(string knotenname)
		{
			foreach (XmlNode node in _XMLFile.SelectNodes("//*")!)
			{
				if (node.Name == knotenname)
				{
					node.ParentNode!.RemoveChild(node);
					return true;
				}
			}
			return false;
		}

		public bool ChangeAttributeValue(string knotenname, string attributename, string replacevalue)
		{
			foreach (XmlNode node in _XMLFile.SelectNodes("//*")!)
			{
				if (node.Name == knotenname)
				{
					foreach (XmlAttribute xatt in node.Attributes!)
					{
						if (xatt.Name == attributename)
						{
							xatt.Value = replacevalue;
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
