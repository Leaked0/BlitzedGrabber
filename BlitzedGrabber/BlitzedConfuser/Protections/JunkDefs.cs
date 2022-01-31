using System;
using BlitzedConfuser.Utils;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Protections
{
	// Token: 0x02000018 RID: 24
	public class JunkDefs : Protection
	{
		// Token: 0x06000048 RID: 72 RVA: 0x000045C8 File Offset: 0x000027C8
		public JunkDefs()
		{
			base.Name = "Junk Defs";
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000045DC File Offset: 0x000027DC
		public override void Execute()
		{
			for (int i = 0; i < MemberRenamer.StringLength(); i++)
			{
				TypeDef type = new TypeDefUser("STONEDEAGLEONTOPLOLFUCKQIZQHEGAYCUH" + Randomizer.String(9))
				{
					Namespace = string.Empty
				};
				Kappa.Module.Types.Add(type);
				this.Amount++;
			}
			foreach (TypeDef type2 in Kappa.Module.Types)
			{
				for (int j = 0; j < MemberRenamer.StringLength(); j++)
				{
					MethodDef strings = this.CreateNewJunkMethod("STVNEDEAGLERUNSULOLWTF" + Randomizer.String(6));
					MethodDef ints = this.CreateNewJunkMethod(MemberRenamer.StringLength());
					type2.Methods.Add(strings);
					type2.Methods.Add(ints);
					this.Amount += 2;
				}
			}
			Console.WriteLine(string.Format("  Added {0} junk defs.", this.Amount));
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004700 File Offset: 0x00002900
		private MethodDef CreateNewJunkMethod(object value)
		{
			CorLibTypeSig corlib = null;
			if (value is int)
			{
				corlib = Kappa.Module.CorLibTypes.Int32;
			}
			else if (value is string)
			{
				corlib = Kappa.Module.CorLibTypes.String;
			}
			MethodDef newMethod = new MethodDefUser("STVNEDEAGLEMADETHISGODDAMNTHISWASHARTWATERMARK" + Randomizer.String(MemberRenamer.StringLength()), MethodSig.CreateStatic(corlib), MethodImplAttributes.IL, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static | MethodAttributes.HideBySig)
			{
				Body = new CilBody()
			};
			if (value is int)
			{
				newMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, Convert.ToInt32(value)));
			}
			else if (value is string)
			{
				newMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldstr, value.ToString()));
			}
			newMethod.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
			return newMethod;
		}

		// Token: 0x04000012 RID: 18
		private int Amount;
	}
}
