using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils
{
	// Token: 0x02000005 RID: 5
	public static class MemberRenamer
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002D35 File Offset: 0x00001135
		public static void GetRenamed(IMemberDef member)
		{
			member.Name = "StvnedEagleWINNINGLOL" + Randomizer.String(45);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002D53 File Offset: 0x00001153
		public static int StringLength()
		{
			return Randomizer.Next(120, 30);
		}
	}
}
