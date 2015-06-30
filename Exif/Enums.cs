using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoWF
{
    /// <summary>
    /// Mode of the file processing
    /// </summary>
    public enum ExifProcessingMode
    {
        //Gathers Exif keywords
        Gather,

        //Normal processing - if a file was tagged by the current program it is skipped
        Process_If_Was_Nottouched,

        //All the files will be processed - independently weather they were processed earlier by the program or not
        Force
    }
}
