﻿using System;
using ProtoBuf;

namespace GrandSteal.Client.Models
{
	// Token: 0x0200000A RID: 10
	[ProtoContract(Name = "GeoInformation")]
	public class GeoInformation
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000021CE File Offset: 0x000003CE
		// (set) Token: 0x06000038 RID: 56 RVA: 0x000021D6 File Offset: 0x000003D6
		[ProtoMember(1, Name = "as")]
		public string As { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000021DF File Offset: 0x000003DF
		// (set) Token: 0x0600003A RID: 58 RVA: 0x000021E7 File Offset: 0x000003E7
		[ProtoMember(2, Name = "city")]
		public string City { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003B RID: 59 RVA: 0x000021F0 File Offset: 0x000003F0
		// (set) Token: 0x0600003C RID: 60 RVA: 0x000021F8 File Offset: 0x000003F8
		[ProtoMember(3, Name = "country")]
		public string Country { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002201 File Offset: 0x00000401
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002209 File Offset: 0x00000409
		[ProtoMember(4, Name = "countryCode")]
		public string CountryCode { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002212 File Offset: 0x00000412
		// (set) Token: 0x06000040 RID: 64 RVA: 0x0000221A File Offset: 0x0000041A
		[ProtoMember(5, Name = "isp")]
		public string Isp { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002223 File Offset: 0x00000423
		// (set) Token: 0x06000042 RID: 66 RVA: 0x0000222B File Offset: 0x0000042B
		[ProtoMember(6, Name = "lat")]
		public double Lat { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002234 File Offset: 0x00000434
		// (set) Token: 0x06000044 RID: 68 RVA: 0x0000223C File Offset: 0x0000043C
		[ProtoMember(7, Name = "lon")]
		public double Lon { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002245 File Offset: 0x00000445
		// (set) Token: 0x06000046 RID: 70 RVA: 0x0000224D File Offset: 0x0000044D
		[ProtoMember(8, Name = "org")]
		public string Org { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002256 File Offset: 0x00000456
		// (set) Token: 0x06000048 RID: 72 RVA: 0x0000225E File Offset: 0x0000045E
		[ProtoMember(9, Name = "query")]
		public string Query { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002267 File Offset: 0x00000467
		// (set) Token: 0x0600004A RID: 74 RVA: 0x0000226F File Offset: 0x0000046F
		[ProtoMember(10, Name = "region")]
		public string Region { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002278 File Offset: 0x00000478
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00002280 File Offset: 0x00000480
		[ProtoMember(11, Name = "regionName")]
		public string RegionName { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002289 File Offset: 0x00000489
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00002291 File Offset: 0x00000491
		[ProtoMember(12, Name = "status")]
		public string Status { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004F RID: 79 RVA: 0x0000229A File Offset: 0x0000049A
		// (set) Token: 0x06000050 RID: 80 RVA: 0x000022A2 File Offset: 0x000004A2
		[ProtoMember(13, Name = "timezone")]
		public string Timezone { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000022AB File Offset: 0x000004AB
		// (set) Token: 0x06000052 RID: 82 RVA: 0x000022B3 File Offset: 0x000004B3
		[ProtoMember(14, Name = "zip")]
		public string Zip { get; set; }
	}
}
