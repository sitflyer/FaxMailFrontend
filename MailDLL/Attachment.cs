using MimeKit;

namespace MailDLL
{
	/// <summary>
	/// Dies ist die Representation eines einzelen Attachments in einer EMail
	/// </summary>
	internal class Attachment
	{
		//Der Name der Datei
		public string Dateiname { get; set; } = "";
		//Die Größe in Bytes
		public long SizeInBytes { get; set; } = 0L;
		//Der attachment-Typ. JPG, TIF, etc
		public AttachmentType Typ = AttachmentType.unbekannt;
		//Ist das Attachment korrekt zur Verarbeitung => AttachmentRule
		public bool Valid { get; set; } = true;
		//Ist das Attachment ein erlaubtes Attachment
		public bool Abweisung { get; set; } = false;
		//Die Byte Representation des Attachments
		private readonly byte[]? _bindata;
		//Der Name der Datei im Stapelfolder
		public string FinalFileName { get; set; } = "";

		/// <summary>
		/// Extraktion des Attachemnt
		/// </summary>
		/// <param name="att"></param>
		internal Attachment(MimeEntity att)
		{
			using MemoryStream memory = new();
			switch (att)
			{
				case MimePart:
					((MimePart)att).Content.DecodeTo(memory);
					_bindata = memory.ToArray();
					MimePart part = (MimePart)att;
					Dateiname = part.FileName;
					SizeInBytes = _bindata.Length;
					Typ = GetAttachmentType(Dateiname);
					//Abweisung = IsAbweisung();
					//Valid = IsValid();
					break;
				default:
					((MessagePart)att).Message.WriteTo(memory);
					Abweisung = true;
					Valid = false;
					Typ = AttachmentType.invalid;
					SizeInBytes = 0L;
					if (att.ContentDisposition?.FileName == "" || att.ContentDisposition?.FileName == null)
					{
						if (att.ContentId != null)
						{
							Dateiname = att.ContentId;
						}
						else
						{
							Dateiname = "Attached EMail";
						}
					}
					else
					{
						if (att.ContentDisposition?.FileName is not null)
						{
							Dateiname = att.ContentDisposition?.FileName!;
						}
						else
						{
							Dateiname = $"Attached EMail";
						}
					}
					break;
			}
		}
		/// <summary>
		/// Check auf Validität (Größe etc.) eines Attachments
		/// </summary>
		/// <returns></returns>
		//private bool IsValid()
		//{
		//	foreach (DataAccess.SQLModels.AttachmentRule ar in AttachmentHandler.attRules.Where(s => s.Operator == "<" && s.Action == "IGNORE_DOCUMENT"))
		//	{
		//		if (Path.GetExtension(Dateiname).ToUpper() == Path.GetExtension(ar.Filter).ToUpper())
		//		{
		//			try
		//			{
		//				int value = int.Parse(ar.ValueByte.ToString());
		//				if (SizeInBytes <= value)
		//				{
		//					return false;
		//				}
		//			}
		//			catch
		//			{
		//				LogBox.GlobalLogProvider.GetInstance().Log.Error($"Max. Dateigröße für *.{Path.GetExtension(ar.Filter)} konnte nicht ermittelt werden");
		//				return true;
		//			}
		//		}
		//	}
		//	foreach (DataAccess.SQLModels.AttachmentRule ar in AttachmentHandler.attRules.Where(s => s.Operator != "<" && s.Action == "IGNORE_DOCUMENT"))
		//	{
		//		//if MedeiaFileName matches .*parser.*.txt
		//		String filter = ar.Filter.ToString();
		//		var filterUpperRegex = new Regex(@"^" + filter);
		//		var filterLowerRegex = new Regex(@"^" + filter.ToLower());
		//		if (filterUpperRegex.IsMatch(Dateiname) || filterLowerRegex.IsMatch(Dateiname))
		//		{
		//			return false;
		//		}
		//	}
		//	return true;
		//}
		/// <summary>
		/// Check ob die Art des Attachments erlaubt ist, Wenn nicht erfolgt eine fachliche Abweisung
		/// </summary>
		/// <returns></returns>
		//private bool IsAbweisung()
		//{
		//	LogBox.GlobalLogProvider.GetInstance().Log.Debug($"AtachmentFileName => {Dateiname}.");
		//	LogBox.GlobalLogProvider.GetInstance().Log.Debug($"AtachmentFileSize => {SizeInBytes} Bytes.");
		//	LogBox.GlobalLogProvider.GetInstance().Log.Debug($"AtachmentFileType => {Typ}.");
		//	foreach (DataAccess.SQLModels.AttachmentRule ar in AttachmentHandler.attRules.Where(s => s.Action == "ACCEPT_DOCUMENT"))
		//	{
		//		if (Path.GetExtension(Dateiname).ToUpper() == Path.GetExtension(ar.Filter).ToUpper())
		//		{
		//			return false;
		//		}
		//	}
		//	return true;
		//}
		/// <summary>
		/// Ermittelt den Typ des Attachment zur Auswertung
		/// </summary>
		/// <param name="dateiname"></param>
		/// <returns></returns>
		private static AttachmentType GetAttachmentType(string dateiname)
		{
			string ext = Path.GetExtension(dateiname).ToUpper();
			return ext switch
			{
				(".PNG") => AttachmentType.PNG,
				(".JPG") => AttachmentType.JPG,
				(".TIFF") => AttachmentType.TIFF,
				(".TIF") => AttachmentType.TIFF,
				(".TXT") => AttachmentType.TXT,
				(".PDF") => AttachmentType.PDF,
				_ => AttachmentType.invalid,
			};
		}
		/// <summary>
		/// Macht aus dem Attachment eine Datei im Filesytem
		/// </summary>
		/// <param name="filename">Name mit Pfad der Ausgabedatei</param>
		/// <returns></returns>
		internal void WriteAttachmentToFile(string filename)
		{
			try
			{
				if (_bindata != null)
				{
					using (FileStream stream = new(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
					{
						stream.Write(_bindata, 0, _bindata.Length);
					}
				}
				else
				{
					throw new Exception("Keine Daten im Attachment");
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Fehler beim Schreiben des Attachments {Dateiname} => {ex.Message}");
			}
		}
	}
}
