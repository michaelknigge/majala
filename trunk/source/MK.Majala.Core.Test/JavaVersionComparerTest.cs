namespace MK.Majala.Core.Test
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    /// <summary>
    /// Unit-Tests of the class JavaVersionComparer.
    /// </summary>
    [TestFixture]
    public static class JavaVersionComparerTest
    {
        /// <summary>
        /// Tests the IComparer implementation JavaVersionComparer.
        /// </summary>
        [Test]
        public static void TestVersionStrings()
        {
            SortedDictionary<string, string> versions = new SortedDictionary<string, string>(new JavaVersionComparer());
            versions.Add("1.0", "x");
            versions.Add("1.0.1", "x");
            versions.Add("1.7.0_02", "x");
            versions.Add("1.7", "x");
            versions.Add("1.3.0", "x");
            versions.Add("1.7.0_01-ea", "x");
            versions.Add("2.0", "x");
            versions.Add("1.3.0_01", "x");
            versions.Add("1.6-ea", "x");
            versions.Add("1.5.5_00", "x");
            versions.Add("1.01.1_00", "x");
            versions.Add("1.3.1", "x");
            versions.Add("3.1-1_12", "x");
            versions.Add("1.01.0_00", "x");
            versions.Add("1.01.0_01", "x");
            versions.Add("1.3.1_01", "x");
            versions.Add("3.0", "x");
            versions.Add("9.9.9.9-1", "x");

            List<string> sorted = new List<string>();
            foreach (string k in versions.Keys)
                sorted.Add(k);

            int ix = 0;
            Assert.AreEqual("1.0", sorted[ix++]);
            Assert.AreEqual("1.0.1", sorted[ix++]);
            Assert.AreEqual("1.01.0_00", sorted[ix++]);
            Assert.AreEqual("1.01.0_01", sorted[ix++]);
            Assert.AreEqual("1.01.1_00", sorted[ix++]);
            Assert.AreEqual("1.3.0", sorted[ix++]);
            Assert.AreEqual("1.3.0_01", sorted[ix++]);
            Assert.AreEqual("1.3.1", sorted[ix++]);
            Assert.AreEqual("1.3.1_01", sorted[ix++]);
            Assert.AreEqual("1.5.5_00", sorted[ix++]);
            Assert.AreEqual("1.6-ea", sorted[ix++]);
            Assert.AreEqual("1.7", sorted[ix++]);
            Assert.AreEqual("1.7.0_01-ea", sorted[ix++]);
            Assert.AreEqual("1.7.0_02", sorted[ix++]);
            Assert.AreEqual("2.0", sorted[ix++]);
            Assert.AreEqual("3.0", sorted[ix++]);
            Assert.AreEqual("3.1-1_12", sorted[ix++]);
            Assert.AreEqual("9.9.9.9-1", sorted[ix++]);
        }
    }
}
