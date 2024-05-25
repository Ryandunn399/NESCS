using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public byte Instruction { get; private set; }

        // Accumulator register
        public Register A { get; set; }

        // X register
        public Register X { get; set; }

        // Y register
        public Register Y { get; set; }

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
            PC += 1;
            Cycles = 0;
            return Instruction;
        }

        /// <summary>
        /// Will execute the instruction currently loaded into memory.
        /// </summary>
        public void Execute()
        {
            // Reset the status flag to be reset by this instruction.
            RefreshStatusFlag();

            switch (Instruction)
            {
                case byte value when value == OpCodes.LdaImmediate:
                    A.LoadImmediate();
                    break;

                case byte value when value == OpCodes.LdaZeroPage:
                    A.LoadZeroPage();
                    break;

                case byte value when value == OpCodes.LdaZeroPageX:
                    A.LoadZeroPageXY(X.Value);
                    break;

                case byte value when value == OpCodes.LdaAbsolute:
                    A.LoadAbsolute();
                    break;

                case byte value when value == OpCodes.LdaAbsoluteX:
                    A.LoadAbsoluteXY(X.Value);
                    break;

                case byte value when value == OpCodes.LdaAbsoluteY:
                    A.LoadAbsoluteXY(Y.Value);
                    break;

                case byte value when value == OpCodes.LdaIndexedIndirect:
                    A.LoadIndexedIndirect();
                    break;

                case byte value when value == OpCodes.LdaIndirectIndexed:
                    A.LoadIndirectIndexed();
                    break;

                case byte value when value == OpCodes.LdxImmediate:
                    X.LoadImmediate();
                    break;

                case byte value when value == OpCodes.LdxZeroPage:
                    X.LoadZeroPage();
                    break;

                case byte value when value == OpCodes.LdxZeroPageY:
                    X.LoadZeroPageXY(Y.Value);
                    break;

                case byte value when value == OpCodes.LdxAbsolute:
                    X.LoadAbsolute();
                    break;

                case byte value when value == OpCodes.LdxAbsoluteY:
                    X.LoadAbsoluteXY(Y.Value);
                    break;

                case byte value when value == OpCodes.LdyImmediate:
                    Y.LoadImmediate();
                    break;

                case byte value when value == OpCodes.LdyZeroPage:
                    Y.LoadZeroPage();
                    break;

                case byte value when value == OpCodes.LdyZeroPageX:
                    Y.LoadZeroPageXY(X.Value);
                    break;

                case byte value when value == OpCodes.LdyAbsolute:
                    Y.LoadAbsolute();
                    break;

                case byte value when value == OpCodes.LdyAbsoluteX:
                    Y.LoadAbsoluteXY(X.Value);
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

        /// <summary>
        /// Will retrieve up to four bytes from memory and return it in one type.
        /// </summary>
        /// <returns>4 byte value</returns>
        public uint ReadInBytes(uint value)
        {
            uint returnValue = 0;
            for (uint i = 0; i < value; i++)
            {
                returnValue <<= 8;
                returnValue = (returnValue | (uint)(Memory.ReadValueFromMemory(PC) & 0xFF));
                PC += 1;
            }

            return returnValue;
        }
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
