using System;
using TextDecoder.Parser;

namespace TextDecoder
{
    public class ScriptAction
    {
        public const char ACTION_TOKEN = '&';
        public const char ACTION_SEPARATOR = ':';
        public const char PARAMETER_SEPARATOR = ',';
        
        public string Name { get; }
        public string[] Parameters { get; }
        
        public ScriptAction(string actionLine)
        {
            actionLine = actionLine.Trim();
            var actionWithParameters = actionLine.Substring(1, actionLine.Length - 1).Split(ACTION_SEPARATOR);
            
            if (actionWithParameters.Length > 2)
            {
                throw new ScriptParsingException($"More than one '{ACTION_SEPARATOR}' detected in line '{actionWithParameters}'");
            }
            
            Name = actionWithParameters[0];
            Parameters = actionWithParameters.Length > 1 ? actionWithParameters[1].Split(PARAMETER_SEPARATOR) : Array.Empty<string>();
        }

        /// <summary>
        /// Determines if a line of dialogue is an action.
        /// </summary>
        /// <param name="line">The line to check.</param>
        /// <returns>If the line is an action (true) or not (false)</returns>
        public static bool IsAction(string line)
        {
            return line != string.Empty && line[0] == ACTION_TOKEN;
        }
    }
}