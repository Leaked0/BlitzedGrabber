using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils.Analyzer
{
	// Token: 0x0200000E RID: 14
	public class FieldDefAnalyzer : DefAnalyzer
	{
		// Token: 0x0600002E RID: 46 RVA: 0x00003634 File Offset: 0x00001834
		public override bool Execute(object context)
		{
			FieldDef field = (FieldDef)context;
			return !field.IsRuntimeSpecialName && (!field.IsLiteral || !field.DeclaringType.IsEnum);
		}
	}
}
