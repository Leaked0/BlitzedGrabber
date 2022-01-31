using System;
using System.Linq;
using System.Text;
using BlitzedConfuser.Utils;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Protections
{
	// Token: 0x0200001C RID: 28
	public class StringEncryption : Protection
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00004FB6 File Offset: 0x000031B6
		public StringEncryption()
		{
			base.Name = "String Encryption";
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00004FC9 File Offset: 0x000031C9
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00004FD1 File Offset: 0x000031D1
		private int Amount { get; set; }

		// Token: 0x06000066 RID: 102 RVA: 0x00004FDC File Offset: 0x000031DC
		public override void Execute()
		{
			MethodDef init = (MethodDef)InjectHelper.Inject(ModuleDefMD.Load(typeof(StringDecoder).Module).ResolveTypeDef(MDToken.ToRID(typeof(StringDecoder).MetadataToken)), Kappa.Module.GlobalType, Kappa.Module).Single((IDnlibDef method) => method.Name == "Decrypt");
			init.GetRenamed();
			foreach (MethodDef method3 in Kappa.Module.GlobalType.Methods)
			{
				if (method3.Name.Equals(".ctor"))
				{
					Kappa.Module.GlobalType.Remove(method3);
					break;
				}
			}
			foreach (TypeDef typeDef in Kappa.Module.Types)
			{
				foreach (MethodDef method2 in typeDef.Methods)
				{
					if (method2.HasBody)
					{
						method2.Body.SimplifyBranches();
						for (int i = 0; i < method2.Body.Instructions.Count; i++)
						{
							if (method2.Body.Instructions[i] != null && method2.Body.Instructions[i].OpCode == OpCodes.Ldstr)
							{
								int key = Randomizer.Next();
								object op = method2.Body.Instructions[i].Operand;
								if (op != null)
								{
									method2.Body.Instructions[i].Operand = this.Encrypt(op.ToString(), key);
									method2.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(Randomizer.Next()));
									method2.Body.Instructions.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(key));
									method2.Body.Instructions.Insert(i + 3, OpCodes.Ldc_I4.ToInstruction(Randomizer.Next()));
									method2.Body.Instructions.Insert(i + 4, OpCodes.Ldc_I4.ToInstruction(Randomizer.Next()));
									method2.Body.Instructions.Insert(i + 5, OpCodes.Ldc_I4.ToInstruction(Randomizer.Next()));
									method2.Body.Instructions.Insert(i + 6, OpCodes.Call.ToInstruction(init));
									int amount = this.Amount + 1;
									this.Amount = amount;
								}
							}
						}
						method2.Body.OptimizeBranches();
					}
				}
			}
			Console.WriteLine(string.Format("  Encrypted {0} strings.", this.Amount));
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00005334 File Offset: 0x00003534
		private string Encrypt(string str, int key)
		{
			StringBuilder builder = new StringBuilder();
			foreach (char c in str.ToCharArray())
			{
				builder.Append((char)((int)c + key));
			}
			return builder.ToString();
		}
	}
}
