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
        public byte LoadInstruction(AddressMode mode, ushort instr, byte supplValue)
        {
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

        /*
        /// <summary>
        /// Will load immedaite value into the A register.
        /// </summary>
        /// <returns>New value stored in the A register.</returns>
        private byte LdaImmediate()
        {
            A = Memory.ReadSecondByte(Instruction);
            UpdateStatusFlags("A");
            Cycles += 2;
            return A;
        }

        /// <summary>
        /// Will load a value into the A register from zero page memory range (0x00 to 0x00ff)
        /// </summary>
        /// <returns>New value stored in the A register.</returns>
        private byte LdaZeroPage()
        {
            A = Memory.ReadValueFromMemory(Memory.ReadSecondByte(Instruction));
            UpdateStatusFlags("A");
            Cycles += 3;
            return 0;
        }
        */
    }
}
