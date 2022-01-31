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
		// Token: 0x06000010 RID: 16 RVA: 0x0000235C File Offset: 0x0000055C
		public static TypeDefUser Clone(TypeDef origin)
		{
			TypeDefUser ret = new TypeDefUser(origin.Namespace, origin.Name)
			{
				Attributes = origin.Attributes
			};
			if (origin.ClassLayout != null)
			{
				ret.ClassLayout = new ClassLayoutUser(origin.ClassLayout.PackingSize, origin.ClassSize);
			}
			foreach (GenericParam genericParam in origin.GenericParameters)
			{
				ret.GenericParameters.Add(new GenericParamUser(genericParam.Number, genericParam.Flags, "-"));
			}
			return ret;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000240C File Offset: 0x0000060C
		public static MethodDefUser Clone(MethodDef origin)
		{
			MethodDefUser ret = new MethodDefUser(origin.Name, null, origin.ImplAttributes, origin.Attributes);
			foreach (GenericParam genericParam in origin.GenericParameters)
			{
				ret.GenericParameters.Add(new GenericParamUser(genericParam.Number, genericParam.Flags, "-"));
			}
			return ret;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002494 File Offset: 0x00000694
		public static FieldDefUser Clone(FieldDef origin)
		{
			return new FieldDefUser(origin.Name, null, origin.Attributes);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000024A8 File Offset: 0x000006A8
		public static TypeDef PopulateContext(TypeDef typeDef, InjectContext ctx)
		{
			IDnlibDef existing;
			TypeDef ret;
			if (!ctx.Map.TryGetValue(typeDef, out existing))
			{
				ret = InjectHelper.Clone(typeDef);
				ctx.Map[typeDef] = ret;
			}
			else
			{
				ret = (TypeDef)existing;
			}
			foreach (TypeDef nestedType in typeDef.NestedTypes)
			{
				ret.NestedTypes.Add(InjectHelper.PopulateContext(nestedType, ctx));
			}
			foreach (MethodDef method in typeDef.Methods)
			{
				ret.Methods.Add((MethodDef)(ctx.Map[method] = InjectHelper.Clone(method)));
			}
			foreach (FieldDef field in typeDef.Fields)
			{
				ret.Fields.Add((FieldDef)(ctx.Map[field] = InjectHelper.Clone(field)));
			}
			return ret;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000025F8 File Offset: 0x000007F8
		public static void CopyTypeDef(TypeDef typeDef, InjectContext ctx)
		{
			TypeDef newTypeDef = (TypeDef)ctx.Map[typeDef];
			newTypeDef.BaseType = ctx.Importer.Import(typeDef.BaseType);
			foreach (InterfaceImpl iface in typeDef.Interfaces)
			{
				newTypeDef.Interfaces.Add(new InterfaceImplUser(ctx.Importer.Import(iface.Interface)));
			}
		}

		private static Func<Instruction, Instruction> nine__zero;
		// Token: 0x06000015 RID: 21 RVA: 0x00002690 File Offset: 0x00000890
		public static void CopyMethodDef(MethodDef methodDef, InjectContext ctx)
		{
			MethodDef newMethodDef = (MethodDef)ctx.Map[methodDef];
			newMethodDef.Signature = ctx.Importer.Import(methodDef.Signature);
			newMethodDef.Parameters.UpdateParameterTypes();
			if (methodDef.ImplMap != null)
			{
				newMethodDef.ImplMap = new ImplMapUser(new ModuleRefUser(ctx.TargetModule, methodDef.ImplMap.Module.Name), methodDef.ImplMap.Name, methodDef.ImplMap.Attributes);
			}
			foreach (CustomAttribute ca in methodDef.CustomAttributes)
			{
				newMethodDef.CustomAttributes.Add(new CustomAttribute((ICustomAttributeType)ctx.Importer.Import(ca.Constructor)));
			}
			if (methodDef.HasBody)
			{
				newMethodDef.Body = new CilBody(methodDef.Body.InitLocals, new List<Instruction>(), new List<ExceptionHandler>(), new List<Local>());
				newMethodDef.Body.MaxStack = methodDef.Body.MaxStack;
				Dictionary<object, object> bodyMap = new Dictionary<object, object>();
				foreach (Local local in methodDef.Body.Variables)
				{
					Local newLocal = new Local(ctx.Importer.Import(local.Type));
					newMethodDef.Body.Variables.Add(newLocal);
					newLocal.Name = local.Name;
					newLocal.Attributes = local.Attributes;
					bodyMap[local] = newLocal;
				}
				foreach (Instruction instr in methodDef.Body.Instructions)
				{
					Instruction newInstr = new Instruction(instr.OpCode, instr.Operand)
					{
						SequencePoint = instr.SequencePoint
					};
					if (newInstr.Operand is IType)
					{
						newInstr.Operand = ctx.Importer.Import((IType)newInstr.Operand);
					}
					else if (newInstr.Operand is IMethod)
					{
						newInstr.Operand = ctx.Importer.Import((IMethod)newInstr.Operand);
					}
					else if (newInstr.Operand is IField)
					{
						newInstr.Operand = ctx.Importer.Import((IField)newInstr.Operand);
					}
					newMethodDef.Body.Instructions.Add(newInstr);
					bodyMap[instr] = newInstr;
				}
				foreach (Instruction instr2 in newMethodDef.Body.Instructions)
				{
					if (instr2.Operand != null && bodyMap.ContainsKey(instr2.Operand))
					{
						instr2.Operand = bodyMap[instr2.Operand];
					}
					else if (instr2.Operand is Instruction[])
					{
						Instruction instruction = instr2;
						IEnumerable<Instruction> source = (Instruction[])instr2.Operand;
						Func<Instruction, Instruction> selector;
						if ((selector = nine__zero) == null)
						{
							selector = (nine__zero = (Instruction target) => (Instruction)bodyMap[target]);
						}
						instruction.Operand = source.Select(selector).ToArray<Instruction>();
					}
				}
				foreach (ExceptionHandler eh in methodDef.Body.ExceptionHandlers)
				{
					newMethodDef.Body.ExceptionHandlers.Add(new ExceptionHandler(eh.HandlerType)
					{
						CatchType = ((eh.CatchType == null) ? null : ctx.Importer.Import(eh.CatchType)),
						TryStart = (Instruction)bodyMap[eh.TryStart],
						TryEnd = (Instruction)bodyMap[eh.TryEnd],
						HandlerStart = (Instruction)bodyMap[eh.HandlerStart],
						HandlerEnd = (Instruction)bodyMap[eh.HandlerEnd],
						FilterStart = ((eh.FilterStart == null) ? null : ((Instruction)bodyMap[eh.FilterStart]))
					});
				}
				newMethodDef.Body.SimplifyMacros(newMethodDef.Parameters);
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002BD8 File Offset: 0x00000DD8
		public static void CopyFieldDef(FieldDef fieldDef, InjectContext ctx)
		{
			((FieldDef)ctx.Map[fieldDef]).Signature = ctx.Importer.Import(fieldDef.Signature);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002C10 File Offset: 0x00000E10
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

		// Token: 0x06000018 RID: 24 RVA: 0x00002CDC File Offset: 0x00000EDC
		public static IEnumerable<IDnlibDef> Inject(TypeDef typeDef, TypeDef newType, ModuleDef target)
		{
			InjectContext ctx = new InjectContext(typeDef.Module, target);
			ctx.Map[typeDef] = newType;
			InjectHelper.PopulateContext(typeDef, ctx);
			InjectHelper.Copy(typeDef, ctx, false);
			return ctx.Map.Values.Except(new TypeDef[] { newType });
		}
	}
}
