using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoWF.Common
{
    /// <summary>
    /// Immutable abstract class for command line tools that should process the content of a passed folder
    /// </summary>
    public abstract class PhotoFolderProcessorBase
    {
        /// <summary>
        /// Folder that is processed
        /// </summary>
        public string SrcDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="srcDirectory_">Folder to be processed</param>
        public PhotoFolderProcessorBase(string srcDirectory_)
        {
            SrcDirectory = srcDirectory_;
        }

        /// <summary>
        /// This method should be implemented in all the derived classes
        /// </summary>
        public abstract int DoWork();
    }
}
