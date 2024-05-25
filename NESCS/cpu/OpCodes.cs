using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESCS.CPU
{
    /// <summary>
    /// Stores the NES 6502 operation codes.
    /// </summary>
    public static class OpCodes
    {
        // LDA INSTRUCTIONS
        public static byte LdaImmediate => 0xA9;
        public static byte LdaZeroPage => 0xA5;
        public static byte LdaZeroPageX => 0xB5;
        public static byte LdaAbsolute => 0xAD;
        public static byte LdaAbsoluteX => 0xBD;
        public static byte LdaAbsoluteY => 0xB9;
        public static byte LdaIndexedIndirect => 0xA1;
        public static byte LdaIndirectIndexed => 0xB1;

        // LDX INSTRUCTIONS
        public static byte LdxImmediate => 0xA2;
        public static byte LdxZeroPage => 0xA6;
        public static byte LdxZeroPageY => 0xB6;
        public static byte LdxAbsolute => 0xAE;
        public static byte LdxAbsoluteY => 0xBE;

        // LDY INSTRUCTIONS
        public static byte LdyImmediate => 0xA0;
        public static byte LdyZeroPage => 0xA4;
        public static byte LdyZeroPageX => 0xB4;
        public static byte LdyAbsolute => 0xAC;
        public static byte LdyAbsoluteX => 0xBC;
    }
}
