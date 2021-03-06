﻿using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace GrandSteal.Client.Models
{
	// Token: 0x0200000B RID: 11
	public static class GeoLocationHelper
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000022BC File Offset: 0x000004BC
		// (set) Token: 0x06000055 RID: 85 RVA: 0x000022C3 File Offset: 0x000004C3
		public static int ImageIndex { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000022CB File Offset: 0x000004CB
		// (set) Token: 0x06000057 RID: 87 RVA: 0x000022D2 File Offset: 0x000004D2
		public static GeoInformation GeoInfo { get; private set; } = new GeoInformation();

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000022DA File Offset: 0x000004DA
		// (set) Token: 0x06000059 RID: 89 RVA: 0x000022E1 File Offset: 0x000004E1
		public static DateTime LastLocated { get; private set; } = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000022E9 File Offset: 0x000004E9
		// (set) Token: 0x0600005B RID: 91 RVA: 0x000022F0 File Offset: 0x000004F0
		public static bool LocationCompleted { get; private set; }

		// Token: 0x0600005D RID: 93 RVA: 0x00003A34 File Offset: 0x00001C34
		public static void Initialize()
		{
			GeoLocationHelper.TryLocate();
			GeoLocationHelper.GeoInfo.Query = (string.IsNullOrEmpty(GeoLocationHelper.GeoInfo.Query) ? "UNKNOWN" : GeoLocationHelper.GeoInfo.Query);
			GeoLocationHelper.GeoInfo.Country = (string.IsNullOrEmpty(GeoLocationHelper.GeoInfo.Country) ? "UNKNOWN" : GeoLocationHelper.GeoInfo.Country);
			GeoLocationHelper.GeoInfo.CountryCode = (string.IsNullOrEmpty(GeoLocationHelper.GeoInfo.CountryCode) ? "UNKNOWN" : GeoLocationHelper.GeoInfo.CountryCode);
			GeoLocationHelper.GeoInfo.Region = (string.IsNullOrEmpty(GeoLocationHelper.GeoInfo.Region) ? "UNKNOWN" : GeoLocationHelper.GeoInfo.Region);
			GeoLocationHelper.GeoInfo.City = (string.IsNullOrEmpty(GeoLocationHelper.GeoInfo.City) ? "UNKNOWN" : GeoLocationHelper.GeoInfo.City);
			GeoLocationHelper.GeoInfo.Timezone = (string.IsNullOrEmpty(GeoLocationHelper.GeoInfo.Timezone) ? "UNKNOWN" : GeoLocationHelper.GeoInfo.Timezone);
			GeoLocationHelper.GeoInfo.Isp = (string.IsNullOrEmpty(GeoLocationHelper.GeoInfo.Isp) ? "UNKNOWN" : GeoLocationHelper.GeoInfo.Isp);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003B7C File Offset: 0x00001D7C
		private static void TryLocate()
		{
			GeoLocationHelper.LocationCompleted = false;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ip-api.com/json/");
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; rv:48.0) Gecko/20100101 Firefox/48.0";
				httpWebRequest.Timeout = 10000;
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream responseStream = httpWebResponse.GetResponseStream())
					{
						using (StreamReader streamReader = new StreamReader(responseStream))
						{
							GeoLocationHelper.GeoInfo = JsonConvert.DeserializeObject<GeoInformation>(streamReader.ReadToEnd());
						}
					}
				}
				GeoLocationHelper.LastLocated = DateTime.UtcNow;
				GeoLocationHelper.LocationCompleted = true;
			}
			catch
			{
				GeoLocationHelper.TryGetWanIp();
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003C50 File Offset: 0x00001E50
		private static void TryGetWanIp()
		{
			string query = "";
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://api.ipify.org/");
				httpWebRequest.Timeout = 5000;
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream responseStream = httpWebResponse.GetResponseStream())
					{
						using (StreamReader streamReader = new StreamReader(responseStream))
						{
							query = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch
			{
			}
			GeoLocationHelper.GeoInfo.Query = query;
		}

		// Token: 0x0400001F RID: 31
		public static readonly string[] ImageList = new string[]
		{
			"ad",
			"ae",
			"af",
			"ag",
			"ai",
			"al",
			"am",
			"an",
			"ao",
			"ar",
			"as",
			"at",
			"au",
			"aw",
			"ax",
			"az",
			"ba",
			"bb",
			"bd",
			"be",
			"bf",
			"bg",
			"bh",
			"bi",
			"bj",
			"bm",
			"bn",
			"bo",
			"br",
			"bs",
			"bt",
			"bv",
			"bw",
			"by",
			"bz",
			"ca",
			"catalonia",
			"cc",
			"cd",
			"cf",
			"cg",
			"ch",
			"ci",
			"ck",
			"cl",
			"cm",
			"cn",
			"co",
			"cr",
			"cs",
			"cu",
			"cv",
			"cx",
			"cy",
			"cz",
			"de",
			"dj",
			"dk",
			"dm",
			"do",
			"dz",
			"ec",
			"ee",
			"eg",
			"eh",
			"england",
			"er",
			"es",
			"et",
			"europeanunion",
			"fam",
			"fi",
			"fj",
			"fk",
			"fm",
			"fo",
			"fr",
			"ga",
			"gb",
			"gd",
			"ge",
			"gf",
			"gh",
			"gi",
			"gl",
			"gm",
			"gn",
			"gp",
			"gq",
			"gr",
			"gs",
			"gt",
			"gu",
			"gw",
			"gy",
			"hk",
			"hm",
			"hn",
			"hr",
			"ht",
			"hu",
			"id",
			"ie",
			"il",
			"in",
			"io",
			"iq",
			"ir",
			"is",
			"it",
			"jm",
			"jo",
			"jp",
			"ke",
			"kg",
			"kh",
			"ki",
			"km",
			"kn",
			"kp",
			"kr",
			"kw",
			"ky",
			"kz",
			"la",
			"lb",
			"lc",
			"li",
			"lk",
			"lr",
			"ls",
			"lt",
			"lu",
			"lv",
			"ly",
			"ma",
			"mc",
			"md",
			"me",
			"mg",
			"mh",
			"mk",
			"ml",
			"mm",
			"mn",
			"mo",
			"mp",
			"mq",
			"mr",
			"ms",
			"mt",
			"mu",
			"mv",
			"mw",
			"mx",
			"my",
			"mz",
			"na",
			"nc",
			"ne",
			"nf",
			"ng",
			"ni",
			"nl",
			"no",
			"np",
			"nr",
			"nu",
			"nz",
			"om",
			"pa",
			"pe",
			"pf",
			"pg",
			"ph",
			"pk",
			"pl",
			"pm",
			"pn",
			"pr",
			"ps",
			"pt",
			"pw",
			"py",
			"qa",
			"re",
			"ro",
			"rs",
			"ru",
			"rw",
			"sa",
			"sb",
			"sc",
			"scotland",
			"sd",
			"se",
			"sg",
			"sh",
			"si",
			"sj",
			"sk",
			"sl",
			"sm",
			"sn",
			"so",
			"sr",
			"st",
			"sv",
			"sy",
			"sz",
			"tc",
			"td",
			"tf",
			"tg",
			"th",
			"tj",
			"tk",
			"tl",
			"tm",
			"tn",
			"to",
			"tr",
			"tt",
			"tv",
			"tw",
			"tz",
			"ua",
			"ug",
			"um",
			"us",
			"uy",
			"uz",
			"va",
			"vc",
			"ve",
			"vg",
			"vi",
			"vn",
			"vu",
			"wales",
			"wf",
			"ws",
			"ye",
			"yt",
			"za",
			"zm",
			"zw"
		};
	}
}
