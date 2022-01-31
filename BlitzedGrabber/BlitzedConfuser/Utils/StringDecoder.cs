using System;
using System.Text;

namespace BlitzedConfuser.Utils
{
	// Token: 0x02000009 RID: 9
	public static class StringDecoder
	{
		// Token: 0x06000026 RID: 38 RVA: 0x000033B0 File Offset: 0x000015B0
		public static string Decrypt(string str, int min, int key, int hash, int length, int max)
		{
			StringBuilder builder = new StringBuilder();
			foreach (char c in str.ToCharArray())
			{
				builder.Append((char)((int)c - key));
			}
			return builder.ToString();
		}
	}
}
