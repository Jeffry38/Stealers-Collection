﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GrandSteal.Client.Data
{
	// Token: 0x02000002 RID: 2
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
	[CompilerGenerated]
	internal sealed partial class SetupStorage : ApplicationSettingsBase
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002065 File Offset: 0x00000265
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002077 File Offset: 0x00000277
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public StringCollection Setups
		{
			get
			{
				return (StringCollection)this["Setups"];
			}
			set
			{
				this["Setups"] = value;
			}
		}
	}
}
