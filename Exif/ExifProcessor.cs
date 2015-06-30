using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using PhotoWF.Common;

namespace PhotoWF
{
    public class ExifProcessor : PhotoFolderProcessorBase
    {
        /// <summary>
        /// Arguments structure for the Exif keyword processing 
        /// </summary>
        internal struct KeywordProcArgs
        {
            /// <summary>
            /// Path to be processed
            /// </summary>
            internal string Path { get; set; }

            /// <summary>
            /// Flag if path based keywords should be populated or not
            /// </summary>
            internal bool PopulatePathBasedKeywords { get; set; }
            
            /// <summary>
            /// Flag if normalization should be performed or not
            /// </summary>
            internal bool applyNormalization { get; set; }

            /// <summary>
            /// Count of the files processed so far
            /// </summary>
            internal int FilesProcessed { get; set; }

            /// <summary>
            /// Count of the total files to be processed
            /// </summary>
            internal int FilesToProcess { get; set; }

            internal KeywordProcArgs Clone()
            {
                return new KeywordProcArgs
                {
                    applyNormalization = this.applyNormalization,
                    FilesProcessed = this.FilesProcessed,
                    FilesToProcess = this.FilesToProcess,
                    Path = this.Path,
                    PopulatePathBasedKeywords = this.PopulatePathBasedKeywords

                };
            }
        }

        /// <summary>
        /// Processing mode
        /// </summary>
        public ExifProcessingMode ProcessMode
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path_">File system path to be processed</param>
        /// <param name="mode_">Processing mode</param>
        public ExifProcessor(string path_, ExifProcessingMode mode_)
            : base(path_)
        {
            ProcessMode = mode_;
        }

