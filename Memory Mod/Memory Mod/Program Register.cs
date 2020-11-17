using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PiTung;
using PiTung.Components;
using PiTung.Console;

namespace Memory_Mod
{
	class ProgramRegister : UpdateHandler
	{
		[SaveThis]
		public int Data;
		protected override void CircuitLogicUpdate()
		{
			if (this.Inputs[0].On)
			{
				if (Data == 255 && !this.Inputs[2].On)
				{
					this.Outputs[0].On = true;
				}
				else
				{
					this.Outputs[0].On = false;
				}
				if (this.Inputs[2].On)
				{
					Data = 0;
					for (int i = 4; i < this.Inputs.Length; i++)
					{
						Data = Data + (int)Math.Pow(2, i - 4) * Convert.ToInt32(this.Inputs[i].On);
					}
				}
				else
				{
					if (this.Inputs[3].On)
					{
						Data++;
					}
				}
				if (Data > 255)
				{
					Data = 0;
				}
			}
			else
			{
				for (int i = 1; i < this.Outputs.Length; i++)
				{ 
					this.Outputs[i].On = Convert.ToBoolean((Data >> (i - 1)) % 2);
				}
			}
			if (this.Inputs[1].On)
			{
				for (int i = 1; i < this.Outputs.Length; i++)
				{
					this.Outputs[i].On = false;
				}
			}
		}
	}
}
