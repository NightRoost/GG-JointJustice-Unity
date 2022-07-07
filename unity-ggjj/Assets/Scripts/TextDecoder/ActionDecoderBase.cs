using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TextDecoder.Parser;

public abstract class ActionDecoderBase : IActionDecoder
{
    public const char ACTION_TOKEN = '&';

    /// <summary>
    ///     Parse action lines inside .ink files
    /// </summary>
    /// <param name="actionLine">Line of a .ink file that starts with &amp; (and thereby is not a "spoken dialogue" line)</param>
    /// <remarks>
    ///     Writers are able to call methods inside .ink files. This is done by using the following syntax:
    ///     <code>
    ///     &amp;{methodName}:{parameter1},{parameter2},...
    ///     </code>
    ///     This method is responsible for...
    ///         1. Getting method details using the GenerateInvocationDetails method
    ///         2. Invoking the found method with its parsed method parameters
    /// </remarks>
    public void InvokeMatchingMethod(string actionLine)
    {
        var method = GenerateInvocationDetails(actionLine, GetType());
        method.MethodInfo.Invoke(this, method.ParsedMethodParameters.ToArray());
    }

    public bool IsAction(string nextLine)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// This method is responsible for:
    ///     1. Finding a method inside this class, matching `methodName`
    ///     2. Verifying the amount of parameters matches the amount of parameters needed in the method
    ///     3. Attempting to parse each parameter into the correct type using <see cref="Parser{T}"/> of the type
    /// </summary>
    /// <param name="actionLine">The action line to parse</param>
    /// <param name="decoderType">The type of decoder to get methods from</param>
    /// <returns>An InvocationDetails with details of the found method and its parameters</returns>
    public static InvocationDetails GenerateInvocationDetails(string actionLine, Type decoderType)
    {
        actionLine = actionLine.Trim();
        const char ACTION_SIDE_SEPARATOR = ':';
        const char ACTION_PARAMETER_SEPARATOR = ',';

        var actionNameAndParameters = actionLine.Substring(1, actionLine.Length - 1).Trim().Split(ACTION_SIDE_SEPARATOR);

        if (actionNameAndParameters.Length > 2)
        {
            throw new ScriptParsingException($"More than one '{ACTION_SIDE_SEPARATOR}' detected in line '{actionLine}'");
        }

        var action = actionNameAndParameters[0];
        var parameters = (actionNameAndParameters.Length == 2) ? actionNameAndParameters[1].Split(ACTION_PARAMETER_SEPARATOR) : Array.Empty<string>();

        // Find method with exact same name as action inside script
        var methodInfo = decoderType.GetMethod(action, BindingFlags.Instance | BindingFlags.NonPublic);
        if (methodInfo == null)
        {
            throw new MethodNotFoundScriptParsingException(decoderType.FullName, action);
        }

        var methodParameters = methodInfo.GetParameters();
        var optionalParameters = methodParameters.Count(parameter => parameter.IsOptional);
        if (parameters.Length < (methodParameters.Length - optionalParameters) || parameters.Length > (methodParameters.Length))
        {
            throw new ScriptParsingException($"'{action}' requires {(optionalParameters == 0 ? "exactly" : "between")} {(optionalParameters == 0 ? methodParameters.Length.ToString() : $"{methodParameters.Length-optionalParameters} and {methodParameters.Length}")} parameters (has {parameters.Length} instead)");
        }

        var parsedMethodParameters = new List<object>();
        // For each supplied parameter of that action...
        for (var index = 0; index < parameters.Length; index++)
        {
            // Determine it's type
            var methodParameter = methodParameters[index];

            // Edge-case for enums
            if (methodParameter.ParameterType.BaseType == typeof(Enum))
            {
                try
                {
                    parsedMethodParameters.Add(Enum.Parse(methodParameter.ParameterType, parameters[index]));
                    continue;
                }
                catch (ArgumentException e)
                {
                    var pattern = new Regex(@"Requested value '(.*)' was not found\.");
                    var match = pattern.Match(e.Message);
                    if (match.Success)
                    {
                        throw new ScriptParsingException($"'{parameters[index]}' is incorrect as parameter #{index + 1} ({methodParameter.Name}) for action '{action}': Cannot convert '{match.Groups[1].Captures[0]}' into an {methodParameter.ParameterType} (valid values include: '{string.Join(", ", Enum.GetValues(methodParameter.ParameterType).Cast<object>().Select(a=>a.ToString()))}')");
                    }

                    if (e.Message == "Must specify valid information for parsing in the string.")
                    {
                        throw new ScriptParsingException($"'' is incorrect as parameter #{index + 1} ({methodParameter.Name}) for action '{action}': Cannot convert '' (empty) into an {methodParameter.ParameterType} (valid values include: '{string.Join(", ", Enum.GetValues(methodParameter.ParameterType).Cast<object>().Select(a => a.ToString()))}')");
                    }
                    throw;
                }
            }

            // Construct a parser for it
            var parser = decoderType.Assembly.GetTypes().FirstOrDefault(type => type.BaseType is { IsGenericType: true } && type.BaseType.GenericTypeArguments[0] == methodParameter.ParameterType);
            if (parser == null)
            {
                throw new MissingParserException($"The TextDecoder.Parser namespace contains no Parser for type {methodParameter.ParameterType}");
            }

            var parserConstructor = parser.GetConstructor(Type.EmptyTypes);
            if (parserConstructor == null)
            {
                throw new ArgumentException($"TextDecoder.Parser for type {methodParameter.ParameterType} has no constructor without parameters");
            }


            // Create a parser
            var parserInstance = parserConstructor.Invoke(Array.Empty<object>());
            object[] parseMethodParameters = { parameters[index], null };

            // Call the 'Parse' method
            var humanReadableParseError = parser.GetMethod("Parse")!.Invoke(parserInstance, parseMethodParameters);
            // If we received an error attempting to parse a parameter to the type, expose it to the user
            if (humanReadableParseError != null)
            {
                throw new ScriptParsingException($"'{parameters[index]}' is incorrect as parameter #{index + 1} ({methodParameter.Name}) for action '{action}': {humanReadableParseError}");
            }

            parsedMethodParameters.Add(parseMethodParameters[1]);
        }

        // If the method supports optional parameters, fill the remaining parameters based on the default value of the method
        for (var suppliedParameterCount = parameters.Length; suppliedParameterCount < methodParameters.Length; suppliedParameterCount++)
        {
            parsedMethodParameters.Add(methodParameters[suppliedParameterCount].DefaultValue);
        }

        return new InvocationDetails
        {
            MethodInfo = methodInfo,
            ParsedMethodParameters = parsedMethodParameters
        };
    }

