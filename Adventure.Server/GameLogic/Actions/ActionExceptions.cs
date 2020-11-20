namespace Adventure.Server.GameLogic.Actions
{
    [System.Serializable]
    public class ActionNotValidException : System.Exception
    {
        public ActionNotValidException() { }
        public ActionNotValidException(string message) : base(message) { }
        public ActionNotValidException(string message, System.Exception inner) : base(message, inner) { }
        protected ActionNotValidException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [System.Serializable]
    public class ParameterNotValidException : System.Exception
    {
        public ParameterNotValidException() { }
        public ParameterNotValidException(string message) : base(message) { }
        public ParameterNotValidException(string message, System.Exception inner) : base(message, inner) { }
        protected ParameterNotValidException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}