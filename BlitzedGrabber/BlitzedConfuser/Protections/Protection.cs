using System;

namespace BlitzedConfuser.Protections
{
	// Token: 0x02000019 RID: 25
	public abstract class Protection
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000047E4 File Offset: 0x000029E4
		// (set) Token: 0x0600004C RID: 76 RVA: 0x000047EC File Offset: 0x000029EC
		public string Name { get; set; }

		// Token: 0x0600004D RID: 77
		public abstract void Execute();
	}
}
