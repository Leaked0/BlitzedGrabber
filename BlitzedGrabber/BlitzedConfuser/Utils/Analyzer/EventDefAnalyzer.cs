using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils.Analyzer
{
	// Token: 0x0200000D RID: 13
	public class EventDefAnalyzer : DefAnalyzer
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00003621 File Offset: 0x00001A21
		public override bool Execute(object context)
		{
			return !((EventDef)context).IsRuntimeSpecialName;
		}
	}
}
