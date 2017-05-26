using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;

namespace SilviaCore.Commands
{
    public class Command
    {
        public delegate void CommandAction(params string[] args);

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public CommandAction Action;
        public string Pattern { get; private set; }

        public Command(string pattern, CommandAction action)
        {
            Pattern = pattern;
            Action = action;
        }

        public void InvokeWithStringParams(string input)
        {
            List<string> args = new List<string>();
            Match isMatch = Regex.Match(input, Pattern);

            if (isMatch.Success)
            {
                logger.Trace("Executing command: " + input);

                string[] inputSplit = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string[] patternSplit = Pattern.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                List<string> options = new List<string>();

                for (int i = 0; i < patternSplit.Length; i++)
                {
                    //Attempt to match regular expressions
                    if (Regex.Match(patternSplit[i], "(?=[^a-zA-Z0-9-\\.,+@:/=?\"])[^\\s]{2,}").Success)
                    {
                        options.Add(inputSplit[i]);
                    }
                }

                Action.Invoke(options.ToArray());
            }
        }

        public static bool operator ==(Command lhs, Command rhs)
        {
            return lhs.Pattern == rhs.Pattern &&
                lhs.Action == rhs.Action;
        }

        public static bool operator !=(Command lhs, Command rhs)
        {
            return !(lhs == rhs);
        }
    }
}
