using System;
using BlitzedConfuser.Utils;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Protections
{
	// Token: 0x0200001A RID: 26
	public class ProxyAdder : Protection
	{
		// Token: 0x0600004F RID: 79 RVA: 0x000047FD File Offset: 0x000029FD
		public ProxyAdder()
		{
			base.Name = "Proxy Adder";
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00004810 File Offset: 0x00002A10
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00004817 File Offset: 0x00002A17
		public static int Intensity { get; set; } = 2;

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000052 RID: 82 RVA: 0x0000481F File Offset: 0x00002A1F
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00004827 File Offset: 0x00002A27
		private int Amount { get; set; }

		// Token: 0x06000054 RID: 84 RVA: 0x00004830 File Offset: 0x00002A30
		public override void Execute()
		{
			for (int o = 0; o < ProxyAdder.Intensity; o++)
			{
				foreach (TypeDef t in Kappa.Module.Types)
				{
					for (int i = 0; i < t.Methods.Count; i++)
					{
						MethodDef j = t.Methods[i];
						if (j.HasBody)
						{
							for (int z = 0; z < j.Body.Instructions.Count; z++)
							{
								if (j.Body.Instructions[z].OpCode == OpCodes.Call)
								{
									MethodDef targetMethod = j.Body.Instructions[z].Operand as MethodDef;
									if (targetMethod != null && targetMethod.FullName.Contains(Kappa.Module.Assembly.Name) && targetMethod.Parameters.Count <= 4)
									{
										MethodDef newMeth = targetMethod.CopyMethod(Kappa.Module);
										targetMethod.DeclaringType.Methods.Add(newMeth);
										targetMethod.CloneSignature(newMeth);
										CilBody body = new CilBody();
										body.Instructions.Add(OpCodes.Nop.ToInstruction());
										if (targetMethod.Parameters.Count > 0)
										{
											for (int x = 0; x < targetMethod.Parameters.Count; x++)
											{
												switch (x)
												{
												case 0:
													body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
													break;
												case 1:
													body.Instructions.Add(OpCodes.Ldarg_1.ToInstruction());
													break;
												case 2:
													body.Instructions.Add(OpCodes.Ldarg_2.ToInstruction());
													break;
												case 3:
													body.Instructions.Add(OpCodes.Ldarg_3.ToInstruction());
													break;
												}
											}
										}
										body.Instructions.Add(OpCodes.Call.ToInstruction(newMeth));
										body.Instructions.Add(OpCodes.Ret.ToInstruction());
										targetMethod.Body = body;
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
