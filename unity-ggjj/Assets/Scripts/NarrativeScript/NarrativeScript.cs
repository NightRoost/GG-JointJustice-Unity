using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Ink.Runtime;
using Newtonsoft.Json;
using TextDecoder.Parser;
using UnityEngine;

[Serializable]
public class NarrativeScript : INarrativeScript
{
    [field: Tooltip("Drag an Ink narrative script here.")]
    [field: SerializeField] public TextAsset Script { get; private set; }

    private ObjectStorage _objectStorage = new ObjectStorage();

    public string Name => Script.name;
    public Story Story { get; private set; }
    public IObjectStorage ObjectStorage => _objectStorage;

    /// <summary>
    /// Initialise values on construction.
    /// </summary>
    /// <param name="script">An Ink narrative script</param>
    /// <param name="actionDecoder">An optional action decoder, used for testing</param>
    public NarrativeScript(TextAsset script, IActionDecoder actionDecoder = null)
    {
        Script = script;
        Initialize(actionDecoder);
    }

    /// <summary>
    /// Initializes script values that cannot be set in the Unity inspector
    /// and begins script reading and object preloading.
    /// </summary>
    /// <param name="actionDecoder">An optional action decoder, used for testing</param>
    public void Initialize(IActionDecoder actionDecoder = null)
    {
        if (Script == null)
        {
            throw new NullReferenceException("Could not initialize narrative script. Script field is null.");
        }
        _objectStorage = new ObjectStorage();
        Story = new Story(Script.text);
        ReadScript(actionDecoder ?? new ObjectPreloader(_objectStorage));
    }

    /// <summary>
    /// Gets all lines from an Ink story and extracts all 
    /// of the action lines using regular expressions.
    /// Calls the OnNewActionLine method on a given IActionDecoder
    /// for each action extracted.
    /// </summary>
    /// <param name="actionDecoder">An optional action decoder, used for testing</param>
    private void ReadScript(IActionDecoder actionDecoder)
    {
        var actions = ExtractActions(Script.text).Distinct();
        foreach (var action in actions)
        {
            try
            {
                actionDecoder.InvokeMatchingMethod(action);
            }
            catch (MethodNotFoundScriptParsingException)
            {
                // these types of exceptions are fine, as only actions
                // with resources need to be handled by the ObjectPreloader
            }
        }
    }

    public static string[] ExtractActions(string script)
    {
        var lines = Regex.Matches(script, @"(&.+?)\"",\""\\n\""");
        var evaluatedLines = new List<string>();

        foreach (Match match in lines)
        {
            var segments = Regex.Split(match.Groups[1].Value,
                @",\"",\""ev\"",{\""(VAR\?\"":\"".+?)\""},\""out\"",\""(?:/ev\"",\""\^,|/ev)");
            var output = "";
            foreach (var segment in segments)
            {
                var segmentToAdd = segment;
                if (segment.Contains("VAR?"))
                {
                    var variableName = Regex.Match(segment, @":\""(.+)").Groups[1].Value;
                    segmentToAdd = Regex.Match(script, @"(?<=,(.+?)),{\""VAR=\"":\""" + variableName + @"\""}")
                        .Groups[1].Value;
                }

                output += $"{segmentToAdd},";
            }

            output = output.Trim(',');
            evaluatedLines.Add(output);
        }

        return evaluatedLines.Where(line => line != string.Empty).ToArray();
    }

    /// <summary>
    /// Resets the state of a story
    /// </summary>
    public void Reset()
    {
        Story.ResetState();
    }

    public override string ToString()
    {
        return Script.name;
    }
}