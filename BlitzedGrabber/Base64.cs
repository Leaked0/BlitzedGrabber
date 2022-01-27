using System;
using System.Text;

namespace BlitzedGrabber
{
	// Token: 0x0200001F RID: 31
	internal class Base64
	{
		// Token: 0x0600008A RID: 138 RVA: 0x0000A022 File Offset: 0x00008422
		public static string Base64Encode(string plainText)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000A034 File Offset: 0x00008434
		public string Base64Decode(string base64EncodedData)
		{
			byte[] bytes = Convert.FromBase64String(base64EncodedData);
			return Encoding.UTF8.GetString(bytes);
		}
	}
}
