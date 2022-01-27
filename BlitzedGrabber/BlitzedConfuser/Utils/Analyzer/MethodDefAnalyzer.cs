using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils.Analyzer
{
	// Token: 0x0200000F RID: 15
	public class MethodDefAnalyzer : DefAnalyzer
	{
		// Token: 0x06000031 RID: 49 RVA: 0x0000367C File Offset: 0x00001A7C
		public override bool Execute(object context)
		{
			MethodDef methodDef = (MethodDef)context;
			return !methodDef.IsRuntimeSpecialName && !methodDef.DeclaringType.IsForwarder && !methodDef.IsConstructor && !methodDef.IsVirtual;
		}
	}
}
