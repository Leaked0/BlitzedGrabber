using System;
using System.IO;

namespace BlitzedConfuser.Protections
{
	// Token: 0x0200001D RID: 29
	public class StripDOSHeader
	{
		// Token: 0x06000069 RID: 105 RVA: 0x0000537C File Offset: 0x0000377C
		public static void Execute()
		{
			byte[] data = new byte[64];
			byte[] data2 = StripDOSHeader.ReadArray(StripDOSHeader.offset_magic, StripDOSHeader.length_magic, Kappa.Stream);
			byte[] data3 = StripDOSHeader.ReadArray(StripDOSHeader.offset_lfanew, StripDOSHeader.length_lfanew, Kappa.Stream);
			Kappa.Stream.Position = 0L;
			StripDOSHeader.WriteArray(0U, data, Kappa.Stream);
			StripDOSHeader.WriteArray(StripDOSHeader.offset_magic, data2, Kappa.Stream);
			StripDOSHeader.WriteArray(StripDOSHeader.offset_lfanew, data3, Kappa.Stream);
			StripDOSHeader.WriteArray(78U, new byte[39], Kappa.Stream);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000540C File Offset: 0x0000380C
		private static byte[] ReadArray(uint offset, int length, Stream stream)
		{
			byte[] array = new byte[length];
			stream.Position = (long)((ulong)offset);
			stream.Read(array, 0, array.Length);
			return array;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00005435 File Offset: 0x00003835
		private static int WriteArray(uint offset, byte[] data, Stream stream)
		{
			stream.Position = (long)((ulong)offset);
			stream.Write(data, 0, data.Length);
			return data.Length;
		}

		// Token: 0x0400001C RID: 28
		private static readonly uint offset_lfanew = 60U;

		// Token: 0x0400001D RID: 29
		private static readonly int length_lfanew = 4;

		// Token: 0x0400001E RID: 30
		private static readonly uint offset_magic = 0U;

		// Token: 0x0400001F RID: 31
		private static readonly int length_magic = 2;
	}
}
