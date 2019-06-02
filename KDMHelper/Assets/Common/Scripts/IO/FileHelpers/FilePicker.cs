using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Common.IO.FileHelpers
{
    public static class FilePicker
    {
        public enum SelectionResult
        {
            Accept,
            Reject,
            RejectAndEnd
        }
        

        public static List<FileLoadHandle> GetFiles(string directoryPath, Func<string, FileLoadHandle> filePathPicker, bool recursive = false)
        {
            return null;
        }
    }
}
