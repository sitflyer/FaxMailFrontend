namespace FaxMailFrontend.Data
{
	public class ErrorHandler
	{
		public ErrorCode EC { get; set; } = ErrorCode.KeinFehler;
		public string Systemmessage { get; set; } = string.Empty;
		public ErrorHandler(ErrorCode ec, string message)
		{
			EC = ec;
			Systemmessage = message;
		}
	}
}
