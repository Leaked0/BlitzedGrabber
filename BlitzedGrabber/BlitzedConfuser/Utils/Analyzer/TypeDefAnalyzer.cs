using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils.Analyzer
{
	// Token: 0x02000012 RID: 18
	public class TypeDefAnalyzer : DefAnalyzer
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00003714 File Offset: 0x00001B14
		public override bool Execute(object context)
		{
			TypeDef typeDef = (TypeDef)context;
			return !typeDef.IsSpecialName && !typeDef.IsWindowsRuntime && !typeDef.IsForwarder && !typeDef.IsRuntimeSpecialName;
		}
	}
}
