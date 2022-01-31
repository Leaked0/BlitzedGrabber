using System;
using BlitzedConfuser.Utils;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Protections
{
	// Token: 0x02000016 RID: 22
	public class IntEncoding : Protection
	{
		// Token: 0x06000042 RID: 66 RVA: 0x00003D79 File Offset: 0x00001F79
		public IntEncoding()
		{
			base.Name = "Integer Encoding";
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00003D8C File Offset: 0x00001F8C
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00003D94 File Offset: 0x00001F94
		private int Amount { get; set; }

		// Token: 0x06000045 RID: 69 RVA: 0x00003DA0 File Offset: 0x00001FA0
		public override void Execute()
		{
			IMethod absMethod = Kappa.Module.Import(typeof(Math).GetMethod("Abs", new Type[] { typeof(int) }));
			IMethod minMethod = Kappa.Module.Import(typeof(Math).GetMethod("Min", new Type[]
			{
				typeof(int),
				typeof(int)
			}));
			foreach (TypeDef typeDef in Kappa.Module.Types)
			{
				foreach (MethodDef method in typeDef.Methods)
				{
					if (method.HasBody)
					{
						for (int i = 0; i < method.Body.Instructions.Count; i++)
						{
							if (method.Body.Instructions[i] != null && method.Body.Instructions[i].IsLdcI4())
							{
								int operand = method.Body.Instructions[i].GetLdcI4Value();
								if (operand > 0)
								{
									method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(absMethod));
									int neg = Randomizer.Next(MemberRenamer.StringLength(), 8);
									if (neg % 2 != 0)
									{
										neg++;
									}
									for (int j = 0; j < neg; j++)
									{
										method.Body.Instructions.Insert(i + j + 1, Instruction.Create(OpCodes.Neg));
									}
									if (operand < 2147483647)
									{
										method.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(int.MaxValue));
										method.Body.Instructions.Insert(i + 2, OpCodes.Call.ToInstruction(minMethod));
									}
									int amount = this.Amount + 1;
									this.Amount = amount;
								}
							}
						}
					}
				}
			}
			Console.WriteLine(string.Format("  Encoded {0} ints.", this.Amount));
		}
	}
}
