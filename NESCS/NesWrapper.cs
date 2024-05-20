using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESCS.CPU;

namespace NESCS
{
    /// <summary>
    /// Will represent the NES machine and contain all the various components.
    /// </summary>
    public class NesWrapper
    {

        // Represents the RAM available to the NES machine
        public Memory Memory { get; private set; }

        public CPU6502 Cpu { get; private set; }

        public NesWrapper()
        {
            Memory = new Memory();
            Cpu = new CPU6502(Memory);
        }
    }
}
