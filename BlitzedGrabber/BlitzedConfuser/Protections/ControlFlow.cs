using System;
using BlitzedConfuser.Utils;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Protections
{
	// Token: 0x02000015 RID: 21
	public class ControlFlow : Protection
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000039E4 File Offset: 0x00001BE4
		public ControlFlow()
		{
			base.Name = "Control Flow";
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003A80 File Offset: 0x00001C80
		public override void Execute()
		{
			int amount = 0;
			for (int x = 0; x < Kappa.Module.Types.Count; x++)
			{
				TypeDef tDef = Kappa.Module.Types[x];
				for (int i = 0; i < tDef.Methods.Count; i++)
				{
					MethodDef mDef = tDef.Methods[i];
					if (!mDef.Name.StartsWith("get_") && !mDef.Name.StartsWith("set_") && mDef.HasBody && !mDef.IsConstructor)
					{
						mDef.Body.SimplifyBranches();
						this.ExecuteMethod(mDef);
						amount++;
					}
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003B34 File Offset: 0x00001D34
		private void ExecuteMethod(MethodDef method)
		{
			for (int i = 0; i < method.Body.Instructions.Count; i++)
			{
				if (method.Body.Instructions[i].IsLdcI4())
				{
					int numorig = Randomizer.Next();
					int div = Randomizer.Next();
					int num = numorig ^ div;
					int lastIndex = Randomizer.Next(this.types.Length, 0);
					Type randType = this.types[lastIndex];
					Instruction nop = OpCodes.Nop.ToInstruction();
					Local local = new Local(method.Module.ImportAsTypeSig(randType));
					Instruction localCode = OpCodes.Stloc.ToInstruction(local);
					method.Body.Variables.Add(local);
					method.Body.Instructions.Insert(i + 1, localCode);
					method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Ldc_I4, method.Body.Instructions[i].GetLdcI4Value() - this.sizes[lastIndex]));
					method.Body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Ldc_I4, num));
					method.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Ldc_I4, div));
					method.Body.Instructions.Insert(i + 5, Instruction.Create(OpCodes.Xor));
					method.Body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Ldc_I4, numorig));
					method.Body.Instructions.Insert(i + 7, Instruction.Create(OpCodes.Bne_Un, nop));
					method.Body.Instructions.Insert(i + 8, Instruction.Create(OpCodes.Ldc_I4, 2));
					method.Body.Instructions.Insert(i + 9, localCode);
					method.Body.Instructions.Insert(i + 10, Instruction.Create(OpCodes.Sizeof, method.Module.Import(randType)));
					method.Body.Instructions.Insert(i + 11, Instruction.Create(OpCodes.Add));
					method.Body.Instructions.Insert(i + 12, nop);
					i += method.Body.Instructions.Count - i;
				}
			}
		}

		// Token: 0x0400000F RID: 15
		private readonly Type[] types = new Type[]
		{
			typeof(uint),
			typeof(int),
			typeof(long),
			typeof(ulong),
			typeof(ushort),
			typeof(short),
			typeof(double)
		};

		// Token: 0x04000010 RID: 16
		private readonly int[] sizes = new int[] { 4, 4, 8, 8, 2, 2, 8 };
	}
}
