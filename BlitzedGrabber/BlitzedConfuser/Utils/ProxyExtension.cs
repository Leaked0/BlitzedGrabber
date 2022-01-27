using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Utils
{
	// Token: 0x02000006 RID: 6
	public static class ProxyExtension
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002D5E File Offset: 0x0000115E
		public static MethodDef CloneSignature(MethodDef from, MethodDef to)
		{
			to.Attributes = from.Attributes;
			if (from.IsHideBySig)
			{
				to.IsHideBySig = true;
			}
			if (from.IsStatic)
			{
				to.IsStatic = true;
			}
			return to;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002D8C File Offset: 0x0000118C
		private static Func<Instruction, Instruction> nine__zero;
		public static MethodDef CopyMethod(this MethodDef originMethod, ModuleDef mod)
		{
			InjectContext injectContext = new InjectContext(mod, mod);
			MethodDefUser methodDefUser = new MethodDefUser
			{
				Signature = injectContext.Importer.Import(originMethod.Signature),
				Name = Randomizer.String(MemberRenamer.StringLength())
			};
			methodDefUser.Parameters.UpdateParameterTypes();
			if (originMethod.ImplMap != null)
			{
				methodDefUser.ImplMap = new ImplMapUser(new ModuleRefUser(injectContext.TargetModule, originMethod.ImplMap.Module.Name), originMethod.ImplMap.Name, originMethod.ImplMap.Attributes);
			}
			foreach (CustomAttribute customAttribute in originMethod.CustomAttributes)
			{
				methodDefUser.CustomAttributes.Add(new CustomAttribute((ICustomAttributeType)injectContext.Importer.Import(customAttribute.Constructor)));
			}
			if (originMethod.HasBody)
			{
				methodDefUser.Body = new CilBody
				{
					InitLocals = originMethod.Body.InitLocals,
					MaxStack = originMethod.Body.MaxStack
				};
				Dictionary<object, object> bodyMap = new Dictionary<object, object>();
				foreach (Local local in originMethod.Body.Variables)
				{
					Local local2 = new Local(injectContext.Importer.Import(local.Type));
					methodDefUser.Body.Variables.Add(local2);
					local2.Name = local.Name;
					bodyMap[local] = local2;
				}
				foreach (Instruction instruction in originMethod.Body.Instructions)
				{
					Instruction instruction2 = new Instruction(instruction.OpCode, instruction.Operand)
					{
						SequencePoint = instruction.SequencePoint
					};
					if (instruction2.Operand is IType)
					{
						instruction2.Operand = injectContext.Importer.Import((IType)instruction2.Operand);
					}
					else if (instruction2.Operand is IMethod)
					{
						instruction2.Operand = injectContext.Importer.Import((IMethod)instruction2.Operand);
					}
					else if (instruction2.Operand is IField)
					{
						instruction2.Operand = injectContext.Importer.Import((IField)instruction2.Operand);
					}
					methodDefUser.Body.Instructions.Add(instruction2);
					bodyMap[instruction] = instruction2;
				}
				foreach (Instruction instruction3 in methodDefUser.Body.Instructions)
				{
					if (instruction3.Operand != null && bodyMap.ContainsKey(instruction3.Operand))
					{
						instruction3.Operand = bodyMap[instruction3.Operand];
					}
					else if (instruction3.Operand is Instruction[])
					{
						Instruction instruction4 = instruction3;
						IEnumerable<Instruction> source = (Instruction[])instruction3.Operand;
						Func<Instruction, Instruction> selector;
						if ((selector = nine__zero) == null)
						{
							selector = (nine__zero = (Instruction target) => (Instruction)bodyMap[target]);
						}
						instruction4.Operand = source.Select(selector).ToArray<Instruction>();
					}
				}
				foreach (ExceptionHandler exceptionHandler in originMethod.Body.ExceptionHandlers)
				{
					methodDefUser.Body.ExceptionHandlers.Add(new ExceptionHandler(exceptionHandler.HandlerType)
					{
						CatchType = ((exceptionHandler.CatchType == null) ? null : injectContext.Importer.Import(exceptionHandler.CatchType)),
						TryStart = (Instruction)bodyMap[exceptionHandler.TryStart],
						TryEnd = (Instruction)bodyMap[exceptionHandler.TryEnd],
						HandlerStart = (Instruction)bodyMap[exceptionHandler.HandlerStart],
						HandlerEnd = (Instruction)bodyMap[exceptionHandler.HandlerEnd],
						FilterStart = ((exceptionHandler.FilterStart == null) ? null : ((Instruction)bodyMap[exceptionHandler.FilterStart]))
					});
				}
				methodDefUser.Body.SimplifyMacros(methodDefUser.Parameters);
			}
			return methodDefUser;
		}
	}
}
