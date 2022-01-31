using System;
using System.IO;
using BlitzedConfuser.Protections;
using BlitzedConfuser.Utils;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using SharpConfigParser;

namespace BlitzedConfuser
{
	// Token: 0x02000002 RID: 2
	internal class Kappa
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		public static ModuleDefMD Module { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000205F File Offset: 0x0000025F
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002066 File Offset: 0x00000266
		public static string FileExtension { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000206E File Offset: 0x0000026E
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002075 File Offset: 0x00000275
		public static bool DontRename { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000207D File Offset: 0x0000027D
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002084 File Offset: 0x00000284
		public static bool ForceWinForms { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000208C File Offset: 0x0000028C
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002093 File Offset: 0x00000293
		public static string FilePath { get; set; }

		// Token: 0x0600000B RID: 11 RVA: 0x0000209C File Offset: 0x0000029C
		public static void Obfuscate(string pathway)
		{
			Console.WriteLine("- Preparing obfuscation...");
			if (!File.Exists("config.txt"))
			{
				Console.WriteLine("Config file not found, continuing without it.");
			}
			else
			{
				Parser p = new Parser
				{
					ConfigFile = "config.txt"
				};
				try
				{
					Kappa.ForceWinForms = bool.Parse(p.Read("ForceWinFormsCompatibility").ReadResponse().ReplaceSpaces());
				}
				catch
				{
				}
				try
				{
					Kappa.DontRename = bool.Parse(p.Read("DontRename").ReadResponse().ReplaceSpaces());
				}
				catch
				{
				}
				try
				{
					ProxyAdder.Intensity = int.Parse(p.Read("ProxyCallsIntensity").ReadResponse().ReplaceSpaces());
				}
				catch
				{
				}
				Console.WriteLine("\n- ForceWinForms: " + Kappa.ForceWinForms.ToString());
				Console.WriteLine("- DontRename: " + Kappa.DontRename.ToString());
				Console.WriteLine("- ProxyCallsIntensity: " + ProxyAdder.Intensity.ToString() + "\n");
			}
			Kappa.Module = ModuleDefMD.Load(pathway);
			Kappa.FileExtension = Path.GetExtension(pathway);
			foreach (Protection protection in new Protection[]
			{
				new Renamer(),
				new AntiTamper(),
				new JunkDefs(),
				new StringEncryption(),
				new AntiDe4dot(),
				new ControlFlow(),
				new IntEncoding(),
				new ProxyAdder()
			})
			{
				Console.WriteLine("- Executing protection: " + protection.Name);
				protection.Execute();
			}
			Console.WriteLine("- Watermarking...");
			Watermark.AddAttribute();
			Console.WriteLine("- Saving file...");
			Kappa.FilePath = string.Concat(new string[]
			{
				"C:\\Users\\",
				Environment.UserName,
				"\\Pictures\\",
				Path.GetFileNameWithoutExtension(pathway),
				"_Blitzed",
				Kappa.FileExtension
			});
			Kappa.Module.Write(Kappa.Stream, new ModuleWriterOptions(Kappa.Module)
			{
				Logger = DummyLogger.NoThrowInstance
			});
			Console.WriteLine("- Stripping DOS header...");
			StripDOSHeader.Execute();
			File.WriteAllBytes(Kappa.FilePath, Kappa.Stream.ToArray());
			if (AntiTamper.Tampered)
			{
				AntiTamper.Inject(Kappa.FilePath);
			}
		}

		// Token: 0x04000006 RID: 6
		public static MemoryStream Stream = new MemoryStream();
	}
}
