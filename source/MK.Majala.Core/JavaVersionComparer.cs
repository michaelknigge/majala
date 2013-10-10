namespace MK.Majala.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This IComparer implementation compares Java version strings according to the specification
    /// at http://www.oracle.com/technetwork/java/javase/versioning-naming-139433.html.
    /// </summary>
    public sealed class JavaVersionComparer : IComparer<string>
    {
        int IComparer<string>.Compare(string x, string y)
        {
            // Non-official Java versions may contain a "-", like "1.6.0_01-earlybird".
            // This appendix is ignored...
            string v1 = x.Contains("-") ? x.Substring(0, x.IndexOf("-", StringComparison.Ordinal)) : x;
            string v2 = y.Contains("-") ? y.Substring(0, y.IndexOf("-", StringComparison.Ordinal)) : y;

            return this.CompareVersions(v1, v2);
        }

        private int CompareVersions(string version1, string version2)
        {
            // The underscore is replaced by a "." so we can use the .NET Version class
            // to compare the versions. So "1.6.0_01" becomes "1.6.0.01" which can be
            // used for the Version class.
            Version v1 = new Version(version1.Replace("_", "."));
            Version v2 = new Version(version2.Replace("_", "."));
            
            return v1.CompareTo(v2);
        }
    }
}
