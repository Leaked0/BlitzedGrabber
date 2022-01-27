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
		// Token: 0x0600003B RID: 59 RVA: 0x00003844 File Offset: 0x00001C44
		public AntiTamper()
		{
			base.Name = "Anti-Tamper";
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00003857 File Offset: 0x00001C57
		// (set) Token: 0x0600003D RID: 61 RVA: 0x0000385E File Offset: 0x00001C5E
		public static bool Tampered { get; set; }

		// Token: 0x0600003E RID: 62 RVA: 0x00003868 File Offset: 0x00001C68
		public static void Inject(string filePath)
		{
			using (MD5 md = MD5.Create())
			{
				byte[] array = md.ComputeHash(File.ReadAllBytes(filePath));
				using (FileStream fileStream = new FileStream(filePath, FileMode.Append))
				{
					fileStream.Write(array, 0, array.Length);
				}
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000038D0 File Offset: 0x00001CD0
		public override void Execute()
		{
			MethodDef methodDef = (MethodDef)InjectHelper.Inject(ModuleDefMD.Load(typeof(TamperClass).Module).ResolveTypeDef(MDToken.ToRID(typeof(TamperClass).MetadataToken)), Kappa.Module.GlobalType, Kappa.Module).Single((IDnlibDef method) => method.Name == "NoTampering");
			MemberRenamer.GetRenamed(methodDef);
			Kappa.Module.GlobalType.FindOrCreateStaticConstructor().Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, methodDef));
			foreach (MethodDef methodDef2 in Kappa.Module.GlobalType.Methods)
			{
				if (methodDef2.Name.Equals(".ctor"))
				{
					Kappa.Module.GlobalType.Remove(methodDef2);
					break;
				}
			}
			AntiTamper.Tampered = true;
		}
	}
}
