using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESCS.CPU
{
    /// <summary>
    /// Enumeration of the available address modes when loading instructions.
    /// </summary>
    public enum AddressMode
    {
        Immediate,
        ZeroPage,
        ZeroPageXY,
        Absolute,
        AbsoluteX,
        AbsoluteY,
        IndirectX,
        IndirectY
    }
}
