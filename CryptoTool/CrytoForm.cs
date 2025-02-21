using CryptoDLL;

namespace CryptoKonverter
{
	public partial class CrytoForm : Form
	{
		public CrytoForm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void But_Crypt_Click(object sender, EventArgs e)
		{
			if (TB_Clear.Text == "")
			{
				MessageBox.Show("Bitte geben Sie einen Text ein!");
				TB_Clear.Focus();
			}
			else
			{
				TB_Crypt.Text = CryptoProvider.EncryptString(TB_Clear.Text);
			}
		}

		private void But_Decrypt_Click(object sender, EventArgs e)
		{
			if (TB_Crypt.Text == "")
			{
				MessageBox.Show("Bitte geben Sie einen Text ein!");
				TB_Crypt.Focus();
			}
			else
			{
				TB_Clear.Text = CryptoProvider.DecryptString(TB_Crypt.Text);
			}
		}
	}
}