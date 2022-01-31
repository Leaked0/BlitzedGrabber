using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Protections
{
	// Token: 0x02000013 RID: 19
	public class AntiDe4dot : Protection
	{
		// Token: 0x06000038 RID: 56 RVA: 0x0000374B File Offset: 0x0000194B
		public AntiDe4dot()
		{
			base.Name = "Anti-De4dot";
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003760 File Offset: 0x00001960
		public override void Execute()
		{
			foreach (ModuleDef module in Kappa.Module.Assembly.Modules)
			{
				InterfaceImplUser @int = new InterfaceImplUser(module.GlobalType);
				for (int i = 0; i < 1; i++)
				{
					TypeDefUser typeDef = new TypeDefUser(string.Empty, string.Format("Form{0}", i), module.CorLibTypes.GetTypeRef("System", "Attribute"));
					InterfaceImplUser int2 = new InterfaceImplUser(typeDef);
					module.Types.Add(typeDef);
					typeDef.Interfaces.Add(int2);
					typeDef.Interfaces.Add(@int);
				}
			}
		}
	}
}