    protected virtual void ADD_EVIDENCE(EvidenceAssetName evidenceName)
    {
        throw new NotImplementedException();
    }
    protected virtual void ADD_RECORD(ActorAssetName actorName)
    {
        throw new NotImplementedException();
    }
    protected virtual void PLAY_SFX(SfxAssetName sfx)
    {
        throw new NotImplementedException();
    }
    protected virtual void PLAY_SONG(SongAssetName songName, float optionalTransitionTime = 0)
    {
        throw new NotImplementedException();
    }
    protected virtual void SCENE(SceneAssetName sceneName)
    {
        throw new NotImplementedException();
    }
    protected virtual void SHOW_ITEM(CourtRecordItemName itemName, ItemDisplayPosition itemPos)
    {
        throw new NotImplementedException();
    }
    protected virtual void ACTOR(ActorAssetName actorName)
    {
        throw new NotImplementedException();
    }
    protected virtual void SPEAK(ActorAssetName actorName)
    {
        throw new NotImplementedException();
    }
    protected virtual void SPEAK_UNKNOWN(ActorAssetName actorName)
    {
        throw new NotImplementedException();
    }
    protected virtual void THINK(ActorAssetName actorName)
    {
        throw new NotImplementedException();
    }
    protected virtual void SET_ACTOR_POSITION(string slotName, ActorAssetName actorName)
    {
        throw new NotImplementedException();
    }
}

