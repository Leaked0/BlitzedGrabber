using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils.Analyzer
{
	// Token: 0x02000010 RID: 16
	public class ParameterAnalyzer : DefAnalyzer
	{
		// Token: 0x06000032 RID: 50 RVA: 0x000036B8 File Offset: 0x000018B8
		public override bool Execute(object context)
		{
			return ((Parameter)context).Name != string.Empty;
		}
	}
}
