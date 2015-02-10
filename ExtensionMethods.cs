using System;
using System.Text;
namespace ExtensionMethods
{
    /// <summary>
    /// StringBuilder Extension Method to prepend string 
    /// </summary>
    public static class MyExtensions
    {
        public static StringBuilder Prepend(this StringBuilder sb, string content)
        {
            return sb.Insert(0, content);
        }

        /// <summary>
        /// Is this string a valid hex # containing character 0-9 a-f A-F ?
        /// </summary>
        public static bool IsHexNum(this string hexVal)
        {
            foreach (char c in hexVal)
            {
                if (('0' <= c && c <= '9') || ('a' <= c && c <= 'f') || ('A' <= c && c <= 'F'))
                    continue;
                else
                    return false;
            }
            return true;
        }
    }
}