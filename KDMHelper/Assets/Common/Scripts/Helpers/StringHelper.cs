using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Helpers
{
    public static class StringHelper
    {
        public static string StringToCSVCell(string i_Str)
        {
            int matchIndex = i_Str.IndexOfAny(new char[] { ',', '"', '\r', '\n' });
            if (matchIndex >= 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('"');

                int startCopy = 0;
                int maxIndex = i_Str.Length - 1;
                matchIndex = i_Str.IndexOf('"', matchIndex);

                while (matchIndex >= 0 )
                {
                    sb.Append(i_Str, startCopy, matchIndex);
                    sb.Append('\\');
                    startCopy = matchIndex;
                    if(matchIndex >= maxIndex)
                    {
                        break;
                    }
                    matchIndex = i_Str.IndexOf('"', matchIndex + 1);
                }

                sb.Append(i_Str, startCopy, i_Str.Length - startCopy);
                sb.Append('"');
                return sb.ToString();
            }

            return i_Str;
        }
    }
}
