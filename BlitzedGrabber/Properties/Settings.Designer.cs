using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace BlitzedGrabber.Properties
{
	// Token: 0x02000022 RID: 34
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000AC RID: 172 RVA: 0x0000A09A File Offset: 0x0000829A
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x04000064 RID: 100
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
