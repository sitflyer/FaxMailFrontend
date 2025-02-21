using System.Security.Cryptography;

namespace CryptoDLL
{
	public class CryptoProvider
	{
		//Das password zur internen automatischen Vercryptung
		//Bei Änderungen immer beide Paare anpassen (Hier und in evtl. existierenden app.konfigs)
		private static readonly string passwd = "There!is#no?specific-key_isIT?";

		/// <summary>
		/// Verschlüsselung von Daten
		/// </summary>
		/// <param name="clearText"></param>
		/// <param name="Key"></param>
		/// <param name="IV"></param>
		/// <returns></returns>
		public static byte[] EncryptString(byte[] clearText, byte[] Key, byte[] IV)
		{
			MemoryStream ms = new();
			Aes alg = Aes.Create();
			alg.Key = Key;
			alg.IV = IV;
			CryptoStream cs = new(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
			cs.Write(clearText, 0, clearText.Length);
			cs.Close();
			byte[] encryptedData = ms.ToArray();
			return encryptedData;
		}
		/// <summary>
		/// Verschlüsselung eines Wortes
		/// </summary>
		/// <param name="clearText"></param>
		/// <returns></returns>
		public static string EncryptString(string clearText)
		{
			string Password = passwd;

			byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
			PasswordDeriveBytes pdb = new(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
			byte[] encryptedData = EncryptString(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
			return Convert.ToBase64String(encryptedData);
		}

		/// <summary>
		/// Entschlüsselung von Daten
		/// </summary>
		/// <param name="cipherData"></param>
		/// <param name="Key"></param>
		/// <param name="IV"></param>
		/// <returns></returns>
		public static byte[]? DecryptString(byte[] cipherData, byte[] Key, byte[] IV)
		{
			CryptoStream? cs = null;
			byte[] decryptedData;

			try
			{
				MemoryStream ms = new();
				Aes alg = Aes.Create();
				alg.Key = Key;
				alg.IV = IV;
				cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
				cs.Write(cipherData, 0, cipherData.Length);
				cs.Close();
				decryptedData = ms.ToArray();
				return decryptedData;
			}
			catch (CryptographicException ex)
			{
				throw new CryptographicException("Fehler bei der Entschlüsselung", ex);
			}
			finally
			{
				cs!.Close();
			}
		}

		/// <summary>
		/// Entschlüsselung eines Wortes 
		/// </summary>
		/// <param name="cipherText"></param>
		/// <returns></returns>
		public static string? DecryptString(string cipherText)
		{
			try
			{
				string Password = passwd;

				byte[] cipherBytes = Convert.FromBase64String(cipherText);
				PasswordDeriveBytes pdb = new(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				byte[]? decryptedData = DecryptString(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
				if (decryptedData != null)
					return System.Text.Encoding.Unicode.GetString(decryptedData!);
			}
			catch (Exception ex)
			{
				throw new CryptographicException("Fehler bei der Entschlüsselung", ex);
			}
			return null;
		}
	}
}
