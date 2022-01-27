using System;
using BlitzedConfuser.Utils;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Protections
{
	// Token: 0x0200001A RID: 26
	public class ProxyAdder : Protection
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00004805 File Offset: 0x00002C05
		public ProxyAdder()
		{
			base.Name = "Proxy Adder";
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00004818 File Offset: 0x00002C18
		// (set) Token: 0x06000052 RID: 82 RVA: 0x0000481F File Offset: 0x00002C1F
		public static int Intensity { get; set; } = 2;

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00004827 File Offset: 0x00002C27
		// (set) Token: 0x06000054 RID: 84 RVA: 0x0000482F File Offset: 0x00002C2F
		private int Amount { get; set; }

		// Token: 0x06000055 RID: 85 RVA: 0x00004838 File Offset: 0x00002C38
		public override void Execute()
		{
			for (int i = 0; i < ProxyAdder.Intensity; i++)
			{
				foreach (TypeDef typeDef in Kappa.Module.Types)
				{
					for (int j = 0; j < typeDef.Methods.Count; j++)
					{
						MethodDef methodDef = typeDef.Methods[j];
						if (methodDef.HasBody)
						{
							for (int k = 0; k < methodDef.Body.Instructions.Count; k++)
							{
								if (methodDef.Body.Instructions[k].OpCode == OpCodes.Call)
								{
									MethodDef methodDef2 = methodDef.Body.Instructions[k].Operand as MethodDef;
									if (methodDef2 != null && methodDef2.FullName.Contains(Kappa.Module.Assembly.Name) && methodDef2.Parameters.Count <= 4)
									{
										MethodDef methodDef3 = methodDef2.CopyMethod(Kappa.Module);
										methodDef2.DeclaringType.Methods.Add(methodDef3);
										ProxyExtension.CloneSignature(methodDef2, methodDef3);
										CilBody cilBody = new CilBody();
										cilBody.Instructions.Add(OpCodes.Nop.ToInstruction());
										if (methodDef2.Parameters.Count > 0)
										{
											for (int l = 0; l < methodDef2.Parameters.Count; l++)
											{
												switch (l)
												{
												case 0:
													cilBody.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
													break;
												case 1:
													cilBody.Instructions.Add(OpCodes.Ldarg_1.ToInstruction());
													break;
												case 2:
													cilBody.Instructions.Add(OpCodes.Ldarg_2.ToInstruction());
													break;
												case 3:
													cilBody.Instructions.Add(OpCodes.Ldarg_3.ToInstruction());
													break;
												}
											}
										}
										cilBody.Instructions.Add(OpCodes.Call.ToInstruction(methodDef3));
										cilBody.Instructions.Add(OpCodes.Ret.ToInstruction());
										methodDef2.Body = cilBody;
										int amount = this.Amount + 1;
										this.Amount = amount;
									}
								}
							}
						}
					}
				}
			}
			Console.WriteLine(string.Format("  Added {0} proxy calls.", this.Amount));
		}
	}
}
