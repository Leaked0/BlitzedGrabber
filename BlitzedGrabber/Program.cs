using System;
using System.Configuration;
using System.Windows.Forms;

namespace BlitzedGrabber
{
	// Token: 0x02000020 RID: 32
	internal static class Program
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00009E0F File Offset: 0x0000800F
		[STAThread]
		[Obsolete]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			AppDomain.CurrentDomain.AppendPrivatePath("bin");
			Application.Run(new Form1());
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00009E38 File Offset: 0x00008038
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

		// Token: 0x06000093 RID: 147 RVA: 0x00009E92 File Offset: 0x00008092
		public static string GetValue(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}
