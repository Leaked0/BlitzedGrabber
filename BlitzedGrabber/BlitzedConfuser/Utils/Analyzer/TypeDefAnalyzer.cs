using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils.Analyzer
{
	// Token: 0x02000012 RID: 18
	public class TypeDefAnalyzer : DefAnalyzer
	{
		// Token: 0x06000036 RID: 54 RVA: 0x0000370C File Offset: 0x0000190C
		public override bool Execute(object context)
		{
			TypeDef type = (TypeDef)context;
			return !type.IsSpecialName && !type.IsWindowsRuntime && !type.IsForwarder && !type.IsRuntimeSpecialName;
		}
	}
}
