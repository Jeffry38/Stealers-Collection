﻿using System;
using System.Threading;

namespace GrandSteal.Client.Models
{
	// Token: 0x02000005 RID: 5
	public class AsyncTask
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000024E0 File Offset: 0x000006E0
		public static AsyncAction<R> StartNew<R>(AsyncAction<R> function)
		{
			R retv = default(R);
			bool completed = false;
			object sync = new object();
			function.BeginInvoke(delegate(IAsyncResult iAsyncResult)
			{
				object sync = sync;
				lock (sync)
				{
					completed = true;
					retv = function.EndInvoke(iAsyncResult);
					Monitor.Pulse(sync);
				}
			}, null);
			return delegate()
			{
				object sync = sync;
				R retv;
				lock (sync)
				{
					if (!completed)
					{
						Monitor.Wait(sync);
					}
					retv = retv;
				}
				return retv;
			};
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002540 File Offset: 0x00000740
		public static AsyncAction StartNew(AsyncAction function)
		{
			bool completed = false;
			object sync = new object();
			function.BeginInvoke(delegate(IAsyncResult iAsyncResult)
			{
				object sync = sync;
				lock (sync)
				{
					completed = true;
					function.EndInvoke(iAsyncResult);
					Monitor.Pulse(sync);
				}
			}, null);
			return delegate()
			{
				object sync = sync;
				lock (sync)
				{
					if (!completed)
					{
						Monitor.Wait(sync);
					}
				}
			};
		}
	}
}
