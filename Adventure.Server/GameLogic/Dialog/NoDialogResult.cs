namespace Adventure.Server.GameLogic.Dialog
{
    public class NoDialogResult : DialogResult
    {

        public NoDialogResult(DialogElement next) : base("no", next)
        {
        }

    }
}