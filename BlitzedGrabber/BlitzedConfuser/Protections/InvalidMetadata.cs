using System;
using BlitzedConfuser.Utils;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Protections
{
	// Token: 0x02000017 RID: 23
	public class InvalidMetadata : Protection
	{
		// Token: 0x06000047 RID: 71 RVA: 0x0000402C File Offset: 0x0000242C
		public InvalidMetadata()
		{
			base.Name = "Invalid Metadata";
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004040 File Offset: 0x00002440
		public override void Execute()
		{
			AssemblyDef assembly = Kappa.Module.Assembly;
			assembly.ManifestModule.Import(new FieldDefUser("STVNEDEAGLEMADETHISGODDAMNTHISWASHARTWATERMARK" + Randomizer.String(MemberRenamer.StringLength())));
			foreach (TypeDef typeDef in assembly.ManifestModule.Types)
			{
				TypeDefUser typeDefUser = new TypeDefUser("STVNEDEAGLEMADETHISGODDAMNTHISWASHARTWATERMARK" + Randomizer.String(MemberRenamer.StringLength()));
				typeDefUser.Methods.Add(new MethodDefUser());
				typeDefUser.NestedTypes.Add(new TypeDefUser("STVNEDEAGLEMADETHISGODDAMNTHISWASHARTWATERMARK" + Randomizer.String(MemberRenamer.StringLength())));
				MethodDefUser item = new MethodDefUser();
				typeDefUser.Methods.Add(item);
				typeDef.NestedTypes.Add(typeDefUser);
				typeDef.Events.Add(new EventDefUser());
				new MethodDefUser().MethodSig = new MethodSig();
				foreach (MethodDef methodDef in typeDef.Methods)
				{
					if (!methodDef.IsConstructor && methodDef.HasBody)
					{
						methodDef.Body.SimplifyBranches();
						if (methodDef.ReturnType.FullName == "System.Void" && methodDef.Body.Instructions.Count > 0 && !methodDef.Body.HasExceptionHandlers)
						{
							Local local = new Local(assembly.ManifestModule.Import(typeof(int)).ToTypeSig(true));
							Local local2 = new Local(assembly.ManifestModule.Import(typeof(bool)).ToTypeSig(true));
							methodDef.Body.Variables.Add(local);
							methodDef.Body.Variables.Add(local2);
							Instruction operand = methodDef.Body.Instructions[methodDef.Body.Instructions.Count - 1];
							Instruction item2 = new Instruction(OpCodes.Ret);
							Instruction instruction = new Instruction(OpCodes.Ldc_I4_1);
							methodDef.Body.Instructions.Insert(0, new Instruction(OpCodes.Ldc_I4_0));
							methodDef.Body.Instructions.Insert(1, new Instruction(OpCodes.Stloc, local));
							methodDef.Body.Instructions.Insert(2, new Instruction(OpCodes.Br, instruction));
							Instruction instruction2 = new Instruction(OpCodes.Ldloc, local);
							methodDef.Body.Instructions.Insert(3, instruction2);
							methodDef.Body.Instructions.Insert(4, new Instruction(OpCodes.Ldc_I4_0));
							methodDef.Body.Instructions.Insert(5, new Instruction(OpCodes.Ceq));
							methodDef.Body.Instructions.Insert(6, new Instruction(OpCodes.Ldc_I4_1));
							methodDef.Body.Instructions.Insert(7, new Instruction(OpCodes.Ceq));
							methodDef.Body.Instructions.Insert(8, new Instruction(OpCodes.Stloc, local2));
							methodDef.Body.Instructions.Insert(9, new Instruction(OpCodes.Ldloc, local2));
							methodDef.Body.Instructions.Insert(10, new Instruction(OpCodes.Brtrue, methodDef.Body.Instructions[10]));
							methodDef.Body.Instructions.Insert(11, new Instruction(OpCodes.Ret));
							methodDef.Body.Instructions.Insert(12, new Instruction(OpCodes.Calli));
							methodDef.Body.Instructions.Insert(13, new Instruction(OpCodes.Sizeof, operand));
							methodDef.Body.Instructions.Insert(14, new Instruction(OpCodes.Nop));
							methodDef.Body.Instructions.Insert(methodDef.Body.Instructions.Count, instruction);
							methodDef.Body.Instructions.Insert(methodDef.Body.Instructions.Count, new Instruction(OpCodes.Stloc, local2));
							methodDef.Body.Instructions.Insert(methodDef.Body.Instructions.Count, new Instruction(OpCodes.Br, instruction2));
							methodDef.Body.Instructions.Insert(methodDef.Body.Instructions.Count, item2);
							ExceptionHandler item3 = new ExceptionHandler(ExceptionHandlerType.Fault)
							{
								HandlerStart = methodDef.Body.Instructions[10],
								HandlerEnd = methodDef.Body.Instructions[11],
								TryEnd = methodDef.Body.Instructions[14],
								TryStart = methodDef.Body.Instructions[12]
							};
							methodDef.Body.ExceptionHandlers.Add(item3);
							methodDef.Body.OptimizeBranches();
							methodDef.Body.OptimizeMacros();
						}
					}
				}
			}
		}
	}
}
