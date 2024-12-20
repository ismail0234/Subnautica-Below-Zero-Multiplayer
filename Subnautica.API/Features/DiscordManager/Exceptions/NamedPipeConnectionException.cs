namespace Subnautica.API.Features.DiscordManager.Exceptions
{
    using System;

    public class NamedPipeConnectionException : Exception
    {
        internal NamedPipeConnectionException(string message) : base(message) { }
    }
}
