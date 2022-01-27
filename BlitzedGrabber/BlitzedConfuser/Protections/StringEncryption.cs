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
		// Token: 0x06000064 RID: 100 RVA: 0x00004FBE File Offset: 0x000033BE
		public StringEncryption()
		{
			base.Name = "String Encryption";
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00004FD1 File Offset: 0x000033D1
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00004FD9 File Offset: 0x000033D9
		private int Amount { get; set; }

		// Token: 0x06000067 RID: 103 RVA: 0x00004FE4 File Offset: 0x000033E4
		public override void Execute()
		{
			MethodDef methodDef = (MethodDef)InjectHelper.Inject(ModuleDefMD.Load(typeof(StringDecoder).Module).ResolveTypeDef(MDToken.ToRID(typeof(StringDecoder).MetadataToken)), Kappa.Module.GlobalType, Kappa.Module).Single((IDnlibDef method) => method.Name == "Decrypt");
			MemberRenamer.GetRenamed(methodDef);
			foreach (MethodDef methodDef2 in Kappa.Module.GlobalType.Methods)
			{
				if (methodDef2.Name.Equals(".ctor"))
				{
					Kappa.Module.GlobalType.Remove(methodDef2);
					break;
				}
			}
			foreach (TypeDef typeDef in Kappa.Module.Types)
			{
				foreach (MethodDef methodDef3 in typeDef.Methods)
				{
					if (methodDef3.HasBody)
					{
						methodDef3.Body.SimplifyBranches();
						for (int i = 0; i < methodDef3.Body.Instructions.Count; i++)
						{
							if (methodDef3.Body.Instructions[i] != null && methodDef3.Body.Instructions[i].OpCode == OpCodes.Ldstr)
							{
								int num = Randomizer.Next();
								object operand = methodDef3.Body.Instructions[i].Operand;
								if (operand != null)
								{
									methodDef3.Body.Instructions[i].Operand = this.Encrypt(operand.ToString(), num);
									methodDef3.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(Randomizer.Next()));
									methodDef3.Body.Instructions.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(num));
									methodDef3.Body.Instructions.Insert(i + 3, OpCodes.Ldc_I4.ToInstruction(Randomizer.Next()));
									methodDef3.Body.Instructions.Insert(i + 4, OpCodes.Ldc_I4.ToInstruction(Randomizer.Next()));
									methodDef3.Body.Instructions.Insert(i + 5, OpCodes.Ldc_I4.ToInstruction(Randomizer.Next()));
									methodDef3.Body.Instructions.Insert(i + 6, OpCodes.Call.ToInstruction(methodDef));
									int amount = this.Amount + 1;
									this.Amount = amount;
								}
							}
						}
						methodDef3.Body.OptimizeBranches();
					}
				}
			}
			Console.WriteLine(string.Format("  Encrypted {0} strings.", this.Amount));
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000533C File Offset: 0x0000373C
		private string Encrypt(string str, int key)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in str.ToCharArray())
			{
				stringBuilder.Append((char)((int)c + key));
			}
			return stringBuilder.ToString();
		}
	}
}
