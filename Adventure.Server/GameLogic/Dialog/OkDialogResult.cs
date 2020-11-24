namespace Adventure.Server.GameLogic.Dialog
{
    public class OkDialogResult : DialogResult
    {

        public OkDialogResult(DialogElement next) : base("ok", next)
        {
        }

    }
}