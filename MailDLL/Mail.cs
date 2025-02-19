using MimeKit;
using MsgReader.Outlook;
using System.Text;

namespace MailDLL
{
	public class Mail
	{
		public readonly MimeMessage _myMessage;
		public string Filename { get; set; }
		private AttachmentHandler? _attachmentHandler;
		static Mail()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}
		public Mail(string filename)
		{
			Filename = filename;
			if (!File.Exists(Filename))
			{
				throw new Exception("Die Maildatei existiert nicht.");
			}
			try
			{
				var extension = Path.GetExtension(Filename).ToLower();
				if (extension == ".eml")
				{
					var stream = File.Open(Filename, FileMode.Open, FileAccess.Read);
					_myMessage = MimeMessage.Load(stream);
					stream.Close();
				}
				else if (extension == ".msg")
				{
					using (var msg = new Storage.Message(Filename))

					{
						_myMessage = MimeMessageConverter.ConvertToMimeMessage(msg);
					}
				}
				else
				{
					throw new Exception("Unbekanntes Dateiformat.");
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Die Mail konnte nicht gelesen werden. {ex.Message}");
			}
		}

		public void CollectAttachments()
		{
			try
			{
				_attachmentHandler = new AttachmentHandler(_myMessage);
			}
			catch (Exception ex)
			{
				throw new Exception($"Fehler beim Extrahieren der Attachments. {ex.Message}");
			}
		}

		public void WriteAttachments()
		{
			foreach (Attachment att in _attachmentHandler!.Attachments)
			{
				try
				{
					att.WriteAttachmentToFile(Path.Combine(Path.GetDirectoryName(Filename)!, att.Dateiname));
				}
				catch (Exception ex)
				{
					throw new Exception($"Fehler beim Schreiben des Attachments {att.Dateiname} => {ex.Message}");
				}
			}
			string mailbody = GetMailBodyAsText();
			try
			{
				File.WriteAllText(Path.Combine(Path.GetDirectoryName(Filename)!, Path.GetFileNameWithoutExtension(Filename) + "_body.txt"), mailbody);
			}
			catch (Exception ex)
			{
				throw new Exception($"Fehler beim Schreiben des Mailbodys => {ex.Message}");
			}
		}
		public string GetMailBodyAsText()
		{
			if (_myMessage == null)
			{
				throw new Exception("Die Mail wurde nicht geladen.");
			}

			var textPart = _myMessage.TextBody ?? _myMessage.HtmlBody;
			if (textPart == null)
			{
				throw new Exception("Der Mail-Body ist leer.");
			}

			return textPart;
		}
	}
}
