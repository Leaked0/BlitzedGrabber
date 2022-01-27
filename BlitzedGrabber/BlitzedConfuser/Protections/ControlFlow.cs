using System;
using BlitzedConfuser.Utils;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Protections
{
	// Token: 0x02000015 RID: 21
	public class ControlFlow : Protection
	{
		// Token: 0x06000040 RID: 64 RVA: 0x000039EC File Offset: 0x00001DEC
		public ControlFlow()
		{
			base.Name = "Control Flow";
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003A88 File Offset: 0x00001E88
		public override void Execute()
		{
			int num = 0;
			for (int i = 0; i < Kappa.Module.Types.Count; i++)
			{
				TypeDef typeDef = Kappa.Module.Types[i];
				for (int j = 0; j < typeDef.Methods.Count; j++)
				{
					MethodDef methodDef = typeDef.Methods[j];
					if (!methodDef.Name.StartsWith("get_") && !methodDef.Name.StartsWith("set_") && methodDef.HasBody && !methodDef.IsConstructor)
					{
						methodDef.Body.SimplifyBranches();
						this.ExecuteMethod(methodDef);
						num++;
					}
				}
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003B3C File Offset: 0x00001F3C
		private void ExecuteMethod(MethodDef method)
		{
			for (int i = 0; i < method.Body.Instructions.Count; i++)
			{
				if (method.Body.Instructions[i].IsLdcI4())
				{
					int num = Randomizer.Next();
					int num2 = Randomizer.Next();
					int value = num ^ num2;
					int num3 = Randomizer.Next(this.types.Length, 0);
					Type type = this.types[num3];
					Instruction instruction = OpCodes.Nop.ToInstruction();
					Local local = new Local(method.Module.ImportAsTypeSig(type));
					Instruction item = OpCodes.Stloc.ToInstruction(local);
					method.Body.Variables.Add(local);
					method.Body.Instructions.Insert(i + 1, item);
					method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Ldc_I4, method.Body.Instructions[i].GetLdcI4Value() - this.sizes[num3]));
					method.Body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Ldc_I4, value));
					method.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Ldc_I4, num2));
					method.Body.Instructions.Insert(i + 5, Instruction.Create(OpCodes.Xor));
					method.Body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Ldc_I4, num));
					method.Body.Instructions.Insert(i + 7, Instruction.Create(OpCodes.Bne_Un, instruction));
					method.Body.Instructions.Insert(i + 8, Instruction.Create(OpCodes.Ldc_I4, 2));
					method.Body.Instructions.Insert(i + 9, item);
					method.Body.Instructions.Insert(i + 10, Instruction.Create(OpCodes.Sizeof, method.Module.Import(type)));
					method.Body.Instructions.Insert(i + 11, Instruction.Create(OpCodes.Add));
					method.Body.Instructions.Insert(i + 12, instruction);
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
