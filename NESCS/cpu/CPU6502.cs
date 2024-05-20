using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESCS.CPU
{
    /// <summary>
    /// Object representing the cpu architecture of the nes.
    /// http://www.6502.org/users/obelisk/
    /// </summary>
    public class CPU6502
    {
        // Program counter
        private ushort PC;

        // Current instruction to be ran in next execute call
        private ushort Instruction;

        // Accumulator register
        public Register A { get; set; }

        // X register
        public Register X { get; private set; }

        // Y register
        public Register Y { get; private set; }

        // Status register, this will just be the 16 bit integer to represent the flags.
        public ushort P { get; private set; }

        // Track the number of cycles the CPU has executed
        public int Cycles;

        public Memory Memory { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="memory">Memory instance of the emulator.</param>
        public CPU6502(Memory memory)
        {
            PC = MemoryValues.MemoryStartAddress;
            Instruction = 0;
            P = 0;
            Cycles = 0;
            Memory = memory;

            A = new Register(this);
            X = new Register(this);
            Y = new Register(this);
        }

        /// <summary>
        /// Will fetch the next instruction in memory and increment the program counter.
        /// </summary>
        /// <returns>the instruction loaded into memory.</returns>
        public int Fetch()
        {
            Instruction = Memory.ReadValueFromMemory(PC);
            Instruction <<= 8;
            PC += 1;
            Instruction = (ushort)(Instruction | Memory.ReadValueFromMemory(PC));
            PC += 1;
            Cycles = 0;
            return Instruction;
        }

        /// <summary>
        /// Will execute the instruction currently loaded into memory.
        /// </summary>
        public void Execute()
        {
            byte instr = Utilities.ReadFirstByte(Instruction);

            // Reset the status flag to be reset by this instruction.
            RefreshStatusFlag();

            switch (instr)
            {
                case byte value when value == OpCodes.LdaImmediate:
                    A.LoadInstruction(AddressMode.Immediate, Instruction, 0);
                    break;

                case byte value when value == OpCodes.LdaZeroPage:
                    A.LoadInstruction(AddressMode.ZeroPage, Instruction, 0);
                    break;
            }
        }

        /// <summary>
        /// Will toggle status bit if it's not already set.
        /// </summary>
        /// <param name="flag">the flag to set.</param>
        public void SetStatusFlag(ushort flag)
        {
            P |= flag;
        }

        /// <summary>
        /// Will reset the status flag.
        /// </summary>
        private void RefreshStatusFlag()
        {
            P = 0;
        }

        /// <summary>
        /// Will determine whether or not the zero flag has been set.
        /// </summary>
        public bool ZeroFlagSet()
        {
            return ((StatusFlags.ZeroFlag & P) > 0);
        }

        /// <summary>
        /// Will determine whether or not the negative flag has been set.
        /// </summary>
        public bool NegativeFlagSet()
        {
            return ((StatusFlags.NegativeFlag & P) > 0);
        }
    }

    /// <summary>
    /// Stores the NES 6502 operation codes.
    /// </summary>
    public static class OpCodes
    {
        // Instruction constants
        public static ushort LdaImmediate => 0xA9;
        public static ushort LdaZeroPage => 0xA5;
    }


    /// <summary>
    /// Stores the status flag constants.
    /// </summary>
    public static class StatusFlags
    {
        public static ushort NegativeFlag => 0x80;
        public static ushort OverflowFlag => 0x40;
        public static ushort BreakFlag => 0x10;
        public static ushort DecimalFlag => 0x8;
        public static ushort InterruptDisableFlag => 0x4;
        public static ushort ZeroFlag => 0x2;
        public static ushort CarryFlag => 0x1;
    }
}
