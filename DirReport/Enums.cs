using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoWF.DirProc
{
    /// <summary>
    /// Mode of the directory processing
    /// </summary>
    public enum DirProcessorMode
    {
        //m switch - looking and reporting for missing best folders
        ReportMissingFolders,

        //f switchs - reporting all the file types found
        ReportFileTypes
    }
}
