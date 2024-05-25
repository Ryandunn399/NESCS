using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESCS.CPU;

namespace NESCS
{

    public class App
    {
        
        // Object representing our NES machine.
        public NesWrapper NES { get; private set; }
        
        public App()
        {
            NES = new NesWrapper();
        }

        /// <summary>
        /// Program entry point.
        /// </summary>
        public static void Main()
        {
            App app = new App();

            Memory memory = app.NES.Memory;
            CPU6502 cpu = app.NES.Cpu;

            memory.WriteByteIntoMemory(0x12);
            memory.WriteByteIntoMemory(0x12);
            memory.WriteByteIntoMemory(0x12);
            memory.WriteByteIntoMemory(0x12);

            uint value = cpu.ReadInBytes(1);
            Console.WriteLine($"{value.ToString("X")}");
        }
    }
}