        /// <summary>
        /// Collecting the keyword statistics
        /// </summary>
        private SortedDictionary<string, int> _distinctKeywords = new SortedDictionary<string, int>();

        
        /// <summary>
        /// Method Processing
        /// </summary>
        /// <returns></returns>
        private int process(KeywordProcArgs args_)
        {
            int result = 0;
            DirectoryInfo currentDir = new DirectoryInfo(args_.Path);

            if (args_.FilesToProcess == 0)
            {
                args_.FilesToProcess = Helper.jpgFileCount(args_.Path);
            }

            try
            {
                if (
                    (File.GetAttributes(args_.Path) & FileAttributes.Directory) == FileAttributes.Directory
                    )
                {
                    result = processFolder(args_);
                }
                else
                {
                    bool res = processFile(args_.Path, args_.PopulatePathBasedKeywords, args_.applyNormalization);
                    result++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine("Error while processing '{0}' path. Message: {1}", args_.Path, ex.Message);
            }

            return result;
        }

        /// <summary>
        ///  Processing a folder
        /// </summary>
        /// <param name="args_">
        /// Arguments for the processing
        /// </param>
        /// <returns></returns>
        private int processFolder(KeywordProcArgs args_)
        {
            int result = 0;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            string[] f = Directory.GetFiles(args_.Path, "*.jpg");
            foreach (string file in f)
            {
                var arg = args_.Clone();
                arg.Path = file;
                result += process(arg);
            }
            int processed = args_.FilesProcessed+ result;
            Helper.PrintStatistics(args_.Path, processed, result, args_.FilesToProcess, sw.ElapsedMilliseconds);

            string[] dirs = Directory.GetDirectories(args_.Path);

            foreach (string dir in dirs)
            {
                var arg = args_.Clone();
                arg.Path = dir;
                arg.FilesProcessed += result;

                var r = process(arg);
                result += r;
            }
            sw.Stop();
            return result;
        }

       
        /// <summary>
        /// Processing a file
        /// </summary>
        /// <param name="path_">path of the file</param>
        /// <param name="populatePathBaseKeyword_">flag if path based keywords should be populated or not</param>
        /// <param name="applyNormalization_">flag if normalization should be performed or not</param>
        /// <returns></returns>
        private bool processFile(string path_, bool populatePathBaseKeyword_, bool applyNormalization_)
        {
            string tmp = null;
            bool saved = false;
            try
            {
                using (JpgMeta meta = Helper.createDecoderForImage(path_))
                {
                    BitmapDecoder original = meta.Decoder;

                    if (original == null)
                    {
                        return false;
                    }

                    var keys = Helper.getKeywords(meta);

                    if (ProcessMode == ExifProcessingMode.Gather)
                    {
                        extendDistincKeywords(keys);
                        return true;
                    }

                    if (ProcessMode == ExifProcessingMode.Process_If_Was_Nottouched
                        &&
                        keys.Contains(Constants.PROGRAM_TAG_SIGNATURE))
                    {
                        return false;
                    }

                    tmp = Path.GetTempFileName();
                    saved = generateTempFile(path_, populatePathBaseKeyword_, applyNormalization_, tmp, original, keys);
                }

                if (saved)
                {
                    //Deleting old file
                    if (File.Exists(path_))
                    {
                        File.Delete(path_);
                    }
                    File.Move(tmp, path_);
                }

                return true;
            }
            finally
            {
                try
                {
                    if (tmp != null)
                    {
                        File.Delete(tmp);
                    }
                }
                catch
                {
                    ;
                }
            }
        }

        /// <summary>
        /// Temporary file creation
        /// </summary>
        /// <param name="path_"></param>
        /// <param name="populatePathBaseKeyword_"></param>
        /// <param name="applyNormalization_"></param>
        /// <param name="tmp"></param>
        /// <param name="original"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        private static bool generateTempFile(string path_, bool populatePathBaseKeyword_, bool applyNormalization_, string tmp, BitmapDecoder original, ICollection<string> keys)
        {
            bool result = true;

            JpegBitmapEncoder jpgEncoder = new JpegBitmapEncoder();
            using (Stream tmpFile = File.Open(tmp, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                var jpgFrame = BitmapFrame.Create(original.Frames[0]);
                var md = jpgFrame.Metadata as BitmapMetadata;

                if (md != null)
                {
                    //Setting the keywords
                    HashSet<string> keyWords = null;

                    if (populatePathBaseKeyword_)
                    {
                        keyWords = Helper.populateKeywords(md, applyNormalization_, path_, keys);
                    }
                    else
                    {
                        keyWords = Helper.populateKeywords(md, applyNormalization_, null, keys);
                    }

                    List<string> l = new List<string>();
                    foreach (string key in keyWords)
                    {
                        l.Add(key);
                    }

                    md.Keywords = new System.Collections.ObjectModel.ReadOnlyCollection<string>(l);

                    //Decision if the file has to be copied into the Lr directory or not
                    if (keyWords.Contains(Helper.normalizeKeyword(Constants.BEST)))
                    {
                        //TODO: Setting the rating  - this fails
                        md.Rating = 2;
                    }

                    jpgEncoder.Frames.Add(jpgFrame);
                    jpgEncoder.Save(tmpFile);
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        ///Updating keyword statistics variable
        /// </summary>
        /// <param name="keys_">keys to add </param>
        private void extendDistincKeywords(ICollection<string> keys_)
        {
            foreach (string key in keys_)
            {
                int value = 1;

                if (_distinctKeywords.TryGetValue(key, out value))
                {
                    _distinctKeywords[key] = value + 1;
                }
                else
                {
                    _distinctKeywords.Add(key, 1);
                }
            }
        }

        /// <summary>
        /// Performing the processing
        /// </summary>
        /// <returns></returns>
        public override int DoWork()
        {
            Console.WriteLine();
            var header = string.Format("-- Exif Processing benath: '{0}' ---", SrcDirectory);
            Console.WriteLine(header);
            Console.WriteLine();

            int result = 0;
            _distinctKeywords.Clear();
    
            var args = new KeywordProcArgs
            {
                Path = SrcDirectory,
                applyNormalization = true,
                PopulatePathBasedKeywords = true,
                FilesToProcess = 0,
                FilesProcessed = 0
            };

            result = process(args);


             if (_distinctKeywords.Count > 0)
             {
                 Console.WriteLine();
                 Console.WriteLine("Distinct keywords found beneath the '{0}' folder:", SrcDirectory);
                 Console.WriteLine("--------------------------------------------------------");
                 foreach (var entry in _distinctKeywords)
                 {
                     Console.WriteLine("{0}:{1}", entry.Key, entry.Value);
                 }
             }
             return result;

            
        }
    }
}
