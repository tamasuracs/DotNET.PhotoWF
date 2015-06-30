using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using PhotoWF.Common;

namespace PhotoWF
{   
    /// <summary>
    /// Main class for Exif tag processing
    /// 
    /// TODO: Ranking does not work
    /// TODO: - if "best" is present apply ranking 3    
    /// </summary>
    class ExifProcessorProgram : CommandLineToolBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="args_">Command line arguments</param>
        public ExifProcessorProgram(string[] args_)
            : base(args_)
        {
        }

        /// <summary>
        /// <see cref="CommandLineToolBase.ProgramName"/>
        /// </summary>
        protected override string ProgramName
        {
            get { return "PhotoWFExifProc"; }
        }

        /// <summary>
        /// The mode how the tool was kicked off
        /// </summary>
        private ExifProcessingMode Mode
        {
            get;
            set;
        }

        /// <summary>
        /// <see cref="CommandLineToolBase.SwitchDescription"/>
        /// </summary>
        protected override string SwitchDescription
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("'g' - Gather distinct EXIF keyword tags");
                sb.AppendLine("'n' - Normal processing - if a file was tagged by the current program it is skipped ");
                sb.AppendLine("'f' - Forced processing - All the files will be processed; independently weather they were processed earlier by the program or not ");
                return sb.ToString();
            }
        }

        /// <summary>
        /// <see cref="Common.CommandLineToolBase.CheckSwitch"/>
        /// </summary>
        protected override bool CheckSwitch(char switch_)
        {

            switch (switch_)
            {
                case 'g': Mode = ExifProcessingMode.Gather; return true;
                case 'n': Mode = ExifProcessingMode.Process_If_Was_Nottouched; return true;
                case 'f': Mode = ExifProcessingMode.Force; return true;
                default: return false;
            }
        }

        /// <summary>
        /// <see cref="Common.CommandLineToolBase.ExampleDescription"/>
        /// </summary>
        protected override string ExampleDescription
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine("Examples:");
                sb.AppendLine("---------");
                sb.AppendLine();
                sb.AppendLine("'g'ather - Gathers Exif keywords:");
                sb.AppendFormat(@"{0} g C:\Photo\2010\ ", ProgramName);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("'n'ormal - processing:");
                sb.AppendFormat(@"{0} n C:\Photo\2010\ ", ProgramName);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("'f'orce:");
                sb.AppendFormat(@"{0} f C:\Photo\2010\ ", ProgramName);
                sb.AppendLine();
                sb.AppendLine();
                return sb.ToString();
            }
        }



        /// <summary>
        /// <see cref="Common.CommandLineToolBase.HelpString"/>
        /// </summary>
        protected override string HelpString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine(" HELP info:");
                sb.AppendLine("=============================");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendFormat("Please use the {0} according to the following instructions:", ProgramName);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendFormat("{0} [switches] 'Path of the processed folder or file'", ProgramName);
                sb.AppendLine();
                return sb.ToString();
            }
        }

        /// <summary>
        /// <see cref="Common.CommandLineToolBase.Tool"/>
        /// </summary>
        protected override PhotoFolderProcessorBase Tool
        {
            get
            {
                return new ExifProcessor(Args[Args.Length - 1], Mode);
            }
        }

        /// <summary>
        /// 1 - Help mode - '?'
        /// 2 - Default switch - 1 file/path name
        /// </summary>
        protected override int MaxArgumentCount
        {
            get { return 2; }
        }

        /// <summary>
        /// The main entry point
        /// </summary>
        /// <param name="args_">Command line arguments passed</param>
        /// <returns></returns>
        public static int Main(string[] args_)
        {
            return CommandLineToolBase.RunMain(new ExifProcessorProgram(args_));
        }  

    }
}