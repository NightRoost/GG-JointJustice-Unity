using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEditor.MemoryProfiler;

namespace TextDecoding
{
    public class ActionDecoder2
    {
        public const char ACTION_TOKEN = '&';
        
        private readonly ActionDecoder2Component _actionDecoderComponent;

        private struct ActionLine
        {
            public ActionLine(string action)
            {
                const char actionSideSeparator = ':';
                const char actionParameterSeparator = ',';

                var actionNameAndParameters = action.Split(ACTION_TOKEN, actionSideSeparator, actionParameterSeparator);
                ActionName = actionNameAndParameters[0];
                Parameters = actionNameAndParameters.Where(item => item != actionNameAndParameters[0]).ToArray();
            }
            
            public string ActionName { get; }
            public string[] Parameters { get; }
        }

        public ActionDecoder2(ActionDecoder2Component actionDecoderComponent)
        {
            _actionDecoderComponent = actionDecoderComponent;
        }

        public void Decode(string action)
        {
            var actionLine = new ActionLine(action);
            var fieldInfo = GetField(actionLine.ActionName);
            var parameterTypes = GetParameterTypes(fieldInfo);
            var parameters = GetParameters(actionLine.Parameters, parameterTypes);
        }

        private FieldInfo GetField(string fieldName)
        {
            var fieldInfo = _actionDecoderComponent.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo == null)
            {
                throw new MissingFieldException($"Action {fieldName} was not found on action decoder");
            }
            return fieldInfo;
        }

        private IEnumerable<Type> GetParameterTypes(FieldInfo fieldInfo)
        {
            return fieldInfo.GetValue(_actionDecoderComponent).GetType().GetGenericArguments();
        }

        private IEnumerable<object> GetParameters(IEnumerable<string> parameters, IEnumerable<Type> parameterTypes)
        {
            return parameters.Zip(parameterTypes, ParseParameter).ToArray();
        }

        private static object ParseParameter(string parameter, Type parameterType)
        {
            var converter = TypeDescriptor.GetConverter(parameterType);
            return converter.ConvertFrom(parameter);
        }
    }
}
