using System;
using BlitzedConfuser.Utils;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Protections
{
	// Token: 0x02000018 RID: 24
	public class JunkDefs : Protection
	{
		// Token: 0x06000049 RID: 73 RVA: 0x000045D0 File Offset: 0x000029D0
		public JunkDefs()
		{
			base.Name = "Junk Defs";
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000045E4 File Offset: 0x000029E4
		public override void Execute()
		{
			for (int i = 0; i < MemberRenamer.StringLength(); i++)
			{
				TypeDef item = new TypeDefUser("STONEDEAGLEONTOPLOLFUCKQIZQHEGAYCUH" + Randomizer.String(9))
				{
					Namespace = string.Empty
				};
				Kappa.Module.Types.Add(item);
				this.Amount++;
			}
			foreach (TypeDef typeDef in Kappa.Module.Types)
			{
				for (int j = 0; j < MemberRenamer.StringLength(); j++)
				{
					MethodDef item2 = this.CreateNewJunkMethod("STVNEDEAGLERUNSULOLWTF" + Randomizer.String(6));
					MethodDef item3 = this.CreateNewJunkMethod(MemberRenamer.StringLength());
					typeDef.Methods.Add(item2);
					typeDef.Methods.Add(item3);
					this.Amount += 2;
				}
			}
			Console.WriteLine(string.Format("  Added {0} junk defs.", this.Amount));
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004708 File Offset: 0x00002B08
		private MethodDef CreateNewJunkMethod(object value)
		{
			CorLibTypeSig retType = null;
			if (value is int)
			{
				retType = Kappa.Module.CorLibTypes.Int32;
			}
			else if (value is string)
			{
				retType = Kappa.Module.CorLibTypes.String;
			}
			MethodDef methodDef = new MethodDefUser("STVNEDEAGLEMADETHISGODDAMNTHISWASHARTWATERMARK" + Randomizer.String(MemberRenamer.StringLength()), MethodSig.CreateStatic(retType), MethodImplAttributes.IL, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static | MethodAttributes.HideBySig)
			{
				Body = new CilBody()
			};
			if (value is int)
			{
				methodDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, Convert.ToInt32(value)));
			}
			else if (value is string)
			{
				methodDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ldstr, value.ToString()));
			}
			methodDef.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
			return methodDef;
		}

		// Token: 0x04000012 RID: 18
		private int Amount;
	}
}
