using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils
{
	// Token: 0x02000005 RID: 5
	public static class MemberRenamer
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002D2D File Offset: 0x00000F2D
		public static void GetRenamed(this IMemberDef member)
		{
			member.Name = "StvnedEagleWINNINGLOL" + Randomizer.String(45);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002D4B File Offset: 0x00000F4B
		public static int StringLength()
		{
			return Randomizer.Next(120, 30);
		}
	}
}
