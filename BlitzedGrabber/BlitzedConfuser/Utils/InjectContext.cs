using System;
using System.Collections.Generic;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils
{
	// Token: 0x02000003 RID: 3
	public class InjectContext
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002324 File Offset: 0x00000524
		public InjectContext(ModuleDef module, ModuleDef target)
		{
			this.OriginModule = module;
			this.TargetModule = target;
			this.Importer = new Importer(target, ImporterOptions.TryToUseTypeDefs);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002352 File Offset: 0x00000552
		public Importer Importer { get; }

		// Token: 0x04000007 RID: 7
		public readonly Dictionary<IDnlibDef, IDnlibDef> Map = new Dictionary<IDnlibDef, IDnlibDef>();

		// Token: 0x04000008 RID: 8
		public readonly ModuleDef OriginModule;

		// Token: 0x04000009 RID: 9
		public readonly ModuleDef TargetModule;
	}
}
