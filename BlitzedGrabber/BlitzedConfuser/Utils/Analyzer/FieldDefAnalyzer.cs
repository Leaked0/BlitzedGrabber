using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils.Analyzer
{
	// Token: 0x0200000E RID: 14
	public class FieldDefAnalyzer : DefAnalyzer
	{
		// Token: 0x0600002F RID: 47 RVA: 0x0000363C File Offset: 0x00001A3C
		public override bool Execute(object context)
		{
			FieldDef fieldDef = (FieldDef)context;
			return !fieldDef.IsRuntimeSpecialName && (!fieldDef.IsLiteral || !fieldDef.DeclaringType.IsEnum);
		}
	}
}
