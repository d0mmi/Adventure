namespace Adventure.Server.GameLogic.Dialog
{
    [System.Serializable]
    public class WrongDialogInputException : System.Exception
    {
        public WrongDialogInputException() { }
        public WrongDialogInputException(string message) : base(message) { }
        public WrongDialogInputException(string message, System.Exception inner) : base(message, inner) { }
        protected WrongDialogInputException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}