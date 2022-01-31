using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using BlitzedConfuser.Utils;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Protections
{
	// Token: 0x02000014 RID: 20
	public class AntiTamper : Protection
	{
		// Token: 0x0600003A RID: 58 RVA: 0x0000383C File Offset: 0x00001A3C
		public AntiTamper()
		{
			base.Name = "Anti-Tamper";
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600003B RID: 59 RVA: 0x0000384F File Offset: 0x00001A4F
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00003856 File Offset: 0x00001A56
		public static bool Tampered { get; set; }

		// Token: 0x0600003D RID: 61 RVA: 0x00003860 File Offset: 0x00001A60
		public static void Inject(string filePath)
		{
			using (MD5 hash = MD5.Create())
			{
				byte[] bytes = hash.ComputeHash(File.ReadAllBytes(filePath));
				using (FileStream fs = new FileStream(filePath, FileMode.Append))
				{
					fs.Write(bytes, 0, bytes.Length);
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000038C8 File Offset: 0x00001AC8
		public override void Execute()
		{
			MethodDef init = (MethodDef)InjectHelper.Inject(ModuleDefMD.Load(typeof(TamperClass).Module).ResolveTypeDef(MDToken.ToRID(typeof(TamperClass).MetadataToken)), Kappa.Module.GlobalType, Kappa.Module).Single((IDnlibDef method) => method.Name == "NoTampering");
			init.GetRenamed();
			Kappa.Module.GlobalType.FindOrCreateStaticConstructor().Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, init));
			foreach (MethodDef method2 in Kappa.Module.GlobalType.Methods)
			{
				if (method2.Name.Equals(".ctor"))
				{
					Kappa.Module.GlobalType.Remove(method2);
					break;
				}
			}
			AntiTamper.Tampered = true;
		}
	}
}
