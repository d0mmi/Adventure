namespace Adventure.Server.GameLogic.Dialog
{
    public class DialogResult
    {
        public string Parameter { get; }
        public DialogElement Next { get; }

        public string Description { get; set; }

        public DialogResult(string parameter, DialogElement next)
        {
            Parameter = parameter;
            Next = next;
        }

        public virtual DialogResult Perform(string action)
        {
            Description = Next.Text;
            return this;
        }

    }
}