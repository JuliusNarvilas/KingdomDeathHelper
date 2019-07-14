using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.IO
{
    public static class IOHelpers
    {
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[4 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }

            output.Flush();
        }
    }
}
