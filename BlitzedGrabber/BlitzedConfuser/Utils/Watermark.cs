using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Utils
{
	// Token: 0x0200000B RID: 11
	public class Watermark
	{
		// Token: 0x06000029 RID: 41 RVA: 0x00003488 File Offset: 0x00001888
		public static void AddAttribute()
		{
			TypeRef typeRef = Kappa.Module.CorLibTypes.GetTypeRef("System", "Attribute");
			TypeDefUser typeDefUser = new TypeDefUser(string.Empty, "BlitzedAttribute", typeRef);
			Kappa.Module.Types.Add(typeDefUser);
			MethodDefUser methodDefUser = new MethodDefUser(".ctor", MethodSig.CreateInstance(Kappa.Module.CorLibTypes.Void, Kappa.Module.CorLibTypes.String), MethodImplAttributes.IL, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName)
			{
				Body = new CilBody
				{
					MaxStack = 1
				}
			};
			methodDefUser.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
			methodDefUser.Body.Instructions.Add(OpCodes.Call.ToInstruction(new MemberRefUser(Kappa.Module, ".ctor", MethodSig.CreateInstance(Kappa.Module.CorLibTypes.Void), typeRef)));
			methodDefUser.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
			typeDefUser.Methods.Add(methodDefUser);
			CustomAttribute customAttribute = new CustomAttribute(methodDefUser);
			customAttribute.ConstructorArguments.Add(new CAArgument(Kappa.Module.CorLibTypes.String, string.Concat(new string[]
			{
				"Obfuscated with ",
				Reference.Name,
				" version ",
				Reference.Version,
				"."
			})));
			Kappa.Module.CustomAttributes.Add(customAttribute);
		}
	}
}
