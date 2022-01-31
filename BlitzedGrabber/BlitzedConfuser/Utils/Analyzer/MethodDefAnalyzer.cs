using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils.Analyzer
{
	// Token: 0x0200000F RID: 15
	public class MethodDefAnalyzer : DefAnalyzer
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00003674 File Offset: 0x00001874
		public override bool Execute(object context)
		{
			MethodDef method = (MethodDef)context;
			return !method.IsRuntimeSpecialName && !method.DeclaringType.IsForwarder && !method.IsConstructor && !method.IsVirtual;
		}
	}
}
