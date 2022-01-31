using System;
using System.IO;

namespace BlitzedConfuser.Protections
{
	// Token: 0x0200001D RID: 29
	public class StripDOSHeader
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00005374 File Offset: 0x00003574
		public static void Execute()
		{
			byte[] blank_dos = new byte[64];
			byte[] magic = StripDOSHeader.ReadArray(StripDOSHeader.offset_magic, StripDOSHeader.length_magic, Kappa.Stream);
			byte[] lfanew = StripDOSHeader.ReadArray(StripDOSHeader.offset_lfanew, StripDOSHeader.length_lfanew, Kappa.Stream);
			Kappa.Stream.Position = 0L;
			StripDOSHeader.WriteArray(0U, blank_dos, Kappa.Stream);
			StripDOSHeader.WriteArray(StripDOSHeader.offset_magic, magic, Kappa.Stream);
			StripDOSHeader.WriteArray(StripDOSHeader.offset_lfanew, lfanew, Kappa.Stream);
			StripDOSHeader.WriteArray(78U, new byte[39], Kappa.Stream);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00005404 File Offset: 0x00003604
		private static byte[] ReadArray(uint offset, int length, Stream stream)
		{
			byte[] data = new byte[length];
			stream.Position = (long)((ulong)offset);
			stream.Read(data, 0, data.Length);
			return data;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000542D File Offset: 0x0000362D
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
