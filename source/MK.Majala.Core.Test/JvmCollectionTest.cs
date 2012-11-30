namespace MK.Majala.Core.Test
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    /// <summary>
    /// Unit-Tests of the class JavaVersionComparer.
    /// </summary>
    [TestFixture]
    public static class JvmCollectionTest
    {
        /// <summary>
        /// Tests adding of custom Java versions.
        /// </summary>
        [Test]
        public static void TestCustomVersions()
        {
            JvmCollection auto = new JvmCollection();
            JvmCollection manual = new JvmCollection(false);

            Assert.IsFalse(manual.AddCustomJvm(@"C:\"));

            foreach (KeyValuePair<string, string> pair in auto.InstalledVersions)
                Assert.IsTrue(manual.AddCustomJvm(pair.Value));
        }

        /// <summary>
        /// Tests adding of installed Java versions.
        /// </summary>
        [Test]
        public static void TestInstalledVersions()
        {
            JvmCollection auto = new JvmCollection();
            JvmCollection manual = new JvmCollection(false);
            
            foreach(KeyValuePair<string, string> pair in auto.InstalledVersions)
                manual.AddInstalledJvm(pair.Key, pair.Value);

            Assert.AreEqual(auto.InstalledVersions, manual.InstalledVersions);
        }
    }
}
