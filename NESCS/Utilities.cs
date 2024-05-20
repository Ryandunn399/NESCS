using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESCS
{
    /// <summary>
    /// Class will contain miscellanious helper functions.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Helper function to extract first byte from 16 bit instruction.
        /// </summary>
        /// <param name="instr">the instruction to extract the value from.</param>
        /// <returns>first byte from instruction.</returns>
        public static byte ReadFirstByte(ushort instr)
        {
            return (byte)((instr & 0xff00) >> 8);
        }

        /// <summary>
        /// Helper function to extract second byte from 16 bit instruction.
        /// </summary>
        /// <param name="instr">the instruction to extract the value from.</param>
        /// <returns>second byte from instruction.</returns>
        public static byte ReadSecondByte(ushort instr)
        {
            return (byte)(instr & 0xff);
        }
    }
}
