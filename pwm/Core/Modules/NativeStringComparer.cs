#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace pwm.Core.Modules
{
    public class NativeStringComparer : IComparer, IComparer<string>
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int StrCmpLogicalW(string x, string y);

        public static readonly NativeStringComparer Default = new();

        private NativeStringComparer() { }

        public int Compare(string x, string y)
        {
            return StrCmpLogicalW(x, y);
        }

        public int Compare(object x, object y)
        {
            return Compare(x.ToString(), y.ToString());
        }
    }
}
