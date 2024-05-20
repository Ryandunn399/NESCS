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
        public void TestLoadImmediateAccumulator()
        {
            Memory mem = Cpu.Memory;

            mem.WriteInstructionIntoMemory(0xA900);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0, Cpu.A.Value);
            Assert.IsTrue(Cpu.ZeroFlagSet());
            Assert.IsFalse(Cpu.NegativeFlagSet());

            mem.WriteInstructionIntoMemory(0xA9FF);

            Cpu.Fetch();
            Cpu.Execute();

            Assert.AreEqual(0xFF, Cpu.A.Value);
            Assert.IsFalse(Cpu.ZeroFlagSet());
            Assert.IsTrue(Cpu.NegativeFlagSet());
        }
    }
}