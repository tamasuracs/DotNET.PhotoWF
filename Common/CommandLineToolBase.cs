using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PhotoWF.Common
{
    /// <summary>
    /// A common abstract class giving features that are important in every command line application.
    /// 
    /// Like:
    /// - input param count validating
    /// - input switch validating
    /// - handling help request/showing help info
    /// - ...
    /// </summary>
    public abstract class CommandLineToolBase
    {
        /// <summary>
        /// Member field storing the command line arguments
        /// </summary>
        private string[] _args;

        /// <summary>
        /// The command line arguments
        /// </summary>
        protected string[] Args { get { return _args; } }


        protected abstract string ProgramName { get; } 

        /// <summary>
        /// Abstract member that specifies how to use the command line tool
        /// </summary>
        protected abstract string HelpString { get; }        

        /// <summary>
        /// Override this if the command line tool can be driven by input switches
        /// </summary>
        protected abstract string SwitchDescription
        {
            get;
        }

        /// <summary>
        /// Override this if you would like to show some examples for the usage of the command line tool
        /// </summary>
        protected virtual string ExampleDescription { get { return string.Empty; } }

        /// <summary>
        /// The underlying Software component that makes the processing
        /// </summary>
        protected abstract PhotoFolderProcessorBase Tool { get; }

        /// <summary>
        /// Maximum number of the arguments that can be applied
        /// </summary>
        protected abstract int MaxArgumentCount { get; }


        /// <summary>
        /// Writes the usage info of the command line tool to the STD OUTPUT
        /// </summary>
        protected void HelpInfo()
        {
            Console.WriteLine(HelpString);
            Console.WriteLine(SwitchDescription);
            Console.WriteLine(ExampleDescription);
        }

        /// <summary>
        /// Returns false the appropriate switch should not be appliead
        /// If the switch is valid the appropriate inner state changes should be made here.
        /// </summary>
        /// <param name="switch_"></param>
        /// <returns></returns>
        protected abstract bool CheckSwitch(char switch_);

        /// <summary>
        /// Checks all the switches
        /// </summary>
        /// <param name="switches_">String containing the switches to be applied.</param>
        private void checkSwitches(string switches_)
        {
            string invalidSwitches = null;

            foreach (char c in switches_)
            {
                if (!CheckSwitch(c))
                {
                    if (invalidSwitches == null)
                    {
                        invalidSwitches = c.ToString();
                    }
                    else
                    {
                        invalidSwitches = string.Format("{0}, '{1}'", invalidSwitches, c);
                    }
                }
            }

            if (invalidSwitches != null)
            {
                string msg = string.Format("Invalid switch(es) specified: {0} Use '?' to see available switches.", invalidSwitches);
                throw new ArgumentException(msg);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="args_">The command line arguments</param>
        public CommandLineToolBase(string[] args_)
        {
            this._args = args_;
        }

        /// <summary>
        /// This method should be called from the Main() method of the derived class.
        /// - Validates the command line arguments
        /// - Checks switches if applicable
        /// - Displays help string if needed
        /// - Starts processing
        /// - Displays processing result and errors 
        /// </summary>
        /// <param name="program_">The program instance that is executed</param>
        /// <returns>
        ///  0 - Everything was okay with the processing
        /// -1 - Help mode
        /// -2 - Error was captured by the underlying analyzer tool.
        /// -3 - Serious exception happend 
        /// </returns>
        public static int RunMain(CommandLineToolBase program_)
        {
            int result = 0;
            int processed = 0;

            try
            {
                int argCount = program_.Args.Length;

                //Input validation
                if (argCount > program_.MaxArgumentCount
                    ||
                        (
                        argCount > 1
                        &&
                        argCount != program_.MaxArgumentCount
                        &&
                        argCount != program_.MaxArgumentCount - 1
                        )
                    )
                {
                    throw new ArgumentException("Invalid number of arguments given. Use '?' to see available switches.");
                }

                

                //If help was requested
                if (argCount == 0 || program_.Args[0] == "?")
                {
                    //info is showed
                    program_.HelpInfo();
                    result = -1;
                }
                //Otherwise
                else
                {
                    //Argument validation
                    program_.checkSwitches(program_.Args[0]);
                    Stopwatch sw = new Stopwatch();

                    
                    try
                    {
                        //Duration of the execution is measured.
                        sw.Start();

                        processed =program_.Tool.DoWork();                    
                    }
                    finally
                    {
                        double avg = 0;
                        if (processed > 0)
                        {
                            avg = sw.ElapsedMilliseconds / processed;
                        }
                        //Stop measuring the ellapsed time
                        sw.Stop();
                        Console.WriteLine("\n\nProcessing finished. Ellapsed time:{0} Processed items: {1} (avg per item: {2} ms)", sw.Elapsed.ToString(), processed, avg);
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception e)
            {
                //If critical exception occured we display it.
                Console.WriteLine("{0}:{1}", e.Message, e.StackTrace);
                result = -3;
            }
            
            return result;
        }

        /// <summary>
        /// Event handler for log entry insertation
        /// Simply displays the newly added entry.
        /// </summary>
        /// <param name="source_">The log that was extended</param>
        /// <param name="args_">Event arguments</param>
        static void Log_EntryAdded(AssertLog source_, LogEntryAddedEventArgs args_)
        {
            Console.WriteLine(args_.Entry.Message);
        }
    }
}
