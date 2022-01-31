using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace BlitzedConfuser.Utils
{
	// Token: 0x0200000A RID: 10
	public static class TamperClass
	{
		// Token: 0x06000027 RID: 39 RVA: 0x00003410 File Offset: 0x00001610
		public static void NoTampering()
		{
			string p = Assembly.GetExecutingAssembly().Location;
			Stream i = new StreamReader(p).BaseStream;
			BinaryReader r = new BinaryReader(i);
			string a = BitConverter.ToString(MD5.Create().ComputeHash(r.ReadBytes(File.ReadAllBytes(p).Length - 16)));
			i.Seek(-16L, SeekOrigin.End);
			if (a != BitConverter.ToString(r.ReadBytes(16)))
			{
				throw new EntryPointNotFoundException();
			}
		}
	}
}
