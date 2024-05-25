using NESCS;
using NESCS.CPU;

namespace NESCS_Test
{
    /// <summary>
    /// Will test the load instructions associated with CPU6502.
    /// </summary>
    [TestClass]
    public class TestLoadInstructions
    {

        private CPU6502 Cpu = null!;

        /// <summary>
        /// Setup function.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            Memory mem = new();
            Cpu = new CPU6502(mem);
        }

        /// <summary>
        /// Cleanup function.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            Cpu.Memory = null!;
            Cpu = null!;
        }

        [TestMethod]
        public void TestLDAImmediate()
        {
            Memory mem = Cpu.Memory;

            mem.WriteByteIntoMemory(0xA9);
            mem.WriteByteIntoMemory(0x39);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0x39, Cpu.A.Value);
            Assert.IsFalse(Cpu.ZeroFlagSet());
            Assert.IsFalse(Cpu.NegativeFlagSet());

            mem.WriteByteIntoMemory(0xA9);
            mem.WriteByteIntoMemory(0x00);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0, Cpu.A.Value);
            Assert.IsTrue(Cpu.ZeroFlagSet());
            Assert.IsFalse(Cpu.NegativeFlagSet());

            mem.WriteByteIntoMemory(0xA9);
            mem.WriteByteIntoMemory(0xF1);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0xF1, Cpu.A.Value);
            Assert.IsFalse(Cpu.ZeroFlagSet());
            Assert.IsTrue(Cpu.NegativeFlagSet());
        }

        [TestMethod]
        public void TestLDAZeroPage()
        {
            Memory mem = Cpu.Memory;

            // Setup memory space
            mem.WriteByteIntoMemory(0x0, 0x69);
            mem.WriteByteIntoMemory(0x08, 0xFF);
            mem.WriteByteIntoMemory(0xFF, 0xAB);
            mem.WriteByteIntoMemory(0xA5);
            mem.WriteByteIntoMemory(0x00);
            mem.WriteByteIntoMemory(0xA5);
            mem.WriteByteIntoMemory(0x08);
            mem.WriteByteIntoMemory(0xA5);
            mem.WriteByteIntoMemory(0xFF);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0x69, Cpu.A.Value);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0xFF, Cpu.A.Value);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0xAB, Cpu.A.Value);
        }

        [TestMethod]
        public void TestLDAZeroPageX()
        {
            Memory mem = Cpu.Memory;

            Cpu.X.UpdateRegisterValue(0x65);

            // We should end up at mem address 5D, write value here
            mem.WriteByteIntoMemory(0x5D, 0xE3);

            // Setup instruction
            mem.WriteByteIntoMemory(0xB5);
            mem.WriteByteIntoMemory(0xF8);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0xE3, Cpu.A.Value);
        }

        [TestMethod]
        public void TestLDAAbsolute()
        {
            Memory mem = Cpu.Memory;

            mem.WriteByteIntoMemory(0xF332, 0xE3);
            mem.WriteByteIntoMemory(0x0, 0xAB);
            mem.WriteByteIntoMemory(0xFFFF, 0x96);

            mem.WriteByteIntoMemory(0xAD);
            mem.WriteByteIntoMemory(0xF3);
            mem.WriteByteIntoMemory(0x32);

            mem.WriteByteIntoMemory(0xAD);
            mem.WriteByteIntoMemory(0x00);
            mem.WriteByteIntoMemory(0x00);

            mem.WriteByteIntoMemory(0xAD);
            mem.WriteByteIntoMemory(0xFF);
            mem.WriteByteIntoMemory(0xFF);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0xE3, Cpu.A.Value);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0xAB, Cpu.A.Value);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0x96, Cpu.A.Value);
        }

        [TestMethod]
        public void TestLDAAbsoluteXY()
        {
            Memory mem = Cpu.Memory;

            mem.WriteByteIntoMemory(0x4000, 0x8A);
            mem.WriteByteIntoMemory(0x1337, 0xCD);

            mem.WriteByteIntoMemory(0xBD);
            mem.WriteByteIntoMemory(0x3F);
            mem.WriteByteIntoMemory(0xFF);

            Cpu.X.UpdateRegisterValue(0x1);

            mem.WriteByteIntoMemory(0xB9);
            mem.WriteByteIntoMemory(0x13);
            mem.WriteByteIntoMemory(0x00);

            Cpu.Y.UpdateRegisterValue(0x37);

            Cpu.Fetch();
            Cpu.Execute();
            Assert.AreEqual(0x8A, Cpu.A.Value);
            Assert.AreEqual(Cpu.Cycles, 5);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0xCD, Cpu.A.Value);
            Assert.AreEqual(Cpu.Cycles, 4);
        }

        [TestMethod]
        public void TestLDAIndexedIndirect()
        {
            Memory mem = Cpu.Memory;

            // Final destination
            mem.WriteByteIntoMemory(0x85, 0xFA);

            // The memory address/value instruction addr points to.
            mem.WriteByteIntoMemory(0x9A, 0x70);

            // The memory address/value register X points to.
            mem.WriteByteIntoMemory(0x37, 0x15);

            // Store address 37 in the register
            Cpu.X.UpdateRegisterValue(0x37);

            // Write instruction and memory address 9a to be executed 
            mem.WriteByteIntoMemory(0xA1);
            mem.WriteByteIntoMemory(0x9A);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0xFA, Cpu.A.Value);

            // Final destination
            mem.WriteByteIntoMemory(0xF7, 0xCD);

            // The memory address/value instruction addr points to.
            mem.WriteByteIntoMemory(0x12, 0xFF);

            // The memory address/value register X points to.
            mem.WriteByteIntoMemory(0x34, 0xF8);

            // Store address 37 in the register
            Cpu.X.UpdateRegisterValue(0x34);

            // Write instruction and memory address 9a to be executed 
            mem.WriteByteIntoMemory(0xA1);
            mem.WriteByteIntoMemory(0x12);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0xCD, Cpu.A.Value);
            Assert.AreEqual(6, Cpu.Cycles);
        }

        [TestMethod]
        public void TestLDAIndirectIndexed()
        {
            Memory mem = Cpu.Memory;

            // Final destination
            mem.WriteByteIntoMemory(0x85, 0xFA);

            // The memory address/value instruction addr points to.
            mem.WriteByteIntoMemory(0x9A, 0x70);

            // Store address 37 in the register
            Cpu.Y.UpdateRegisterValue(0x15);

            // Write instruction and memory address 9a to be executed 
            mem.WriteByteIntoMemory(0xB1);
            mem.WriteByteIntoMemory(0x9A);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0xFA, Cpu.A.Value);
            Assert.AreEqual(5, Cpu.Cycles);

            // Final destination
            mem.WriteByteIntoMemory(0xF7, 0xCD);

            // The memory address/value instruction addr points to.
            mem.WriteByteIntoMemory(0x12, 0xFF);

            // Store address 37 in the register
            Cpu.Y.UpdateRegisterValue(0xF8);

            // Write instruction and memory address 9a to be executed 
            mem.WriteByteIntoMemory(0xB1);
            mem.WriteByteIntoMemory(0x12);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0xCD, Cpu.A.Value);
            Assert.AreEqual(6, Cpu.Cycles);
        }
    }
}