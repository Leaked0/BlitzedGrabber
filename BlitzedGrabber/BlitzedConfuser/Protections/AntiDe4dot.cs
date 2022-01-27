using System;
using dnlib.DotNet;

namespace BlitzedConfuser.Protections
{
	// Token: 0x02000013 RID: 19
	public class AntiDe4dot : Protection
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00003753 File Offset: 0x00001B53
		public AntiDe4dot()
		{
			base.Name = "Anti-De4dot";
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003768 File Offset: 0x00001B68
		public override void Execute()
		{
			foreach (ModuleDef moduleDef in Kappa.Module.Assembly.Modules)
			{
				InterfaceImplUser item = new InterfaceImplUser(moduleDef.GlobalType);
				for (int i = 0; i < 1; i++)
				{
					TypeDefUser typeDefUser = new TypeDefUser(string.Empty, string.Format("Form{0}", i), moduleDef.CorLibTypes.GetTypeRef("System", "Attribute"));
					InterfaceImplUser item2 = new InterfaceImplUser(typeDefUser);
					moduleDef.Types.Add(typeDefUser);
					typeDefUser.Interfaces.Add(item2);
					typeDefUser.Interfaces.Add(item);
				}
			}
		}
	}
}
