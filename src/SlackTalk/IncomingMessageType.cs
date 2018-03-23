namespace Devalp.SlackTalk
{
    /// <summary></summary>
    public enum IncomingMessageType : byte
    {
        /// <summary></summary>
        Command = 0,
        /// <summary></summary>
        Action = 1,
        /// <summary></summary>
        Event = 2
    }
}