using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using Ink.UnityIntegration;
using UnityEditor;
using UnityEngine;

namespace Editor.Ink
{
    public class Validation : AssetPostprocessor
    {
        /// <summary>
        /// Called when an Ink file is compiled after editing it
        /// </summary>
        private static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var inkFiles = importedAssets.Where(InkEditorUtils.IsInkFile).Select(InkLibrary.GetInkFileWithPath).ToList();
            if (!inkFiles.Any()) { return; }

            EditorApplication.delayCall += () => FindAndReportErrorsForFiles(inkFiles);
        }

        /// <summary>
        /// Menu item to allow validation of all Ink files in the project
        /// </summary>
        [UnityEditor.MenuItem("Assets/Validate Narrative Scripts", false, 202)]
        private static void ValidateAllNarrativeScripts()
        {
            FindAndReportErrorsForFiles(InkLibrary.GetMasterInkFiles());
        }
        
        /// <summary>
        /// Uses <see cref="FindErrorsInFile"/> to generate errors for all <see cref="inkFiles"/> and print them to the console
        /// Prints a log message if the process completed without any errors
        /// </summary>
        /// <param name="inkFiles"><see cref="InkFile"/> instances to check lines for errors</param>
        private static void FindAndReportErrorsForFiles(IEnumerable<InkFile> inkFiles)
        {
            var errorsInFiles = inkFiles.Select(FindErrorsInFile).SelectMany(error => error).ToList();
            errorsInFiles.ForEach(Debug.LogError);

            if (!errorsInFiles.Any())
            {
                Debug.Log("Narrative Scripts validated without errors");
            }
        }

        /// <summary>
        /// Reads an Ink file and attempts to parse every action.
        /// Returns a collection of all errors found
        /// </summary>
        /// <param name="inkFile">The Ink file to read</param>
        private static IEnumerable<string> FindErrorsInFile(InkFile inkFile)
        {
            var errors = new List<string>();

            if (inkFile.jsonAsset == null)
            {
                return errors;
            }
            
            var lines = NarrativeScript.ExtractActions(inkFile.jsonAsset.text);
            foreach (var line in lines)
            {
                try
                {
                    ActionDecoderBase.GenerateInvocationDetails(line, typeof(ActionDecoder));
                }
                catch (Exception exception)
                {
                    errors.Add($"Error in {inkFile} on line \"{line}\"\n{exception.Message}");
                }
            }

            return errors;
        }
    }
}
