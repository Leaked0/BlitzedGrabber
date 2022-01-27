using System;
using System.Text;

namespace BlitzedConfuser.Utils
{
	// Token: 0x02000009 RID: 9
	public static class StringDecoder
	{
		// Token: 0x06000027 RID: 39 RVA: 0x000033B8 File Offset: 0x000017B8
		public static string Decrypt(string str, int min, int key, int hash, int length, int max)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in str.ToCharArray())
			{
				stringBuilder.Append((char)((int)c - key));
			}
			return stringBuilder.ToString();
		}
	}
}
