namespace Adventure.Server.GameLogic.Dialog
{
    public class YesDialogResult : DialogResult
    {

        public YesDialogResult(DialogElement next) : base("yes", next)
        {
        }

    }
}