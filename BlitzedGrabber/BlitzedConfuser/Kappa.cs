using System;
using System.IO;
using System.Runtime.CompilerServices;
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
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000450
		// (set) Token: 0x06000003 RID: 3 RVA: 0x00002057 File Offset: 0x00000457
		public static ModuleDefMD Module
		{
			get
			{
				return Kappa.Modulek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				Kappa.Modulek__BackingField = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x0000205F File Offset: 0x0000045F
		// (set) Token: 0x06000005 RID: 5 RVA: 0x00002066 File Offset: 0x00000466
		public static string FileExtension
		{
			get
			{
				return Kappa.FileExtensionk__BackingField;
			}
			set
			{
				Kappa.FileExtensionk__BackingField = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x0000206E File Offset: 0x0000046E
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002075 File Offset: 0x00000475
		public static bool DontRename
		{
			get
			{
				return Kappa.DontRenamek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				Kappa.DontRenamek__BackingField = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000207D File Offset: 0x0000047D
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002084 File Offset: 0x00000484
		public static bool ForceWinForms { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000208C File Offset: 0x0000048C
		// (set) Token: 0x0600000B RID: 11 RVA: 0x00002093 File Offset: 0x00000493
		public static string FilePath
		{
			[CompilerGenerated]
			get
			{
				return Kappa.FilePathk__BackingField;
			}
			[CompilerGenerated]
			set
			{
				Kappa.FilePathk__BackingField = value;
			}
		}

        public static ModuleDefMD Modulek__BackingField { get; private set; }
        public static bool DontRenamek__BackingField { get; private set; }

        // Token: 0x0600000C RID: 12 RVA: 0x0000209C File Offset: 0x0000049C
        public static void Obfuscate(string pathway)
		{
			Console.WriteLine("- Drag & drop your file:");
			Console.WriteLine("- Preparing obfuscation...");
			if (!File.Exists("config.txt"))
			{
				Console.WriteLine("Config file not found, continuing without it.");
			}
			else
			{
				Parser parser = new Parser
				{
					ConfigFile = "config.txt"
				};
				try
				{
					Kappa.ForceWinForms = bool.Parse(parser.Read("ForceWinFormsCompatibility").ReadResponse().ReplaceSpaces());
				}
				catch
				{
				}
				try
				{
					Kappa.DontRename = bool.Parse(parser.Read("DontRename").ReadResponse().ReplaceSpaces());
				}
				catch
				{
				}
				try
				{
					ProxyAdder.Intensity = int.Parse(parser.Read("ProxyCallsIntensity").ReadResponse().ReplaceSpaces());
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

		// Token: 0x04000002 RID: 2
		private static string FileExtensionk__BackingField;

		// Token: 0x04000005 RID: 5
		private static string FilePathk__BackingField;

		// Token: 0x04000006 RID: 6
		public static MemoryStream Stream = new MemoryStream();
	}
}
