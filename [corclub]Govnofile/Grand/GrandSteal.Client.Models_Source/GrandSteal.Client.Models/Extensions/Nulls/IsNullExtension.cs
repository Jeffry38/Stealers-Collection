﻿using System;

namespace GrandSteal.Client.Models.Extensions.Nulls
{
	// Token: 0x02000012 RID: 18
	public static class IsNullExtension
	{
		// Token: 0x06000079 RID: 121 RVA: 0x0000241A File Offset: 0x0000061A
		public static bool IsNotNull<T>(this T data)
		{
			return data != null;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002425 File Offset: 0x00000625
		public static string IsNull(this string value, string defaultValue)
		{
			if (!string.IsNullOrEmpty(value))
			{
				return value;
			}
			return defaultValue;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002432 File Offset: 0x00000632
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000243A File Offset: 0x0000063A
		public static bool IsNull(this bool? value, bool def)
		{
			if (value != null)
			{
				return value.Value;
			}
			return def;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000244E File Offset: 0x0000064E
		public static T IsNull<T>(this T value) where T : class
		{
			if (value == null)
			{
				return Activator.CreateInstance<T>();
			}
			return value;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000245F File Offset: 0x0000065F
		public static T IsNull<T>(this T value, T def) where T : class
		{
			if (value != null)
			{
				return value;
			}
			if (def == null)
			{
				return Activator.CreateInstance<T>();
			}
			return def;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000247A File Offset: 0x0000067A
		public static int IsNull(this int? value, int def)
		{
			if (value != null)
			{
				return value.Value;
			}
			return def;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000248E File Offset: 0x0000068E
		public static long IsNull(this long? value, long def)
		{
			if (value != null)
			{
				return value.Value;
			}
			return def;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000024A2 File Offset: 0x000006A2
		public static double IsNull(this double? value, double def)
		{
			if (value != null)
			{
				return value.Value;
			}
			return def;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000024B6 File Offset: 0x000006B6
		public static DateTime IsNull(this DateTime? value, DateTime def)
		{
			if (value != null)
			{
				return value.Value;
			}
			return def;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000024CA File Offset: 0x000006CA
		public static Guid IsNull(this Guid? value, Guid def)
		{
			if (value != null)
			{
				return value.Value;
			}
			return def;
		}
	}
}
