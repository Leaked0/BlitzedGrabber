using System;
using System.Security.Cryptography;
using System.Text;

namespace BlitzedConfuser.Utils
{
	// Token: 0x02000007 RID: 7
	public class Randomizer
	{
		// Token: 0x0600001E RID: 30 RVA: 0x000032CC File Offset: 0x000016CC
		public static int Next(int maxValue, int minValue = 0)
		{
			if (minValue >= maxValue)
			{
				throw new ArgumentOutOfRangeException("minValue");
			}
			long num = (long)maxValue - (long)minValue;
			long num2 = (-1 / num * num);
			uint num3;
			do
			{
				num3 = Randomizer.RandomUInt();
			}
			while ((ulong)num3 >= (ulong)num2);
			return (int)((long)minValue + (long)((ulong)num3 % (ulong)num));
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003308 File Offset: 0x00001708
		public static string String(int size)
		{
			return Encoding.UTF7.GetString(Randomizer.RandomBytes(size)).Replace("\0", ".").Replace("\n", ".")
				.Replace("\r", ".");
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003347 File Offset: 0x00001747
		public static int Next()
		{
			return BitConverter.ToInt32(Randomizer.RandomBytes(4), 0);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003355 File Offset: 0x00001755
		private static uint RandomUInt()
		{
			return BitConverter.ToUInt32(Randomizer.RandomBytes(4), 0);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003364 File Offset: 0x00001764
		private static byte[] RandomBytes(int bytes)
		{
			byte[] array = new byte[bytes];
			Randomizer.csp.GetBytes(array);
			return array;
		}

		// Token: 0x0400000B RID: 11
		private static readonly RandomNumberGenerator csp = RandomNumberGenerator.Create();
	}
}
