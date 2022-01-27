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
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x0000A2E6 File Offset: 0x000086E6
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x04000065 RID: 101
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
