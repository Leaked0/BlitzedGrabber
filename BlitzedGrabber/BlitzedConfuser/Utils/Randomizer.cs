using System;
using System.Security.Cryptography;
using System.Text;

namespace BlitzedConfuser.Utils
{
	// Token: 0x02000007 RID: 7
	public class Randomizer
	{
		// Token: 0x0600001D RID: 29 RVA: 0x000032C4 File Offset: 0x000014C4
		public static int Next(int maxValue, int minValue = 0)
		{
			if (minValue >= maxValue)
			{
				throw new ArgumentOutOfRangeException("minValue");
			}
			long diff = (long)maxValue - (long)minValue;
			long upperBound = (long)(-1 / diff * diff);
			uint ui;
			do
			{
				ui = Randomizer.RandomUInt();
			}
			while ((ulong)ui >= (ulong)upperBound);
			return (int)((long)minValue + (long)((ulong)ui % (ulong)diff));
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003300 File Offset: 0x00001500
		public static string String(int size)
		{
			return Encoding.UTF7.GetString(Randomizer.RandomBytes(size)).Replace("\0", ".").Replace("\n", ".")
				.Replace("\r", ".");
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000333F File Offset: 0x0000153F
		public static int Next()
		{
			return BitConverter.ToInt32(Randomizer.RandomBytes(4), 0);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000334D File Offset: 0x0000154D
		private static uint RandomUInt()
		{
			return BitConverter.ToUInt32(Randomizer.RandomBytes(4), 0);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000335C File Offset: 0x0000155C
		private static byte[] RandomBytes(int bytes)
		{
			byte[] buffer = new byte[bytes];
			Randomizer.csp.GetBytes(buffer);
			return buffer;
		}

		// Token: 0x0400000B RID: 11
		private static readonly RandomNumberGenerator csp = RandomNumberGenerator.Create();
	}
}
