using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using PhotoWF.Common;

namespace PhotoWF.DirProc
{
    /// <summary>
    /// Class for the File System Directory processing
    /// </summary>
    public class DirectoryProcessor : PhotoFolderProcessorBase
    {
        /// <summary>
        /// File types found
        /// </summary> 
        private HashSet<string> _fileTypes = new HashSet<string>();

        /// <summary>
        /// Processing mode
        /// </summary>
        public DirProcessorMode Mode
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dir_">File system directory to be processed</param>
        /// <param name="mode_">Processing mode</param>
        public DirectoryProcessor(string dir_, DirProcessorMode mode_)
            : base(dir_)
        {
            Mode = mode_;
        }

        /// <summary>
        /// Performs the processing and writes the result to the std output
        /// </summary>
        /// <returns></returns>
        public override int DoWork()
        {
            Console.WriteLine();
            int result = 0;
            _fileTypes.Clear();
            if (Mode == DirProcessorMode.ReportMissingFolders )
            {
                
                Console.WriteLine("-- FOLDERS WITH NON EXISTING BEST FOLDER benath: '{0}' ---", SrcDirectory);
                Console.WriteLine();
                result = ProcessPath(base.SrcDirectory);
            }

            if (Mode == DirProcessorMode.ReportFileTypes)
            {
                Console.WriteLine("-- File Types beaneath: '{0}' ---", SrcDirectory);
                Console.WriteLine();
                result = ProcessPath(base.SrcDirectory);
            }

            return result;
        }

        /// <summary>
        /// Processes the path - if it's a directory
        /// </summary>
        /// <param name="path_"></param>
        /// <returns></returns>
        public int ProcessPath(string path_)
        {
            int result = 0;
            DirectoryInfo currentDir = new DirectoryInfo(path_);

            try
            {
                if ((File.GetAttributes(path_) & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    result = processFolder(path_);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine("Error while processing '{0}' path. Message: {1}", path_, ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Based on the mode either the missing BEST folders are reported 
        /// or the types of the found files are printed
        /// </summary>
        /// <param name="path_">The path to be checked</param>
        /// <returns></returns>
        private int processFolder(string path_)
        {
            int result = 1;
            if (Helper.isMainFolder(path_))
            {
                bool valid = false;
                DirectoryInfo currentDir = new DirectoryInfo(path_);
                var dirs = currentDir.GetDirectories();

                if (dirs.Length > 0)
                {
                    foreach (DirectoryInfo dir in dirs)
                    {
                        if (Helper.isBestFolder(dir.FullName))
                        {
                            valid = true;
                            break;
                        }
                    }

                    result++;
                }

                if (Mode == DirProcessorMode.ReportMissingFolders)
                {
                    if (!valid)
                    {
                        System.Console.WriteLine("{0}", path_);
                    }

                    return result;
                }
                
                if (Mode == DirProcessorMode.ReportFileTypes)
                {
                    var files = currentDir.GetFiles();

                    if (files.Length > 0)
                    {
                        
                        Array.ForEach(files, 
                                            file =>{
                                                var ext = file.Extension.ToLower().Replace(".",string.Empty);
                                                if (!_fileTypes.Contains(ext))
                                                {
                                                    _fileTypes.Add(ext);
                                                    System.Console.WriteLine("{0} :         found in '{1}' directory",ext, path_);
                                                }
                                                    }
                                            );
                    }
                }
            }


            string[] dirss = Directory.GetDirectories(path_);

            foreach (string dir in dirss)
            {
                result += ProcessPath(dir);
            }

            return result;
        }

        /// <summary>
        /// Looks for a file - based on the name
        /// </summary>
        /// <param name="dstFiles_">FileInfo array of the checked files </param>
        /// <param name="srcFileInfo_">The file searched</param>
        /// <returns></returns>
        private static bool findFile(FileInfo[] dstFiles_, FileInfo srcFileInfo_)
        {
            bool found = false;

            foreach (var dstFile in dstFiles_)
            {
                if (dstFile.Name == srcFileInfo_.Name)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }


    }
}
