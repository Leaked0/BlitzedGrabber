using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils.Analyzer
{
	// Token: 0x02000011 RID: 17
	public class PropertyDefAnalyzer : DefAnalyzer
	{
		// Token: 0x06000034 RID: 52 RVA: 0x000036D8 File Offset: 0x000018D8
		public override bool Execute(object context)
		{
			PropertyDef propertyDef = (PropertyDef)context;
			return !propertyDef.IsRuntimeSpecialName && !propertyDef.IsEmpty && propertyDef.IsSpecialName;
		}
	}
}
