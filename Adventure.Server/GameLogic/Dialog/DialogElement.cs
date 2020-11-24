using System.Collections.Generic;

namespace Adventure.Server.GameLogic.Dialog
{
    public class DialogElement
    {
        public string Text { get; }
        public readonly List<DialogResult> Results = new List<DialogResult>();

        public DialogElement(string text, List<DialogResult> results)
        {
            Text = text;
            Results = results;
        }

        public DialogResult Perform(string action)
        {
            foreach (var result in Results)
            {
                if (result.Parameter == action || (result.Parameter.StartsWith("<") || result.Parameter.EndsWith(">")))
                {
                    return result.Perform(action);
                }
            }
            throw new WrongDialogInputException();
        }

    }
}