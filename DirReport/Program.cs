using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PhotoWF.Common;

namespace PhotoWF.DirProc
{
    /// <summary>
    /// Main program for Directory processing
    /// </summary>
    class DirectoryProcessorProgram : CommandLineToolBase
    {
        /// <summary>
        /// The mode how the tool was kicked off
        /// </summary>
        public DirProcessorMode Mode
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="CommandLineToolBase.ProgramName"/>
        /// </summary>
        protected override string ProgramName
        {
            get { return "PhotoWFDirReport"; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="args_">Command line arguments</param>
        public DirectoryProcessorProgram(string[] args_)
            : base(args_)
        {
        }

        /// <summary>
        /// <see cref="CommandLineToolBase.SwitchDescription"/>
        /// </summary>
        protected override string SwitchDescription
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("'m' - Missing 'best' folder report mode, checks the 1st path and list the YYYY-MM-DD folders that do not contain 'best' folders");
                sb.AppendLine("'f' - File type report mode, checks the 1st path and lists all the file types found");
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
                case 'm': Mode = DirProcessorMode.ReportMissingFolders; return true;
                case 'f': Mode = DirProcessorMode.ReportFileTypes; return true;
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
                sb.AppendLine("'m'issing 'best' folder report mode:");
                sb.AppendFormat(@"{0} m C:\Photo\2010\ ",ProgramName);
                sb.AppendLine();
                sb.AppendLine("'f'ile type report mode:");
                sb.AppendFormat(@"{0} f C:\Photo\2010\ ", ProgramName);
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
                sb.AppendFormat("Please use the {0} according to the following instructions:", ProgramName);
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
                return new DirectoryProcessor(Args[Args.Length - 1], Mode);
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
            return CommandLineToolBase.RunMain(new DirectoryProcessorProgram(args_));
        }  
    }
}
