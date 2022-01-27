using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils.Analyzer
{
	// Token: 0x02000011 RID: 17
	public class PropertyDefAnalyzer : DefAnalyzer
	{
		// Token: 0x06000035 RID: 53 RVA: 0x000036E0 File Offset: 0x00001AE0
		public override bool Execute(object context)
		{
			PropertyDef propertyDef = (PropertyDef)context;
			return !propertyDef.IsRuntimeSpecialName && !propertyDef.IsEmpty && propertyDef.IsSpecialName;
		}
	}
}
