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
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x0000205E File Offset: 0x0000025E
		public static SetupStorage Default
		{
			get
			{
				return SetupStorage.defaultInstance;
			}
		}

		// Token: 0x04000001 RID: 1
		private static SetupStorage defaultInstance = (SetupStorage)SettingsBase.Synchronized(new SetupStorage());
	}
}
