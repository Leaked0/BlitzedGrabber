using System;
using BlitzedConfuser.Utils;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Protections
{
	// Token: 0x02000016 RID: 22
	public class IntEncoding : Protection
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00003D81 File Offset: 0x00002181
		public IntEncoding()
		{
			base.Name = "Integer Encoding";
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003D94 File Offset: 0x00002194
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00003D9C File Offset: 0x0000219C
		private int Amount { get; set; }

		// Token: 0x06000046 RID: 70 RVA: 0x00003DA8 File Offset: 0x000021A8
		public override void Execute()
		{
			IMethod method = Kappa.Module.Import(typeof(Math).GetMethod("Abs", new Type[] { typeof(int) }));
			IMethod method2 = Kappa.Module.Import(typeof(Math).GetMethod("Min", new Type[]
			{
				typeof(int),
				typeof(int)
			}));
			foreach (TypeDef typeDef in Kappa.Module.Types)
			{
				foreach (MethodDef methodDef in typeDef.Methods)
				{
					if (methodDef.HasBody)
					{
						for (int i = 0; i < methodDef.Body.Instructions.Count; i++)
						{
							if (methodDef.Body.Instructions[i] != null && methodDef.Body.Instructions[i].IsLdcI4())
							{
								int ldcI4Value = methodDef.Body.Instructions[i].GetLdcI4Value();
								if (ldcI4Value > 0)
								{
									methodDef.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(method));
									int num = Randomizer.Next(MemberRenamer.StringLength(), 8);
									if (num % 2 != 0)
									{
										num++;
									}
									for (int j = 0; j < num; j++)
									{
										methodDef.Body.Instructions.Insert(i + j + 1, Instruction.Create(OpCodes.Neg));
									}
									if (ldcI4Value < 2147483647)
									{
										methodDef.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(int.MaxValue));
										methodDef.Body.Instructions.Insert(i + 2, OpCodes.Call.ToInstruction(method2));
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
