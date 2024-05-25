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
        /// Helper function to extract first byte from 16 bit value.
        /// </summary>
        /// <param name="instr">the instruction to extract the value from.</param>
        /// <returns>first byte from instruction.</returns>
        public static byte ReadFirstByte(ushort val)
        {
            return (byte)((val & 0xff00) >> 8);
        }

        /// <summary>
        /// Helper function to extract second byte from 16 bit value.
        /// </summary>
        /// <param name="instr">the instruction to extract the value from.</param>
        /// <returns>second byte from instruction.</returns>
        public static byte ReadSecondByte(ushort val)
        {
            return (byte)(val & 0xff);
        }

        /// <summary>
        /// Will try and determine if the pages of an address and supplemental index cross pages.
        /// </summary>
        /// <param name="addr">The original address.</param>
        /// <param name="rVal">The index we will apply to the address.</param>
        /// <returns>True if the first byte of a 16 bit value remains the same after addition.</returns>
        public static bool PagesCrossed(uint addr, byte rVal)
        {
            uint absAddr = addr + rVal;
            return ((absAddr & 0xFF00) != (addr & 0xFF00));
        }

        public static byte ZeroPageWrapAround(byte v1, byte v2)
        {
            return (byte)((v1 + v2) & 0xFF);
        }
    }
}
