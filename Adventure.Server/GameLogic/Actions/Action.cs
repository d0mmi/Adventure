using System.Collections.Generic;

namespace Adventure.Server.GameLogic.Actions
{
    public class Action
    {
        public string Verb { get; }

        private string[] _allowedParameters;
        private ActionResult _result;

        public Action(string verb, ActionResult result, params string[] allowedParameters)
        {
            Verb = verb;
            _result = result;
            _allowedParameters = allowedParameters;
        }

        private ActionResult Perform(string[] parameters)
        {
            if (parameters.Length == 0 && _allowedParameters.Length == 0)
            {
                return _result.Perform("");
            }
            else
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    for (var j = 0; j < _allowedParameters.Length; j++)
                    {
                        if (parameters[i].ToLower() == _allowedParameters[j].ToLower())
                        {
                            return _result.Perform(parameters[i]);
                        }
                    }
                }
            }
            throw new ParameterNotValidException();
        }

        public ActionResult PerformIfVerbValid(string actionString)
        {
            return Perform(parseActionString(actionString));
        }

        private string[] parseActionString(string actionString)
        {
            var parts = actionString.Split(" ");
            if (parts.Length >= 1)
            {
                if (parts[0].ToLower() == Verb.ToLower())
                {
                    if (parts.Length == 1)
                    {
                        return new string[0];
                    }
                    else
                    {
                        var parameters = new string[parts.Length - 1];
                        for (var i = 0; i < parameters.Length; i++)
                        {
                            parameters[i] = parts[i + 1];
                        }
                        return parameters;
                    }
                }
            }
            throw new ActionNotValidException("Action not valid!");
        }
    }
}