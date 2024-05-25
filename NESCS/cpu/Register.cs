using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESCS;

namespace NESCS.CPU
{
    /// <summary>
    /// Class will represent the accumulator register and the corresponding
    /// instructions that are used to handle the data inside.
    /// </summary>
    public class Register
    {
        /// <summary>
        /// The value currently stored inside the A register.
        /// </summary>
        public byte Value { get; private set; }

        /// <summary>
        /// The cpu object for this register.
        /// </summary>
        private CPU6502 Cpu;

        /// <summary>
        /// The memory reference associated with the application.
        /// </summary>
        private Memory Memory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cpu">The cpu this register is attached to.</param>
        public Register(CPU6502 cpu)
        {
            Value = 0;
            Cpu = cpu;
            Memory = Cpu.Memory;
        }

        /// <summary>
        /// Will load the immediate value stored in the second byte of the instruction.
        /// </summary>
        /// <param name="instr">The instruction being called.</param>
        public void LoadImmediate()
        {
            Value = (byte)Cpu.ReadInBytes(1); // Next immediate value
            UpdateStatusFlags();
            Cpu.Cycles += 2;
        }

        /// <summary>
        /// Will execute the load instruction using the zero page addressing mode.
        /// </summary>
        /// <param name="instr"></param>
        public void LoadZeroPage()
        {
            byte addr = (byte)Cpu.ReadInBytes(1); // The one byte address passed?
            Value = Memory.ReadValueFromMemory(addr);
            UpdateStatusFlags();
            Cpu.Cycles += 3;
        }

        /// <summary>
        /// Handles the logic for handling the Zero Page X/Y addressing mode load instruction.
        /// </summary>
        /// <param name="instr">the instruction currently being executed.</param>
        /// <param name="supplValue">the value of the x/y register</param>
        public void LoadZeroPageXY(byte supplValue)
        {
            byte addr = (byte)((Cpu.ReadInBytes(1) + (uint)Cpu.X.Value) % 0x100);
            Value = Memory.ReadValueFromMemory(addr);
            UpdateStatusFlags();
            Cpu.Cycles += 4;
        }

        /// <summary>
        /// Will load the instruction at the memory location
        /// </summary>
        public void LoadAbsolute()
        {
            uint addr = Cpu.ReadInBytes(2);

            // Wrap the memory index if it exceeds 0xFFFF
            addr %= 0x10000;
            Value = Memory.ReadValueFromMemory((ushort)addr);
            UpdateStatusFlags();
            Cpu.Cycles += 4;
        }

        /// <summary>
        /// Will load the instruction taking a register value and adding it to the address 
        /// </summary>
        /// <param name="supplValue">The X/Y regsiter value</param>
        public void LoadAbsoluteXY(byte supplValue)
        {
            uint addr = Cpu.ReadInBytes(2);

            addr = (addr + supplValue) % 0x10000;
            Value = Memory.ReadValueFromMemory((ushort)addr);
            UpdateStatusFlags();
            Cpu.Cycles += 4;

            // Determine if pages crossed and update cycle count
            if (Utilities.PagesCrossed(addr, supplValue))
            {
                Cpu.Cycles += 1;
            }
        }

        /// <summary>
        /// Adds a memory address stored in the zero page pointed by the next byte in memory
        /// and adds it to another value stored in the zero page address pointed to by the X register.
        /// </summary>
        public void LoadIndexedIndirect()
        {
            // The second byte of the instruction contains the address FOR the value
            byte addr = Memory.ReadValueFromMemory((byte)Cpu.ReadInBytes(1));

            // Next the value at the memory address pointed to by the X register.
            byte supplAddr = Memory.ReadValueFromMemory(Cpu.X.Value);

            // Zero page wrap around
            addr = Utilities.ZeroPageWrapAround(addr, supplAddr);
            Value = Memory.ReadValueFromMemory(addr);
            Cpu.Cycles += 6;
        }

        /// <summary>
        /// Will load the immediate value of the Y address to the memory address stored
        /// at the location of the next memory value in memory.
        /// </summary>
        /// <param name="supplValue">The value of the register</param>
        public void LoadIndirectIndexed()
        {
            // The second byte of the instruction contains the address FOR the value
            ushort addr = Memory.ReadValueFromMemory((byte)Cpu.ReadInBytes(1));
            addr = (ushort)((addr + Cpu.Y.Value) & 0xFFFF);

            Value = Memory.ReadValueFromMemory(addr);

            Cpu.Cycles += 5;

            if (Utilities.PagesCrossed(addr, Cpu.Y.Value))
            {
                Cpu.Cycles += 1;
            }
        }

        /// <summary>
        /// Will update the status flags based on the register that was updated.
        /// </summary>
        /// <param name="register">The name of the register being updated.</param>
        private void UpdateStatusFlags()
        {
            // Determine if the negative flag is set by reading the MSB of the value.
            if ((Value & StatusFlags.NegativeFlag) > 0)
            {
                Cpu.SetStatusFlag(StatusFlags.NegativeFlag);
            }

            // Determine if the zero flag is set by determining if the register has zero stored.
            if (Value == 0)
            {
                Cpu.SetStatusFlag(StatusFlags.ZeroFlag);
            }
        }
    }
}
