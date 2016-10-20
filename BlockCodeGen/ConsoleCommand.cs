﻿using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BlockCodeGen
{
    public class ConsoleCommand
    {
        private List<string> _arguments;

        public string Name { get; set; }
        public string LibraryClassName { get; set; }
        public IEnumerable<string> Arguments
        {
            get
            {
                return _arguments;
            }
        }

        public ConsoleCommand(string input)
        {
            // Ugly regex to split string on spaces, but preserve quoted text intact:
            var stringArray = Regex.Split(input, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            _arguments = new List<string>();
            for (int i = 0; i < stringArray.Length; i++)
            {
                // The first element is always the command:
                if (i == 0)
                {
                    this.Name = stringArray[i];

                    // Set the default:
                    this.LibraryClassName = "DefaultCommands";
                    string[] s = stringArray[0].Split('.');
                    if (s.Length == 2)
                    {
                        this.LibraryClassName = s[0];
                        this.Name = s[1];
                    }
                }
                else
                {
                    var inputArgument = stringArray[i];
                    string argument = inputArgument;

                    // Is the argument a quoted text string?
                    var regex = new Regex("\"(.*?)\"", RegexOptions.Singleline);
                    var match = regex.Match(inputArgument);

                    if (match.Captures.Count > 0)
                    {
                        // Get the unquoted text:
                        var captureQuotedText = new Regex("[^\"]*[^\"]");
                        var quoted = captureQuotedText.Match(match.Captures[0].Value);
                        argument = quoted.Captures[0].Value;
                    }

                    _arguments.Add(argument);
                }
            }
        }
    }
}
