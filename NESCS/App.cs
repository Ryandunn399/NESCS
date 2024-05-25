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

            Memory mem = app.NES.Memory;
            CPU6502 Cpu = app.NES.Cpu;

            // Final destination
            mem.WriteByteIntoMemory(0xF7, 0xCD);

            // The memory address/value instruction addr points to.
            mem.WriteByteIntoMemory(0x12, 0xFF);

            // Store address 37 in the register
            Cpu.Y.UpdateRegisterValue(0xF8);

            // Write instruction and memory address 9a to be executed 
            mem.WriteByteIntoMemory(0xB1);
            mem.WriteByteIntoMemory(0x12);

            Cpu.Fetch();
            Cpu.Execute();

            Console.WriteLine($"{Cpu.A.Value}");
        }
    }
}
