using MimeKit;
using MsgReader.Outlook;

namespace MailDLL
{
	public static class MimeMessageConverter
	{
		public static MimeMessage ConvertToMimeMessage(Storage.Message msg)
		{
			try
			{
				var mimeMessage = new MimeMessage
				{
					Subject = msg.Subject,
					Body = new TextPart("plain") { Text = msg.BodyText }
				};
				mimeMessage.From.Add(new MailboxAddress(msg.Sender.DisplayName, msg.Sender.Email));
				foreach (var recipient in msg.Recipients)
				{
					mimeMessage.To.Add(new MailboxAddress(recipient.DisplayName, recipient.Email));
				}

				// Add attachments
				var multipart = new Multipart("mixed")
				{
					mimeMessage.Body
				};

				foreach (var attachment in msg.Attachments)
				{
					if (attachment is Storage.Attachment storageAttachment)
					{
						//if (storageAttachment.MimeType is not null)
						//{
						var mimePart = new MimePart(storageAttachment.MimeType)
						{
							Content = new MimeContent(new MemoryStream(storageAttachment.Data)),
							ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
							ContentTransferEncoding = ContentEncoding.Base64,
							FileName = storageAttachment.FileName
						};
						multipart.Add(mimePart);
						//}
					}
				}

				mimeMessage.Body = multipart;

				return mimeMessage;
			}
			catch
			{
				throw new Exception($"Die eingereichte E-Mail enthält nicht auslesbaren Inhalt. Bitte die Dokumente einzeln einreichen.");
			}
		}
	}
}
