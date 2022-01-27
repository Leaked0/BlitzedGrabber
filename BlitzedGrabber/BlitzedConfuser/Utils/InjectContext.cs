using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using dnlib.DotNet;

namespace BlitzedConfuser.Utils
{
	// Token: 0x02000003 RID: 3
	public class InjectContext
	{
		// Token: 0x0600000F RID: 15 RVA: 0x0000232C File Offset: 0x0000072C
		public InjectContext(ModuleDef module, ModuleDef target)
		{
			this.OriginModule = module;
			this.TargetModule = target;
			this.Importerk__BackingField = new Importer(target, ImporterOptions.TryToUseTypeDefs);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000235A File Offset: 0x0000075A
		public Importer Importer
		{
			[CompilerGenerated]
			get
			{
				return this.Importerk__BackingField;
			}
		}

		// Token: 0x04000007 RID: 7
		public readonly Dictionary<IDnlibDef, IDnlibDef> Map = new Dictionary<IDnlibDef, IDnlibDef>();

		// Token: 0x04000008 RID: 8
		public readonly ModuleDef OriginModule;

		// Token: 0x04000009 RID: 9
		public readonly ModuleDef TargetModule;

		// Token: 0x0400000A RID: 10
		private readonly Importer Importerk__BackingField;
	}
}
