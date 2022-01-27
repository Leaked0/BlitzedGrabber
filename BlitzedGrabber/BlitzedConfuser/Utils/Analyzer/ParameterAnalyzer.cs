using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils.Analyzer
{
	// Token: 0x02000010 RID: 16
	public class ParameterAnalyzer : DefAnalyzer
	{
		// Token: 0x06000033 RID: 51 RVA: 0x000036C0 File Offset: 0x00001AC0
		public override bool Execute(object context)
		{
			return ((Parameter)context).Name != string.Empty;
		}
	}
}
