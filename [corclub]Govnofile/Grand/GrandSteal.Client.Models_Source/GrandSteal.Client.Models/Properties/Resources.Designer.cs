﻿using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace GrandSteal.Client.Models.Properties
{
	// Token: 0x0200000C RID: 12
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[CompilerGenerated]
	[DebuggerNonUserCode]
	internal class Resources
	{
		// Token: 0x06000060 RID: 96 RVA: 0x0000205A File Offset: 0x0000025A
		internal Resources()
		{
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000061 RID: 97 RVA: 0x000022F8 File Offset: 0x000004F8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("GrandSteal.Client.Models.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002324 File Offset: 0x00000524
		// (set) Token: 0x06000063 RID: 99 RVA: 0x0000232B File Offset: 0x0000052B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x04000024 RID: 36
		private static ResourceManager resourceMan;

		// Token: 0x04000025 RID: 37
		private static CultureInfo resourceCulture;
	}
}
