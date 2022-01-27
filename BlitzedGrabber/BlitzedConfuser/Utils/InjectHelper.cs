using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Utils
{
	// Token: 0x02000004 RID: 4
	public static class InjectHelper
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002364 File Offset: 0x00000764
		public static TypeDefUser Clone(TypeDef origin)
		{
			TypeDefUser typeDefUser = new TypeDefUser(origin.Namespace, origin.Name)
			{
				Attributes = origin.Attributes
			};
			if (origin.ClassLayout != null)
			{
				typeDefUser.ClassLayout = new ClassLayoutUser(origin.ClassLayout.PackingSize, origin.ClassSize);
			}
			foreach (GenericParam genericParam in origin.GenericParameters)
			{
				typeDefUser.GenericParameters.Add(new GenericParamUser(genericParam.Number, genericParam.Flags, "-"));
			}
			return typeDefUser;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002414 File Offset: 0x00000814
		public static MethodDefUser Clone(MethodDef origin)
		{
			MethodDefUser methodDefUser = new MethodDefUser(origin.Name, null, origin.ImplAttributes, origin.Attributes);
			foreach (GenericParam genericParam in origin.GenericParameters)
			{
				methodDefUser.GenericParameters.Add(new GenericParamUser(genericParam.Number, genericParam.Flags, "-"));
			}
			return methodDefUser;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000249C File Offset: 0x0000089C
		public static FieldDefUser Clone(FieldDef origin)
		{
			return new FieldDefUser(origin.Name, null, origin.Attributes);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000024B0 File Offset: 0x000008B0
		public static TypeDef PopulateContext(TypeDef typeDef, InjectContext ctx)
		{
			IDnlibDef dnlibDef;
			TypeDef typeDef2;
			if (!ctx.Map.TryGetValue(typeDef, out dnlibDef))
			{
				typeDef2 = InjectHelper.Clone(typeDef);
				ctx.Map[typeDef] = typeDef2;
			}
			else
			{
				typeDef2 = (TypeDef)dnlibDef;
			}
			foreach (TypeDef typeDef3 in typeDef.NestedTypes)
			{
				typeDef2.NestedTypes.Add(InjectHelper.PopulateContext(typeDef3, ctx));
			}
			foreach (MethodDef methodDef in typeDef.Methods)
			{
				typeDef2.Methods.Add((MethodDef)(ctx.Map[methodDef] = InjectHelper.Clone(methodDef)));
			}
			foreach (FieldDef fieldDef in typeDef.Fields)
			{
				typeDef2.Fields.Add((FieldDef)(ctx.Map[fieldDef] = InjectHelper.Clone(fieldDef)));
			}
			return typeDef2;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002600 File Offset: 0x00000A00
		public static void CopyTypeDef(TypeDef typeDef, InjectContext ctx)
		{
			TypeDef typeDef2 = (TypeDef)ctx.Map[typeDef];
			typeDef2.BaseType = ctx.Importer.Import(typeDef.BaseType);
			foreach (InterfaceImpl interfaceImpl in typeDef.Interfaces)
			{
				typeDef2.Interfaces.Add(new InterfaceImplUser(ctx.Importer.Import(interfaceImpl.Interface)));
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002698 File Offset: 0x00000A98
		private static Func<Instruction, Instruction> nine__zero;
		public static void CopyMethodDef(MethodDef methodDef, InjectContext ctx)
		{
			MethodDef methodDef2 = (MethodDef)ctx.Map[methodDef];
			methodDef2.Signature = ctx.Importer.Import(methodDef.Signature);
			methodDef2.Parameters.UpdateParameterTypes();
			if (methodDef.ImplMap != null)
			{
				methodDef2.ImplMap = new ImplMapUser(new ModuleRefUser(ctx.TargetModule, methodDef.ImplMap.Module.Name), methodDef.ImplMap.Name, methodDef.ImplMap.Attributes);
			}
			foreach (CustomAttribute customAttribute in methodDef.CustomAttributes)
			{
				methodDef2.CustomAttributes.Add(new CustomAttribute((ICustomAttributeType)ctx.Importer.Import(customAttribute.Constructor)));
			}
			if (methodDef.HasBody)
			{
				methodDef2.Body = new CilBody(methodDef.Body.InitLocals, new List<Instruction>(), new List<ExceptionHandler>(), new List<Local>());
				methodDef2.Body.MaxStack = methodDef.Body.MaxStack;
				Dictionary<object, object> bodyMap = new Dictionary<object, object>();
				foreach (Local local in methodDef.Body.Variables)
				{
					Local local2 = new Local(ctx.Importer.Import(local.Type));
					methodDef2.Body.Variables.Add(local2);
					local2.Name = local.Name;
					local2.Attributes = local.Attributes;
					bodyMap[local] = local2;
				}
				foreach (Instruction instruction in methodDef.Body.Instructions)
				{
					Instruction instruction2 = new Instruction(instruction.OpCode, instruction.Operand)
					{
						SequencePoint = instruction.SequencePoint
					};
					if (instruction2.Operand is IType)
					{
						instruction2.Operand = ctx.Importer.Import((IType)instruction2.Operand);
					}
					else if (instruction2.Operand is IMethod)
					{
						instruction2.Operand = ctx.Importer.Import((IMethod)instruction2.Operand);
					}
					else if (instruction2.Operand is IField)
					{
						instruction2.Operand = ctx.Importer.Import((IField)instruction2.Operand);
					}
					methodDef2.Body.Instructions.Add(instruction2);
					bodyMap[instruction] = instruction2;
				}
				foreach (Instruction instruction3 in methodDef2.Body.Instructions)
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
				foreach (ExceptionHandler exceptionHandler in methodDef.Body.ExceptionHandlers)
				{
					methodDef2.Body.ExceptionHandlers.Add(new ExceptionHandler(exceptionHandler.HandlerType)
					{
						CatchType = ((exceptionHandler.CatchType == null) ? null : ctx.Importer.Import(exceptionHandler.CatchType)),
						TryStart = (Instruction)bodyMap[exceptionHandler.TryStart],
						TryEnd = (Instruction)bodyMap[exceptionHandler.TryEnd],
						HandlerStart = (Instruction)bodyMap[exceptionHandler.HandlerStart],
						HandlerEnd = (Instruction)bodyMap[exceptionHandler.HandlerEnd],
						FilterStart = ((exceptionHandler.FilterStart == null) ? null : ((Instruction)bodyMap[exceptionHandler.FilterStart]))
					});
				}
				methodDef2.Body.SimplifyMacros(methodDef2.Parameters);
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002BE0 File Offset: 0x00000FE0
		public static void CopyFieldDef(FieldDef fieldDef, InjectContext ctx)
		{
			((FieldDef)ctx.Map[fieldDef]).Signature = ctx.Importer.Import(fieldDef.Signature);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002C18 File Offset: 0x00001018
		public static void Copy(TypeDef typeDef, InjectContext ctx, bool copySelf)
		{
			if (copySelf)
			{
				InjectHelper.CopyTypeDef(typeDef, ctx);
			}
			foreach (TypeDef typeDef2 in typeDef.NestedTypes)
			{
				InjectHelper.Copy(typeDef2, ctx, true);
			}
			foreach (MethodDef methodDef in typeDef.Methods)
			{
				InjectHelper.CopyMethodDef(methodDef, ctx);
			}
			foreach (FieldDef fieldDef in typeDef.Fields)
			{
				InjectHelper.CopyFieldDef(fieldDef, ctx);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002CE4 File Offset: 0x000010E4
		public static IEnumerable<IDnlibDef> Inject(TypeDef typeDef, TypeDef newType, ModuleDef target)
		{
			InjectContext injectContext = new InjectContext(typeDef.Module, target);
			injectContext.Map[typeDef] = newType;
			InjectHelper.PopulateContext(typeDef, injectContext);
			InjectHelper.Copy(typeDef, injectContext, false);
			return injectContext.Map.Values.Except(new TypeDef[] { newType });
		}
	}
}
