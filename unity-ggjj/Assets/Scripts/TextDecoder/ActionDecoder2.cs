using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace TextDecoding
{
    public class ActionDecoder2
    {
        private static readonly Dictionary<string, (ActionDecoder2Component, FieldInfo)> _actionEvents = new Dictionary<string, (ActionDecoder2Component, FieldInfo)>();

        private readonly ActionDecoder2Component _actionDecoderComponent;
        private readonly char _actionToken;

        private readonly struct ActionLine
        {
            public ActionLine(string action, char actionToken)
            {
                const char actionSideSeparator = ':';
                const char actionParameterSeparator = ',';

                action = action.Replace(actionToken.ToString(), "");
                var actionNameAndParameters = action.Split(actionToken, actionSideSeparator, actionParameterSeparator);
                ActionName = actionNameAndParameters[0];
                Parameters = actionNameAndParameters.Where(item => item != actionNameAndParameters[0]).ToArray();
            }
            
            public string ActionName { get; }
            public string[] Parameters { get; }
        }

        public ActionDecoder2(ActionDecoder2Component actionDecoderComponent, char actionToken)
        {
            _actionDecoderComponent = actionDecoderComponent;
            _actionToken = actionToken;
        }

        public void Decode(string action)
        {
            var actionLine = new ActionLine(action, _actionToken);
            var fieldInfo = GetField(actionLine.ActionName);
            var parameterTypes = GetParameterTypes(fieldInfo);
            var parameters = GetParameters(actionLine, parameterTypes);
            InvokeEvent(fieldInfo, parameters);
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

        private Type[] GetParameterTypes(FieldInfo fieldInfo)
        {
            return fieldInfo.GetValue(_actionDecoderComponent).GetType().GetGenericArguments();
        }

        private static object[] GetParameters(ActionLine actionLine, Type[] parameterTypes)
        {
            if (actionLine.Parameters.Length != parameterTypes.Length)
            {
                throw new ArgumentException($"Invalid number of arguments for action {actionLine.ActionName}. Expected {parameterTypes.Length}, got {actionLine.Parameters.Length}");
            }
            
            return actionLine.Parameters.Zip(parameterTypes, ParseParameter).ToArray();
        }

        private static object ParseParameter(string parameter, Type parameterType)
        {
            var converter = TypeDescriptor.GetConverter(parameterType);
            return converter.ConvertFrom(parameter);
        }
        
        private void InvokeEvent(FieldInfo fieldInfo, object[] parameters)
        {
            var actionEvent = fieldInfo.GetValue(_actionDecoderComponent);
            var invokeMethodInfo = actionEvent.GetType().GetMethod("Invoke");
            if (invokeMethodInfo == null)
            {
                throw new Exception("Failed to get invoke method on event");
            }
            invokeMethodInfo.Invoke(actionEvent, parameters);
        }
    }
}
