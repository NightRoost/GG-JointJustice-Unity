using TextDecoder.Parser;

namespace TextDecoder
{
    public class ActionParser
    {
        public const char ACTION_TOKEN = '&';
        public const char ACTION_SEPARATOR = ':';
        public const char PARAMETER_SEPARATOR = ',';
        
        public ScriptAction ScriptAction { get; }
        
        public ActionParser(string actionLine)
        {
            ScriptAction = GetActionAndParameters(actionLine);
        }
        
        private static ScriptAction GetActionAndParameters(string actionLine)
        {
            actionLine = actionLine.Trim();
            ScriptAction scriptAction = default;
            var actionWithParameters = actionLine.Substring(1, actionLine.Length - 1).Split(ACTION_SEPARATOR);
            
            if (actionWithParameters.Length > 2)
            {
                throw new ScriptParsingException($"More than one '{ACTION_SEPARATOR}' detected in line '{actionWithParameters}'");
            }
            
            scriptAction.Name = actionWithParameters[0];
            if (actionWithParameters.Length > 1)
            {
                scriptAction.Parameters = actionWithParameters[1].Split(PARAMETER_SEPARATOR);
            }

            return scriptAction;
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