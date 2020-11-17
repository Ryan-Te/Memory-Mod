using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using PiTung;
using PiTung.Components;
using PiTung.Console;

namespace Memory_Mod
{
	class RAM : UpdateHandler
	{
		[SaveThis]
		public int[] Data;

		[SaveThis]
		public string ROMFlash;

		int DataWidth;

		int AddressWidth;

		int AddressSize;

		protected override void CircuitLogicUpdate()
		{
			if(Data == null)
			{
				Data = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15};
			}
			if(ROMFlash == null)
			{
				ROMFlash = "";
			}
			if(DataWidth == 0)
			{
				DataWidth = this.Outputs.Length;
				AddressWidth = this.Inputs.Length - 4 - DataWidth;
				AddressSize = (int)Math.Pow(2, AddressWidth);
			}
			if (this.Inputs[1].On)
			{
				if (File.Exists(Directory.GetCurrentDirectory() + ROMFlash))
				{
					string data = File.ReadAllText(Directory.GetCurrentDirectory() + ROMFlash);
					data = data.ToLower();
					if (data.Substring(0, 5) == "mmrom")
					{
						data = data.Substring(5);
						if (data.Contains(";"))
						{
							int sc = data.IndexOf(";");
							string settingsNS = data.Substring(0, sc);
							while (settingsNS.Contains(" "))
							{
								int first = settingsNS.IndexOf(" ");
								var Removed = settingsNS.Substring(0, first) + settingsNS.Substring(first + 1);
								settingsNS = Removed;
							}
							while (settingsNS.Contains("\n"))
							{
								int first = settingsNS.IndexOf("\n");
								var Removed = settingsNS.Substring(0, first) + settingsNS.Substring(first + 1);
								settingsNS = Removed;
							}
							while (settingsNS.Contains("\r"))
							{
								int first = settingsNS.IndexOf("\r");
								var Removed = settingsNS.Substring(0, first) + settingsNS.Substring(first + 1);
								settingsNS = Removed;
							}
							string[] settings = settingsNS.Split(',');
							data = data.Substring(sc + 1);

							string readMode = "digit";
							if(settings.Length > 1)
							{
								if(settings[1] == "lb")
								{
									readMode = "lb";

								}else if (settings[1] == "lbs")
								{
									readMode = "lbs";

								}
							}
							
							int format = 0;
							int dpa = 0;
							bool continu = true;
							if (settings[0] == "h")
							{
								format = 16;
								dpa = DataWidth / 4;
							}
							else if (settings[0] == "b")
							{
								format = 2;
								dpa = DataWidth;
							}
							else if (settings[0] == "d")
							{
								format = 10;
								dpa = Math.Pow(2, DataWidth).ToString().Length;
							}
							else if (settings[0] == "o")
							{
								format = 8;
								dpa = (int)Math.Ceiling((double)DataWidth / 3);
							}
							else
							{
								IGConsole.Log($"{Directory.GetCurrentDirectory() + ROMFlash} is not a correctly formatted Memory Mod ROM file. Error 2");
								continu = false;
							}
							if (continu)
							{
								if (readMode == "digit")
								{
									while (data.Contains(" "))
									{
										int first = data.IndexOf(" ");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									while (data.Contains("\n"))
									{
										int first = data.IndexOf("\n");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									while (data.Contains("\r"))
									{
										int first = data.IndexOf("\r");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									int[] NewData = new int[AddressSize];
									int j = 0;
									try
									{

										for (int i = 0; i < data.Length / dpa && i < AddressSize; i++)
										{
											NewData[i] = Convert.ToInt32(data.Substring(i * dpa, dpa), format);
											j++;
										}
									}
									catch (System.FormatException)
									{
										string basestr = "???";
										if (format == 2)
										{
											basestr = "binary";
										}
										if (format == 8)
										{
											basestr = "octal";
										}
										if (format == 10)
										{
											basestr = "decimal";
										}

										if (format == 16)
										{
											basestr = "hexadecimal";
										}
										IGConsole.Log($"{Directory.GetCurrentDirectory() + ROMFlash} has {data.Substring(j * dpa, dpa)} which is not a valid {basestr} number.");
									}
									Data = NewData;
								}
								else if(readMode == "lb")
								{
									while (data.Contains(" "))
									{
										int first = data.IndexOf(" ");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									while (data.Contains("\r"))
									{
										int first = data.IndexOf("\r");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									if (data.Substring(0, 1) == "\n")
									{
										data = data.Substring(1);
									}
									if (data.Substring(data.Length - 1) == "\n")
									{
										data = data.Substring(0, data.Length - 1);
									}
									string[] NewDataStr = data.Split('\n');
									int[] NewData = new int[AddressSize];
									int j = 0;
									try 
									{ 
										for(int i = 0; i < NewDataStr.Length && i < AddressSize; i++)
										{
											string dataa = NewDataStr[i];
											if (dataa.Length > dpa)
											{
												dataa = dataa.Substring(0, dpa);
											}
											NewData[i] = Convert.ToInt32(dataa, format);
											j++;
										}
									}
									catch (System.FormatException)
									{
										string basestr = "???";
										if (format == 2)
										{
											basestr = "binary";
										}
										if (format == 8)
										{
											basestr = "octal";
										}
										if (format == 10)
										{
											basestr = "decimal";
										}
										if (format == 16)
										{
											basestr = "hexadecimal";
										}
										IGConsole.Log($"{Directory.GetCurrentDirectory() + ROMFlash} has {NewDataStr[j]} which is not a valid {basestr} number.");
									}
									Data = NewData;
								}
								else if (readMode == "lbs")
								{
									while (data.Contains("\r"))
									{
										int first = data.IndexOf("\r");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									if (data.Substring(0, 1) == "\n")
									{
										data = data.Substring(1);
									}
									string[] NewDataStr = data.Split(new char[] {'\n', ' '});
									int[] NewData = new int[AddressSize];
									int j = 0;
									try
									{
										for (int i = 0; i < NewDataStr.Length && i < AddressSize; i++)
										{
											string dataa = NewDataStr[i];
											if (dataa.Length > dpa)
											{
												dataa = dataa.Substring(0, dpa);
											}
											NewData[i] = Convert.ToInt32(dataa, format);
											j++;
										}
									}
									catch (System.FormatException)
									{
										string basestr = "???";
										if (format == 2)
										{
											basestr = "binary";
										}
										if (format == 8)
										{
											basestr = "octal";
										}
										if (format == 10)
										{
											basestr = "decimal";
										}

										if (format == 16)
										{
											basestr = "hexadecimal";
										}
										IGConsole.Log($"{Directory.GetCurrentDirectory() + ROMFlash} has {NewDataStr[j]} which is not a valid {basestr} number.");
									}
									Data = NewData;
								}
							}
						}
						else
						{
							IGConsole.Log($"{Directory.GetCurrentDirectory() + ROMFlash} is not a correctly formatted Memory Mod ROM file. Error 1");
						}
					}
					else
					{
						IGConsole.Log($"{Directory.GetCurrentDirectory() + ROMFlash} is not a Memory Mod ROM file");
					}
				}
				else
				{
					IGConsole.Log($"{Directory.GetCurrentDirectory() + ROMFlash} is not a real file. REMEMBER: it's looking for that location in the TUNG directory!!!");
				}

			}
			if (this.Inputs[2].On)
			{
				string[] SaveData = new string[Data.Length + 2];
				SaveData[0] = "MMROM";
				SaveData[1] = "D, LB;";
				for(int i = 0; i < Data.Length; i++)
				{
					SaveData[i + 2] = Data[i].ToString();
				}
				File.WriteAllLines(Directory.GetCurrentDirectory() + ROMFlash, SaveData, Encoding.UTF8);
			}
			if (this.Inputs[3].On)
			{
				ROMFlash = Memory_Mod.FlashLocationGlobal;
			}
			int Address = 0;
			for(int i = 4; i < this.Inputs.Length - DataWidth; i++){
				Address = Address + (int)Math.Pow(2, i - 4) * Convert.ToInt32(this.Inputs[i].On);
			}
			if (this.Inputs[0].On)
			{
				int DataToWrite = 0;
				for (int i = 4 + AddressWidth; i < this.Inputs.Length; i++)
				{
					DataToWrite = DataToWrite + (int)Math.Pow(2, i - (4 + AddressWidth)) * Convert.ToInt32(this.Inputs[i].On);
				}
				Data[Address] = DataToWrite;
			}
			int DataToOutput = 0;
			bool IndexError = false;
			try
			{
				DataToOutput = Data[Address];
			}
			catch (System.IndexOutOfRangeException)
			{
				IndexError = true;
			}
			for (int i = 0; i < this.Outputs.Length; i++)
			{
				if (!IndexError)
				{
					this.Outputs[i].On = Convert.ToBoolean((DataToOutput >> i) % 2);
				}
				else
				{
					this.Outputs[i].On = false;
				}
			}
		}
	}
}
