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

            memory.WriteInstructionIntoMemory(0xA9CD);
            memory.WriteInstructionIntoMemory(0xA500);
            memory.WriteByteIntoMemory(0, 0x73);

            cpu.Fetch();
            cpu.Execute();

            cpu.Fetch();
            cpu.Execute();

            //Console.WriteLine($"{cpu.A.Value.ToString("X")}");
            int v = 0xffff;
            v = v % 0x10000;
            Console.WriteLine($"{v.ToString("x")}");
        }
    }
}
