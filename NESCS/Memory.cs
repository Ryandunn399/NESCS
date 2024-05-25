using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESCS
{
    public class Memory
    {
        // Stores the index of the next available entry in memory
        private ushort MemoryIndex;

        // Addressable memory where instructions will be stored when a program is loaded
        private readonly byte[] RAM;

        public Memory()
        {
            MemoryIndex = MemoryValues.MemoryStartAddress;
            RAM = new byte[MemoryValues.MemorySize];
        }

        /// <summary>
        /// Will write a byte into the next available memory address.
        /// </summary>
        /// <param name="b">the 8-bit value to write into memory.</param>
        public void WriteByteIntoMemory(byte b)
        {
            RAM[MemoryIndex] = b;
            MemoryIndex += 1;
        }

        /// <summary>
        /// Will write a byte into the specified address.
        /// </summary>
        /// <param name="addr">memory address to write into.</param>
        /// <param name="b">value to store.</param>
        public void WriteByteIntoMemory(ushort addr, byte b)
        {
            RAM[addr] = b;
        }

        /// <summary>
        /// Will retrieve the current 8 bit value stored in memory.
        /// </summary>
        /// <param name="addr">the address to index.</param>
        /// <returns>byte stored in memory at the given address.</returns>
        public byte ReadValueFromMemory(ushort addr)
        {
            return RAM[addr];
        }
    }

    /// <summary>
    /// Class for organizing the various memory constants used.
    /// </summary>
    public static class MemoryValues
    {
        // Zero page memory locations
        public static ushort ZeroPageStart => 0x0;
        public static ushort ZeroPageEnd => 0xFF;

        // Starting memory address for actual memory
        // Stack memory exists between 0100 and 01ff but we will use
        // a prebuilt stack data structure to handle this information
        public static ushort MemoryStartAddress => 0x0200;
        public static ushort MemoryEndAddress => 0xFFF9;
        public static ushort MemorySize => 0xFFFF;
    }
}
