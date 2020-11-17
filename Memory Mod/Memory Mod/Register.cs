using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PiTung;
using PiTung.Components;
using PiTung.Console;

namespace Memory_Mod
{
	class Register : UpdateHandler
	{
		[SaveThis]
		public int Data;
		protected override void CircuitLogicUpdate()
		{
			if (this.Inputs[0].On)
			{
				Data = 0;
				for(int i = 2; i < this.Inputs.Length; i++)
				{
					Data = Data + (int)Math.Pow(2, i - 2) * Convert.ToInt32(this.Inputs[i].On);
				}
			}
			else
			{
				for (int i = 0; i < this.Outputs.Length; i++)
				{
					this.Outputs[i].On = Convert.ToBoolean((Data >> i) % 2);
				}
			}
			if (this.Inputs[1].On)
			{
				for (int i = 0; i < this.Outputs.Length; i++)
				{
					this.Outputs[i].On = false;
				}
			}
		}
	}
}
