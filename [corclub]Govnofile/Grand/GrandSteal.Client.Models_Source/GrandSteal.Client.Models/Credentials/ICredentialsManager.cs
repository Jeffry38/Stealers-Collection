﻿using System;
using System.Collections.Generic;

namespace GrandSteal.Client.Models.Credentials
{
	// Token: 0x0200000E RID: 14
	public interface ICredentialsManager<T>
	{
		// Token: 0x06000067 RID: 103
		IEnumerable<T> GetAll();
	}
}
