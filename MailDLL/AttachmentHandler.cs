using MimeKit;

namespace MailDLL
{
	/// <summary>
	/// Klasse zur Verwaltung aler Attachments einer EMail
	/// </summary>
	internal class AttachmentHandler
	{
		internal List<Attachment> Attachments { get; set; } = new() { };
		//internal static List<DataAccess.SQLModels.AttachmentRule> attRules;
		public int NumberOfValidAttachments;
		/// <summary>
		/// Erzeigt einen Attachmentverwalter der alle einzelnen Attachments extrahiert
		/// </summary>
		/// <param name="message"></param>
		internal AttachmentHandler(MimeMessage message)
		{
			try
			{
				//attRules = DataAccess.Workdata.GetAttachmentRules();
				ExtractAttachments(message);
			}
			catch (Exception ex)
			{
				throw new Exception($"Fehler beim Extrahieren der Attachments. {ex.Message}");
			}
		}
		/// <summary>
		/// Ist Anzahl der Attachments für den Dokumententyp erlaubt
		/// </summary>
		/// <param name="rwa"></param>
		/// <returns></returns>
		//internal bool IsAttachmentCountValid(DataAccess.SQLModels.Regelwerk rwa)
		//{
		//    if (NumberOfValidAttachments > rwa.MaxAnzahlAcceptedMedien)
		//    {
		//        return false;
		//    }
		//    return true;
		//}
		/// <summary>
		/// Ist der Typ des Attachment erlaubt
		/// </summary>
		/// <returns></returns>
		//internal bool IsAttachmentNotAllowed()
		//{
		//    foreach (Attachment att in Attachments)
		//    {
		//        if (att.Abweisung)
		//        {
		//            return true;
		//        }
		//    }
		//    return false;
		//}
		/// <summary>
		/// Extrahiert die Attachments aus der Mail
		/// </summary>
		/// <param name="message">Die originale MimeMessage die aus der EML-Datei eingelesen wurde</param>
		private void ExtractAttachments(MimeMessage message)
		{
			var attachments = message.Attachments.ToList();

			foreach (MimeEntity attachment in attachments)
			{
				Attachments.Add(new(attachment));
			}
			NumberOfValidAttachments = Attachments.Where(a => a.Valid == true).ToList().Count;
		}
	}
}
