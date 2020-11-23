using System.Collections.Generic;

namespace Adventure.Server.GameLogic.Actions
{
    public abstract class Action
    {
        public string Verb { get; }

        public string[] allowedParameters { get; }
        private ActionResult _result;

        public Action(string verb, ActionResult result, params string[] allowedParameters)
        {
            Verb = verb;
            _result = result;
            this.allowedParameters = allowedParameters;
        }

        private ActionResult Perform(string[] parameters)
        {
            if (parameters.Length == 0 && allowedParameters.Length == 0)
            {
                return _result.Perform("");
            }
            else
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    for (var j = 0; j < allowedParameters.Length; j++)
                    {
                        if (parameters[i].ToLower() == allowedParameters[j].ToLower())
                        {
                            return _result.Perform(parameters[i]);
                        }
                        else if (allowedParameters[i].StartsWith("<") && allowedParameters[i].EndsWith(">"))
                        {
                            var parameter = "";
                            foreach (var p in parameters)
                            {
                                parameter += p + " ";
                            }
                            return _result.Perform(parameter.Trim());
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