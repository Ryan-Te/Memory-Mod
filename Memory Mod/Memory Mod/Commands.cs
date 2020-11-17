using PiTung.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Memory_Mod
{

    public class setRomLoc : Command
    {
        public override string Name => "setRomLoc";
        public override string Usage => $"{Name} Location(in TUNG folder)";
        public override string Description => "Lets you set the ROM flash location to the location at the file listed starting in the TUNG directory";

        public override bool Execute(IEnumerable<string> args)
        {
            if (args.Count() == 0)
                return false;

            Memory_Mod.FlashLocationGlobal = "/" + args.ElementAt(0);

            return true;
        }
    }
}
