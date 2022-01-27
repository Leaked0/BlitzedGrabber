using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace BlitzedConfuser.Utils
{
	// Token: 0x0200000A RID: 10
	public static class TamperClass
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00003418 File Offset: 0x00001818
		public static void NoTampering()
		{
			string location = Assembly.GetExecutingAssembly().Location;
			Stream baseStream = new StreamReader(location).BaseStream;
			BinaryReader binaryReader = new BinaryReader(baseStream);
			string a = BitConverter.ToString(MD5.Create().ComputeHash(binaryReader.ReadBytes(File.ReadAllBytes(location).Length - 16)));
			baseStream.Seek(-16L, SeekOrigin.End);
			if (a != BitConverter.ToString(binaryReader.ReadBytes(16)))
			{
				throw new EntryPointNotFoundException();
			}
		}
	}
}
