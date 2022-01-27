using System;
using System.Configuration;
using System.Windows.Forms;

namespace BlitzedGrabber
{
	// Token: 0x02000020 RID: 32
	internal static class Program
	{
		// Token: 0x0600008D RID: 141 RVA: 0x0000A05B File Offset: 0x0000845B
		[STAThread]
		[Obsolete]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			AppDomain.CurrentDomain.AppendPrivatePath("bin");
			Application.Run(new Form1());
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000A084 File Offset: 0x00008484
		public static void SetValue(string key, string value)
		{
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;
			if (settings[key] == null)
			{
				settings.Add(key, value);
			}
			else
			{
				settings[key].Value = value;
			}
			configuration.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection(configuration.AppSettings.SectionInformation.Name);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000A0DE File Offset: 0x000084DE
		public static string GetValue(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}
