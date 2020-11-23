using ConsoleTables;
using System.Collections.Generic;
using System.Text;

namespace Adventure.Server.GameLogic.Actions
{
    class HelpAction : Action
    {
        public HelpAction(IEnumerable<Action> actions) : base("help", new HelpActionResult(actions), new string[0])
        {

        }
    }

    class HelpActionResult : ActionResult
    {
        private IEnumerable<Action> _actions;
        public HelpActionResult(IEnumerable<Action> actions)
        {
            this._actions = actions;
        }

        public override ActionResult Perform(string param)
        {
            var table = new ConsoleTable("Action", "Parameters");
            foreach (var action in _actions)
            {
                var sb = new StringBuilder();
                foreach (var parameter in action.allowedParameters)
                {
                    sb.Append(parameter + " ");
                }
                table.AddRow(action.Verb, sb.ToString());
            }
            Description = table.ToString();
            return this; 
        }
    }
}