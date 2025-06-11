using System;
using System.Collections.Generic;
using CommandLine;
using OwlToT4templatesTool.ArgumentParser;

namespace OwlToT4templatesTool.ArgumentParser
{
    public static class CMDParse
    {
        /// <summary>
        /// Input arguments application
        /// </summary>
        public static InputArguments InputArguments;

        /// <summary>
        /// Parse arguments to InputArguments 
        /// </summary>
        /// <param name="args"></param>
        public static void ParseArguments(string[] args)
        {
            Parser.Default.ParseArguments<InputArguments>(args)
                .WithParsed(Run)
                .WithNotParsed(HandleParseError);
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            if (errs.IsVersion())
            {
                //Program.LogMessage("Version Request");
                return;
            }

            if (errs.IsHelp())
            {
                //Program.LogMessage("Help Request");
                return;
            }
            //Program.LogMessage("Parser Fail");
        }

        private static void Run(InputArguments opts)
        {
            InputArguments = opts;
            //Program.LogMessage("Parser success");
        }
    }
}
