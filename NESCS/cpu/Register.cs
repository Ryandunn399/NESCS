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
        /// Will determine the necessary load instruction based on the given address mode.
        /// </summary>
        /// <param name="mode">the address mode to use loading a value into this register.</param>
        /// <param name="instr">the 16 bit instruction being executed.</param>
        /// <param name="suppl">supplemental register value required for some load instructions.</param>
        /// <returns>the value now stored in the register.</returns>
        public byte LoadInstruction(AddressMode mode, byte supplValue)
        {
            ushort instr = Cpu.Instruction;

            switch(mode)
            {
                case AddressMode.Immediate:
                    LoadImmediate(instr);
                    break;

                case AddressMode.ZeroPage:
                    LoadZeroPage(instr);
                    break;

                case AddressMode.ZeroPageXY:
                    LoadZeroPageXY(instr, supplValue);
                    break;


            }

            return Value;
        }

        /// <summary>
        /// Will load the immediate value stored in the second byte of the instruction.
        /// </summary>
        /// <param name="instr">The instruction being called.</param>
        private void LoadImmediate(ushort instr)
        {
            Value = Utilities.ReadSecondByte(instr);
            UpdateStatusFlags();
            Cpu.Cycles += 2;
        }

        /// <summary>
        /// Will execute the load instruction using the zero page addressing mode.
        /// </summary>
        /// <param name="instr"></param>
        private void LoadZeroPage(ushort instr)
        {
            byte addr = Utilities.ReadSecondByte(instr);
            Value = Memory.ReadValueFromMemory(addr);
            UpdateStatusFlags();
            Cpu.Cycles += 3;
        }

        /// <summary>
        /// Handles the logic for handling the Zero Page X/Y addressing mode load instruction.
        /// </summary>
        /// <param name="instr">the instruction currently being executed.</param>
        /// <param name="supplValue">the value of the x/y register</param>
        private void LoadZeroPageXY(ushort instr, byte supplValue)
        {
            byte instrValue = Utilities.ReadSecondByte(instr);
            ushort addr;

            // Handle wrapping logic if the values of sum of these two registers exceeds its range.
            if (supplValue > instrValue && supplValue + instrValue > 0xFF)
            { 
                addr = (ushort)(supplValue - instrValue);
            }
            else if (instrValue > supplValue && supplValue + instrValue > 0xFF)
            {
                addr = (ushort)(instrValue - supplValue);
            }
            else
            {
                addr = (ushort)(supplValue + instrValue);
            }

            Value = Memory.ReadValueFromMemory(addr);
            UpdateStatusFlags();
            Cpu.Cycles += 4;
        }

        /// <summary>
        /// Will load the instruction at the memory location
        /// </summary>
        private void LoadAbsolute(ushort instr)
        {
            int memoryIndex = Utilities.ReadSecondByte(instr);

            // Wrap the memory index if it exceeds 0xFFFF
            memoryIndex %= 0x10000;
            Value = Memory.ReadValueFromMemory((ushort)memoryIndex);
        }

        /// <summary>
        /// Will load the instruction
        /// </summary>
        /// <param name="instr"></param>
        /// <param name="supplValue"></param>
        private void LoadAbsoluteXY(ushort instr, byte supplValue)
        {

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
