using System;
using BlitzedConfuser.Utils;
using BlitzedConfuser.Utils.Analyzer;
using dnlib.DotNet;

namespace BlitzedConfuser.Protections
{
	// Token: 0x0200001B RID: 27
	public class Renamer : Protection
	{
		// Token: 0x06000056 RID: 86 RVA: 0x00004AD8 File Offset: 0x00002CD8
		public Renamer()
		{
			base.Name = "Renamer";
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00004AEB File Offset: 0x00002CEB
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00004AF3 File Offset: 0x00002CF3
		private int MethodAmount { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00004AFC File Offset: 0x00002CFC
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00004B04 File Offset: 0x00002D04
		private int ParameterAmount { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00004B0D File Offset: 0x00002D0D
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00004B15 File Offset: 0x00002D15
		private int PropertyAmount { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00004B1E File Offset: 0x00002D1E
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00004B26 File Offset: 0x00002D26
		private int FieldAmount { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00004B2F File Offset: 0x00002D2F
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00004B37 File Offset: 0x00002D37
		private int EventAmount { get; set; }

		// Token: 0x06000061 RID: 97 RVA: 0x00004B40 File Offset: 0x00002D40
		public override void Execute()
		{
			if (Kappa.DontRename)
			{
				return;
			}
			Kappa.Module.Mvid = new Guid?(Guid.NewGuid());
			Kappa.Module.EncId = new Guid?(Guid.NewGuid());
			Kappa.Module.EncBaseId = new Guid?(Guid.NewGuid());
			Kappa.Module.Name = "urnotfinnacrackthisretardlolSTONEDEAGLE" + Randomizer.String(12);
			Kappa.Module.EntryPoint.Name = Randomizer.String(MemberRenamer.StringLength()) + "StvnedEagleWINNING";
			foreach (TypeDef type in Kappa.Module.Types)
			{
				if (Renamer.CanRename(type))
				{
					type.Namespace = string.Empty;
					type.Name = "STONEDEAGLEWINNINGLOLSTONEDEAGLEWINNINGLOLSTONEDEAGLEWINNINGLOL" + Randomizer.String(5);
				}
				foreach (MethodDef i in type.Methods)
				{
					if (Renamer.CanRename(i) && !Kappa.ForceWinForms && !Kappa.FileExtension.Contains("dll"))
					{
						i.Name = "STONEDEAGLEWINNINGLOLSTONEDEAGLEWINNINGLOLSTONEDEAGLEWINNINGLOLSTONEDEAGLEWINNINGLOL" + Randomizer.String(6);
						int num = this.MethodAmount + 1;
						this.MethodAmount = num;
					}
					foreach (Parameter para in i.Parameters)
					{
						if (Renamer.CanRename(para))
						{
							para.Name = "STVNEDEAGLE" + Randomizer.String(7);
							int num = this.ParameterAmount + 1;
							this.ParameterAmount = num;
						}
					}
				}
				foreach (PropertyDef p in type.Properties)
				{
					if (Renamer.CanRename(p))
					{
						p.Name = Randomizer.String(MemberRenamer.StringLength()) + "StvnedEagle";
						int num = this.PropertyAmount + 1;
						this.PropertyAmount = num;
					}
				}
				foreach (FieldDef field in type.Fields)
				{
					if (Renamer.CanRename(field))
					{
						field.Name = "STONEREAGLEZLOLSTONEREAGLEZLOLSTONEREAGLEZLOLSTONEREAGLEZLOLSTONEREAGLEZLOL" + Randomizer.String(15);
						int num = this.FieldAmount + 1;
						this.FieldAmount = num;
					}
				}
				foreach (EventDef e in type.Events)
				{
					if (Renamer.CanRename(e))
					{
						e.Name = "StvnedEagleStvnedEagleStvnedEagleStvnedEagleStvnedEagleStvnedEagleStvnedEagleStvnedEagle" + Randomizer.String(6);
						int num = this.EventAmount + 1;
						this.EventAmount = num;
					}
				}
			}
			Console.WriteLine(string.Format("  Renamed {0} methods.\n  Renamed {1} parameters.", this.MethodAmount, this.ParameterAmount) + string.Format("\n  Renamed {0} properties.\n  Renamed {1} fields.\n  Renamed {2} events.", this.PropertyAmount, this.FieldAmount, this.EventAmount));
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004F40 File Offset: 0x00003140
		public static bool CanRename(object obj)
		{
			DefAnalyzer analyze;
			if (obj is MethodDef)
			{
				analyze = new MethodDefAnalyzer();
			}
			else if (obj is PropertyDef)
			{
				analyze = new PropertyDefAnalyzer();
			}
			else if (obj is EventDef)
			{
				analyze = new EventDefAnalyzer();
			}
			else if (obj is FieldDef)
			{
				analyze = new FieldDefAnalyzer();
			}
			else if (obj is Parameter)
			{
				analyze = new ParameterAnalyzer();
			}
			else
			{
				if (!(obj is TypeDef))
				{
					return false;
				}
				analyze = new TypeDefAnalyzer();
			}
			return analyze.Execute(obj);
		}
	}
}
