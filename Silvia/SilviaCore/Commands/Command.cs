using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;
using System.Runtime.CompilerServices;

namespace SilviaCore.Commands
{
    public class Command
    {
        public delegate void CommandAction(params string[] args);

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public CommandAction Action;
        public string Pattern { get; private set; }
        public string Prediction { get; private set; }
        public bool IsHidden { get; set; }

        public Command(string pattern, CommandAction action)
        {
            Pattern = pattern;
            Action = action;

            string ptrn = Pattern;

            if (ptrn.StartsWith("^"))
                ptrn = ptrn.Remove(0, 1);

            if (ptrn.EndsWith("$"))
                ptrn = ptrn.Remove(ptrn.Length - 1, 1);

            foreach (string s in ptrn.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (Regex.Match(s, "^[a-zA-Z0-9]+$").Success)
                {
                    Prediction += s;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsMatch(string input)
        {
            return Regex.Match(input, Pattern).Success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool PredictMatch(string input)
        {
            if (input.Length <= Prediction.Length)
            {
                return Prediction.StartsWith(input, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                string[] ptrnSplit = Pattern.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string[] predSplit = Prediction.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return Prediction.StartsWith(input.Substring(0, Prediction.Length), StringComparison.OrdinalIgnoreCase) && ptrnSplit.Length > predSplit.Length;
            }
        }

        public void InvokeWithStringParams(string input)
        {
            if (IsMatch(input))
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

        public override string ToString()
        {
            return Prediction;
        }
    }
}
